using System.ComponentModel.DataAnnotations;
using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class UserDadosRequest
{
    [Required(ErrorMessage = "O campo 'Email' deve ser preenchido")]
    [EmailAddress]
    [StringLength(200, ErrorMessage = "Email deve ter no máximo 200 caracteres")]
    public string Email { get; set; }
        
    [Required(ErrorMessage = "O campo 'Telefone' deve ser preenchido")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Telefone deve ter 11 caracteres")]
    public string Telefone { get; set; }
    
    [Required(ErrorMessage = "O campo 'Senha' deve ser preenchido")]
    [StringLength(30, MinimumLength = 10, ErrorMessage = "Senha deve ter entre 10 à 30 caracteres")]
    public string Senha { get; set; }
    
    [Required(ErrorMessage = "O campo 'UrlImg' deve ser preenchido")]
    [StringLength(30, MinimumLength = 2)]
    public string UrlImg { get; set; }
    
}