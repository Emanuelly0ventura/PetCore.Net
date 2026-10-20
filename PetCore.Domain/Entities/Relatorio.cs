namespace PetCore.Domain.Entities;

public class Relatorio
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Observacao { get; set; } 
    
    //N:1 Historico
    public Guid? IdHistorico { get; set; }
    //N:1 Medico
    public Guid IdMedicoResponsavel { get; set; }
    public Guid? PetId { get; private set; }
    public Guid? TutorId { get; private set; }
    public string PetNome { get; private set; } = string.Empty;
    public string TutorNome { get; private set; } = string.Empty;
    public string ArquivoHistorico { get; private set; } = string.Empty;
    public string Prontuarios { get; private set; } = string.Empty;
    
    
    //RELACIONAMENTO
    public List<Clinica> Clinicas { get; set; } = [];
    public Medico Medico { get; private set; }
    public Historico Historico { get; private set; }

    
    
    
    public Relatorio(string observacao, Guid? idHistorico, Guid idMedicoResponsavel,
        Guid? petId = null, Guid? tutorId = null, string? petNome = null, string? tutorNome = null,
        string? arquivoHistorico = null, string? prontuarios = null)
    {
        UpdateObs(observacao);

        if (idHistorico == Guid.Empty) throw new Exception("Id do histórico inválido");
        IdHistorico = idHistorico;

        if (idMedicoResponsavel == Guid.Empty)
            throw new Exception("Id do Médico está vazio");
        IdMedicoResponsavel = idMedicoResponsavel;
        if (petId == Guid.Empty || tutorId == Guid.Empty) throw new Exception("Pet/tutor inválidos");
        PetId = petId; TutorId = tutorId; PetNome = petNome?.Trim() ?? string.Empty;
        TutorNome = tutorNome?.Trim() ?? string.Empty; ArquivoHistorico = arquivoHistorico?.Trim() ?? string.Empty;
        Prontuarios = prontuarios ?? string.Empty;
    }

    public void UpdateObs(string observacao)
    {
        if (string.IsNullOrWhiteSpace(observacao))
            throw new Exception("Observação está vazia");

        Observacao = observacao.Trim();
    }
    
    public void Update(string observacao)
    {
        UpdateObs(observacao);
    }
    private Relatorio(){}
}
