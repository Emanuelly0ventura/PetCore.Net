using System.ComponentModel.DataAnnotations;
using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class ProtocoloRequest
{
    [StringLength(20)] public string? Id { get; set; }
    [Required, StringLength(150)] public string Titulo { get; set; }
    [Required, StringLength(2000)] public string Texto { get; set; }
    public Guid? TutorId { get; set; }
    public string? TutorNome { get; set; }
    public Guid? PetId { get; set; }
    public string? PetNome { get; set; }
    public Guid? VeterinarioId { get; set; }
    public string? VeterinarioNome { get; set; }
    public string? TipoRegistro { get; set; }
    public string? DataEnvio { get; set; }

    public Protocolo ToDomain()
    {
        DateTime? sentAt = null;
        if (!string.IsNullOrWhiteSpace(DataEnvio))
        {
            if (DateTime.TryParseExact(DataEnvio, "dd/MM/yyyy", System.Globalization.CultureInfo.GetCultureInfo("pt-BR"), System.Globalization.DateTimeStyles.AssumeUniversal, out var localDate) ||
                DateTime.TryParse(DataEnvio, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal, out localDate)) sentAt = localDate.ToUniversalTime();
        }
        return new(Id ?? string.Empty, Titulo, Texto, TutorId, TutorNome, PetId, PetNome, VeterinarioId, VeterinarioNome, TipoRegistro, sentAt);
    }
}
