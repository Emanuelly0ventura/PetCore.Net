using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class ClinicaResponse
{
    public Guid Id { get; set; }
    public string Nome { get;  set; }
    public string Cnpj { get;  set; } 
    public Guid IdEndereco { get;  set; }
    public string? Cep { get; set; }
    public string? Complemento { get; set; }

    public ClinicaResponse(Clinica clinica)
    {
        Id = clinica.Id;
        Nome = clinica.Nome;
        Cnpj = clinica.Cnpj;
        IdEndereco = clinica.IdEndereco;
        Cep = clinica.Endereco?.Cep;
        Complemento = clinica.Endereco?.Complemento;
    }
}
