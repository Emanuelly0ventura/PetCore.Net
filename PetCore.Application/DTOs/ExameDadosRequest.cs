using System.ComponentModel.DataAnnotations;

namespace PetCore.Application.DTOs;

public class ExameDadosRequest
{
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 150 caracteres")]
    public string? Nome { get; set; }

    public DateOnly? Data { get; set; }

    [StringLength(150, MinimumLength = 2, ErrorMessage = "Tipo deve ter entre 2 e 150 caracteres")]
    public string? Tipo { get; set; }
}