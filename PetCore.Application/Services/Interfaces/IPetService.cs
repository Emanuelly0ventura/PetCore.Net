using PetCore.Application.DTOs;

namespace PetCore.Application.Services.Interfaces;

public interface IPetService
{
    PetResponse Create(PetRequest request);
    
    PetResponse? Patch(Guid id, PetDadosRequest request);
    
    bool Delete(Guid id);
    
    IReadOnlyCollection<PetResponse> FetchAll(Guid? tutorId = null);
    PagedData<PetResponse> FetchPage(PageQuery query);
    
    IReadOnlyCollection<PetMenuResponse> FetchMenuAll();
    PagedData<PetMenuResponse> FetchMenuPage(PageQuery query);
    
    PetResponse? FetchById(Guid id);
}
