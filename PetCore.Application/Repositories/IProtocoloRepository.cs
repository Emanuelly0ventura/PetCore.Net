using PetCore.Application.DTOs;
using PetCore.Domain.Entities;

namespace PetCore.Application.Interfaces;

public interface IProtocoloRepository
{
    IReadOnlyCollection<Protocolo> FetchAll();
    PagedData<Protocolo> FetchPage(PageQuery query);
    PagedData<Protocolo> FetchPageForVeterinarian(PageQuery query, Guid veterinarianId);
    Protocolo? FetchById(string id);
    void Create(Protocolo protocolo);
    void Delete(Protocolo protocolo);
    void SaveChanges();
}
