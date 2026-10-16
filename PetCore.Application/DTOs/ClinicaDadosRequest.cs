using System.ComponentModel.DataAnnotations;

namespace PetCore.Application.DTOs;

public class ClinicaDadosRequest
{
    [Required(ErrorMessage = "O campo 'Nome' deve ser preenchido")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 à 150 caracteres")]
    public string Nome { get;  set; }

    [Required(ErrorMessage = "O campo 'Id do Endereço' deve ser preenchido")]
    public Guid IdEndereco { get;  set; }
}