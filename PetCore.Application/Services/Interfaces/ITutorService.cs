using PetCore.Application.DTOs;

namespace PetCore.Application.Services.Interfaces;

public interface ITutorService
{
    IReadOnlyCollection<TutorResponse> FetchAll();
    
    TutorResponse? FetchById(Guid id);
    
    TutorResponse Create(TutorRequest request);
    
    TutorResponse? Patch(Guid id, UserDadosRequest request);
    
    bool Delete(Guid id);
    
    TutorResponse? FetchByEmail(string email, string senha);
}