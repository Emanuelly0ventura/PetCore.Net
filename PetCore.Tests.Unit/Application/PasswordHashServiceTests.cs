using PetCore.Application.Security;
using Xunit;

namespace PetCore.Tests.Unit.Application;

public sealed class PasswordHashServiceTests
{
    [Fact]
    public void Hash_SenhaValida_GeraValorVerificavelSemTextoOriginal()
    {
        // Arrange
        var service = new Pbkdf2PasswordHashService();
        const string password = "SenhaSegura#2026";

        // Act
        var hash = service.Hash(password);

        // Assert
        Assert.NotEqual(password, hash);
        Assert.True(service.IsHash(hash));
        Assert.True(service.Verify(password, hash));
        Assert.False(service.Verify("senha incorreta", hash));
    }
}
