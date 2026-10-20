using System.ComponentModel.DataAnnotations;
using PetCore.Domain.Entities;
using PetCore.Domain.Enums;
using System.Text.Json.Serialization;

namespace PetCore.Application.DTOs;

public class PetRequest
{
    [Required(ErrorMessage = "O campo 'Nome' deve ser preenchido")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 à 150 caracteres")]
    public string Nome { get;  set; }
    
    [Required(ErrorMessage = "O campo 'Espécie' deve ser preenchido")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Espécie deve ter entre 2 à 150 caracteres")]
    public string Especie { get;  set; }
    
    [Required(ErrorMessage = "O campo 'Raça' deve ser preenchido")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Raça deve ter entre 2 à 150 caracteres")]
    public string Raca { get;  set; }
    
    [Required(ErrorMessage = "O campo 'Data de Nascimento' deve ser preenchido")]
    public DateOnly DataNasc { get;  set; }

    // Alias accepted by the mobile app (DD/MM/YYYY).
    [JsonPropertyName("nascimento")]
    public string? Nascimento
    {
        get => null;
        set
        {
            if (!string.IsNullOrWhiteSpace(value) && DateOnly.TryParseExact(value, new[] { "dd/MM/yyyy", "yyyy-MM-dd" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var parsed))
                DataNasc = parsed;
        }
    }
    
    [Required(ErrorMessage = "O campo 'Pelagem' deve ser preenchido")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Pelagem deve ter entre 2 à 150 caracteres")]
    public string Pelagem { get;  set; }
    
    [Required(ErrorMessage = "O campo 'Porte' deve ser preenchido")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Porte deve ter entre 2 à 150 caracteres")]
    public string Porte { get;  set; }
    
    [System.Text.Json.Serialization.JsonConverter(typeof(PetSexJsonConverter))]
    [Required(ErrorMessage = "O campo 'Gênero' deve ser preenchido")]
    public GeneroSexoEnum Sexo { get;  set; }
    
    public List<Guid> IdTutores { get; set; } = [];

    [JsonPropertyName("tutorId")]
    public Guid? TutorIdMobile { get; set; }
    
    [StringLength(2048)]
    public string UrlImg { get;  set; } = string.Empty;

    [JsonPropertyName("obitoInformado")]
    public bool ObitoInformado { get; set; }

    [JsonPropertyName("comedouroStatus")]
    public string ComedouroStatus { get; set; } = "vazio";

    public bool Status { get; set; } = true;

    public Pet ToDomain() =>
        BuildDomain();

    private Pet BuildDomain()
    {
        if (TutorIdMobile.HasValue && !IdTutores.Contains(TutorIdMobile.Value)) IdTutores.Add(TutorIdMobile.Value);
        return new Pet(Nome, Especie, Raca, DataNasc, Pelagem, Porte, Sexo, Status, UrlImg, ObitoInformado, ComedouroStatus);
    }
}

public sealed class PetSexJsonConverter : System.Text.Json.Serialization.JsonConverter<GeneroSexoEnum>
{
    public override GeneroSexoEnum Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
    {
        if (reader.TokenType == System.Text.Json.JsonTokenType.Number && reader.TryGetInt32(out var number))
            return (GeneroSexoEnum)number;
        if (reader.TokenType == System.Text.Json.JsonTokenType.String)
        {
            var value = reader.GetString();
            if (Enum.TryParse<GeneroSexoEnum>(value, true, out var parsed)) return parsed;
            if (string.Equals(value, "Macho", StringComparison.OrdinalIgnoreCase)) return GeneroSexoEnum.M;
            if (string.Equals(value, "Fêmea", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "Femea", StringComparison.OrdinalIgnoreCase)) return GeneroSexoEnum.F;
        }
        throw new System.Text.Json.JsonException("Sexo deve ser F, M, Macho ou Fêmea.");
    }

    public override void Write(System.Text.Json.Utf8JsonWriter writer, GeneroSexoEnum value, System.Text.Json.JsonSerializerOptions options) => writer.WriteStringValue(value == GeneroSexoEnum.M ? "Macho" : "Fêmea");
}
