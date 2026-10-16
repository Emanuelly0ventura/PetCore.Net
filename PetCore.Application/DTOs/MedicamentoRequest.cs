using System.ComponentModel.DataAnnotations;
using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class MedicamentoRequest
{
    [Required(ErrorMessage = "O campo 'Nome' deve ser preenchido")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 à 150 caracteres")]
    public string Nome { get;  set; }
    
    [Required(ErrorMessage = "O campo 'Dosagem' deve ser preenchido")]
    [StringLength(10, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 à 10 caracteres")]
    public string Dosagem { get;  set; }
    
    [Required(ErrorMessage = "O campo 'Instrução' deve ser preenchido")]
    [StringLength(300, MinimumLength = 2, ErrorMessage = "Instrucao deve ter entre 2 à 300 caracteres")]
    public string Instrucao { get; set; }
    
    public Medicamento ToDomain() =>
        new (Nome, Dosagem, Instrucao);
}