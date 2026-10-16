using PetCore.Domain.Entities;

namespace PetCore.Application.Interfaces;

public interface IClinicaRepository : IRepository<Clinica> {
    IReadOnlyCollection<Clinica> FetchAllById(List<Guid> ids);
}