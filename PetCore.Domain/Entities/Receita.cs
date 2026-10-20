using PetCore.Domain.Commons;

namespace PetCore.Domain.Entities;

public class Receita : DocumentoBase
{
    public DateOnly? Validade { get; private set; }
    //N:1 Medico
    public Guid IdMedicoResponsavel { get; private set; }
    //N:1 Prontuario
    public Guid? IdProntuario { get; private set; }
    public Guid? PetId { get; private set; }
    public Guid? TutorId { get; private set; }
    public string PetNome { get; private set; } = string.Empty;
    public string TutorNome { get; private set; } = string.Empty;
    public string VeterinarioNome { get; private set; } = string.Empty;
    public string ArquivoReceita { get; private set; } = string.Empty;
    public DateTime? DataEnvio { get; private set; }
    
    //RELACIONAMENTOS
    public List<Medicamento> Medicamentos { get; set; } = [];
    public Medico Medico { get; private set; } 
    public Prontuario Prontuario { get; private set; }

    
    private Receita()
    {
    }
    public Receita(String nome, DateOnly? validade, Guid idMedicoResponsavel, Guid? idProntuario,
        Guid? petId = null, Guid? tutorId = null, string? petNome = null, string? tutorNome = null,
        string? veterinarioNome = null, string? arquivoReceita = null, DateTime? dataEnvio = null)
    {
        if (string.IsNullOrEmpty(nome))
            throw new Exception("Nome está vazio");
        Nome = nome;

        if (validade.HasValue && validade.Value < DateOnly.FromDateTime(DateTime.UtcNow)) throw new Exception("Validade inválida");
        Validade = validade;
        
        if (idMedicoResponsavel == Guid.Empty)
            throw new Exception("Id do Médico está vazio");
        IdMedicoResponsavel = idMedicoResponsavel;

        if (idProntuario == Guid.Empty) throw new Exception("Id do Prontuário inválido");
        IdProntuario = idProntuario;
        if (petId == Guid.Empty || tutorId == Guid.Empty) throw new Exception("Pet/tutor inválidos");
        PetId = petId; TutorId = tutorId; PetNome = petNome?.Trim() ?? string.Empty;
        TutorNome = tutorNome?.Trim() ?? string.Empty; VeterinarioNome = veterinarioNome?.Trim() ?? string.Empty;
        ArquivoReceita = arquivoReceita?.Trim() ?? string.Empty; DataEnvio = dataEnvio;
    }
}
