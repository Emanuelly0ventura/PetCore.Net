using System.ComponentModel.DataAnnotations;
using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class PetDadosRequest
{
    [Required(ErrorMessage = "O campo 'UrlImg' deve ser preenchido")]
    public string UrlImg { get; set; }
    
    [Required(ErrorMessage = "O campo 'Status' deve ser preenchido")]
    public bool Status { get;  set; }
    
    public PetDadosRequest(Pet pet)
    {
        UrlImg = pet.UrlImg;
        Status = pet.Status;
    }
}