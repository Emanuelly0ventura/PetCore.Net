using PetCore.Application.Interfaces;
using PetCore.Domain.Entities;
using PetCore.Infrastructure.Persistence;

namespace PetCore.Infrastructure.Repositories;

public class TutorRepository : Repository<Tutor>, ITutorRepository
{
    public TutorRepository(PetCoreContext context) : base(context) { }
    public Tutor? FetchByEmail(string email)
    {
        return Set.FirstOrDefault(x => x.Email == email);
    }

    public IReadOnlyCollection<Tutor> FetchAllById(List<Guid> ids)
    {
        return Set
            .Where(x => ids.Contains(x.Id))
            .ToList();
    }
}
