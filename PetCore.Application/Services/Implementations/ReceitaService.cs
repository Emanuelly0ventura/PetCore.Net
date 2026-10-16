using PetCore.Application.DTOs;
using PetCore.Application.Interfaces;
using PetCore.Application.Services.Interfaces;

namespace PetCore.Application.Services.Implementations;

public class ReceitaService(IReceitaRepository receitaRepository,
    IMedicamentoRepository medicamentoRepository) : IReceitaService
{
    public IReadOnlyCollection<ReceitaResponse> FetchAll()
    {
        return receitaRepository.FetchAll()
            .Select(rec => new ReceitaResponse(rec))
            .ToList();
    }
    
    public ReceitaResponse? FetchById(Guid id)
    {
        var rec = receitaRepository.FetchById(id);
        
        return rec is null ? null : new ReceitaResponse(rec);
    }
    
    public ReceitaResponse Create(ReceitaRequest recRequest)
    {
        var rec = recRequest.ToDomain();

        var medicamentos = medicamentoRepository
            .FetchAllById(recRequest.IdMedicamentos ?? []);

        rec.Medicamentos = medicamentos.ToList();

        receitaRepository.Create(rec);
        receitaRepository.SaveChanges();

        return new ReceitaResponse(rec);
    }
    
    public bool Delete(Guid id)
    {
        var rec = receitaRepository.FetchById(id);

        if (rec is null)
            return false;

        receitaRepository.Delete(rec);
        receitaRepository.SaveChanges();
        return true;
    }
}
