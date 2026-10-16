using System.ComponentModel.DataAnnotations;
using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class ReceitaRequest
{
    [Required(ErrorMessage = "O campo 'Nome' deve ser preenchido")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 à 150 caracteres")]
    public string Nome { get; set; }
    
    [Required(ErrorMessage = "O campo 'Validade' deve ser preenchido")]
    public DateOnly Validade { get; set; }
    
    [Required(ErrorMessage = "O campo 'Id Medico' deve ser preenchido")]
    public Guid IdMedicoResponsavel { get; set; }
    
    [Required(ErrorMessage = "O campo 'Id Prontuario' deve ser preenchido")]
    public Guid IdProntuario { get; set; }
    
    public List<Guid> IdMedicamentos { get; set; } = [];

    public Receita ToDomain() =>
        new (Nome, Validade, IdMedicoResponsavel, IdProntuario);
}
