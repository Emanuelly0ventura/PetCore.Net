using PetCore.Domain.Entities;
using PetCore.Domain.Enums;
using System.Text.Json.Serialization;

namespace PetCore.Application.DTOs;

public class TutorResponse
{
    public Guid Id { get;  set; }
    public string Nome { get;  set; }
    [JsonIgnore] public DateOnly DataNascimento { get;  set; }
    public string Nascimento => DataNascimento.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
    public string Telefone { get;  set; }
    public string Email{ get;  set; }
    [JsonIgnore] public GeneroSexoEnum Sexo { get;  set; }
    public string Genero => Sexo == GeneroSexoEnum.F ? "Feminino" : "Masculino";

    [JsonPropertyName("fotoUrl")] public string UrlImg { get; set; }
    public string TipoPerfil => "tutor";

    public TutorResponse(Tutor tutor)
    {
        Id = tutor.Id;
        Nome = tutor.Nome;
        DataNascimento = tutor.DataNascimento;
        Telefone = tutor.Telefone;
        Email = tutor.Email;
        Sexo = tutor.Sexo;
        UrlImg = tutor.UrlImg;
    }
}
