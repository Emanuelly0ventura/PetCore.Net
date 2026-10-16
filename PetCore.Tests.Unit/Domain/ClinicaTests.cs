using PetCore.Domain.Entities;
using Xunit;

namespace PetCore.Tests.Unit.Domain;

public class ClinicaTests
{
    [Fact]
    public void UpdateNome_NomeValido_AtualizaNome()
    {
        // Arrange
        var clinica = new Clinica("Pet Core", "12345678901234", Guid.NewGuid());

        // Act
        clinica.UpdateNome("Pet Core Centro");

        // Assert
        Assert.Equal("Pet Core Centro", clinica.Nome);
    }

    [Fact]
    public void UpdateEndereco_IdEnderecoVazio_LancaExcecao()
    {
        // Arrange
        var clinica = new Clinica("Pet Core", "12345678901234", Guid.NewGuid());

        // Act
        var action = () => clinica.UpdateEndereco(Guid.Empty);

        // Assert
        Assert.Throws<Exception>(action);
    }
}
