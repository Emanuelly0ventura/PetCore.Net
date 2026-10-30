using PetCore.Application.DTOs;
using PetCore.Application.Interfaces;
using PetCore.Application.Services.Interfaces;
using PetCore.Domain.Entities;

namespace PetCore.Application.Services.Implementations;

public class HistoricoService(
    IHistoricoRepository historicoRepository,
    IPetRepository petRepository) : IHistoricoService
{
    public IReadOnlyCollection<HistoricoResponse> FetchAll()
    {
        return historicoRepository.FetchAll()
            .Select(hist => new HistoricoResponse(hist))
            .ToList();
    }

    public PagedData<HistoricoResponse> FetchPage(PageQuery query)
    {
        var page = historicoRepository.FetchPage(query);
        return new PagedData<HistoricoResponse>(page.Items.Select(item => new HistoricoResponse(item)).ToList(), page.Page, page.PageSize, page.TotalItems, page.TotalPages);
    }
    
    public HistoricoResponse? FetchById(Guid id)
    {
        var hist = historicoRepository.FetchById(id);
        
        return hist is null ? null : new HistoricoResponse(hist);
    }
    
    public HistoricoResponse Create(HistoricoRequest historicoRequest)
    {
        var pet = petRepository.FetchById(historicoRequest.IdPet);

        if (pet is null)
            throw new KeyNotFoundException("Pet não encontrado para o histórico.");

        var hist = historicoRequest.ToDomain();
        
        historicoRepository.Create(hist);
        historicoRepository.SaveChanges();

        return new HistoricoResponse(hist);
    }
    
    
    public bool Delete(Guid id)
    {
        var hist = historicoRepository.FetchById(id);

        if (hist is null)
            return false;

        historicoRepository.Delete(hist);
        historicoRepository.SaveChanges();
        return true;
    }
}
