using PetCore.Application.Interfaces;
using PetCore.Domain.Entities;
using PetCore.Infrastructure.Persistence;

namespace PetCore.Infrastructure.Repositories;

public class ReceitaRepository (PetCoreContext context) : Repository<Receita>(context), IReceitaRepository
{
    public IReadOnlyCollection<Receita> FetchAllById(List<Guid> ids)
    {
        return Set
            .Where(x => ids.Contains(x.Id))
            .ToList();
    }
}