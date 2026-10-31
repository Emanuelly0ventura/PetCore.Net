using PetCore.Application.Interfaces;
using PetCore.Domain.Entities;
using PetCore.Infrastructure.Persistence;

namespace PetCore.Infrastructure.Repositories;

public class MedicoRepository : Repository<Medico>, IMedicoRepository
{
    public MedicoRepository(PetCoreContext context) : base(context) { }
    public Medico? FetchByEmail(string email)
    {
        return Set.FirstOrDefault(x => x.Email == email);
    }
}
