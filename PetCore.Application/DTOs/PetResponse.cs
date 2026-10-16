using PetCore.Domain.Entities;
using PetCore.Domain.Enums;

namespace PetCore.Application.DTOs;

public class PetResponse
{
    public Guid Id { get;  set; }
    public string Nome { get;  set; }
    public string Especie { get;  set; }
    public string Raca { get;  set; }
    public DateOnly DataNasc { get;  set; }
    public string Pelagem { get;  set; }
    public string Porte { get;  set; } 
    public GeneroSexoEnum Sexo { get;  set; }
    public bool? Status { get;  set; }
    public string UrlImg { get; set; }

    public PetResponse(Pet pet)
    {
        Id = pet.Id;
        Nome = pet.Nome;
        Especie = pet.Especie;
        Raca = pet.Raca;
        DataNasc = pet.DataNasc;
        Pelagem = pet.Pelagem;
        Porte = pet.Porte;
        Sexo = pet.Sexo;
        Status = pet.Status; 
        UrlImg = pet.UrlImg;
    }
}
