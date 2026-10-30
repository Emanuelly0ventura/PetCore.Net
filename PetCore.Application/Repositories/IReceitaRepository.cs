using PetCore.Domain.Entities;
using PetCore.Application.DTOs;

namespace PetCore.Application.Interfaces;

public interface IReceitaRepository
{
    void Create(Receita receita);
    void Delete(Receita receita);
    IReadOnlyCollection<Receita> FetchAll();
    PagedData<Receita> FetchPage(PageQuery query);
    Receita? FetchById(Guid id);
    IReadOnlyCollection<Receita> FetchAllById(List<Guid> ids);
    void SaveChanges();
    
}
