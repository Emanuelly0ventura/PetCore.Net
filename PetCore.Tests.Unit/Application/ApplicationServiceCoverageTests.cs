using Moq;
using PetCore.Application.DTOs;
using PetCore.Application.Interfaces;
using PetCore.Application.Services.Implementations;
using PetCore.Application.Security;
using PetCore.Domain.Entities;
using PetCore.Domain.Enums;
using Xunit;

namespace PetCore.Tests.Unit.Application;

public sealed class ApplicationServiceCoverageTests
{
    private static readonly DateOnly Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2));
    private static readonly DateOnly AdultBirth = new(1990, 1, 1);

    [Fact]
    public void EnderecoService_ListagemBuscaPatchEExclusao_ExecutaFluxosValidosEAusentes()
    {
        // Arrange
        var entity = new Endereco("12345678", "Casa");
        var repo = new Mock<IEnderecoRepository>();
        repo.Setup(x => x.FetchAll()).Returns([entity]);
        repo.Setup(x => x.FetchById(entity.Id)).Returns(entity);
        var service = new EnderecoService(repo.Object);
        // Act
        var all = service.FetchAll();
        var found = service.FetchById(entity.Id);
        var patched = service.Patch(entity.Id, new EnderecoRequest { Cep = "87654321", Complemento = "Apartamento" });
        var deleted = service.Delete(entity.Id);
        var missing = service.Delete(Guid.NewGuid());
        // Assert
        Assert.Single(all);
        Assert.NotNull(found);
        Assert.Equal("87654321", patched!.Cep);
        Assert.True(deleted);
        Assert.False(missing);
        repo.Verify(x => x.SaveChanges(), Times.Exactly(2));
    }

    [Fact]
    public void ClinicaService_PatchSemAlteracoes_NaoPersiste()
    {
        // Arrange
        var entity = NewClinica();
        var repo = new Mock<IClinicaRepository>();
        repo.Setup(x => x.FetchById(entity.Id)).Returns(entity);
        var service = new ClinicaService(repo.Object);
        // Act
        var result = service.Patch(entity.Id, new ClinicaDadosRequest());
        // Assert
        Assert.NotNull(result);
        repo.Verify(x => x.SaveChanges(), Times.Never);
    }

    [Fact]
    public void ClinicaService_PatchClinicaInexistente_RetornaNulo()
    {
        // Arrange
        var repo = new Mock<IClinicaRepository>();
        var service = new ClinicaService(repo.Object);
        // Act
        var result = service.Patch(Guid.NewGuid(), new ClinicaDadosRequest { Nome = "Nova" });
        // Assert
        Assert.Null(result);
        repo.Verify(x => x.Patch(It.IsAny<Clinica>()), Times.Never);
    }

    [Fact]
    public void PetService_CreateComTutorValido_AssociaTutorEPersiste()
    {
        // Arrange
        var tutor = NewTutor();
        var request = new PetRequest { Nome = "Nino", Especie = "Canino", Raca = "SRD", DataNasc = Date, Pelagem = "Preta", Porte = "Médio", Sexo = GeneroSexoEnum.M, IdTutores = [tutor.Id] };
        var pets = new Mock<IPetRepository>();
        var tutors = new Mock<ITutorRepository>();
        tutors.Setup(x => x.FetchAllById(It.IsAny<List<Guid>>())).Returns([tutor]);
        var service = new PetService(pets.Object, tutors.Object);
        // Act
        var result = service.Create(request);
        // Assert
        Assert.Equal("Nino", result.Nome);
        pets.Verify(x => x.Create(It.Is<Pet>(p => p.Tutores.Contains(tutor))), Times.Once);
        pets.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public void PetService_CreateSemTutor_LancaArgumentException()
    {
        // Arrange
        var service = new PetService(new Mock<IPetRepository>().Object, new Mock<ITutorRepository>().Object);
        var request = new PetRequest { Nome = "Nino", Especie = "Canino", Raca = "SRD", DataNasc = Date, Pelagem = "Preta", Porte = "Médio", Sexo = GeneroSexoEnum.M };
        // Act / Assert
        Assert.Throws<ArgumentException>(() => service.Create(request));
    }

    [Fact]
    public void PetService_FiltroTutor_RetornaSomentePetsVinculados()
    {
        // Arrange
        var tutor = NewTutor();
        var pet1 = NewPet(); pet1.Tutores.Add(tutor);
        var pet2 = NewPet();
        var repo = new Mock<IPetRepository>();
        repo.Setup(x => x.FetchAllByTutorId(tutor.Id)).Returns([pet1]);
        var service = new PetService(repo.Object, new Mock<ITutorRepository>().Object);
        // Act
        var result = service.FetchAll(tutor.Id);
        // Assert
        Assert.Single(result);
        Assert.Equal(pet1.Id, result.Single().Id);
    }

    [Fact]
    public void HistoricoService_CriarSemPet_LancaKeyNotFound()
    {
        // Arrange
        var petRepo = new Mock<IPetRepository>();
        var service = new HistoricoService(new Mock<IHistoricoRepository>().Object, petRepo.Object);
        var request = new HistoricoRequest { IdPet = Guid.NewGuid(), Data = Date };
        // Act / Assert
        Assert.Throws<KeyNotFoundException>(() => service.Create(request));
    }

    [Fact]
    public void HistoricoService_CriarComPet_PersisteHistorico()
    {
        // Arrange
        var pet = NewPet();
        var petRepo = new Mock<IPetRepository>();
        petRepo.Setup(x => x.FetchById(pet.Id)).Returns(pet);
        var historyRepo = new Mock<IHistoricoRepository>();
        var service = new HistoricoService(historyRepo.Object, petRepo.Object);
        // Act
        var result = service.Create(new HistoricoRequest { IdPet = pet.Id, Data = Date });
        // Assert
        Assert.Equal(pet.Id, result.IdPet);
        historyRepo.Verify(x => x.Create(It.IsAny<Historico>()), Times.Once);
        historyRepo.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public void ProntuarioService_Create_AssociaColecoesIndicadas()
    {
        // Arrange
        var exam = NewExame();
        var prescription = NewReceita();
        var examRepo = new Mock<IExameRepository>();
        examRepo.Setup(x => x.FetchAllById(It.IsAny<List<Guid>>())).Returns([exam]);
        var prescriptionRepo = new Mock<IReceitaRepository>();
        prescriptionRepo.Setup(x => x.FetchAllById(It.IsAny<List<Guid>>())).Returns([prescription]);
        var prontRepo = new Mock<IProntuarioRepository>();
        var service = new ProntuarioService(prontRepo.Object, examRepo.Object, prescriptionRepo.Object);
        var request = new ProntuarioRequest { DataEmissao = Date, Descricao = "Consulta", IdMedico = Guid.NewGuid(), IdHistorico = Guid.NewGuid(), IdExames = [exam.Id], IdReceitas = [prescription.Id] };
        // Act
        var result = service.Create(request);
        // Assert
        Assert.Single(result.IdExames);
        Assert.Single(result.IdReceitas);
        prontRepo.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public void ProntuarioService_PatchSemAlteracao_NaoPersiste()
    {
        // Arrange
        var prontuario = new Prontuario(Date, "Consulta", Guid.NewGuid(), Guid.NewGuid());
        var repo = new Mock<IProntuarioRepository>();
        repo.Setup(x => x.FetchById(prontuario.Id)).Returns(prontuario);
        var service = new ProntuarioService(repo.Object, new Mock<IExameRepository>().Object, new Mock<IReceitaRepository>().Object);
        // Act
        var result = service.Patch(prontuario.Id, new ProntuarioDadosRequest());
        // Assert
        Assert.NotNull(result);
        repo.Verify(x => x.SaveChanges(), Times.Never);
    }

    [Fact]
    public void RelatorioService_CreateEAtribuiClinicas()
    {
        // Arrange
        var clinic = NewClinica();
        var clinicRepo = new Mock<IClinicaRepository>();
        clinicRepo.Setup(x => x.FetchAllById(It.IsAny<List<Guid>>())).Returns([clinic]);
        var reportRepo = new Mock<IRelatorioRepository>();
        var service = new RelatorioService(reportRepo.Object, clinicRepo.Object);
        var request = new RelatorioRequest { Observacao = "Resumo", IdMedico = Guid.NewGuid(), IdHistorico = Guid.NewGuid(), IdClinicas = [clinic.Id] };
        // Act
        var result = service.Create(request);
        // Assert
        Assert.Equal("Resumo", result.Observacao);
        reportRepo.Verify(x => x.Create(It.Is<Relatorio>(r => r.Clinicas.Contains(clinic))), Times.Once);
        reportRepo.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public void MedicamentoService_PatchDescricaoEInstrucao_PersisteAlteracao()
    {
        // Arrange
        var medicamento = new Medicamento("Antibiótico", "5mg", "Uma vez ao dia");
        var repo = new Mock<IMedicamentoRepository>();
        repo.Setup(x => x.FetchById(medicamento.Id)).Returns(medicamento);
        var service = new MedicamentoService(repo.Object);
        // Act
        var result = service.Patch(medicamento.Id, new MedicamentoDadosRequest { Dosagem = "8mg", Instrucao = "Após refeição" });
        // Assert
        Assert.NotNull(result);
        Assert.Equal("8mg", result.Dosagem);
        Assert.Equal("Após refeição", result.Instrucao);
        repo.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public void ProtocoloService_PatchProtocoloExistente_PersisteNovosDados()
    {
        // Arrange
        var protocolo = new Protocolo("P-1", "Título", "Texto");
        var repo = new Mock<IProtocoloRepository>();
        repo.Setup(x => x.FetchById("P-1")).Returns(protocolo);
        var service = new ProtocoloService(repo.Object);
        // Act
        var result = service.Patch("P-1", new ProtocoloDadosRequest { Titulo = "Atualizado", Texto = "Novo texto" });
        // Assert
        Assert.Equal("Atualizado", result!.Titulo);
        repo.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public void TutorService_LoginPorEmailESenha_EncontraConta()
    {
        // Arrange
        var tutor = NewTutor();
        var repo = new Mock<ITutorRepository>();
        repo.Setup(x => x.FetchByEmail(tutor.Email)).Returns(tutor);
        var service = new TutorService(repo.Object, new Pbkdf2PasswordHashService());
        // Act
        var result = service.FetchByEmail(tutor.Email, tutor.Senha);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(tutor.Id, result.Id);
        Assert.True(new Pbkdf2PasswordHashService().IsHash(tutor.Senha));
        Assert.Null(service.FetchByEmail(tutor.Email, "senha errada"));
    }

    private static Clinica NewClinica() => new("Clínica", "12345678901234", Guid.NewGuid());
    private static Tutor NewTutor() => new("Ana", AdultBirth, "11987654321", $"ana.{Guid.NewGuid():N}@test.com", GeneroSexoEnum.F, "Senha#123");
    private static Pet NewPet() => new("Nino", "Canino", "SRD", Date, "Preta", "Médio", GeneroSexoEnum.M, true, "");
    private static Exame NewExame() => new("Hemograma", Date, "Laboratório", Guid.NewGuid(), Guid.NewGuid());
    private static Receita NewReceita() => new("Receita", Date.AddDays(5), Guid.NewGuid(), Guid.NewGuid());
}
