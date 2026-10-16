using PetCore.Domain.Entities;

namespace PetCore.Application.Interfaces;

public interface IHistoricoRepository{
    void Create(Historico historico);
    void Delete(Historico historico);
    IReadOnlyCollection<Historico> FetchAll();
    Historico? FetchById(Guid id);
    void SaveChanges();
}