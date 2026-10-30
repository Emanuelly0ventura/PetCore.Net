using PetCore.Application.Interfaces;
using PetCore.Application.DTOs;
using PetCore.Domain.Entities;
using PetCore.Infrastructure.Persistence;
using PetCore.Infrastructure.Persistence.Pagination;
using Microsoft.EntityFrameworkCore;

namespace PetCore.Infrastructure.Repositories;

public class PetRepository(PetCoreContext context) : Repository<Pet>(context), IPetRepository
{
    protected override IQueryable<Pet> Queryable => Set.Include(x => x.Tutores).AsNoTracking();

    public new IReadOnlyCollection<Pet> FetchAll() => Set.Include(x => x.Tutores).ToList();

    public override PagedData<Pet> FetchPage(PageQuery query)
    {
        var source = Queryable;
        var filters = query.Filters;
        if (filters.TryGetValue("tutorId", out var tutorValue) && Guid.TryParse(tutorValue, out var tutorId))
        {
            source = source.Where(pet => pet.Tutores.Any(tutor => tutor.Id == tutorId));
            filters = filters.Where(item => !item.Key.Equals("tutorId", StringComparison.OrdinalIgnoreCase))
                .ToDictionary(item => item.Key, item => item.Value, StringComparer.OrdinalIgnoreCase);
        }
        return EfPagination.FetchPage(source, new PageQuery { Page = query.Page, PageSize = query.PageSize, SortBy = query.SortBy, Descending = query.Descending, Filters = filters });
    }

    public PagedData<Pet> FetchMenuPage(PageQuery query) => FetchPage(query);

    public new Pet? FetchById(Guid id) => Set.Include(x => x.Tutores).FirstOrDefault(x => x.Id == id);

    public IReadOnlyCollection<Pet> FetchMenuAll()
    {
        return Set.ToList();
    }

    public IReadOnlyCollection<Pet> FetchAllById(List<Guid> ids)
    {
        return Set
            .Where(x => ids.Contains(x.Id))
            .ToList();
    }
}
