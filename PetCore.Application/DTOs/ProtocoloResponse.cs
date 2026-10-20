using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class ProtocoloResponse(Protocolo protocolo)
{
    public string Id { get; set; } = protocolo.Id;
    public string Titulo { get; set; } = protocolo.Titulo;
    public string Texto { get; set; } = protocolo.Texto;
    public Guid? TutorId { get; set; } = protocolo.TutorId;
    public string TutorNome { get; set; } = protocolo.TutorNome;
    public Guid? PetId { get; set; } = protocolo.PetId;
    public string PetNome { get; set; } = protocolo.PetNome;
    public Guid? VeterinarioId { get; set; } = protocolo.VeterinarioId;
    public string VeterinarioNome { get; set; } = protocolo.VeterinarioNome;
    public string TipoRegistro { get; set; } = protocolo.TipoRegistro;
    public DateTime? DataEnvio { get; set; } = protocolo.DataEnvio;
}
