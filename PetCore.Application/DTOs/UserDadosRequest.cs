using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PetCore.Domain.Enums;

namespace PetCore.Application.DTOs;

public class UserDadosRequest
{
    [StringLength(150, MinimumLength = 2)] public string? Nome { get; set; }
    [EmailAddress, StringLength(200)] public string? Email { get; set; }
    [StringLength(11, MinimumLength = 11)] public string? Telefone { get; set; }
    [StringLength(30, MinimumLength = 10)] public string? Senha { get; set; }
    [StringLength(2048)] public string? UrlImg { get; set; }
    [JsonPropertyName("fotoUrl")] public string? FotoUrl { get => UrlImg; set => UrlImg = value; }
    [JsonPropertyName("nascimento")] public string? Nascimento { get; set; }
    [JsonPropertyName("genero")] public string? Genero { get; set; }
    [JsonPropertyName("especializacao")] public string? Especializacao { get; set; }
    public string? Clinica { get; set; }
    public string? NomeClinica { get; set; }
    public string? Cnpj { get; set; }
    public string? Cep { get; set; }
    public string? Complemento { get; set; }

    public DateOnly? ParseNascimento()
    {
        if (string.IsNullOrWhiteSpace(Nascimento)) return null;
        if (DateOnly.TryParseExact(Nascimento, new[] { "dd/MM/yyyy", "yyyy-MM-dd" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date)) return date;
        throw new ValidationException("Nascimento deve estar no formato DD/MM/AAAA.");
    }

    public GeneroSexoEnum? ParseGenero()
    {
        if (string.IsNullOrWhiteSpace(Genero)) return null;
        if (Enum.TryParse<GeneroSexoEnum>(Genero, true, out var result)) return result;
        if (Genero.Equals("Feminino", StringComparison.OrdinalIgnoreCase)) return GeneroSexoEnum.F;
        if (Genero.Equals("Masculino", StringComparison.OrdinalIgnoreCase)) return GeneroSexoEnum.M;
        throw new ValidationException("Gênero inválido.");
    }
}
