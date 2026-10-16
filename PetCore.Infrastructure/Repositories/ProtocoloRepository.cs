using PetCore.Application.Interfaces;
using PetCore.Domain.Entities;
using PetCore.Infrastructure.Persistence;

namespace PetCore.Infrastructure.Repositories;

public class ProtocoloRepository(PetCoreContext context) : IProtocoloRepository
{
    public IReadOnlyCollection<Protocolo> FetchAll() => context.Protocolo.OrderBy(x => x.Id).ToList();
    public Protocolo? FetchById(string id) => context.Protocolo.Find(id);
    public void Create(Protocolo protocolo) => context.Protocolo.Add(protocolo);
    public void Delete(Protocolo protocolo) => context.Protocolo.Remove(protocolo);
    public void SaveChanges() => context.SaveChanges();
}
