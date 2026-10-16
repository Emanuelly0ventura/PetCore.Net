using PetCore.Domain.Commons;

namespace PetCore.Domain.Entities;

public class Medicamento : DocumentoBase
{
    public string Dosagem { get; private set; }
    public string Instrucao { get; private set; }
    
    //RELACIONAMENTO
    public List<Receita> Receitas { get; private set; }

    private Medicamento()
    {
    }
    
    public Medicamento(string nome, string dosagem, string instrucao)
    {
        if (string.IsNullOrEmpty(nome))
            throw new Exception("Nome está vazio");
        Nome = nome.Trim();
        
        UpdateDosagem(dosagem);
        
        UpdateInstrucao(instrucao);
    }

    public void Update(string dosagem, string instrucao)
    {
        UpdateDosagem(dosagem);
        UpdateInstrucao(instrucao);
    }

    public void UpdateDosagem(string dosagem)
    {
        if (string.IsNullOrEmpty(dosagem))
            throw new Exception("Dosagem está vazio");
        Dosagem = dosagem.Trim();
    }
    
    public void UpdateInstrucao(string instrucao)
    {
        if (string.IsNullOrEmpty(instrucao))
            throw new Exception("Instrucao está vazio");
        Instrucao = instrucao.Trim();    
    }
}