using PetCore.Application.Interfaces;
using PetCore.Domain.Entities;
using PetCore.Infrastructure.Persistence;

namespace PetCore.Infrastructure.Repositories;

public class ClinicaRepository (PetCoreContext context) : Repository<Clinica>(context), IClinicaRepository
{
    public IReadOnlyCollection<Clinica> FetchAllById(List<Guid> ids)
    {
        return Set
            .Where(x => ids.Contains(x.Id))
            .ToList();
    }
}