using System.ComponentModel.DataAnnotations;
using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class ExameRequest
{
    [Required(ErrorMessage = "O campo 'Nome' deve ser preenchido")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 à 150 caracteres")]
    public string Nome { get;  set; }
    
    [Required(ErrorMessage = "O campo 'Data' deve ser preenchido")]
    public DateOnly Data { get;  set; }
    
    [Required(ErrorMessage = "O campo 'Tipo' deve ser preenchido")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Tipo deve ter entre 2 à 150 caracteres")]
    public string Tipo{ get;  set; }

    [Required(ErrorMessage = "O campo 'IdMedico' deve ser preenchido")]
    public Guid IdMedico { get; set; }
    
    [Required(ErrorMessage = "O campo 'IdProntuario' deve ser preenchido")]
    public Guid IdProntuario { get; set; }

    public Exame ToDomain() =>
        new(Nome, Data, Tipo, IdMedico, IdProntuario);
}