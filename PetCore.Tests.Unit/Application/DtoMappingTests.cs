using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using PetCore.Application.DTOs;
using PetCore.Domain.Entities;
using PetCore.Domain.Enums;
using Xunit;

namespace PetCore.Tests.Unit.Application;

public sealed class DtoMappingTests
{
    private static readonly DateOnly RecentDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3));
    private static readonly DateOnly AdultBirth = new(1990, 1, 1);

    [Fact]
    public void ExameRequest_AceitaAliasMobileEDataEnvio_ConverteParaDominio()
    {
        // Arrange
        var doctorId = Guid.NewGuid();
        var request = new ExameRequest { TipoExame = "Laboratório", IdMedico = doctorId, IdProntuario = Guid.NewGuid(), DataEnvio = "2026-10-20T12:00:00Z" };
        // Act
        var entity = request.ToDomain();
        var response = new ExameResponse(entity);
        // Assert
        Assert.Equal("Laboratório", entity.Nome);
        Assert.Equal("Laboratório", response.TipoMobile);
        Assert.Equal(doctorId, response.VeterinarioId);
        Assert.NotNull(response.DataEnvio);
    }

    [Fact]
    public void ExameRequest_SemTipo_LancaValidationException()
    {
        // Arrange
        var request = new ExameRequest { IdMedico = Guid.NewGuid() };
        // Act / Assert
        Assert.Throws<ValidationException>(() => request.ToDomain());
    }

    [Fact]
    public void MedicamentoRequest_ToDomain_CriaMedicamento()
    {
        // Arrange
        var request = new MedicamentoRequest { Nome = "  Analgésico ", Dosagem = "5mg", Instrucao = "Após refeição" };
        // Act
        var entity = request.ToDomain();
        // Assert
        Assert.Equal("Analgésico", entity.Nome);
        Assert.Equal("5mg", entity.Dosagem);
    }

    [Fact]
    public void MedicoRequest_ToDomainEResponse_MapeiaPerfil()
    {
        // Arrange
        var request = new MedicoRequest { Nome = "Dra Ana", DataNascimento = AdultBirth, Telefone = "11987654321", Email = "ana@vet.test", Sexo = GeneroSexoEnum.F, Senha = "Senha#2026", Especialidade = "Clínica geral" };
        // Act
        var entity = request.ToDomain();
        entity.UpdateDadosClinica("Vet", "Clínica Central", "123", "045", "Sala 1");
        var response = new MedicoResponse(entity);
        // Assert
        Assert.Equal("Feminino", response.Genero);
        Assert.Equal("Clínica Central", response.NomeClinica);
        Assert.Equal("veterinario", response.TipoPerfil);
    }

    [Fact]
    public void PetDadosRequestEDashboardResponse_MapeiamPet()
    {
        // Arrange
        var pet = NewPet();
        // Act
        var patch = new PetDadosRequest(pet);
        var menu = new PetMenuResponse(pet);
        // Assert
        Assert.Equal(pet.Nome, patch.Nome);
        Assert.Equal(pet.Id, menu.Id);
        Assert.Equal(pet.DataNasc, menu.DataNasc);
    }

    [Theory]
    [InlineData("F", GeneroSexoEnum.F)]
    [InlineData("Macho", GeneroSexoEnum.M)]
    [InlineData("Fêmea", GeneroSexoEnum.F)]
    public void PetSexJsonConverter_ValorTextoAceito_LeEnum(string input, GeneroSexoEnum expected)
    {
        // Arrange / Act
        var options = new JsonSerializerOptions();
        options.Converters.Add(new PetSexJsonConverter());
        var result = JsonSerializer.Deserialize<GeneroSexoEnum>($"\"{input}\"", options);
        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void PetSexJsonConverter_EnumSerializado_UsaRotuloMobile()
    {
        // Arrange / Act
        var options = new JsonSerializerOptions();
        options.Converters.Add(new PetSexJsonConverter());
        var json = JsonSerializer.Serialize(GeneroSexoEnum.M, options);
        // Assert
        Assert.Equal("\"Macho\"", json);
    }

    [Fact]
    public void PetSexJsonConverter_ValorInvalido_LancaJsonException()
    {
        // Arrange / Act / Assert
        var options = new JsonSerializerOptions();
        options.Converters.Add(new PetSexJsonConverter());
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<GeneroSexoEnum>("\"desconhecido\"", options));
    }

    [Fact]
    public void PetRequest_AliasNascimentoETutorId_AtualizaCampos()
    {
        // Arrange
        var tutor = Guid.NewGuid();
        var request = new PetRequest { Nome = "Nino", Especie = "Canino", Raca = "SRD", Pelagem = "Preta", Porte = "Médio", Sexo = GeneroSexoEnum.M };
        // Act
        request.Nascimento = "15/05/2020";
        request.TutorIdMobile = tutor;
        var entity = request.ToDomain();
        // Assert
        Assert.Equal(new DateOnly(2020, 5, 15), entity.DataNasc);
        Assert.Contains(tutor, request.IdTutores);
    }

    [Fact]
    public void ProtocoloRequest_DataEnvioValida_ConverteParaUtc()
    {
        // Arrange
        var request = new ProtocoloRequest { Titulo = "Cuidados", Texto = "Manter observação", DataEnvio = "20/10/2026" };
        // Act
        var entity = request.ToDomain();
        var response = new ProtocoloResponse(entity);
        // Assert
        Assert.Equal("CUIDADOS", entity.Titulo.ToUpperInvariant());
        Assert.Equal(entity.Id, response.Id);
    }

    [Fact]
    public void ReceitaRequest_UsaArquivoComoNomeAlternativo_MapeiaMedicamentos()
    {
        // Arrange
        var entity = new ReceitaRequest { ArquivoReceita = "receita.pdf", IdMedicoResponsavel = Guid.NewGuid(), IdProntuario = Guid.NewGuid(), DataEnvio = "2026-10-20T12:00:00Z" }.ToDomain();
        entity.Medicamentos.Add(new Medicamento("Item", "5mg", "Uso diário"));
        // Act
        var response = new ReceitaResponse(entity);
        // Assert
        Assert.Equal("receita.pdf", response.Nome);
        Assert.Single(response.IdMedicamentos);
        Assert.Equal("receita", response.TipoRegistro);
    }

    [Fact]
    public void ReceitaRequest_SemNomeEArquivo_LancaValidationException()
    {
        // Arrange
        var request = new ReceitaRequest { IdMedicoResponsavel = Guid.NewGuid() };
        // Act / Assert
        Assert.Throws<ValidationException>(() => request.ToDomain());
    }

    [Fact]
    public void RelatorioRequest_ExtraiHistoricoDaListaJsonDoMobile()
    {
        // Arrange
        var historyId = Guid.NewGuid();
        var request = new RelatorioRequest { IdMedico = Guid.NewGuid(), Prontuarios = $"[{{\"idHistorico\":\"{historyId}\"}}]", Observacoes = "Resumo" };
        // Act
        var entity = request.ToDomain();
        // Assert
        Assert.Equal(historyId, entity.IdHistorico);
        Assert.Equal("Resumo", entity.Observacao);
    }

    [Fact]
    public void UserDadosRequest_ConverteNascimentoGeneroEFotoUrl()
    {
        // Arrange
        var request = new UserDadosRequest { Nascimento = "01/01/1990", Genero = "Feminino", FotoUrl = "https://img.test/a" };
        // Act
        var date = request.ParseNascimento();
        var gender = request.ParseGenero();
        // Assert
        Assert.Equal(AdultBirth, date);
        Assert.Equal(GeneroSexoEnum.F, gender);
        Assert.Equal("https://img.test/a", request.UrlImg);
    }

    [Fact]
    public void UserDadosRequest_DataOuGeneroInvalido_LancaValidationException()
    {
        // Arrange
        var invalidDate = new UserDadosRequest { Nascimento = "ontem" };
        var invalidGender = new UserDadosRequest { Genero = "outro" };
        // Act / Assert
        Assert.Throws<ValidationException>(() => invalidDate.ParseNascimento());
        Assert.Throws<ValidationException>(() => invalidGender.ParseGenero());
    }

    private static Pet NewPet() => new("Nino", "Canino", "SRD", RecentDate, "Preta", "Médio", GeneroSexoEnum.M, true, "https://img.test/nino");
}

