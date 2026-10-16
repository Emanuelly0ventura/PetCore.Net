using Moq;
using PetCore.Application.DTOs;
using PetCore.Application.Interfaces;
using PetCore.Application.Services.Implementations;
using PetCore.Domain.Entities;
using Xunit;
  
namespace PetCore.Tests.Unit.Application;

public class ClinicaServiceTests
{
    [Fact]
    public void FetchById_ClinicaExistente_RetornaResponse()
    {
        // Arrange
        var clinica = new Clinica("Pet Core", "12345678901234", Guid.NewGuid());
        var repository = new Mock<IClinicaRepository>();
        repository.Setup(item => item.FetchById(clinica.Id)).Returns(clinica);
        var service = new ClinicaService(repository.Object);

        // Act
        var result = service.FetchById(clinica.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(clinica.Id, result.Id);
        Assert.Equal("Pet Core", result.Nome);
    }

    [Fact]
    public void Create_RequisicaoValida_PersisteEDevolveResponse()
    {
        // Arrange
        var request = new ClinicaRequest
        {
            Nome = "Pet Core",
            Cnpj = "12345678901234",
            IdEndereco = Guid.NewGuid()
        };
        var repository = new Mock<IClinicaRepository>();
        var service = new ClinicaService(repository.Object);

        // Act
        var result = service.Create(request);

        // Assert
        repository.Verify(item => item.Create(It.Is<Clinica>(clinica =>
            clinica.Nome == request.Nome && clinica.Cnpj == request.Cnpj)), Times.Once);
        repository.Verify(item => item.SaveChanges(), Times.Once);
        Assert.Equal(request.Nome, result.Nome);
    }
}
