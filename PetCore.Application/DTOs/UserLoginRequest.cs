using System.ComponentModel.DataAnnotations;

namespace PetCore.Application.DTOs;

/// <summary>
/// Login do aplicativo (Tutor ou médico)
/// </summary>
public class UserLoginRequest
{
    [Required(ErrorMessage = "O campo 'Email' deve ser preenchido")]
    [EmailAddress]
    [StringLength(200, ErrorMessage = "Email deve ter no máximo 200 caracteres")]
    public string Email { get; set; }
        
    [Required(ErrorMessage = "O campo 'Senha' deve ser preenchido")]
    [StringLength(30, MinimumLength = 10, ErrorMessage = "Senha deve ter entre 10 à 30 caracteres")]
    public string Senha { get; set; }
}