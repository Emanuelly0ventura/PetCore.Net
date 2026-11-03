using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using PetCore.Domain.Entities;
using PetCore.Domain.Enums;
using PetCore.Infrastructure.Persistence;
using Xunit;

namespace PetCore.Tests.Integration;

[Collection(ApiCollection.Name)]
public sealed class MobileContractIntegrationTests(PetCoreApiFactory factory)
{
    [Fact]
    public async Task PetApi_CriaConsultaAtualizaEExcluiComEnvelope()
    {
        // Arrange
        var client = factory.CreateClient();
        var email = $"pet.mobile.{Guid.NewGuid():N}@exemplo.test";
        const string password = "SenhaTeste#2026";
        var tutorResponse = await client.PostAsJsonAsync("/api/Tutor", new
        {
            nome = "Tutor Mobile Pet",
            dataNascimento = "1990-01-01",
            telefone = "11987654321",
            email,
            sexo = 0,
            senha = password
        });
        Assert.Equal(HttpStatusCode.Created, tutorResponse.StatusCode);
        using var tutorJson = JsonDocument.Parse(await tutorResponse.Content.ReadAsStringAsync());
        var tutorId = tutorJson.RootElement.GetProperty("data").GetProperty("id").GetString();
        Assert.False(string.IsNullOrWhiteSpace(tutorId));

        var otherTutorEmail = $"pet.mobile.other.{Guid.NewGuid():N}@exemplo.test";
        var otherTutorResponse = await client.PostAsJsonAsync("/api/Tutor", new
        {
            nome = "Outro Tutor",
            dataNascimento = "1990-01-01",
            telefone = "11987654322",
            email = otherTutorEmail,
            sexo = 0,
            senha = password
        });
        Assert.Equal(HttpStatusCode.Created, otherTutorResponse.StatusCode);
        using var otherTutorJson = JsonDocument.Parse(await otherTutorResponse.Content.ReadAsStringAsync());
        var otherTutorId = otherTutorJson.RootElement.GetProperty("data").GetProperty("id").GetString();
        Guid foreignPetId;
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<PetCoreContext>();
            var otherTutor = db.Tutor.Single(tutor => tutor.Id == Guid.Parse(otherTutorId!));
            var foreignPet = new Pet("Pet de terceiro", "Cão", "SRD", new DateOnly(2020, 1, 1), "Curta", "Pequeno", GeneroSexoEnum.M, true, string.Empty);
            foreignPet.Tutores.Add(otherTutor);
            db.Pet.Add(foreignPet);
            db.SaveChanges();
            foreignPetId = foreignPet.Id;
        }

        var login = await client.PostAsJsonAsync("/api/auth/login", new { email, password, role = "tutor" });
        Assert.Equal(HttpStatusCode.OK, login.StatusCode);
        using var loginJson = JsonDocument.Parse(await login.Content.ReadAsStringAsync());
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", loginJson.RootElement.GetProperty("accessToken").GetString());

        // Act
        var createResponse = await client.PostAsJsonAsync("/api/Pet", new
        {
            nome = "Luna",
            especie = "Cão",
            raca = "SRD",
            nascimento = "01/01/2020",
            pelagem = "Curta",
            porte = "Pequeno",
            sexo = "Fêmea",
            tutorId = otherTutorId,
            obitoInformado = false,
            comedouroStatus = "vazio",
            status = true
        });

        // Assert creation contract
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        using var createdJson = JsonDocument.Parse(await createResponse.Content.ReadAsStringAsync());
        Assert.Equal(JsonValueKind.Object, createdJson.RootElement.ValueKind);
        var createdData = createdJson.RootElement.GetProperty("data");
        var petId = createdData.GetProperty("id").GetString();
        Assert.Equal("Fêmea", createdData.GetProperty("sexo").GetString());
        Assert.Equal(tutorId, createdData.GetProperty("tutorId").GetString());
        Assert.Contains("/api/Pet/", createResponse.Headers.Location?.ToString());

        var pagedPetsResponse = await client.GetAsync("/api/Pet?page=1&pageSize=10");
        Assert.Equal(HttpStatusCode.OK, pagedPetsResponse.StatusCode);
        using var pagedPetsJson = JsonDocument.Parse(await pagedPetsResponse.Content.ReadAsStringAsync());
        Assert.True(pagedPetsJson.RootElement.TryGetProperty("items", out var petItems));
        Assert.True(pagedPetsJson.RootElement.TryGetProperty("totalCount", out var petTotalCount));
        Assert.Contains(petItems.EnumerateArray(), item => item.GetProperty("id").GetString() == petId);
        Assert.True(petTotalCount.GetInt32() >= petItems.GetArrayLength());

