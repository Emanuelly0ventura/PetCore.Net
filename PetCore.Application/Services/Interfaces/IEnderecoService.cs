using PetCore.Application.DTOs;

namespace PetCore.Application.Services.Interfaces;

public interface IEnderecoService
{
    IReadOnlyCollection<EnderecoResponse> FetchAll();
    
    EnderecoResponse? FetchById(Guid id);
    
    EnderecoResponse Create(EnderecoRequest request);
    
    EnderecoResponse? Patch(Guid id, EnderecoRequest request);
    
    bool Delete(Guid id);
}