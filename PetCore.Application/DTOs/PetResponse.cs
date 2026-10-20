using PetCore.Domain.Entities;
using PetCore.Domain.Enums;
using System.Text.Json.Serialization;

namespace PetCore.Application.DTOs;

public class PetResponse
{
    public Guid Id { get;  set; }
    public string Nome { get;  set; }
    public string Especie { get;  set; }
    public string Raca { get;  set; }
    [JsonIgnore]
    public DateOnly DataNasc { get;  set; }
    public string Nascimento => DataNasc.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
    public string Pelagem { get;  set; }
    public string Porte { get;  set; } 
    [JsonIgnore]
    public GeneroSexoEnum Sexo { get;  set; }
    [JsonPropertyName("sexo")]
    public string SexoMobile => Sexo == GeneroSexoEnum.M ? "Macho" : "Fêmea";
    public bool? Status { get;  set; }
    public string UrlImg { get; set; }
    public Guid? TutorId { get; set; }
    public bool ObitoInformado { get; set; }
    public string ComedouroStatus { get; set; }

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
        TutorId = pet.Tutores.FirstOrDefault()?.Id;
        ObitoInformado = pet.ObitoInformado;
        ComedouroStatus = pet.ComedouroStatus;
    }
}