        // Act query and update
        var readResponse = await client.GetAsync($"/api/Pet/{petId}");
        var updateResponse = await client.PutAsJsonAsync($"/api/Pet/{petId}/patch", new
        {
            nome = "Luna Atualizada",
            especie = "Cão",
            raca = "SRD",
            nascimento = "01/01/2020",
            pelagem = "Curta",
            porte = "Pequeno",
            sexo = "Fêmea",
            tutorId,
            obitoInformado = false,
            comedouroStatus = "cheio",
            status = true
        });

        // Assert query and update contracts
        Assert.Equal(HttpStatusCode.OK, readResponse.StatusCode);
        using var readJson = JsonDocument.Parse(await readResponse.Content.ReadAsStringAsync());
        Assert.Equal(JsonValueKind.Object, readJson.RootElement.GetProperty("data").ValueKind);
        Assert.Contains(readResponse.Headers.GetValues("Link"), value => value.Contains("rel=\"self\"", StringComparison.Ordinal));

        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        using var updatedJson = JsonDocument.Parse(await updateResponse.Content.ReadAsStringAsync());
        Assert.Equal("Luna Atualizada", updatedJson.RootElement.GetProperty("data").GetProperty("nome").GetString());
        Assert.Equal("cheio", updatedJson.RootElement.GetProperty("data").GetProperty("comedouroStatus").GetString());

