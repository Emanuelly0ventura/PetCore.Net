using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class ReceitaRequest
{
    public string? Nome { get; set; }
    public DateOnly? Validade { get; set; }
    [Required] public Guid IdMedicoResponsavel { get; set; }
    [JsonPropertyName("veterinarioId")] public Guid? VeterinarioId { get => null; set { if (value.HasValue) IdMedicoResponsavel = value.Value; } }
    public Guid? IdProntuario { get; set; }
    public List<Guid> IdMedicamentos { get; set; } = [];
    public Guid? TutorId { get; set; }
    public string? TutorNome { get; set; }
    public Guid? PetId { get; set; }
    public string? PetNome { get; set; }
    public string? VeterinarioNome { get; set; }
    public string? ArquivoReceita { get; set; }
    public string? DataEnvio { get; set; }
    public string? TipoRegistro { get; set; }

    public Receita ToDomain()
    {
        DateTime? sentAt = null;
        if (!string.IsNullOrWhiteSpace(DataEnvio) && DateTime.TryParse(DataEnvio, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal, out var parsed)) sentAt = parsed.ToUniversalTime();
        var name = string.IsNullOrWhiteSpace(Nome) ? ArquivoReceita : Nome;
        if (string.IsNullOrWhiteSpace(name)) throw new ValidationException("Informe nome ou arquivoReceita.");
        return new Receita(name, Validade, IdMedicoResponsavel, IdProntuario, PetId, TutorId,
            PetNome, TutorNome, VeterinarioNome, ArquivoReceita, sentAt);
    }
}
