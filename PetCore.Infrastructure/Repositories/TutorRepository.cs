using PetCore.Application.Interfaces;
using PetCore.Domain.Entities;
using PetCore.Infrastructure.Persistence;

namespace PetCore.Infrastructure.Repositories;

public class TutorRepository (PetCoreContext context) : Repository<Tutor>(context), ITutorRepository
{
    public Tutor? FetchByEmail(string email, string senha)
    {
        return context.Set<Tutor>()
            .FirstOrDefault(x =>
                x.Email == email &&
                x.Senha == senha);
    }

    public IReadOnlyCollection<Tutor> FetchAllById(List<Guid> ids)
    {
        return Set
            .Where(x => ids.Contains(x.Id))
            .ToList();
    }
}