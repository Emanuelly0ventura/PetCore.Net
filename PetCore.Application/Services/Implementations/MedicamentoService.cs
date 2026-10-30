using PetCore.Application.DTOs;
using PetCore.Application.Interfaces;
using PetCore.Application.Services.Interfaces;

namespace PetCore.Application.Services.Implementations;
 
public class MedicamentoService(IMedicamentoRepository medicamentoRepository) : IMedicamentoService
{
    public IReadOnlyCollection<MedicamentoResponse> FetchAll()
    {
        return medicamentoRepository.FetchAll()
            .Select(med => new MedicamentoResponse(med))
            .ToList();
    }
    
    public PagedData<MedicamentoResponse> FetchPage(PageQuery query)
    {
        var page = medicamentoRepository.FetchPage(query);
        return new PagedData<MedicamentoResponse>(page.Items.Select(item => new MedicamentoResponse(item)).ToList(), page.Page, page.PageSize, page.TotalItems, page.TotalPages);
    }

    public MedicamentoResponse? FetchById(Guid id)
    {
        var med = medicamentoRepository.FetchById(id);
        
        return med is null ? null : new MedicamentoResponse(med);
    }
    
    public MedicamentoResponse Create(MedicamentoRequest medcRequest)
    {
        var med = medcRequest.ToDomain();
        medicamentoRepository.Create(med);
        medicamentoRepository.SaveChanges();

        return new MedicamentoResponse(med);
    }
    
    public MedicamentoResponse? Patch(Guid id, MedicamentoDadosRequest medcRequest)
    {
        var changed = false;
        var rel = medicamentoRepository.FetchById(id);

        if (rel is null)
            return null;
        
        if (!string.IsNullOrWhiteSpace(medcRequest.Dosagem))
        {
            rel.UpdateDosagem(medcRequest.Dosagem);
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(medcRequest.Instrucao))
        {
            rel.UpdateInstrucao(medcRequest.Instrucao);
            changed = true;
        }
        
        if (changed)
        {
            medicamentoRepository.Patch(rel);
            medicamentoRepository.SaveChanges();
        }
        
        return new MedicamentoResponse(rel);
    }
    
    public bool Delete(Guid id)
    {
        var medc = medicamentoRepository.FetchById(id);

        if (medc is null)
            return false;

        medicamentoRepository.Delete(medc);
        medicamentoRepository.SaveChanges();
        return true;
    }
}