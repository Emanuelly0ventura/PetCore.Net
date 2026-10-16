using System.ComponentModel.DataAnnotations;
using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class EnderecoRequest
{
    [Required(ErrorMessage = "O campo 'Cep' deve ser preenchido")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "Cep deve ter 8 caracteres")]
    public string Cep { get; set; }
    
    public string Complemento { get; set; }
    
    public Endereco ToDomain() =>
        new(Cep, Complemento);
}