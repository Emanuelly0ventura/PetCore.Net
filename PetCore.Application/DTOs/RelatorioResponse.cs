using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class RelatorioResponse
{
    public Guid Id { get;  set; }
    
    public string Observacao { get; set; }
    public Guid? IdHistorico { get; set; }
    public Guid IdMedicoResponsavel { get; set; }
    public Guid? PetId { get; set; }
    public string PetNome { get; set; }
    public Guid? TutorId { get; set; }
    public string TutorNome { get; set; }
    public string ArquivoHistorico { get; set; }
    public string Prontuarios { get; set; }

    public RelatorioResponse(Relatorio relatorio)
    {
        Id = relatorio.Id;
        IdHistorico = relatorio.IdHistorico;
        IdMedicoResponsavel = relatorio.IdMedicoResponsavel;
        Observacao = relatorio.Observacao;
        PetId = relatorio.PetId; PetNome = relatorio.PetNome;
        TutorId = relatorio.TutorId; TutorNome = relatorio.TutorNome;
        ArquivoHistorico = relatorio.ArquivoHistorico; Prontuarios = relatorio.Prontuarios;
    }
}
