using PetCore.Domain.Entities;

namespace PetCore.Application.Interfaces;

public interface IProntuarioRepository : IRepository<Prontuario> {
    IReadOnlyCollection<Prontuario> FetchAllById(List<Guid> ids);
}