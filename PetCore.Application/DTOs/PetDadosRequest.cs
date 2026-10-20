using System.Text.Json.Serialization;
using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class PetDadosRequest
{
    public string? Nome { get; set; }
    public string? Especie { get; set; }
    public string? Raca { get; set; }
    public string? Pelagem { get; set; }
    public string? Porte { get; set; }
    [System.Text.Json.Serialization.JsonConverter(typeof(PetSexJsonConverter))]
    public PetCore.Domain.Enums.GeneroSexoEnum? Sexo { get; set; }
    public DateOnly? DataNasc { get; set; }
    [JsonPropertyName("nascimento")]
    public string? Nascimento
    {
        get => null;
        set { if (DateOnly.TryParseExact(value, new[] { "dd/MM/yyyy", "yyyy-MM-dd" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date)) DataNasc = date; }
    }
    public Guid? TutorId { get; set; }
    public bool? ObitoInformado { get; set; }
    public string? ComedouroStatus { get; set; }
    public string? UrlImg { get; set; }
    public bool? Status { get; set; }

    public PetDadosRequest() { }
    public PetDadosRequest(Pet pet)
    {
        Nome = pet.Nome; Especie = pet.Especie; Raca = pet.Raca; DataNasc = pet.DataNasc;
        Pelagem = pet.Pelagem; Porte = pet.Porte; Sexo = pet.Sexo; UrlImg = pet.UrlImg;
        Status = pet.Status; ObitoInformado = pet.ObitoInformado; ComedouroStatus = pet.ComedouroStatus;
    }
}
