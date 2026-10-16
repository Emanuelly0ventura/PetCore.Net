using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class ReceitaResponse
{
    public Guid Id { get;  set; }
    public string Nome { get;  set; }
    public DateOnly Validade { get;  set; }
    public Guid IdMedicoResponsavel { get;  set; }
    public List<Guid> IdMedicamentos { get; set; }

    public ReceitaResponse(Receita receita)
    {
        Id = receita.Id;
        Nome = receita.Nome;
        Validade = receita.Validade;
        IdMedicoResponsavel = receita.IdMedicoResponsavel;
        IdMedicamentos = (receita.Medicamentos ?? [])
            .Select(x => x.Id)
            .ToList();
    }
}
