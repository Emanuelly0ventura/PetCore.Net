using PetCore.Application.DTOs;
using PetCore.Application.Interfaces;
using PetCore.Application.Services.Interfaces;

namespace PetCore.Application.Services.Implementations;

public class ExameService(IExameRepository exameRepository) : IExameService
{
    public IReadOnlyCollection<ExameResponse> FetchAll()
    {
        return exameRepository.FetchAll()
            .Select(exame => new ExameResponse(exame))
            .ToList();
    }
    
    public ExameResponse? FetchById(Guid id)
    {
        var exame = exameRepository.FetchById(id);
        
        return exame is null ? null : new ExameResponse(exame);
    }
    
    public ExameResponse Create(ExameRequest exameRequest)
    {
        var exame = exameRequest.ToDomain();
        exameRepository.Create(exame);
        exameRepository.SaveChanges();

        return new ExameResponse(exame);
    }
    
    public ExameResponse? Patch(Guid id, ExameDadosRequest exameRequest)
    {
        var changed = false;
        var ex = exameRepository.FetchById(id);

        if (ex is null)
            return null;
        
        if (!string.IsNullOrWhiteSpace(exameRequest.Nome))
        {
            ex.UpdateNome(exameRequest.Nome);
            changed = true;
        }

        if (!exameRequest.Data.Equals(null))
        {
            ex.UpdateDate(exameRequest.Data);
            changed = true;
        }
        
        if (!string.IsNullOrWhiteSpace(exameRequest.Tipo))
        {
            ex.UpdateTipo(exameRequest.Tipo);
            changed = true;
        }
        
        if (changed)
        {
            exameRepository.Patch(ex);
            exameRepository.SaveChanges();
        }
        
        return new ExameResponse(ex);
    }
    
    public bool Delete(Guid id)
    {
        var ex = exameRepository.FetchById(id);

        if (ex is null)
            return false;

        exameRepository.Delete(ex);
        exameRepository.SaveChanges();
        return true;
    }
}