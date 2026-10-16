using PetCore.Application.Interfaces;
using PetCore.Domain.Entities;
using PetCore.Infrastructure.Persistence;

namespace PetCore.Infrastructure.Repositories;

public class MedicamentoRepository (PetCoreContext context) : Repository<Medicamento>(context), IMedicamentoRepository
{
    public IReadOnlyCollection<Medicamento> FetchAllById(List<Guid> ids)
    {
        return Set
            .Where(x => ids.Contains(x.Id))
            .ToList();
    }
}