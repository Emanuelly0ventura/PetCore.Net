using PetCore.Application.Interfaces;
using PetCore.Domain.Entities;
using PetCore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PetCore.Infrastructure.Repositories;

public class HistoricoRepository : Repository<Historico>, IHistoricoRepository
{
    private readonly PetCoreContext _context;

    public HistoricoRepository(PetCoreContext context) : base(context) => _context = context;

    public IReadOnlyCollection<Historico> FetchAllByTutorId(Guid tutorId) => _context.Historico
        .Where(historico => historico.TutorId == tutorId)
        .AsNoTracking()
        .ToList();
}
