using PetCore.Application.DTOs;

namespace PetCore.Application.Services.Interfaces;

public interface IPetService
{
    PetResponse Create(PetRequest request);
    
    PetResponse? Patch(Guid id, PetDadosRequest request);
    
    bool Delete(Guid id);
    
    IReadOnlyCollection<PetResponse> FetchAll();
    
    IReadOnlyCollection<PetMenuResponse> FetchMenuAll();
    
    PetResponse? FetchById(Guid id);
}