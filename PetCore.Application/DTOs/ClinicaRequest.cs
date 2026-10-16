using System.ComponentModel.DataAnnotations;
using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class ClinicaRequest
{
    [Required(ErrorMessage = "O campo 'Nome' deve ser preenchido")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 à 150 caracteres")]
    public string Nome { get;  set; }
    
    [Required(ErrorMessage = "O campo 'Cnpj' deve ser preenchido")]
    [StringLength(14, MinimumLength = 14, ErrorMessage = "Cnpj deve ter 14 caracteres")]
    public string Cnpj { get;  set; }

    [Required(ErrorMessage = "O campo 'Id do Endereço' deve ser preenchido")]
    public Guid IdEndereco { get;  set; }
    
    public Clinica ToDomain() =>
        new(Nome,Cnpj,IdEndereco);
}