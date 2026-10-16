using PetCore.Application.DTOs;

namespace PetCore.Application.Services.Interfaces;

public interface IReceitaService
{
    IReadOnlyCollection<ReceitaResponse> FetchAll();
    
    ReceitaResponse? FetchById(Guid id);
    
    ReceitaResponse Create(ReceitaRequest request);

    bool Delete(Guid id);
}