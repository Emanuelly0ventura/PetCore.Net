using PetCore.Domain.Entities;

namespace PetCore.Application.Interfaces;

public interface IMedicoRepository :  IRepository<Medico> {
    Medico? FetchByEmail(string email, string senha);
}