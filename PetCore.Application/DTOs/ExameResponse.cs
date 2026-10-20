using PetCore.Domain.Entities;
using System.Text.Json.Serialization;

namespace PetCore.Application.DTOs;

public class ExameResponse
{
    public Guid Id { get;  set; }
    public string Nome { get;  set; }
    public DateOnly Data { get;  set; }
    [JsonIgnore] public string Tipo { get;  set; }
    [JsonPropertyName("tipoExame")] public string TipoMobile => Tipo;
    public Guid IdMedico { get; set; }
    [JsonPropertyName("veterinarioId")] public Guid VeterinarioId => IdMedico;
    public Guid? IdProntuario { get; set; }
    public Guid? PetId { get; set; }
    public string PetNome { get; set; }
    public Guid? TutorId { get; set; }
    public string TutorNome { get; set; }
    public string VeterinarioNome { get; set; }
    public string ArquivoSolicitacao { get; set; }
    public DateTime? DataEnvio { get; set; }

    public ExameResponse(Exame exame) {
        Id = exame.Id;
        Nome = exame.Nome;
        Data = exame.Data;
        Tipo = exame.Tipo;
        IdMedico = exame.IdMedico;
        IdProntuario = exame.IdProntuario;
        PetId = exame.PetId; PetNome = exame.PetNome; TutorId = exame.TutorId;
        TutorNome = exame.TutorNome; VeterinarioNome = exame.VeterinarioNome;
        ArquivoSolicitacao = exame.ArquivoSolicitacao; DataEnvio = exame.DataEnvio ?? exame.Data.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);

    }
}
