using PetCore.Domain.Entities;
using PetCore.Domain.Enums;

namespace PetCore.Application.DTOs;

public class MedicoResponse
{
    public Guid Id { get;  set; } 
    public string Nome { get;  set; } 
    public DateOnly DataNascimento { get;  set; }
    public string Telefone { get;  set; } 
    public string Email{ get;  set; }
    public GeneroSexoEnum Sexo { get;  set; }
    public string Especialidade { get;  set; }
    public List<Guid> IdRelatorios { get; set; }
    public List<Guid> IdProntuarios { get; set; }
    public List<Guid> IdExames { get; set; }
    public List<Guid> IdReceitas { get; set; }

    public MedicoResponse(Medico medico)
    {
        Id = medico.Id;
        Nome = medico.Nome;
        DataNascimento = medico.DataNascimento;
        Telefone = medico.Telefone;
        Email = medico.Email;
        Sexo = medico.Sexo;
        Especialidade = medico.Especialidade;
        
        IdRelatorios = (medico.Relatorios ?? [])
            .Select(x => x.Id)
            .ToList();
        IdProntuarios = (medico.Prontuarios ?? [])
            .Select(x => x.Id)
            .ToList();
        IdExames = (medico.Exames ?? [])
            .Select(x => x.Id)
            .ToList();
        IdReceitas = (medico.Receitas ?? [])
            .Select(x => x.Id)
            .ToList();
        
    }
}
