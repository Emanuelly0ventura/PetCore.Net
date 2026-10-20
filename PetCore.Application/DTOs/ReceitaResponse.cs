using PetCore.Domain.Entities;
using System.Text.Json.Serialization;

namespace PetCore.Application.DTOs;

public class ReceitaResponse
{
    public Guid Id { get;  set; }
    public string Nome { get;  set; }
    public DateOnly? Validade { get;  set; }
    public Guid IdMedicoResponsavel { get;  set; }
    [JsonPropertyName("veterinarioId")] public Guid VeterinarioId => IdMedicoResponsavel;
    public string VeterinarioNome { get; set; }
    public Guid? TutorId { get; set; }
    public string TutorNome { get; set; }
    public Guid? PetId { get; set; }
    public string PetNome { get; set; }
    public string ArquivoReceita { get; set; }
    public DateTime? DataEnvio { get; set; }
    public string TipoRegistro => "receita";
    public List<Guid> IdMedicamentos { get; set; }

    public ReceitaResponse(Receita receita)
    {
        Id = receita.Id;
        Nome = receita.Nome;
        Validade = receita.Validade;
        IdMedicoResponsavel = receita.IdMedicoResponsavel;
        IdMedicamentos = (receita.Medicamentos ?? [])
            .Select(x => x.Id)
            .ToList();
        VeterinarioNome = receita.VeterinarioNome; TutorId = receita.TutorId; TutorNome = receita.TutorNome;
        PetId = receita.PetId; PetNome = receita.PetNome; ArquivoReceita = receita.ArquivoReceita;
        DataEnvio = receita.DataEnvio;
    }
}
