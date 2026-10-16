using PetCore.Domain.Entities;

namespace PetCore.Application.Interfaces;

public interface IMedicamentoRepository : IRepository<Medicamento> {
    IReadOnlyCollection<Medicamento> FetchAllById(List<Guid> ids);
}