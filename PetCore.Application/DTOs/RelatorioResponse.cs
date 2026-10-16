using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class RelatorioResponse
{
    public Guid Id { get;  set; }
    
    public string Observacao { get; set; }
    public Guid IdHistorico { get; set; }
    public Guid IdMedicoResponsavel { get; set; }

    public RelatorioResponse(Relatorio relatorio)
    {
        Id = relatorio.Id;
        IdHistorico = relatorio.IdHistorico;
        IdMedicoResponsavel = relatorio.IdMedicoResponsavel;
        Observacao = relatorio.Observacao;
    }
}