using MongoDB.Driver;
using PetCore.Application.Interfaces;
using PetCore.Domain.Entities;
using PetCore.Infrastructure.Persistence.Mongo;

namespace PetCore.Infrastructure.Repositories;

/// <summary>Repositório Mongo para históricos; demais recursos continuam no PostgreSQL/EF Core.</summary>
public sealed class MongoHistoricoRepository : IHistoricoRepository
{
    private readonly IMongoCollection<HistoricoDocument> _collection;

    public MongoHistoricoRepository(IMongoCollection<HistoricoDocument> collection) => _collection = collection;

    public IReadOnlyCollection<Historico> FetchAll() =>
        _collection.Find(FilterDefinition<HistoricoDocument>.Empty)
            .SortBy(document => document.DataAbertura)
            .ToList()
            .Select(ToDomain)
            .ToList();

    public Historico? FetchById(Guid id)
    {
        var document = _collection.Find(item => item.Id == id).FirstOrDefault();
        return document is null ? null : ToDomain(document);
    }

    public void Create(Historico historico) =>
        _collection.InsertOne(new HistoricoDocument
        {
            Id = historico.Id,
            DataAbertura = historico.DataAbertura.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc),
            Status = historico.Status,
            IdPet = historico.IdPet
        });

    public void Delete(Historico historico) =>
        _collection.DeleteOne(document => document.Id == historico.Id);

    // MongoDB persiste cada operação imediatamente; mantido para compatibilidade com o contrato atual.
    public void SaveChanges() { }

    private static Historico ToDomain(HistoricoDocument document)
    {
        if (document.IdPet is null || document.IdPet == Guid.Empty)
            throw new InvalidOperationException($"O histórico {document.Id} não contém um IdPet válido.");

        return Historico.Restore(document.Id, DateOnly.FromDateTime(document.DataAbertura), document.Status, document.IdPet.Value);
    }
}
