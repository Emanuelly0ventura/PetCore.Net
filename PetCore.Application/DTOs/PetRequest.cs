using System.ComponentModel.DataAnnotations;
using PetCore.Domain.Entities;
using PetCore.Domain.Enums;

namespace PetCore.Application.DTOs;

public class PetRequest
{
    [Required(ErrorMessage = "O campo 'Nome' deve ser preenchido")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 à 150 caracteres")]
    public string Nome { get;  set; }
    
    [Required(ErrorMessage = "O campo 'Espécie' deve ser preenchido")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Espécie deve ter entre 2 à 150 caracteres")]
    public string Especie { get;  set; }
    
    [Required(ErrorMessage = "O campo 'Raça' deve ser preenchido")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Raça deve ter entre 2 à 150 caracteres")]
    public string Raca { get;  set; }
    
    [Required(ErrorMessage = "O campo 'Data de Nascimento' deve ser preenchido")]
    public DateOnly DataNasc { get;  set; }
    
    [Required(ErrorMessage = "O campo 'Pelagem' deve ser preenchido")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Pelagem deve ter entre 2 à 150 caracteres")]
    public string Pelagem { get;  set; }
    
    [Required(ErrorMessage = "O campo 'Porte' deve ser preenchido")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Porte deve ter entre 2 à 150 caracteres")]
    public string Porte { get;  set; }
    
    [Required(ErrorMessage = "O campo 'Gênero' deve ser preenchido")]
    public GeneroSexoEnum Sexo { get;  set; }
    
    public List<Guid> IdTutores { get; set; } = [];
    
    [Required(ErrorMessage = "O campo 'UrlImg' deve ser preenchido")]
    [StringLength(30, MinimumLength = 2, ErrorMessage = "UrlImg deve ter entre 2 à 30 caracteres")]
    public string UrlImg { get;  set; }

    public bool Status { get; set; } = true;

    public Pet ToDomain() =>
        new(Nome, Especie, Raca, DataNasc, Pelagem, Porte, Sexo, Status, UrlImg);
}
