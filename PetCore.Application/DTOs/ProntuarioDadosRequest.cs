using System.ComponentModel.DataAnnotations;

namespace PetCore.Application.DTOs;

public class ProntuarioDadosRequest
{
    [Required(ErrorMessage = "O campo 'Descricao' deve ser preenchido")]
    [StringLength(300, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 à 300 caracteres")]
    public string Descricao { get;  set; }
}