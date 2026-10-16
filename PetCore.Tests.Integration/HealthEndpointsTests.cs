using Xunit;

namespace PetCore.Tests.Integration;

[Collection(ApiCollection.Name)]
public class HealthEndpointsTests(PetCoreApiFactory factory)
{
    [Fact]
    public async Task HealthLive_ApiDisponivel_RetornaOk()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/health/live");

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task HealthReady_DependenciasDisponiveis_RetornaOk()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/health/ready");

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task RotaInexistente_RequisicaoInvalida_RetornaNotFound()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/recurso-inexistente");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}
