using PetCore.Domain.Entities;

namespace PetCore.Application.Interfaces;

public interface IProtocoloRepository
{
    IReadOnlyCollection<Protocolo> FetchAll();
    Protocolo? FetchById(string id);
    void Create(Protocolo protocolo);
    void Delete(Protocolo protocolo);
    void SaveChanges();
}
