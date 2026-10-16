using PetCore.Application.Interfaces;
using PetCore.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;


namespace PetCore.Infrastructure.Repositories;

public class Repository<T>(PetCoreContext context)  : IRepository<T> where T : class
{
    protected DbSet<T> Set => context.Set<T>();
    public IReadOnlyCollection<T> FetchAll()
    {
        return Set.ToList();
    }

    public T? FetchById(Guid id)
    {
        //select * from T where _id = id
        return Set.Find(id);
    }

    public void Create(T entity)
    {
        Set.Add(entity);
    }

    public void Patch(T entity)
    {
        Set.Update(entity);
    }

    public void Delete(T entity)
    {
        Set.Remove(entity);
    }

    public void SaveChanges()
    {
        context.SaveChanges();
    }
}