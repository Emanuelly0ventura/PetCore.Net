using PetCore.Application.DTOs;

namespace PetCore.Application.Services.Interfaces;

public interface IMedicoService
{
    IReadOnlyCollection<MedicoResponse> FetchAll();
    
    MedicoResponse? FetchById(Guid id);
    
    MedicoResponse Create(MedicoRequest request);
    
    MedicoResponse? Patch(Guid id, UserDadosRequest request);
    
    bool Delete(Guid id);
    
    MedicoResponse? FetchByEmail(string email, string senha);
}