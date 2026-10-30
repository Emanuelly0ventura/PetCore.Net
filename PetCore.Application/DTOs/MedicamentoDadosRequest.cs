using System.ComponentModel.DataAnnotations;

namespace PetCore.Application.DTOs;

public class MedicamentoDadosRequest
{
    [StringLength(10, MinimumLength = 3, ErrorMessage = "Dosagem deve ter entre 3 e 10 caracteres")]
    public string? Dosagem { get; set; }

    [StringLength(300, MinimumLength = 2, ErrorMessage = "Instrução deve ter entre 2 e 300 caracteres")]
    public string? Instrucao { get; set; }
}