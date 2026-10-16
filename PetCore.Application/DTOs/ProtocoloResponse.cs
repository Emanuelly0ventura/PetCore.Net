using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class ProtocoloResponse(Protocolo protocolo)
{
    public string Id { get; set; } = protocolo.Id;
    public string Titulo { get; set; } = protocolo.Titulo;
    public string Texto { get; set; } = protocolo.Texto;
}
