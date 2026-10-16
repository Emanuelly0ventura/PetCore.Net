using PetCore.Domain.Entities;

namespace PetCore.Application.Interfaces;

public interface ITutorRepository : IRepository<Tutor> {
    Tutor? FetchByEmail(string email, string senha);
    IReadOnlyCollection<Tutor> FetchAllById(List<Guid> ids);
}