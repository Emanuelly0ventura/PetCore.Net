using PetCore.Application.Interfaces;
using PetCore.Application.DTOs;
using PetCore.Domain.Entities;
using PetCore.Infrastructure.Persistence;
using PetCore.Infrastructure.Persistence.Pagination;
using Microsoft.EntityFrameworkCore;

namespace PetCore.Infrastructure.Repositories;

public class ProtocoloRepository(PetCoreContext context) : IProtocoloRepository
{
    public IReadOnlyCollection<Protocolo> FetchAll() => context.Protocolo.OrderBy(x => x.Id).ToList();
    public PagedData<Protocolo> FetchPage(PageQuery query) => EfPagination.FetchPage(context.Protocolo.AsNoTracking(), query);
    public PagedData<Protocolo> FetchPageForVeterinarian(PageQuery query, Guid veterinarianId)
    {
        var filters = new Dictionary<string, string>(query.Filters, StringComparer.OrdinalIgnoreCase);
        filters.Remove(nameof(Protocolo.VeterinarioId));

        var scopedQuery = new PageQuery
        {
            Page = query.Page,
            PageSize = query.PageSize,
            SortBy = query.SortBy,
            Descending = query.Descending,
            Filters = filters
        };

        var visibleProtocols = context.Protocolo.AsNoTracking().Where(protocol =>
            protocol.VeterinarioId == veterinarianId ||
            (protocol.TipoRegistro == "protocolo" &&
             protocol.TutorId == null &&
             protocol.PetId == null &&
             protocol.VeterinarioId == null &&
             protocol.DataEnvio == null));

        return EfPagination.FetchPage(visibleProtocols, scopedQuery);
    }
    public Protocolo? FetchById(string id) => context.Protocolo.Find(id);
    public void Create(Protocolo protocolo) => context.Protocolo.Add(protocolo);
    public void Delete(Protocolo protocolo) => context.Protocolo.Remove(protocolo);
    public void SaveChanges() => context.SaveChanges();
}
