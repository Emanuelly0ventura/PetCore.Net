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
    public Guid? TutorId { get; set; }
    public string TutorNome { get; set; } = string.Empty;
    public string PetNome { get; set; } = string.Empty;
    public string TipoRegistro { get; set; } = "historico";
    public string StatusMobile { get; set; } = "Ativo";
    public DateTime? DataSolicitacao { get; set; }
}
