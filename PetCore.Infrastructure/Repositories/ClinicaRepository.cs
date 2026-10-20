using PetCore.Application.Interfaces;
using PetCore.Domain.Entities;
using PetCore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PetCore.Infrastructure.Repositories;

public class ClinicaRepository (PetCoreContext context) : Repository<Clinica>(context), IClinicaRepository
{
    public new IReadOnlyCollection<Clinica> FetchAll() => Set.Include(x => x.Endereco).ToList();

    public new Clinica? FetchById(Guid id) => Set.Include(x => x.Endereco).FirstOrDefault(x => x.Id == id);

    public IReadOnlyCollection<Clinica> FetchAllById(List<Guid> ids)
    {
        return Set
            .Where(x => ids.Contains(x.Id))
            .ToList();
    }
}
