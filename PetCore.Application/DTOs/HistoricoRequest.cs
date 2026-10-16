using System.ComponentModel.DataAnnotations;
using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class HistoricoRequest
{
    [Required(ErrorMessage = "O campo 'Data' deve ser preenchido")]
    public DateOnly Data { get; set; }

    [Required(ErrorMessage = "O campo 'IdPet' deve ser preenchido")]
    public Guid IdPet { get; set; }
    
    public Historico ToDomain() =>
        new(Data, IdPet);
}