using PetCore.Application.Interfaces;
using PetCore.Domain.Entities;
using PetCore.Infrastructure.Persistence;

namespace PetCore.Infrastructure.Repositories;

public class PetRepository(PetCoreContext context) : Repository<Pet>(context), IPetRepository
{
    public IReadOnlyCollection<Pet> FetchMenuAll()
    {
        return Set.ToList();
    }

    public IReadOnlyCollection<Pet> FetchAllById(List<Guid> ids)
    {
        return Set
            .Where(x => ids.Contains(x.Id))
            .ToList();
    }
}