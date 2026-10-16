using PetCore.Application.DTOs;

namespace PetCore.Application.Services.Interfaces;

public interface IRelatorioService
{
    IReadOnlyCollection<RelatorioResponse> FetchAll();
    
    RelatorioResponse? FetchById(Guid id);
    
    RelatorioResponse Create(RelatorioRequest request);
    
    RelatorioResponse? Patch(Guid id, RelatorioDadosRequest request);
    
    bool Delete(Guid id);
}