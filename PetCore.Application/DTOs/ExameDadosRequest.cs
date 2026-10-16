using System.ComponentModel.DataAnnotations;

namespace PetCore.Application.DTOs;

public class ExameDadosRequest
{
    [Required(ErrorMessage = "O campo 'Nome' deve ser preenchido")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 à 150 caracteres")]
    public string Nome { get;  set; }
    
    [Required(ErrorMessage = "O campo 'Data' deve ser preenchido")]
    public DateOnly Data { get;  set; }
    
    [Required(ErrorMessage = "O campo 'Tipo' deve ser preenchido")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Tipo deve ter entre 2 à 150 caracteres")]
    public string Tipo{ get;  set; }
}