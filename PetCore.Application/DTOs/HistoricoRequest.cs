using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class HistoricoRequest
{
    public DateOnly Data { get; set; }
    [Required(ErrorMessage = "O campo 'petId' deve ser preenchido")]
    public Guid IdPet { get; set; }
    [JsonPropertyName("petId")] public Guid? PetIdMobile { get; set; }
    public Guid? TutorId { get; set; }
    public string? TutorNome { get; set; }
    public string? PetNome { get; set; }
    public string TipoRegistro { get; set; } = "solicitacao_historico";
    public string Status { get; set; } = "Solicitado";
    public string? DataSolicitacao { get; set; }

    public Historico ToDomain()
    {
        var petId = PetIdMobile ?? IdPet;
        if (petId == Guid.Empty) throw new ValidationException("petId é obrigatório.");
        var date = Data;
        DateTime? solicitacao = null;
        if (!string.IsNullOrWhiteSpace(DataSolicitacao) && DateTime.TryParse(DataSolicitacao, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal, out var parsed))
        {
            solicitacao = parsed.ToUniversalTime();
            if (date == default) date = DateOnly.FromDateTime(parsed);
        }
        if (date == default) date = DateOnly.FromDateTime(DateTime.UtcNow);
        var historico = new Historico(date, petId);
        historico.SetMobileData(TutorId, TutorNome, PetNome, TipoRegistro, Status, solicitacao);
        return historico;
    }
}
