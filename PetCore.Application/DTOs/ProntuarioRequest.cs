using System.ComponentModel.DataAnnotations;
using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class ProntuarioRequest
{
    [Required(ErrorMessage = "O campo 'Data' deve ser preenchido")]
    public DateOnly DataEmissao { get;  set; }
    
    [Required(ErrorMessage = "O campo 'Descricao' deve ser preenchido")]
    [StringLength(300, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 à 300 caracteres")]
    public string Descricao { get;  set; }

    [Required(ErrorMessage = "O campo 'IdMedico' deve ser preenchido")]
    public Guid IdMedico { get; set; }
    
    [Required(ErrorMessage = "O campo 'IdHistorico' deve ser preenchido")]
    public Guid IdHistorico { get; set; }
    
    public List<Guid> IdExames { get; set; } = [];

    public List<Guid> IdReceitas { get; set; } = [];
    

    
    public Prontuario ToDomain() =>
        new(DataEmissao, Descricao, IdMedico, IdHistorico);
}
