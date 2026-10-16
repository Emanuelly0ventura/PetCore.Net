using PetCore.Application.DTOs;
using PetCore.Application.Interfaces;
using PetCore.Application.Services.Interfaces;

namespace PetCore.Application.Services.Implementations;

public class RelatorioService(IRelatorioRepository relatorioRepository,
    IClinicaRepository clinicaRepository) : IRelatorioService
{
    public IReadOnlyCollection<RelatorioResponse> FetchAll()
    {
        return relatorioRepository.FetchAll()
            .Select(relatorio => new RelatorioResponse(relatorio))
            .ToList();
    }
    
    public RelatorioResponse? FetchById(Guid id)
    {
        var content = relatorioRepository.FetchById(id);
        
        return content is null ? null : new RelatorioResponse(content);
    }
    
    public RelatorioResponse Create(RelatorioRequest relatorioRequest)
    {
        var content = relatorioRequest.ToDomain();
        
        var clinicas = clinicaRepository.FetchAllById(relatorioRequest.IdClinicas ?? []);
        content.Clinicas = clinicas.ToList();
        
        relatorioRepository.Create(content);
        relatorioRepository.SaveChanges();

        return new RelatorioResponse(content);
    }
    
    public RelatorioResponse? Patch(Guid id, RelatorioDadosRequest relatorioRequest)
    {
        var changed = false;
        var rel = relatorioRepository.FetchById(id);

        if (rel is null)
            return null;
        
        if (!string.IsNullOrWhiteSpace(relatorioRequest.Observacao))
        {
            rel.UpdateObs(relatorioRequest.Observacao);
            changed = true;
        }

        
        if (changed)
        {
            relatorioRepository.Patch(rel);
            relatorioRepository.SaveChanges();
        }
        
        return new RelatorioResponse(rel);
    }
    
    public bool Delete(Guid id)
    {
        var rel = relatorioRepository.FetchById(id);

        if (rel is null)
            return false;

        relatorioRepository.Delete(rel);
        relatorioRepository.SaveChanges();
        return true;
    }
}
