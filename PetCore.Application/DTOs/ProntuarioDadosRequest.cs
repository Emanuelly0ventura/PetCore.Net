using System.Text.Json.Serialization;

namespace PetCore.Application.DTOs;

public class ProntuarioDadosRequest
{
    public string? Descricao { get; set; }
    [JsonPropertyName("observacoes")] public string? Observacoes { get; set; }
    [JsonPropertyName("dataConsulta")] public string? DataConsulta { get; set; }
    public string? Temperatura { get; set; }
    public string? Peso { get; set; }
    public string? Tratamento { get; set; }
    public Guid? PetId { get; set; }
    public string? PetNome { get; set; }
    public Guid? TutorId { get; set; }
    public string? TutorNome { get; set; }

    public DateOnly? ParseDataConsulta()
    {
        if (string.IsNullOrWhiteSpace(DataConsulta)) return null;
        if (DateOnly.TryParseExact(DataConsulta, new[] { "dd/MM/yyyy", "yyyy-MM-dd" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var value)) return value;
        throw new System.ComponentModel.DataAnnotations.ValidationException("Data da consulta inválida.");
    }

    public decimal? ParseMedida(string? value, string name)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        if (decimal.TryParse(value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var number) ||
            decimal.TryParse(value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.GetCultureInfo("pt-BR"), out number)) return number;
        throw new System.ComponentModel.DataAnnotations.ValidationException($"{name} inválido.");
    }
}
