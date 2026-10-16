using System.ComponentModel.DataAnnotations;

namespace PetCore.Application.DTOs;

public class RelatorioDadosRequest
{
    [Required(ErrorMessage = "O campo 'Observacao' deve ser preenchido")]
    [StringLength(500, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 à 500 caracteres")]
    public string Observacao { get; set; }
}