using MongoDB.Bson.Serialization.Attributes;

namespace PetCore.Infrastructure.Persistence.Mongo;

/// <summary>Documento MongoDB usado para persistir o histórico clínico.</summary>
public sealed class HistoricoDocument
{
    [BsonId]
    public Guid Id { get; set; }

    public DateTime DataAbertura { get; set; }

    public bool Status { get; set; }

    public Guid? IdPet { get; set; }
}
