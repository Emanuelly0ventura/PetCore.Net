namespace PetCore.Domain.Entities;

public class Historico
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    
    public DateOnly DataAbertura { get; private set; }
    
    public bool Status { get; private set; }

    public Guid? IdPet { get; private set; }
    public Guid? TutorId { get; private set; }
    public string TutorNome { get; private set; } = string.Empty;
    public string PetNome { get; private set; } = string.Empty;
    public string TipoRegistro { get; private set; } = "historico";
    public string StatusMobile { get; private set; } = "Ativo";
    public DateTime? DataSolicitacao { get; private set; }
    
    //RELACIONAMENTOS
    public List<Relatorio> Relatorios { get; private set; }
    
    public List<Prontuario> Prontuarios { get; private set; }
    
    public Pet Pet { get; private set; }
    
    private Historico()
    {
    }

    public Historico(DateOnly data, Guid idPet)
    {
        if (data.Year > DateTime.Now.Year || data.Equals(null))
            throw new Exception("Data inválida");
        DataAbertura = data;

        if (idPet == Guid.Empty)
            throw new ArgumentException("Id do Pet está vazio", nameof(idPet));
        IdPet = idPet;

        Status = true;
    }

    public void SetMobileData(Guid? tutorId, string? tutorNome, string? petNome, string? tipoRegistro, string? status, DateTime? dataSolicitacao)
    {
        TutorId = tutorId;
        TutorNome = tutorNome?.Trim() ?? string.Empty;
        PetNome = petNome?.Trim() ?? string.Empty;
        TipoRegistro = string.IsNullOrWhiteSpace(tipoRegistro) ? "historico" : tipoRegistro.Trim();
        StatusMobile = string.IsNullOrWhiteSpace(status) ? "Solicitado" : status.Trim();
        DataSolicitacao = dataSolicitacao;
    }

    /// <summary>Reconstitui um histórico persistido sem gerar um novo identificador.</summary>
    public static Historico Restore(Guid id, DateOnly data, bool status, Guid idPet)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id do histórico está vazio", nameof(id));

        var historico = new Historico(data, idPet)
        {
            Id = id,
            Status = status
        };
        return historico;
    }
    
}
