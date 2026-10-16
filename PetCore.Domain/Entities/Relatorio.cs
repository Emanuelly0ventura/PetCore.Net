namespace PetCore.Domain.Entities;

public class Relatorio
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Observacao { get; set; } 
    
    //N:1 Historico
    public Guid IdHistorico { get; set; }
    //N:1 Medico
    public Guid IdMedicoResponsavel { get; set; }
    
    
    //RELACIONAMENTO
    public List<Clinica> Clinicas { get; set; } = [];
    public Medico Medico { get; private set; }
    public Historico Historico { get; private set; }

    
    
    
    public Relatorio(string observacao, Guid idHistorico, Guid idMedicoResponsavel)
    {
        UpdateObs(observacao);

        if (idHistorico == Guid.Empty) 
            throw new Exception("Id do historico está vazio");
        IdHistorico = idHistorico;

        if (idMedicoResponsavel == Guid.Empty)
            throw new Exception("Id do Médico está vazio");
        IdMedicoResponsavel = idMedicoResponsavel;
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
