using System.ComponentModel.DataAnnotations;
using PetCore.Domain.Entities;
using System.Text.Json.Serialization;

namespace PetCore.Application.DTOs;

/// <summary>
/// Cadastro de Relatorio
/// </summary>
public class RelatorioRequest
{
    [StringLength(500, MinimumLength = 2, ErrorMessage = "Observação deve ter entre 2 à 500 caracteres")]
    public string? Observacao { get; set; }

    [JsonPropertyName("observacoes")] public string? Observacoes { get; set; }
    
    [Required(ErrorMessage = "O campo 'IdMedico' deve ser preenchido")]
    public Guid IdMedico { get; set; }

    [JsonPropertyName("veterinarioId")] public Guid? VeterinarioId { get => null; set { if (value.HasValue) IdMedico = value.Value; } }
    
    [Required(ErrorMessage = "O campo 'IdHistorico' deve ser preenchido")]
    public Guid IdHistorico { get; set; }

    public string? ArquivoHistorico { get; set; }
    public string? Prontuarios { get; set; }
    public Guid? PetId { get; set; }
    public string? PetNome { get; set; }
    public Guid? TutorId { get; set; }
    public string? TutorNome { get; set; }
    
    public List<Guid> IdClinicas { get; set; } = [];
    
    public Relatorio ToDomain()
    {
        var historyId = IdHistorico;
        if (historyId == Guid.Empty && !string.IsNullOrWhiteSpace(Prontuarios))
        {
            try
            {
                using var json = System.Text.Json.JsonDocument.Parse(Prontuarios);
                if (json.RootElement.ValueKind == System.Text.Json.JsonValueKind.Array && json.RootElement.GetArrayLength() > 0 &&
                    json.RootElement[0].TryGetProperty("idHistorico", out var idValue) && Guid.TryParse(idValue.ToString(), out var parsed)) historyId = parsed;
            }
            catch (System.Text.Json.JsonException) { }
        }
        var observation = !string.IsNullOrWhiteSpace(Observacoes) ? Observacoes : !string.IsNullOrWhiteSpace(Observacao) ? Observacao : ArquivoHistorico;
        if (string.IsNullOrWhiteSpace(observation)) throw new System.ComponentModel.DataAnnotations.ValidationException("Informe observações ou arquivoHistorico.");
        // O construtor recebe o histórico antes do médico. Nomear os argumentos
        // impede que dois GUIDs válidos sejam gravados nas colunas trocadas.
        return new(observacao: observation, idHistorico: historyId == Guid.Empty ? null : historyId,
            idMedicoResponsavel: IdMedico, petId: PetId, tutorId: TutorId, petNome: PetNome,
            tutorNome: TutorNome, arquivoHistorico: ArquivoHistorico, prontuarios: Prontuarios);
    }
}
