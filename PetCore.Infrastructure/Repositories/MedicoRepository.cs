using PetCore.Application.Interfaces;
using PetCore.Domain.Entities;
using PetCore.Infrastructure.Persistence;

namespace PetCore.Infrastructure.Repositories;

public class MedicoRepository(PetCoreContext context) : Repository<Medico>(context), IMedicoRepository
{
    public Medico? FetchByEmail(string email, string senha)
    {
        return context.Set<Medico>()
            .FirstOrDefault(x =>
                x.Email == email &&
                x.Senha == senha);
    }
}