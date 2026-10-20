using PetCore.Domain.Entities;
using PetCore.Domain.Enums;
using System.Text.Json.Serialization;

namespace PetCore.Application.DTOs;

public class MedicoResponse
{
    public Guid Id { get;  set; } 
    public string Nome { get;  set; } 
    [JsonIgnore] public DateOnly DataNascimento { get;  set; }
    public string Nascimento => DataNascimento.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
    public string Telefone { get;  set; } 
    public string Email{ get;  set; }
    [JsonIgnore] public GeneroSexoEnum Sexo { get;  set; }
    public string Genero => Sexo == GeneroSexoEnum.F ? "Feminino" : "Masculino";
    [JsonIgnore] public string Especialidade { get;  set; }
    public string Especializacao => Especialidade;
    public string? Clinica { get; set; }
    public string? NomeClinica { get; set; }
    public string? Cnpj { get; set; }
    public string? Cep { get; set; }
    public string? Complemento { get; set; }
    [JsonPropertyName("fotoUrl")] public string UrlImg { get; set; }
    public string TipoPerfil => "veterinario";
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
        Clinica = medico.Clinica; NomeClinica = medico.NomeClinica; Cnpj = medico.CnpjClinica;
        Cep = medico.CepClinica; Complemento = medico.ComplementoClinica;
        UrlImg = medico.UrlImg;
        
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
