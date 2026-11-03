using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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
    public async Task ApiComTokenAssinadoComOutraChave_RetornaUnauthorized()
    {
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken("wrong-signing-key-with-more-than-32-bytes", DateTime.UtcNow.AddMinutes(5)));

        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/Tutor")).StatusCode);
    }

    [Fact]
    public async Task ApiComTokenExpirado_RetornaUnauthorized()
    {
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken("test-only-signing-key-with-32-bytes-minimum", DateTime.UtcNow.AddMinutes(-5)));

        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/Tutor")).StatusCode);
    }

    [Fact]
    public async Task LoginValido_ListaPaginada_IncluiMetadadosELinks()
    {
        // Arrange
        var email = $"teste.{Guid.NewGuid():N}@exemplo.test";
        var vetEmail = $"veterinario.{Guid.NewGuid():N}@exemplo.test";
        const string password = "SenhaTeste#2026";
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<PetCoreContext>();
            db.Tutor.AddRange(
                new Tutor("Tutor A", new DateOnly(1990, 1, 1), "11987654321", email, GeneroSexoEnum.F, password),
                new Tutor("Tutor B", new DateOnly(1990, 1, 1), "11987654322", $"outro.{Guid.NewGuid():N}@exemplo.test", GeneroSexoEnum.M, password));
            db.Medico.Add(new Medico("Veterinário", new DateOnly(1985, 1, 1), "11987654323", vetEmail, GeneroSexoEnum.F, password, "Clínica geral"));
            db.SaveChanges();
        }
        var client = factory.CreateClient();
        var login = await client.PostAsJsonAsync("/api/auth/login", new { email = vetEmail, password, role = "veterinario" });
        Assert.Equal(HttpStatusCode.OK, login.StatusCode);
        using var loginJson = JsonDocument.Parse(await login.Content.ReadAsStringAsync());
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginJson.RootElement.GetProperty("accessToken").GetString());
        // Act
        var response = await client.GetAsync("/api/Tutor?page=1&pageSize=1&sortBy=nome");
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(1, json.RootElement.GetProperty("items").GetArrayLength());
        Assert.True(json.RootElement.GetProperty("totalCount").GetInt32() >= 2);
        Assert.Contains(json.RootElement.GetProperty("links").EnumerateArray(), link => link.GetProperty("rel").GetString() == "next");
    }

    [Fact]
    public async Task Paginacao_ValidaLimitesFiltrosOrdenacaoELinks()
    {
        var vetEmail = $"paginacao.{Guid.NewGuid():N}@exemplo.test";
        const string password = "SenhaTeste#2026";
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<PetCoreContext>();
            db.Medico.Add(new Medico("Veterinário Paginação", new DateOnly(1985, 1, 1), "11987654323", vetEmail, GeneroSexoEnum.F, password, "Clínica geral"));
            db.Tutor.AddRange(
                new Tutor("Filtro Z", new DateOnly(1990, 1, 1), "11987654324", $"filtro.z.{Guid.NewGuid():N}@exemplo.test", GeneroSexoEnum.M, password),
                new Tutor("Filtro A", new DateOnly(1990, 1, 1), "11987654325", $"filtro.a.{Guid.NewGuid():N}@exemplo.test", GeneroSexoEnum.F, password),
                new Tutor("Ignorado", new DateOnly(1990, 1, 1), "11987654326", $"ignorado.{Guid.NewGuid():N}@exemplo.test", GeneroSexoEnum.F, password));
            db.SaveChanges();
        }

        var client = factory.CreateClient();
        var login = await client.PostAsJsonAsync("/api/auth/login", new { email = vetEmail, password, role = "veterinario" });
        using var loginJson = JsonDocument.Parse(await login.Content.ReadAsStringAsync());
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginJson.RootElement.GetProperty("accessToken").GetString());

        var first = await client.GetAsync("/api/Tutor?page=1&pageSize=1&sortBy=nome&descending=false&nome=Filtro");
        Assert.Equal(HttpStatusCode.OK, first.StatusCode);
        using var firstJson = JsonDocument.Parse(await first.Content.ReadAsStringAsync());
        Assert.Equal(2, firstJson.RootElement.GetProperty("totalCount").GetInt32());
        Assert.Equal("Filtro A", firstJson.RootElement.GetProperty("items")[0].GetProperty("nome").GetString());
        Assert.Contains(firstJson.RootElement.GetProperty("links").EnumerateArray(), link => link.GetProperty("rel").GetString() == "next");
        Assert.Contains(first.Headers.GetValues("Link"), link => link.Contains("nome=Filtro", StringComparison.Ordinal) && link.Contains("sortBy=nome", StringComparison.Ordinal));

        var last = await client.GetAsync("/api/Tutor?page=2&pageSize=1&sortBy=nome&descending=true&nome=Filtro");
        using var lastJson = JsonDocument.Parse(await last.Content.ReadAsStringAsync());
        Assert.Equal("Filtro A", lastJson.RootElement.GetProperty("items")[0].GetProperty("nome").GetString());
        Assert.Contains(lastJson.RootElement.GetProperty("links").EnumerateArray(), link => link.GetProperty("rel").GetString() == "previous");

        var beyondLast = await client.GetAsync("/api/Tutor?page=3&pageSize=1&sortBy=nome&nome=Filtro");
        using var emptyJson = JsonDocument.Parse(await beyondLast.Content.ReadAsStringAsync());
        Assert.Empty(emptyJson.RootElement.GetProperty("items").EnumerateArray());

        Assert.Equal(HttpStatusCode.BadRequest, (await client.GetAsync("/api/Tutor?page=1&pageSize=101")).StatusCode);
        var maximum = await client.GetAsync("/api/Tutor?page=1&pageSize=100");
        using var maximumJson = JsonDocument.Parse(await maximum.Content.ReadAsStringAsync());
        Assert.Equal(100, maximumJson.RootElement.GetProperty("pageSize").GetInt32());
    }

    [Fact]
    public async Task Medico_CriaRegistroComPropriaIdentidade_EOutroMedicoRecebe403()
    {
        const string password = "SenhaTeste#2026";
        var firstEmail = $"medico.proprio.{Guid.NewGuid():N}@exemplo.test";
        var secondEmail = $"medico.outro.{Guid.NewGuid():N}@exemplo.test";
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<PetCoreContext>();
            db.Medico.AddRange(
                new Medico("Médico 1", new DateOnly(1985, 1, 1), "11987654321", firstEmail, GeneroSexoEnum.F, password, "Clínica geral"),
                new Medico("Médico 2", new DateOnly(1985, 1, 1), "11987654322", secondEmail, GeneroSexoEnum.M, password, "Clínica geral"));
            db.SaveChanges();
        }

        var firstClient = await LoginAsVeterinarian(firstEmail, password);
        var secondClient = await LoginAsVeterinarian(secondEmail, password);
        var forgedId = Guid.NewGuid();
        var create = await firstClient.PostAsJsonAsync("/api/Exame", new
        {
            nome = "Hemograma de teste",
            tipoExame = "Hemograma",
            data = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
            idMedico = forgedId
        });
        Assert.True(create.StatusCode == HttpStatusCode.Created, await create.Content.ReadAsStringAsync());
        using var created = JsonDocument.Parse(await create.Content.ReadAsStringAsync());
        var recordId = created.RootElement.GetProperty("id").GetString();
        Assert.NotEqual(forgedId.ToString(), created.RootElement.GetProperty("idMedico").GetString());

        Assert.Equal(HttpStatusCode.OK, (await firstClient.GetAsync($"/api/Exame/{recordId}")).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await secondClient.GetAsync($"/api/Exame/{recordId}")).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await secondClient.DeleteAsync($"/api/Exame/{recordId}")).StatusCode);
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
            var mobileTutor = new Tutor("Tutor Mobile", new DateOnly(1990, 1, 1), "11987654321", email, GeneroSexoEnum.F, password);
            db.Tutor.Add(mobileTutor);
            db.Tutor.AddRange(Enumerable.Range(1, 25).Select(index => new Tutor($"Tutor {index}", new DateOnly(1990, 1, 1), $"119{index:00000000}", $"mobile.{Guid.NewGuid():N}@exemplo.test", GeneroSexoEnum.M, password)));
            var pet = new Pet("Luna Mobile", "Cão", "SRD", new DateOnly(2020, 1, 1), "Curta", "Pequeno", GeneroSexoEnum.F, true, string.Empty);
            pet.Tutores.Add(mobileTutor);
            db.Pet.Add(pet);
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
        var authenticatedTutorId = loginJson.RootElement.GetProperty("id").GetString();
        var list = await client.GetAsync($"/api/Pet?tutorId={authenticatedTutorId}");
        Assert.Equal(HttpStatusCode.OK, list.StatusCode);
        using var listJson = JsonDocument.Parse(await list.Content.ReadAsStringAsync());
        Assert.Equal(JsonValueKind.Array, listJson.RootElement.GetProperty("data").ValueKind);
        Assert.Equal(1, listJson.RootElement.GetProperty("data").GetArrayLength());
        Assert.Equal("Luna Mobile", listJson.RootElement.GetProperty("data")[0].GetProperty("nome").GetString());
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
        Assert.Equal(email, json.RootElement.GetProperty("data").GetProperty("email").GetString());
        Assert.Equal("tutor", json.RootElement.GetProperty("data").GetProperty("tipoPerfil").GetString());
    }

    private static string CreateToken(string key, DateTime expires)
    {
        var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken("PetCore", "PetCore.Client", [new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()), new Claim(ClaimTypes.Role, "tutor")], expires: expires, signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<HttpClient> LoginAsVeterinarian(string email, string password)
    {
        var client = factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/auth/login", new { email, password, role = "veterinario" });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var login = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.RootElement.GetProperty("accessToken").GetString());
        return client;
    }
}
