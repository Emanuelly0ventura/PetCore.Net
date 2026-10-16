using PetCore.Application.Interfaces;
using PetCore.Domain.Entities;
using PetCore.Infrastructure.Persistence;

namespace PetCore.Infrastructure.Repositories;

public class ProntuarioRepository (PetCoreContext context) : Repository<Prontuario>(context), IProntuarioRepository
{
    public IReadOnlyCollection<Prontuario> FetchAllById(List<Guid> ids)
    {
        return Set
            .Where(x => ids.Contains(x.Id))
            .ToList();
    }
}