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
public sealed class ApiSecurityAndPaginationTests(PetCoreApiFactory factory)
{
    [Fact]
    public async Task ApiSemToken_RetornaUnauthorized()
    {
        // Arrange
        var client = factory.CreateClient();
        // Act
        var response = await client.GetAsync("/api/Tutor");
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task LoginValido_ListaPaginada_IncluiMetadadosELinks()
    {
        // Arrange
        var email = $"teste.{Guid.NewGuid():N}@exemplo.test";
        const string password = "SenhaTeste#2026";
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<PetCoreContext>();
            db.Tutor.AddRange(
                new Tutor("Tutor A", new DateOnly(1990, 1, 1), "11987654321", email, GeneroSexoEnum.F, password),
                new Tutor("Tutor B", new DateOnly(1990, 1, 1), "11987654322", $"outro.{Guid.NewGuid():N}@exemplo.test", GeneroSexoEnum.M, password));
            db.SaveChanges();
        }
        var client = factory.CreateClient();
        var login = await client.PostAsJsonAsync("/api/auth/login", new { email, password, role = "tutor" });
        Assert.Equal(HttpStatusCode.OK, login.StatusCode);
        using var loginJson = JsonDocument.Parse(await login.Content.ReadAsStringAsync());
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginJson.RootElement.GetProperty("accessToken").GetString());
        // Act
        var response = await client.GetAsync("/api/Tutor?page=1&pageSize=1&sortBy=nome");
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(1, json.RootElement.GetProperty("items").GetArrayLength());
        Assert.True(json.RootElement.GetProperty("totalItems").GetInt32() >= 2);
        Assert.Contains(json.RootElement.GetProperty("links").EnumerateArray(), link => link.GetProperty("rel").GetString() == "next");
    }

    [Fact]
    public async Task LoginMobile_Legado_RetornaUsuarioETokenEListaSemPaginacaoRetornaArray()
    {
        // Arrange
        var email = $"mobile.{Guid.NewGuid():N}@exemplo.test";
        const string password = "SenhaTeste#2026";
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<PetCoreContext>();
            db.Tutor.Add(new Tutor("Tutor Mobile", new DateOnly(1990, 1, 1), "11987654321", email, GeneroSexoEnum.F, password));
            db.Tutor.AddRange(Enumerable.Range(1, 25).Select(index => new Tutor($"Tutor {index}", new DateOnly(1990, 1, 1), $"119{index:00000000}", $"mobile.{Guid.NewGuid():N}@exemplo.test", GeneroSexoEnum.M, password)));
            db.SaveChanges();
        }
        var client = factory.CreateClient();

        // Act
        var login = await client.GetAsync($"/api/Tutor/login?email={Uri.EscapeDataString(email)}&senha={Uri.EscapeDataString(password)}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, login.StatusCode);
        using var loginJson = JsonDocument.Parse(await login.Content.ReadAsStringAsync());
        Assert.Equal("Tutor Mobile", loginJson.RootElement.GetProperty("nome").GetString());
        Assert.False(string.IsNullOrWhiteSpace(loginJson.RootElement.GetProperty("token").GetString()));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginJson.RootElement.GetProperty("token").GetString());
        var list = await client.GetAsync("/api/Tutor");
        Assert.Equal(HttpStatusCode.OK, list.StatusCode);
        using var listJson = JsonDocument.Parse(await list.Content.ReadAsStringAsync());
        Assert.Equal(JsonValueKind.Array, listJson.RootElement.ValueKind);
        Assert.True(listJson.RootElement.GetArrayLength() >= 26);
        Assert.Contains(list.Headers.GetValues("Link"), value => value.Contains("rel=\"self\"", StringComparison.Ordinal));
    }

    [Fact]
    public async Task CadastroMobile_TutorSemToken_Retorna201EPerfilDireto()
    {
        // Arrange
        var client = factory.CreateClient();
        var email = $"cadastro.{Guid.NewGuid():N}@exemplo.test";

        // Act
        var response = await client.PostAsJsonAsync("/api/Tutor", new
        {
            nome = "Tutor Cadastro",
            dataNascimento = "1990-01-01",
            telefone = "11987654321",
            email,
            sexo = 0,
            senha = "SenhaTeste#2026"
        });

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(email, json.RootElement.GetProperty("email").GetString());
        Assert.Equal("tutor", json.RootElement.GetProperty("tipoPerfil").GetString());
    }}
