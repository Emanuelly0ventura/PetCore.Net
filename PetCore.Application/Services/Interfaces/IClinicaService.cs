using PetCore.Application.DTOs;

namespace PetCore.Application.Services.Interfaces;

public interface IClinicaService
{
    IReadOnlyCollection<ClinicaResponse> FetchAll();
    PagedData<ClinicaResponse> FetchPage(PageQuery query);
    
    ClinicaResponse? FetchById(Guid id);
    
    ClinicaResponse Create(ClinicaRequest request);
    
    ClinicaResponse? Patch(Guid id, ClinicaDadosRequest request);
    
    bool Delete(Guid id);
}