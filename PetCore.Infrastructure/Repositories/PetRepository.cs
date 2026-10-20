using PetCore.Application.Interfaces;
using PetCore.Domain.Entities;
using PetCore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PetCore.Infrastructure.Repositories;

public class PetRepository(PetCoreContext context) : Repository<Pet>(context), IPetRepository
{
    public new IReadOnlyCollection<Pet> FetchAll() => Set.Include(x => x.Tutores).ToList();

    public new Pet? FetchById(Guid id) => Set.Include(x => x.Tutores).FirstOrDefault(x => x.Id == id);

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
