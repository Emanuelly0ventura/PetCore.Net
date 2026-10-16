using PetCore.Domain.Entities;

namespace PetCore.Application.Interfaces;

public interface IPetRepository
{
    void Create(Pet pet);
    void Patch(Pet pet);
    void Delete(Pet pet);
    IReadOnlyCollection<Pet> FetchAll();
    IReadOnlyCollection<Pet> FetchMenuAll();
    Pet? FetchById(Guid id);
    IReadOnlyCollection<Pet> FetchAllById(List<Guid> ids);
    void SaveChanges();
}