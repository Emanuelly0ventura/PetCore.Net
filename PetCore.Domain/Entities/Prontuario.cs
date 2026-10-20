namespace PetCore.Domain.Entities;

public class Prontuario
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateOnly DataEmissao { get; private set; }
    public string Descricao { get; private set; }
        
    //1:1 Historico
    public Guid? IdHistorico { get; private set; }
    //N:1 Medico 
    public Guid? IdMedico { get; private set; }
    public Guid? PetId { get; private set; }
    public Guid? TutorId { get; private set; }
    public string PetNome { get; private set; } = string.Empty;
    public string TutorNome { get; private set; } = string.Empty;
    public decimal? Temperatura { get; private set; }
    public decimal? Peso { get; private set; }
    public string Tratamento { get; private set; } = string.Empty;
    
    //RELACIONAMENTOS
    public List<Exame> Exames { get; set; } = [];
    
    public List<Receita> Receitas { get; set; } = [];
    
    public Medico Medico { get; private set; }
    
    public Historico Historico { get; private set; }

    
    private Prontuario()
    {
    }

    public Prontuario(DateOnly dataEmissao, string descricao, Guid? idMedico, Guid? idHistorico,
        Guid? petId = null, Guid? tutorId = null, string? petNome = null, string? tutorNome = null,
        decimal? temperatura = null, decimal? peso = null, string? tratamento = null)
    {
        if (dataEmissao.Year > DateTime.Now.Year || dataEmissao.Equals(null))
            throw new Exception("Data de emissão inválida");
        DataEmissao = dataEmissao;

        UpdateDesc(descricao);
        
        if (idMedico == Guid.Empty) throw new Exception("Id do Médico inválido");
        IdMedico = idMedico;
        
        if (idHistorico == Guid.Empty) throw new Exception("Id do Histórico inválido");
        IdHistorico = idHistorico;
        if (petId == Guid.Empty || tutorId == Guid.Empty) throw new Exception("Pet/tutor inválidos");
        PetId = petId; TutorId = tutorId; PetNome = petNome?.Trim() ?? string.Empty; TutorNome = tutorNome?.Trim() ?? string.Empty;
        Temperatura = temperatura; Peso = peso; Tratamento = tratamento?.Trim() ?? string.Empty;
    }

    public void UpdateDesc(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new Exception("Descrição está vazia");
        Descricao = descricao.Trim();
    }

    public void UpdateMobileData(DateOnly? dataConsulta, string? observacoes, decimal? temperatura, decimal? peso,
        string? tratamento, Guid? petId, string? petNome, Guid? tutorId, string? tutorNome)
    {
        if (dataConsulta.HasValue)
        {
            if (dataConsulta.Value == default || dataConsulta.Value > DateOnly.FromDateTime(DateTime.UtcNow)) throw new ArgumentException("Data da consulta inválida.");
            DataEmissao = dataConsulta.Value;
        }
        if (!string.IsNullOrWhiteSpace(observacoes)) UpdateDesc(observacoes);
        if (petId.HasValue) { if (petId == Guid.Empty) throw new ArgumentException("Pet inválido."); PetId = petId; }
        if (tutorId.HasValue) { if (tutorId == Guid.Empty) throw new ArgumentException("Tutor inválido."); TutorId = tutorId; }
        if (petNome is not null) PetNome = petNome.Trim();
        if (tutorNome is not null) TutorNome = tutorNome.Trim();
        if (temperatura.HasValue) Temperatura = temperatura;
        if (peso.HasValue) Peso = peso;
        if (tratamento is not null) Tratamento = tratamento.Trim();
    }

    public void Update()
    {
        UpdateDesc(Descricao);
    }
}
