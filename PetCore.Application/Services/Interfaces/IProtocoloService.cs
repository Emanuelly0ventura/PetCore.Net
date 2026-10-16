using PetCore.Application.DTOs;

namespace PetCore.Application.Services.Interfaces;

public interface IProtocoloService
{
    IReadOnlyCollection<ProtocoloResponse> FetchAll();
    ProtocoloResponse? FetchById(string id);
    ProtocoloResponse Create(ProtocoloRequest request);
    ProtocoloResponse? Patch(string id, ProtocoloDadosRequest request);
    bool Delete(string id);
}
