using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using PetCore.Application.DTOs;
using PetCore.Application.Interfaces;
using PetCore.Domain.Entities;
using PetCore.Infrastructure.Persistence.Mongo;

namespace PetCore.Infrastructure.Repositories;

/// <summary>Repositório Mongo para históricos; demais recursos relacionais usam EF Core.</summary>
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

    public PagedData<Historico> FetchPage(PageQuery query)
    {
        var builder = Builders<HistoricoDocument>.Filter;
        var filter = builder.Empty;
        foreach (var (key, value) in query.Filters)
        {
            var field = key.ToLowerInvariant() switch
            {
                "petnome" => nameof(HistoricoDocument.PetNome),
                "tutornome" => nameof(HistoricoDocument.TutorNome),
                "tiporegistro" => nameof(HistoricoDocument.TipoRegistro),
                "statusmobile" => nameof(HistoricoDocument.StatusMobile),
                "status" when bool.TryParse(value, out _) => nameof(HistoricoDocument.Status),
                "idpet" when Guid.TryParse(value, out _) => nameof(HistoricoDocument.IdPet),
                "tutorid" when Guid.TryParse(value, out _) => nameof(HistoricoDocument.TutorId),
                _ => string.Empty
            };
            if (field is nameof(HistoricoDocument.PetNome) or nameof(HistoricoDocument.TutorNome) or nameof(HistoricoDocument.TipoRegistro) or nameof(HistoricoDocument.StatusMobile))
                filter &= builder.Regex(field, new BsonRegularExpression(Regex.Escape(value), "i"));
            else if (field == nameof(HistoricoDocument.Status) && bool.TryParse(value, out var status))
                filter &= builder.Eq(field, status);
            else if (field == nameof(HistoricoDocument.IdPet) && Guid.TryParse(value, out var petId))
                filter &= builder.Eq(field, petId);
            else if (field == nameof(HistoricoDocument.TutorId) && Guid.TryParse(value, out var tutorId))
                filter &= builder.Eq(field, tutorId);
        }

        var sortField = query.SortBy?.ToLowerInvariant() switch
        {
            "petnome" => nameof(HistoricoDocument.PetNome),
            "tutornome" => nameof(HistoricoDocument.TutorNome),
            "tiporegistro" => nameof(HistoricoDocument.TipoRegistro),
            "statusmobile" => nameof(HistoricoDocument.StatusMobile),
            "status" => nameof(HistoricoDocument.Status),
            "idpet" => nameof(HistoricoDocument.IdPet),
            "tutorid" => nameof(HistoricoDocument.TutorId),
            "datasolicitacao" => nameof(HistoricoDocument.DataSolicitacao),
            "id" => nameof(HistoricoDocument.Id),
            _ => nameof(HistoricoDocument.DataAbertura)
        };
        var sort = query.Descending ? Builders<HistoricoDocument>.Sort.Descending(sortField) : Builders<HistoricoDocument>.Sort.Ascending(sortField);
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);
        var offset = (int)Math.Min((long)(page - 1) * pageSize, int.MaxValue);
        var total = (int)Math.Min(_collection.CountDocuments(filter), int.MaxValue);
        var documents = _collection.Find(filter).Sort(sort).Skip(offset).Limit(pageSize).ToList();
        var items = documents.Select(ToDomain).ToList();
        return new PagedData<Historico>(items, page, pageSize, total, (int)Math.Ceiling(total / (double)pageSize));
    }

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
            IdPet = historico.IdPet,
            TutorId = historico.TutorId,
            TutorNome = historico.TutorNome,
            PetNome = historico.PetNome,
            TipoRegistro = historico.TipoRegistro,
            StatusMobile = historico.StatusMobile,
            DataSolicitacao = historico.DataSolicitacao
        });

    public void Delete(Historico historico) =>
        _collection.DeleteOne(document => document.Id == historico.Id);

    // MongoDB persiste cada operação imediatamente; mantido para compatibilidade com o contrato atual.
    public void SaveChanges() { }

    private static Historico ToDomain(HistoricoDocument document)
    {
        if (document.IdPet is null || document.IdPet == Guid.Empty)
            throw new InvalidOperationException($"O histórico {document.Id} não contém um IdPet válido.");

        var historico = Historico.Restore(document.Id, DateOnly.FromDateTime(document.DataAbertura), document.Status, document.IdPet.Value);
        historico.SetMobileData(document.TutorId, document.TutorNome, document.PetNome, document.TipoRegistro, document.StatusMobile, document.DataSolicitacao);
        return historico;
    }
}

