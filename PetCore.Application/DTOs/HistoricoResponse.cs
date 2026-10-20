using PetCore.Domain.Entities;
using System.Text.Json.Serialization;

namespace PetCore.Application.DTOs;

public class HistoricoResponse
{
    public Guid Id { get; set; }
    [JsonIgnore] public bool Status { get; set; }
    [JsonPropertyName("status")] public string StatusMobile { get; set; }
    public List<Guid> IdProntuarios { get; set; }
    [JsonIgnore] public Guid? IdPet { get; set; }
    public Guid? PetId { get; set; }
    public Guid? TutorId { get; set; }
    public string TutorNome { get; set; }
    public string PetNome { get; set; }
    public string TipoRegistro { get; set; }
    public DateTime? DataSolicitacao { get; set; }

    public HistoricoResponse(Historico historico)
    {
        Id = historico.Id;
        Status = historico.Status;
        IdProntuarios = (historico.Prontuarios ?? [])
            .Select(x => x.Id)
            .ToList();
        IdPet = historico.IdPet;
        PetId = historico.IdPet;
        TutorId = historico.TutorId;
        TutorNome = historico.TutorNome;
        PetNome = historico.PetNome;
        TipoRegistro = historico.TipoRegistro;
        DataSolicitacao = historico.DataSolicitacao;
        StatusMobile = historico.StatusMobile;
    }
}
