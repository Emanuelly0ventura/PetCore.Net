using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class ProntuarioResponse
{
    public Guid Id { get;  set; }
    public DateOnly DataEmissao { get;  set; }
    public string Descricao { get; set; }
    public Guid IdMedico { get; set; }
    public List<Guid> IdExames { get; set; }
    public List<Guid> IdReceitas { get; set; }
    public Guid IdHistorico { get; set; }
    

    public ProntuarioResponse(Prontuario prontuario)
    {
        Id = prontuario.Id;
        DataEmissao = prontuario.DataEmissao;
        Descricao = prontuario.Descricao;
        IdMedico = prontuario.IdMedico;
        IdExames = (prontuario.Exames ?? [])
            .Select(x => x.Id)
            .ToList();
        IdReceitas = (prontuario.Receitas ?? [])
            .Select(x => x.Id)
            .ToList();
        IdHistorico = prontuario.IdHistorico;
    }
}
