using PetCore.Domain.Entities;

namespace PetCore.Application.Interfaces;

public interface IExameRepository : IRepository<Exame> {    
    IReadOnlyCollection<Exame> FetchAllById(List<Guid> ids);
}