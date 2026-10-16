using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class EnderecoResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Cep { get; set; } = string.Empty;
    public string? Complemento { get; set; }
    public Guid IdClinica { get; set; }

    public EnderecoResponse(Endereco endereco)
    {
        Id = endereco.Id;
        Cep = endereco.Cep;
        Complemento = endereco.Complemento;
        IdClinica = endereco.Clinica?.Id ?? Guid.Empty;
    }
    
}   
