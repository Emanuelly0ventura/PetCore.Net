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
public sealed class ProtocoloCatalogIntegrationTests(PetCoreApiFactory factory)
{
    [Fact]
    public async Task Veterinario_ListaTemplatesEPropriosProtocolos_TutorVeSomenteSeusEnvios()
    {
        const string password = "SenhaTeste#2026";
        var suffix = Guid.NewGuid().ToString("N");
        var veterinarianEmail = $"vet.protocolos.{suffix}@exemplo.test";
        var otherVeterinarianEmail = $"vet.outro.{suffix}@exemplo.test";
        var tutorEmail = $"tutor.protocolos.{suffix}@exemplo.test";
        var otherTutorEmail = $"tutor.outro.{suffix}@exemplo.test";
        var templateId = $"T{suffix[..19]}".ToUpperInvariant();
        var ownSentId = $"A{suffix[..19]}".ToUpperInvariant();
        var otherSentId = $"B{suffix[..19]}".ToUpperInvariant();
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<PetCoreContext>();
            var veterinarian = new Medico("Vet protocolos", new DateOnly(1985, 1, 1), "11987654321", veterinarianEmail, GeneroSexoEnum.F, password, "Clínica geral");
            var otherVeterinarian = new Medico("Outro vet", new DateOnly(1985, 1, 1), "11987654322", otherVeterinarianEmail, GeneroSexoEnum.M, password, "Clínica geral");
            var tutor = new Tutor("Tutor protocolos", new DateOnly(1990, 1, 1), "11987654323", tutorEmail, GeneroSexoEnum.F, password);
            var otherTutor = new Tutor("Outro tutor", new DateOnly(1990, 1, 1), "11987654324", otherTutorEmail, GeneroSexoEnum.M, password);
            db.AddRange(veterinarian, otherVeterinarian, tutor, otherTutor);
            db.Protocolo.AddRange(
                new Protocolo(templateId, "Template de teste", "Orientação global."),
                new Protocolo(ownSentId, "Protocolo enviado", "Orientação enviada.", tutor.Id, tutor.Nome, null, null, veterinarian.Id, veterinarian.Nome, "protocolo", DateTime.UtcNow),
                new Protocolo(otherSentId, "Envio de outro veterinário", "Não deve aparecer.", otherTutor.Id, otherTutor.Nome, null, null, otherVeterinarian.Id, otherVeterinarian.Nome, "protocolo", DateTime.UtcNow));
            db.SaveChanges();
        }

        var veterinarianClient = await Login(veterinarianEmail, password, "veterinario");

        var listResponse = await veterinarianClient.GetAsync("/api/Protocolo");
        Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);
        using var list = JsonDocument.Parse(await listResponse.Content.ReadAsStringAsync());
        var listItems = list.RootElement.EnumerateArray().ToArray();
        Assert.Contains(listItems, item => item.GetProperty("id").GetString() == templateId);
        Assert.Contains(listItems, item => item.GetProperty("id").GetString() == ownSentId);
        Assert.DoesNotContain(listItems, item => item.GetProperty("id").GetString() == otherSentId);

        var pagedResponse = await veterinarianClient.GetAsync("/api/Protocolo?page=1&pageSize=50&veterinarioId=" + Guid.NewGuid());
        Assert.Equal(HttpStatusCode.OK, pagedResponse.StatusCode);
        using var paged = JsonDocument.Parse(await pagedResponse.Content.ReadAsStringAsync());
        var pagedItems = paged.RootElement.GetProperty("items").EnumerateArray().ToArray();
        Assert.Contains(pagedItems, item => item.GetProperty("id").GetString() == templateId);
        Assert.Contains(pagedItems, item => item.GetProperty("id").GetString() == ownSentId);
        Assert.DoesNotContain(pagedItems, item => item.GetProperty("id").GetString() == otherSentId);

        Assert.Equal(HttpStatusCode.OK, (await veterinarianClient.GetAsync($"/api/Protocolo/{templateId}")).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await veterinarianClient.PutAsJsonAsync(
            $"/api/Protocolo/{templateId}/patch", new { titulo = "Alteração", texto = "Não deve ser permitido." })).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await veterinarianClient.DeleteAsync($"/api/Protocolo/{templateId}")).StatusCode);

        var tutorClient = await Login(tutorEmail, password, "tutor");
        var tutorListResponse = await tutorClient.GetAsync("/api/Protocolo");
        Assert.Equal(HttpStatusCode.OK, tutorListResponse.StatusCode);
        using var tutorList = JsonDocument.Parse(await tutorListResponse.Content.ReadAsStringAsync());
        var tutorItems = tutorList.RootElement.EnumerateArray().ToArray();
        Assert.Contains(tutorItems, item => item.GetProperty("id").GetString() == ownSentId);
        Assert.DoesNotContain(tutorItems, item => item.GetProperty("id").GetString() == templateId);
        Assert.DoesNotContain(tutorItems, item => item.GetProperty("id").GetString() == otherSentId);
        Assert.Equal(HttpStatusCode.Forbidden, (await tutorClient.GetAsync($"/api/Protocolo/{templateId}")).StatusCode);
    }

    [Fact]
    public async Task VeterinarioAutenticado_PodeEnviarProtocoloParaPetDoTutor()
    {
        const string password = "SenhaTeste#2026";
        var suffix = Guid.NewGuid().ToString("N");
        var veterinarianEmail = $"vet.envio.{suffix}@exemplo.test";
        var tutorEmail = $"tutor.envio.{suffix}@exemplo.test";
        Guid tutorId;
        Guid petId;
        Guid veterinarianId;

        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<PetCoreContext>();
            var veterinarian = new Medico("Vet envio", new DateOnly(1985, 1, 1), "11987654325", veterinarianEmail, GeneroSexoEnum.F, password, "Clínica geral");
            var tutor = new Tutor("Tutor envio", new DateOnly(1990, 1, 1), "11987654326", tutorEmail, GeneroSexoEnum.F, password);
            var pet = new Pet("Pet envio", "Cão", "SRD", new DateOnly(2020, 1, 1), "Curta", "Pequeno", GeneroSexoEnum.F, true, string.Empty);
            pet.Tutores.Add(tutor);
            db.AddRange(veterinarian, tutor, pet);
            db.SaveChanges();
            veterinarianId = veterinarian.Id;
            tutorId = tutor.Id;
            petId = pet.Id;
        }

        var client = await Login(veterinarianEmail, password, "medico");
        var response = await client.PostAsJsonAsync("/api/Protocolo", new
        {
            id = "",
            titulo = "Orientação enviada pelo teste",
            texto = "Conteúdo do protocolo enviado.",
            tutorId,
            tutorNome = "Tutor informado",
            petId,
            petNome = "Nome informado",
            veterinarioId = Guid.NewGuid(),
            veterinarioNome = "Nome informado",
            tipoRegistro = "protocolo",
            dataEnvio = DateTime.UtcNow.ToString("O")
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        using var created = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(tutorId.ToString(), created.RootElement.GetProperty("tutorId").GetString());
        Assert.Equal(petId.ToString(), created.RootElement.GetProperty("petId").GetString());
        Assert.Equal(veterinarianId.ToString(), created.RootElement.GetProperty("veterinarioId").GetString());
        Assert.Equal("protocolo", created.RootElement.GetProperty("tipoRegistro").GetString());
    }

    private async Task<HttpClient> Login(string email, string password, string role)
    {
        var client = factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/auth/login", new { email, password, role });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var login = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", login.RootElement.GetProperty("accessToken").GetString());
        return client;
    }
}
