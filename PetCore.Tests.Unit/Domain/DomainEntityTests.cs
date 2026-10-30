using PetCore.Domain.Entities;
using PetCore.Domain.Exceptions;
using PetCore.Domain.Enums;
using Xunit;

namespace PetCore.Tests.Unit.Domain;

public sealed class DomainEntityTests
{
    private static readonly DateOnly AdultBirth = new(1990, 1, 1);
    private static readonly DateOnly RecentDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));

    [Fact]
    public void Endereco_ValoresValidos_NormalizaCampos()
    {
        // Arrange / Act
        var endereco = new Endereco(" 01234567 ", " Sala 4 ");
        // Assert
        Assert.Equal("01234567", endereco.Cep);
        Assert.Equal("Sala 4", endereco.Complemento);
    }

    [Theory]
    [InlineData("", "Sala")]
    [InlineData("123", " ")]
    public void Endereco_CampoObrigatorioVazio_LancaExcecao(string cep, string complemento)
    {
        // Arrange / Act / Assert
        Assert.Throws<DomainValidationException>(() => new Endereco(cep, complemento));
    }

    [Fact]
    public void Pet_AtualizaDadosEStatusMobile_RefleteNovosValores()
    {
        // Arrange
        var pet = NewPet();
        // Act
        pet.UpdateDados(" Nino ", "Canino", "SRD", RecentDate, "Preta", "Médio", GeneroSexoEnum.M, true, "cheio");
        pet.UpdateStatus(false);
        pet.updateUrlImg(" https://img.test/nino ");
        // Assert
        Assert.Equal("Nino", pet.Nome);
        Assert.False(pet.Status);
        Assert.Equal("cheio", pet.ComedouroStatus);
        Assert.Equal("https://img.test/nino", pet.UrlImg);
        Assert.True(pet.ObitoInformado);
    }

    [Fact]
    public void Pet_StatusComedouroInvalido_LancaArgumentException()
    {
        // Arrange
        var pet = NewPet();
        // Act / Assert
        Assert.Throws<ArgumentException>(() => pet.UpdateComedouroStatus("meio"));
    }

    [Fact]
    public void Pet_DadosObrigatoriosVazios_LancaArgumentException()
    {
        // Arrange
        var pet = NewPet();
        // Act / Assert
        Assert.Throws<ArgumentException>(() => pet.UpdateDados("", "Canino", "SRD", RecentDate, "Preta", "Médio", GeneroSexoEnum.M, false, "vazio"));
    }

    [Fact]
    public void Tutor_ConstrutorEAtualizacao_NormalizamEmailTelefoneESenha()
    {
        // Arrange / Act
        var tutor = new Tutor("Ana", AdultBirth, " 11987654321 ", " ana@test.com ", GeneroSexoEnum.F, " segredo ");
        tutor.Update("11900000000", " nova@test.com ", " nova senha ", " https://img.test ");
        // Assert
        Assert.Equal("nova@test.com", tutor.Email);
        Assert.Equal("11900000000", tutor.Telefone);
        Assert.Equal("nova senha", tutor.Senha);
        Assert.Equal("https://img.test", tutor.UrlImg);
    }

    [Fact]
    public void Tutor_MenorDeIdade_LancaExcecao()
    {
        // Arrange
        var birth = DateOnly.FromDateTime(DateTime.Now.AddYears(-10));
        // Act / Assert
        Assert.Throws<DomainValidationException>(() => new Tutor("Ana", birth, "11987654321", "ana@test.com", GeneroSexoEnum.F, "senha"));
    }

    [Fact]
    public void Medico_AtualizaEspecialidadeEDadosClinica_NormalizaCampos()
    {
        // Arrange
        var medico = NewMedico();
        // Act
        medico.UpdateEspecialidade(" Cardiologia ");
        medico.UpdateDadosClinica(" Vet ", " Clínica Central ", " 123 ", " 045 ", " Sala 2 ");
        // Assert
        Assert.Equal("Cardiologia", medico.Especialidade);
        Assert.Equal("Clínica Central", medico.NomeClinica);
        Assert.Equal("123", medico.CnpjClinica);
        Assert.Equal("Sala 2", medico.ComplementoClinica);
    }

    [Fact]
    public void Medico_EspecialidadeVazia_LancaArgumentException()
    {
        // Arrange
        var medico = NewMedico();
        // Act / Assert
        Assert.Throws<ArgumentException>(() => medico.UpdateEspecialidade(" "));
    }

    [Fact]
    public void Historico_RestoreEMobileData_PreservaIdentificadorEValores()
    {
        // Arrange
        var id = Guid.NewGuid();
        var petId = Guid.NewGuid();
        // Act
        var historico = Historico.Restore(id, RecentDate, false, petId);
        historico.SetMobileData(Guid.NewGuid(), " Ana ", " Nino ", " consulta ", " Em análise ", DateTime.UtcNow);
        // Assert
        Assert.Equal(id, historico.Id);
        Assert.Equal(petId, historico.IdPet);
        Assert.Equal("Ana", historico.TutorNome);
        Assert.Equal("consulta", historico.TipoRegistro);
        Assert.False(historico.Status);
    }

    [Fact]
    public void Historico_IdPetVazio_LancaArgumentException()
    {
        // Arrange / Act / Assert
        Assert.Throws<ArgumentException>(() => new Historico(RecentDate, Guid.Empty));
    }

    [Fact]
    public void Medicamento_Update_AtualizaDosagemEInstrucao()
    {
        // Arrange
        var medicamento = new Medicamento("Remédio", "5mg", "1 vez");
        // Act
        medicamento.Update("10mg", "2 vezes");
        // Assert
        Assert.Equal("10mg", medicamento.Dosagem);
        Assert.Equal("2 vezes", medicamento.Instrucao);
    }

    [Fact]
    public void Exame_Update_AtualizaNomeDataETipo()
    {
        // Arrange
        var exame = NewExame();
        // Act
        exame.Update("Raio X", RecentDate, "Imagem");
        // Assert
        Assert.Equal("Raio X", exame.Nome);
        Assert.Equal("Imagem", exame.Tipo);
        Assert.Equal(RecentDate, exame.Data);
    }

    [Fact]
    public void Receita_ValidadePassada_LancaExcecao()
    {
        // Arrange
        var past = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
        // Act / Assert
        Assert.Throws<DomainValidationException>(() => new Receita("Receita", past, Guid.NewGuid(), Guid.NewGuid()));
    }

    [Fact]
    public void Relatorio_UpdateObservacao_NormalizaTexto()
    {
        // Arrange
        var relatorio = new Relatorio("Inicial", Guid.NewGuid(), Guid.NewGuid());
        // Act
        relatorio.Update("  Evolução clínica  ");
        // Assert
        Assert.Equal("Evolução clínica", relatorio.Observacao);
    }

    [Fact]
    public void Prontuario_UpdateMobileData_AtualizaDadosOpcionais()
    {
        // Arrange
        var prontuario = new Prontuario(RecentDate, "Consulta", Guid.NewGuid(), Guid.NewGuid());
        var petId = Guid.NewGuid();
        // Act
        prontuario.UpdateMobileData(RecentDate, "Retorno", 38.5m, 12.3m, "Tratamento", petId, " Nino ", null, null);
        // Assert
        Assert.Equal("Retorno", prontuario.Descricao);
        Assert.Equal(petId, prontuario.PetId);
        Assert.Equal("Nino", prontuario.PetNome);
        Assert.Equal(38.5m, prontuario.Temperatura);
        Assert.Equal(12.3m, prontuario.Peso);
    }

    [Fact]
    public void Prontuario_DataConsultaFutura_LancaArgumentException()
    {
        // Arrange
        var prontuario = new Prontuario(RecentDate, "Consulta", Guid.NewGuid(), Guid.NewGuid());
        // Act / Assert
        Assert.Throws<ArgumentException>(() => prontuario.UpdateMobileData(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), null, null, null, null, null, null, null, null));
    }

    [Fact]
    public void Protocolo_IdVazio_GeraIdentificadorEPermiteAtualizacao()
    {
        // Arrange / Act
        var protocolo = new Protocolo("", "Orientação", "Texto inicial");
        protocolo.Update("  Cuidados  ", "  Manter acompanhamento  ");
        // Assert
        Assert.StartsWith("PROTO-", protocolo.Id);
        Assert.Equal("Cuidados", protocolo.Titulo);
        Assert.Equal("Manter acompanhamento", protocolo.Texto);
    }

    private static Pet NewPet() => new("Nino", "Canino", "SRD", RecentDate, "Branca", "Médio", GeneroSexoEnum.M, true, "");
    private static Medico NewMedico() => new("Dra Ana", AdultBirth, "11987654321", "ana@vet.test", GeneroSexoEnum.F, "senha", "Clínica geral");
    private static Exame NewExame() => new("Hemograma", RecentDate, "Laboratório", Guid.NewGuid(), Guid.NewGuid());
}

