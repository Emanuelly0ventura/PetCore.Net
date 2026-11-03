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
public sealed class CreateRecordsTests(PetCoreApiFactory factory)
{
    [Fact]
    public async Task SwaggerDocument_RefleteCatalogosPublicosEOperacoesProtegidas()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/swagger/v1/swagger.json");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var paths = document.RootElement.GetProperty("paths");
        var medicoList = paths.GetProperty("/api/Medico").GetProperty("get");
        Assert.True(medicoList.TryGetProperty("security", out var medicoSecurity));
        Assert.Empty(medicoSecurity.EnumerateArray());

        var petList = paths.GetProperty("/api/Pet").GetProperty("get");
        Assert.False(petList.TryGetProperty("security", out _));
        Assert.NotEmpty(document.RootElement.GetProperty("security").EnumerateArray());
    }

    [Fact]
    public async Task CatalogosPublicos_RetornamSomenteCamposNecessariosEProtegemDetalhes()
    {
        // Arrange
        var specialization = $"Especialidade-{Guid.NewGuid():N}";
        var examName = $"Exame-{Guid.NewGuid():N}";
        var clinicName = $"Clínica-{Guid.NewGuid():N}";
        var ownedExamName = $"Exame do tutor-{Guid.NewGuid():N}";
        var tutorEmail = $"tutor.exames.{Guid.NewGuid():N}@exemplo.test";
        const string tutorPassword = "SenhaTeste#2026";
        Guid medicoId;
        Guid exameId;
        Guid tutorId;

        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<PetCoreContext>();
            var medico = new Medico(
                "Médico de catálogo",
                new DateOnly(1985, 1, 1),
                "11987654321",
                $"catalogo.{Guid.NewGuid():N}@exemplo.test",
                GeneroSexoEnum.F,
                "SenhaInterna#2026",
                specialization);
            var exame = new Exame(
                examName,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
                "laboratorial",
                medico.Id,
                null);
            var endereco = new Endereco("01234000", "Sala 2");
            var clinica = new Clinica(clinicName, "12345678000195", endereco.Id);
            var tutor = new Tutor("Tutor dono de exame", new DateOnly(1990, 1, 1), "11987654322", tutorEmail, GeneroSexoEnum.F, tutorPassword);
            var ownedExamEntity = new Exame(ownedExamName, DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)), "laboratorial", medico.Id, null, null, tutor.Id, "Pet do tutor", tutor.Nome);

            db.Medico.Add(medico);
            db.Exame.Add(exame);
            db.Tutor.Add(tutor);
            db.Exame.Add(ownedExamEntity);
            db.Endereco.Add(endereco);
            db.Clinicas.Add(clinica);
            db.SaveChanges();
            medicoId = medico.Id;
            exameId = exame.Id;
            tutorId = tutor.Id;
        }

        var client = factory.CreateClient();

        // Act
        var medicosResponse = await client.GetAsync("/api/Medico");
        var examesResponse = await client.GetAsync("/api/Exame");
        var clinicasResponse = await client.GetAsync("/api/Clinica");

        // Assert
        Assert.Equal(HttpStatusCode.OK, medicosResponse.StatusCode);
        using var medicosJson = JsonDocument.Parse(await medicosResponse.Content.ReadAsStringAsync());
        Assert.Equal(JsonValueKind.Array, medicosJson.RootElement.ValueKind);
        var catalogItem = Assert.Single(medicosJson.RootElement.EnumerateArray(), item =>
            item.GetProperty("especializacao").GetString() == specialization);
        Assert.Single(catalogItem.EnumerateObject());

        var pagedMedicosResponse = await client.GetAsync("/api/Medico?page=1&pageSize=1");
        Assert.Equal(HttpStatusCode.OK, pagedMedicosResponse.StatusCode);
        using var pagedMedicosJson = JsonDocument.Parse(await pagedMedicosResponse.Content.ReadAsStringAsync());
        Assert.True(pagedMedicosJson.RootElement.TryGetProperty("items", out _));
        Assert.True(pagedMedicosJson.RootElement.TryGetProperty("totalCount", out _));
        Assert.False(pagedMedicosJson.RootElement.TryGetProperty("data", out _));

        Assert.Equal(HttpStatusCode.OK, examesResponse.StatusCode);
        using var examesJson = JsonDocument.Parse(await examesResponse.Content.ReadAsStringAsync());
        Assert.Equal(JsonValueKind.Array, examesJson.RootElement.ValueKind);
        var examCatalogItem = Assert.Single(examesJson.RootElement.EnumerateArray(), item =>
            item.GetProperty("nome").GetString() == examName);
        Assert.Single(examCatalogItem.EnumerateObject());

        Assert.Equal(HttpStatusCode.OK, clinicasResponse.StatusCode);
        using var clinicasJson = JsonDocument.Parse(await clinicasResponse.Content.ReadAsStringAsync());
        Assert.Equal(JsonValueKind.Array, clinicasJson.RootElement.ValueKind);
        var clinicCatalogItem = Assert.Single(clinicasJson.RootElement.EnumerateArray(), item =>
            item.GetProperty("nome").GetString() == clinicName);
        Assert.Equal("12345678000195", clinicCatalogItem.GetProperty("cnpj").GetString());
        Assert.Equal("01234000", clinicCatalogItem.GetProperty("cep").GetString());
        Assert.Equal("Sala 2", clinicCatalogItem.GetProperty("complemento").GetString());

        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync($"/api/Medico/{medicoId}")).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync($"/api/Exame/{exameId}")).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync($"/api/Clinica/{Guid.NewGuid()}")).StatusCode);

        var login = await client.PostAsJsonAsync("/api/auth/login", new { email = tutorEmail, password = tutorPassword, role = "tutor" });
        Assert.Equal(HttpStatusCode.OK, login.StatusCode);
        using var loginJson = JsonDocument.Parse(await login.Content.ReadAsStringAsync());
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginJson.RootElement.GetProperty("accessToken").GetString());
        var tutorExamsResponse = await client.GetAsync("/api/Exame");
        Assert.Equal(HttpStatusCode.OK, tutorExamsResponse.StatusCode);
        using var tutorExams = JsonDocument.Parse(await tutorExamsResponse.Content.ReadAsStringAsync());
        Assert.Equal(JsonValueKind.Array, tutorExams.RootElement.ValueKind);
        var ownedExam = Assert.Single(tutorExams.RootElement.EnumerateArray());
        Assert.Equal(ownedExamName, ownedExam.GetProperty("nome").GetString());
        Assert.Equal(tutorId.ToString(), ownedExam.GetProperty("tutorId").GetString());
        Assert.True(ownedExam.TryGetProperty("idMedico", out _));
    }
}
