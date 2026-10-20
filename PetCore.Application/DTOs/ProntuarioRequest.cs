using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class ProntuarioRequest
{
    public DateOnly DataEmissao { get; set; }
    [JsonPropertyName("dataConsulta")]
    public string? DataConsulta
    {
        get => null;
        set { if (DateOnly.TryParseExact(value, new[] { "dd/MM/yyyy", "yyyy-MM-dd" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date)) DataEmissao = date; }
    }
    public string? Descricao { get; set; }
    [JsonPropertyName("observacoes")] public string? Observacoes { get; set; }
    public string? Temperatura { get; set; }
    public string? Peso { get; set; }
    public string? Tratamento { get; set; }
    public Guid? IdMedico { get; set; }
    public Guid? IdHistorico { get; set; }
    public Guid? PetId { get; set; }
    public string? PetNome { get; set; }
    public Guid? TutorId { get; set; }
    public string? TutorNome { get; set; }
    public List<Guid> IdExames { get; set; } = [];
    public List<Guid> IdReceitas { get; set; } = [];

    public Prontuario ToDomain()
    {
        if (DataEmissao == default) throw new ValidationException("Data da consulta inválida; use DD/MM/AAAA.");
        var descricao = Descricao ?? Observacoes;
        if (string.IsNullOrWhiteSpace(descricao)) descricao = "Registro clínico criado pelo aplicativo.";
        return new Prontuario(DataEmissao, descricao, IdMedico, IdHistorico, PetId, TutorId,
            PetNome, TutorNome, ParseMedida(Temperatura, "temperatura"), ParseMedida(Peso, "peso"), Tratamento);
    }

    private static decimal? ParseMedida(string? value, string name)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        if (decimal.TryParse(value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var parsed) ||
            decimal.TryParse(value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.GetCultureInfo("pt-BR"), out parsed)) return parsed;
        throw new ValidationException($"{name} inválido.");
    }
}
