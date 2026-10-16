using PetCore.Application.DTOs;

namespace PetCore.Application.Services.Interfaces;

public interface IHistoricoService
{
    IReadOnlyCollection<HistoricoResponse> FetchAll();
    
    HistoricoResponse? FetchById(Guid id);
    
    HistoricoResponse Create(HistoricoRequest request);
    
    bool Delete(Guid id);
}