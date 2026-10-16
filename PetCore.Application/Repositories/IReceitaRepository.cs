using PetCore.Domain.Entities;

namespace PetCore.Application.Interfaces;

public interface IReceitaRepository
{
    void Create(Receita receita);
    void Delete(Receita receita);
    IReadOnlyCollection<Receita> FetchAll();
    Receita? FetchById(Guid id);
    IReadOnlyCollection<Receita> FetchAllById(List<Guid> ids);
    void SaveChanges();
    
}