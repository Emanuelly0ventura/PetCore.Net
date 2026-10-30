using PetCore.Application.DTOs;

namespace PetCore.Application.Services.Interfaces;

public interface ITutorService
{
    IReadOnlyCollection<TutorResponse> FetchAll();
    PagedData<TutorResponse> FetchPage(PageQuery query);
    
    TutorResponse? FetchById(Guid id);
    
    TutorResponse Create(TutorRequest request);
    
    TutorResponse? Patch(Guid id, UserDadosRequest request);
    
    bool Delete(Guid id);
    
    TutorResponse? FetchByEmail(string email, string senha);
}