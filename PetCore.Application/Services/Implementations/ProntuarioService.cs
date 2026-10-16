using PetCore.Application.DTOs;
using PetCore.Application.Interfaces;
using PetCore.Application.Services.Interfaces;

namespace PetCore.Application.Services.Implementations;

public class ProntuarioService(IProntuarioRepository prontuarioRepository,
    IExameRepository exameRepository,
    IReceitaRepository receitaRepository) : IProntuarioService
{
    public IReadOnlyCollection<ProntuarioResponse> FetchAll()
    {
        return prontuarioRepository.FetchAll()
            .Select(pront => new ProntuarioResponse(pront))
            .ToList();
    }
    
    public ProntuarioResponse? FetchById(Guid id)
    {
        var pront = prontuarioRepository.FetchById(id);
        
        return pront is null ? null : new ProntuarioResponse(pront);
    }
    
    public ProntuarioResponse Create(ProntuarioRequest prontuarioRequest)
    {
        var pront = prontuarioRequest.ToDomain();

        var exames = exameRepository
            .FetchAllById(prontuarioRequest.IdExames ?? []);

        var receitas = receitaRepository
            .FetchAllById(prontuarioRequest.IdReceitas ?? []);

        pront.Exames = exames.ToList();
        pront.Receitas = receitas.ToList();

        prontuarioRepository.Create(pront);
        prontuarioRepository.SaveChanges();

        return new ProntuarioResponse(pront);
    }
    
    public ProntuarioResponse? Patch(Guid id, ProntuarioDadosRequest prontuarioRequest)
    {
        var changed = false;
        var pront = prontuarioRepository.FetchById(id);

        if (pront is null)
            return null;
        
        if (!string.IsNullOrWhiteSpace(prontuarioRequest.Descricao))
        {
            pront.UpdateDesc(prontuarioRequest.Descricao);
            changed = true;
        }

        
        if (changed)
        {
            prontuarioRepository.Patch(pront);
            prontuarioRepository.SaveChanges();
        }
        
        return new ProntuarioResponse(pront);
    }
    
    public bool Delete(Guid id)
    {
        var pront = prontuarioRepository.FetchById(id);

        if (pront is null)
            return false;

        prontuarioRepository.Delete(pront);
        prontuarioRepository.SaveChanges();
        return true;
    }
}
