using PetCore.Application.DTOs;

namespace PetCore.Application.Services.Interfaces;

public interface IProntuarioService
{
    IReadOnlyCollection<ProntuarioResponse> FetchAll();
    
    ProntuarioResponse? FetchById(Guid id);
    
    ProntuarioResponse Create(ProntuarioRequest request);
    
    ProntuarioResponse? Patch(Guid id, ProntuarioDadosRequest request);
    
    bool Delete(Guid id);
}