        Assert.Equal(HttpStatusCode.Forbidden, (await client.GetAsync($"/api/Pet/{foreignPetId}")).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.PutAsJsonAsync($"/api/Pet/{foreignPetId}/patch", new { nome = "Tentativa" })).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.DeleteAsync($"/api/Pet/{foreignPetId}")).StatusCode);

        // Act and assert deletion
        var deleteResponse = await client.DeleteAsync($"/api/Pet/{petId}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        Assert.Empty(await deleteResponse.Content.ReadAsByteArrayAsync());
    }

    [Fact]
    public async Task PerfilMobile_PutAtualizaCamposEDevolveDtoSemSenha()
    {
        // Arrange
        var client = factory.CreateClient();
        var email = $"perfil.mobile.{Guid.NewGuid():N}@exemplo.test";
        const string password = "SenhaTeste#2026";
        var tutorResponse = await client.PostAsJsonAsync("/api/Tutor", new
        {
            nome = "Tutor Perfil",
            dataNascimento = "1990-01-01",
            telefone = "11987654321",
            email,
            sexo = 0,
            senha = password
        });
        Assert.Equal(HttpStatusCode.Created, tutorResponse.StatusCode);
        using var tutorJson = JsonDocument.Parse(await tutorResponse.Content.ReadAsStringAsync());
        var tutorId = tutorJson.RootElement.GetProperty("data").GetProperty("id").GetString();
        var otherEmail = $"perfil.outro.{Guid.NewGuid():N}@exemplo.test";
        var otherTutorResponse = await client.PostAsJsonAsync("/api/Tutor", new
        {
            nome = "Outro Perfil",
            dataNascimento = "1990-01-01",
            telefone = "11987654322",
            email = otherEmail,
            sexo = 0,
            senha = password
        });
        Assert.Equal(HttpStatusCode.Created, otherTutorResponse.StatusCode);
        using var otherTutorJson = JsonDocument.Parse(await otherTutorResponse.Content.ReadAsStringAsync());
        var otherTutorId = otherTutorJson.RootElement.GetProperty("data").GetProperty("id").GetString();
        var login = await client.PostAsJsonAsync("/api/auth/login", new { email, password, role = "tutor" });
        Assert.Equal(HttpStatusCode.OK, login.StatusCode);
        using var loginJson = JsonDocument.Parse(await login.Content.ReadAsStringAsync());
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", loginJson.RootElement.GetProperty("accessToken").GetString());

        var ownProfile = await client.GetAsync($"/api/Tutor/{tutorId}");
        Assert.Equal(HttpStatusCode.OK, ownProfile.StatusCode);
        using (var ownProfileJson = JsonDocument.Parse(await ownProfile.Content.ReadAsStringAsync()))
            Assert.Equal(tutorId, ownProfileJson.RootElement.GetProperty("data").GetProperty("id").GetString());
        Assert.Equal(HttpStatusCode.Forbidden, (await client.GetAsync($"/api/Tutor/{otherTutorId}")).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.GetAsync("/api/Tutor")).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.PutAsJsonAsync($"/api/Tutor/{otherTutorId}/patch", new { nome = "Tentativa" })).StatusCode);

        // Act
        var response = await client.PutAsJsonAsync($"/api/Tutor/{tutorId}/patch", new
        {
            nome = "Tutor Atualizado",
            email = $"atualizado.{Guid.NewGuid():N}@exemplo.test",
            telefone = "11912345678",
            nascimento = "01/01/1985",
            genero = "Masculino",
            fotoUrl = "https://example.test/foto.png"
        });

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var result = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(JsonValueKind.Object, result.RootElement.GetProperty("data").ValueKind);
        var updatedProfile = result.RootElement.GetProperty("data");
        Assert.Equal("Tutor Atualizado", updatedProfile.GetProperty("nome").GetString());
        Assert.Equal("Masculino", updatedProfile.GetProperty("genero").GetString());
        Assert.Equal("01/01/1985", updatedProfile.GetProperty("nascimento").GetString());
        Assert.Equal("https://example.test/foto.png", updatedProfile.GetProperty("fotoUrl").GetString());
        Assert.False(updatedProfile.TryGetProperty("senha", out _));
    }

    [Fact]
    public async Task Medico_CadastraConsultaAtualizaELoginPermaneceDireto()
    {
        var client = factory.CreateClient();
        var email = $"medico.envelope.{Guid.NewGuid():N}@exemplo.test";
        const string password = "SenhaTeste#2026";

        var create = await client.PostAsJsonAsync("/api/Medico", new
        {
            nome = "Médica Envelope",
            dataNascimento = "1985-01-01",
            telefone = "11987654321",
            email,
            sexo = 0,
            senha = password,
            especialidade = "Clínica geral"
        });

        Assert.Equal(HttpStatusCode.Created, create.StatusCode);
        Assert.Contains("/api/Medico/", create.Headers.Location?.ToString());
        using var createdJson = JsonDocument.Parse(await create.Content.ReadAsStringAsync());
        var medicoId = createdJson.RootElement.GetProperty("data").GetProperty("id").GetString();

        var authLogin = await client.PostAsJsonAsync("/api/auth/login", new { email, password, role = "medico" });
        Assert.Equal(HttpStatusCode.OK, authLogin.StatusCode);
        using var authLoginJson = JsonDocument.Parse(await authLogin.Content.ReadAsStringAsync());
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authLoginJson.RootElement.GetProperty("accessToken").GetString());

        var profile = await client.GetAsync($"/api/Medico/{medicoId}");
        Assert.Equal(HttpStatusCode.OK, profile.StatusCode);
        using var profileJson = JsonDocument.Parse(await profile.Content.ReadAsStringAsync());
        Assert.Equal("Médica Envelope", profileJson.RootElement.GetProperty("data").GetProperty("nome").GetString());

        var update = await client.PutAsJsonAsync($"/api/Medico/{medicoId}/patch", new { nome = "Médica Atualizada", telefone = "11912345678" });
        Assert.Equal(HttpStatusCode.OK, update.StatusCode);
        using var updatedJson = JsonDocument.Parse(await update.Content.ReadAsStringAsync());
        Assert.Equal("Médica Atualizada", updatedJson.RootElement.GetProperty("data").GetProperty("nome").GetString());

        client.DefaultRequestHeaders.Authorization = null;
        var legacyLogin = await client.GetAsync($"/api/Medico/login?email={Uri.EscapeDataString(email)}&senha={Uri.EscapeDataString(password)}");
        Assert.Equal(HttpStatusCode.OK, legacyLogin.StatusCode);
        using var legacyLoginJson = JsonDocument.Parse(await legacyLogin.Content.ReadAsStringAsync());
        Assert.Equal("Médica Atualizada", legacyLoginJson.RootElement.GetProperty("nome").GetString());
        Assert.True(legacyLoginJson.RootElement.TryGetProperty("token", out _));
        Assert.False(legacyLoginJson.RootElement.TryGetProperty("data", out _));
    }
}
