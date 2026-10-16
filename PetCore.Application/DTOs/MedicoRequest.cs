using System.ComponentModel.DataAnnotations;
using PetCore.Domain.Entities;
using PetCore.Domain.Enums;

namespace PetCore.Application.DTOs;

public class MedicoRequest
{
    [Required(ErrorMessage = "O campo 'Nome' deve ser preenchido")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 à 150 caracteres")]
    public string Nome { get; set; }
    
    [Required(ErrorMessage = "O campo 'Data de Nascimento' deve ser preenchido")]
    public DateOnly DataNascimento { get; set; }
    
    [Required(ErrorMessage = "O campo 'Telefone' deve ser preenchido")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Telefone deve ter 11 caracteres")]
    public string Telefone { get; set; }
    
    [Required(ErrorMessage = "O campo 'Email' deve ser preenchido")]
    [EmailAddress]
    [StringLength(200, ErrorMessage = "Email deve ter no máximo 200 caracteres")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "O campo 'Gênero' deve ser preenchido")]
    public GeneroSexoEnum Sexo { get; set; }
    
    [Required(ErrorMessage = "O campo 'Senha' deve ser preenchido")]
    [StringLength(30, MinimumLength = 10, ErrorMessage = "Senha deve ter entre 10 à 30 caracteres")]
    public string Senha { get; set; }
    
    [Required(ErrorMessage = "O campo 'Especialidade' deve ser preenchido")]
    [StringLength(200, ErrorMessage = "Especialidade deve ter no máximo 200 caracteres")]
    public string Especialidade { get; set; }

    public Medico ToDomain() =>
        new(Nome, DataNascimento, Telefone, Email, Sexo, Senha, Especialidade);
}