using PetCore.Application.Interfaces;
using PetCore.Domain.Entities;
using PetCore.Infrastructure.Persistence;

namespace PetCore.Infrastructure.Repositories;

public class HistoricoRepository (PetCoreContext context) : Repository<Historico>(context), IHistoricoRepository {
}