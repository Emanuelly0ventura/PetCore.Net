using PetCore.Application.DTOs;

namespace PetCore.Application.Services.Interfaces;

public interface IReceitaService
{
    IReadOnlyCollection<ReceitaResponse> FetchAll();
    PagedData<ReceitaResponse> FetchPage(PageQuery query);
    
    ReceitaResponse? FetchById(Guid id);
    
    ReceitaResponse Create(ReceitaRequest request);

    bool Delete(Guid id);
}