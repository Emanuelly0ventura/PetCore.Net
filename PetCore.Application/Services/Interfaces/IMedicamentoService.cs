using PetCore.Application.DTOs;

namespace PetCore.Application.Services.Interfaces;

public interface IMedicamentoService
{
    IReadOnlyCollection<MedicamentoResponse> FetchAll();
    
    MedicamentoResponse? FetchById(Guid id);
    
    MedicamentoResponse Create(MedicamentoRequest request);
    
    MedicamentoResponse? Patch(Guid id, MedicamentoDadosRequest request);
    
    bool Delete(Guid id);
}