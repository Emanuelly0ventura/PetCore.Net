using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class ExameRequest
{
    public string? Nome { get; set; }
    public DateOnly Data { get; set; }
    public string? Tipo { get; set; }
    [JsonPropertyName("tipoExame")] public string? TipoExame { get; set; }
    [Required] public Guid IdMedico { get; set; }
    [JsonPropertyName("veterinarioId")] public Guid? VeterinarioId { get => null; set { if (value.HasValue) IdMedico = value.Value; } }
    public Guid? IdProntuario { get; set; }
    public Guid? PetId { get; set; }
    public string? PetNome { get; set; }
    public Guid? TutorId { get; set; }
    public string? TutorNome { get; set; }
    public string? VeterinarioNome { get; set; }
    public string? ArquivoSolicitacao { get; set; }
    public string? DataEnvio { get; set; }

    public Exame ToDomain()
    {
        DateTime? sentAt = null;
        if (!string.IsNullOrWhiteSpace(DataEnvio) && DateTime.TryParse(DataEnvio, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal, out var parsed))
        {
            sentAt = parsed.ToUniversalTime();
            if (Data == default) Data = DateOnly.FromDateTime(parsed);
        }
        if (Data == default) Data = DateOnly.FromDateTime(DateTime.UtcNow);
        var tipo = TipoExame ?? Tipo;
        if (string.IsNullOrWhiteSpace(tipo)) throw new ValidationException("tipoExame é obrigatório.");
        var nome = string.IsNullOrWhiteSpace(Nome) ? tipo : Nome;
        return new Exame(nome, Data, tipo, IdMedico, IdProntuario, PetId, TutorId, PetNome, TutorNome,
            VeterinarioNome, ArquivoSolicitacao, sentAt);
    }
}
