using PetCore.Application.DTOs;
using PetCore.Application.Interfaces;
using PetCore.Application.Services.Interfaces;

namespace PetCore.Application.Services.Implementations;

public class ProtocoloService(IProtocoloRepository protocoloRepository) : IProtocoloService
{
    public IReadOnlyCollection<ProtocoloResponse> FetchAll() => protocoloRepository.FetchAll().Select(x => new ProtocoloResponse(x)).ToList();
    public ProtocoloResponse? FetchById(string id) => protocoloRepository.FetchById(id) is { } item ? new ProtocoloResponse(item) : null;
    public ProtocoloResponse Create(ProtocoloRequest request) { var p = request.ToDomain(); protocoloRepository.Create(p); protocoloRepository.SaveChanges(); return new ProtocoloResponse(p); }
    public ProtocoloResponse? Patch(string id, ProtocoloDadosRequest request) { var p = protocoloRepository.FetchById(id); if (p is null) return null; p.Update(request.Titulo, request.Texto); protocoloRepository.SaveChanges(); return new ProtocoloResponse(p); }
    public bool Delete(string id) { var p = protocoloRepository.FetchById(id); if (p is null) return false; protocoloRepository.Delete(p); protocoloRepository.SaveChanges(); return true; }
}
