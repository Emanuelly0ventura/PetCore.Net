using PetCore.Domain.Entities;

namespace PetCore.Application.Interfaces;

public interface IRelatorioRepository : IRepository<Relatorio>{
    IReadOnlyCollection<Relatorio> FetchAllById(List<Guid> ids);
}