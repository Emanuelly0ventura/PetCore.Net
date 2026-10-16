using PetCore.Application.Interfaces;
using PetCore.Domain.Entities;
using PetCore.Infrastructure.Persistence;

namespace PetCore.Infrastructure.Repositories;

public class EnderecoRepository (PetCoreContext context) : Repository<Endereco>(context), IEnderecoRepository {
}