using PetCore.Domain.Entities;
using PetCore.Domain.Enums;

namespace PetCore.Application.DTOs;

public class PetMenuResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public DateOnly DataNasc { get; set; }
    public GeneroSexoEnum Sexo { get; set; }
    public string UrlImg { get; set; }

    public PetMenuResponse(Pet pet)
    {
        Id = pet.Id;
        Nome = pet.Nome;
        DataNasc = pet.DataNasc;
        Sexo = pet.Sexo;
        UrlImg = pet.UrlImg;
    }

}