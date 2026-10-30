using Moq;
using PetCore.Application.DTOs;
using PetCore.Application.Interfaces;
using PetCore.Application.Services.Implementations;
using PetCore.Application.Security;
using PetCore.Domain.Entities;
using PetCore.Domain.Enums;
using Xunit;

namespace PetCore.Tests.Unit.Application;

public sealed class AdditionalServiceTests
{
    private static readonly DateOnly Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2));
    private static readonly DateOnly AdultBirth = new(1990, 1, 1);

    [Fact]
    public void ExameService_CreatePatchDelete_PersisteCadaOperacao()
    {
        // Arrange
        var repo = new Mock<IExameRepository>();
        var service = new ExameService(repo.Object);
        var request = new ExameRequest { Nome = "Hemograma", Tipo = "Sangue", Data = Date, IdMedico = Guid.NewGuid(), IdProntuario = Guid.NewGuid() };
        // Act
        var created = service.Create(request);
        repo.Setup(x => x.FetchById(created.Id)).Returns(new Exame("Hemograma", Date, "Sangue", request.IdMedico, request.IdProntuario));
        var patched = service.Patch(created.Id, new ExameDadosRequest { Nome = "Hemograma completo", Tipo = "Laboratório" });
        repo.Setup(x => x.FetchById(created.Id)).Returns(new Exame("Hemograma", Date, "Sangue", request.IdMedico, request.IdProntuario));
        var deleted = service.Delete(created.Id);
        // Assert
        Assert.Equal("Hemograma", created.Nome);
        Assert.Equal("Hemograma completo", patched!.Nome);
        Assert.Equal(Date, patched.Data); // Data não enviada no PATCH deve permanecer inalterada.
        Assert.True(deleted);
        repo.Verify(x => x.SaveChanges(), Times.Exactly(3));
    }

    [Fact]
    public void MedicoService_CreateBuscaEPatch_MapeiaDadosDePerfil()
    {
        // Arrange
        var repo = new Mock<IMedicoRepository>();
        var service = new MedicoService(repo.Object, new Pbkdf2PasswordHashService());
        var request = new MedicoRequest { Nome = "Dra Ana", DataNascimento = AdultBirth, Telefone = "11987654321", Email = "ana@vet.test", Sexo = GeneroSexoEnum.F, Senha = "Senha#2026", Especialidade = "Clínica geral" };
        // Act
        var created = service.Create(request);
        repo.Verify(x => x.Create(It.Is<Medico>(item => item.Senha.StartsWith("pbkdf2-sha256$"))), Times.Once);
        repo.Setup(x => x.FetchByEmail("ana@vet.test")).Returns(request.ToDomain());
        var login = service.FetchByEmail("ana@vet.test", "Senha#2026");
        repo.Setup(x => x.FetchById(created.Id)).Returns(request.ToDomain());
        var patched = service.Patch(created.Id, new UserDadosRequest { Nome = "Ana", Especializacao = "Cardiologia", Clinica = "Vet" });
        // Assert
        Assert.Equal("veterinario", created.TipoPerfil);
        Assert.NotNull(login);
        Assert.Equal("Cardiologia", patched!.Especializacao);
        repo.Verify(x => x.SaveChanges(), Times.Exactly(3));
    }

    [Fact]
    public void ReceitaService_CreateBuscaEExclui_AssociaMedicamentos()
    {
        // Arrange
        var medicine = new Medicamento("Antibiótico", "5mg", "Diário");
        var medicineRepo = new Mock<IMedicamentoRepository>();
        medicineRepo.Setup(x => x.FetchAllById(It.IsAny<List<Guid>>())).Returns([medicine]);
        var receiptRepo = new Mock<IReceitaRepository>();
        var service = new ReceitaService(receiptRepo.Object, medicineRepo.Object);
        var request = new ReceitaRequest { Nome = "Receita", IdMedicoResponsavel = Guid.NewGuid(), IdProntuario = Guid.NewGuid(), IdMedicamentos = [medicine.Id] };
        // Act
        var created = service.Create(request);
        receiptRepo.Setup(x => x.FetchById(created.Id)).Returns(request.ToDomain());
        var found = service.FetchById(created.Id);
        var deleted = service.Delete(created.Id);
        // Assert
        Assert.Equal("Receita", created.Nome);
        Assert.NotNull(found);
        Assert.True(deleted);
        receiptRepo.Verify(x => x.SaveChanges(), Times.Exactly(2));
    }

    [Fact]
    public void TutorService_CreatePatchEExclui_AtualizaCamposDoUsuario()
    {
        // Arrange
        var repo = new Mock<ITutorRepository>();
        var service = new TutorService(repo.Object, new Pbkdf2PasswordHashService());
        var request = new TutorRequest { Nome = "Ana", DataNascimento = AdultBirth, Telefone = "11987654321", Email = "ana@test.com", Sexo = GeneroSexoEnum.F, Senha = "Senha#2026" };
        // Act
        var created = service.Create(request);
        repo.Verify(x => x.Create(It.Is<Tutor>(item => item.Senha.StartsWith("pbkdf2-sha256$"))), Times.Once);
        var existing = request.ToDomain();
        repo.Setup(x => x.FetchById(created.Id)).Returns(existing);
        var patched = service.Patch(created.Id, new UserDadosRequest { Nome = "Ana Maria", Email = "maria@test.com", UrlImg = "https://img.test/a", Nascimento = "01/01/1990", Genero = "Feminino" });
        var deletedEntity = request.ToDomain();
        repo.Setup(x => x.FetchById(created.Id)).Returns(deletedEntity);
        var deleted = service.Delete(created.Id);
        // Assert
        Assert.Equal("Ana Maria", patched!.Nome);
        Assert.Equal("maria@test.com", patched.Email);
        Assert.True(deleted);
        repo.Verify(x => x.SaveChanges(), Times.Exactly(3));
    }
}
