using PetCore.Application.Interfaces;
using PetCore.Domain.Entities;
using PetCore.Infrastructure.Persistence;

namespace PetCore.Infrastructure.Repositories;

public class RelatorioRepository (PetCoreContext context) : Repository<Relatorio>(context), IRelatorioRepository
{
    public IReadOnlyCollection<Relatorio> FetchAllById(List<Guid> ids)
    {
        return Set
            .Where(x => ids.Contains(x.Id))
            .ToList();
    }
}