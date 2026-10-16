namespace PetCore.Domain.Entities;

public class Prontuario
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateOnly DataEmissao { get; private set; }
    public string Descricao { get; private set; }
        
    //1:1 Historico
    public Guid IdHistorico { get; private set; }
    //N:1 Medico 
    public Guid IdMedico { get; private set; }
    
    //RELACIONAMENTOS
    public List<Exame> Exames { get; set; } = [];
    
    public List<Receita> Receitas { get; set; } = [];
    
    public Medico Medico { get; private set; }
    
    public Historico Historico { get; private set; }

    
    private Prontuario()
    {
    }

    public Prontuario(DateOnly dataEmissao, string descricao, Guid idMedico, Guid idHistorico)
    {
        if (dataEmissao.Year > DateTime.Now.Year || dataEmissao.Equals(null))
            throw new Exception("Data de emissão inválida");
        DataEmissao = dataEmissao;

        UpdateDesc(descricao);
        
        if (idMedico == Guid.Empty)
            throw new Exception("Id do Médico está vazio");
        IdMedico = idMedico;
        
        if (idHistorico == Guid.Empty)
            throw new Exception("Id do Historico está vazio");
        IdHistorico = idHistorico;
    }

    public void UpdateDesc(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new Exception("Descrição está vazia");
        Descricao = descricao.Trim();
    }

    public void Update()
    {
        UpdateDesc(Descricao);
    }
}
