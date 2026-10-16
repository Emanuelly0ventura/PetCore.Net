using PetCore.Domain.Commons;

namespace PetCore.Domain.Entities;

public class Exame : DocumentoBase
{
    public DateOnly Data { get; private set; }
    public String Tipo { get; private set; }
    //N:1 Medico
    public Guid IdMedico { get; set; }
    //N:1 Prontuario
    public Guid IdProntuario { get; set; }
    
    //RELACIONAMENTOS
    public Medico Medico { get; private set; } 
    public Prontuario Prontuario { get; private set; }

    private Exame()
    {
    }
    
    public Exame(string nome, DateOnly data, string tipo, Guid idMedico, Guid idProntuario)
    {
        UpdateNome(nome);
        
        UpdateDate(data);
        
        UpdateTipo(tipo);
        
        if (idMedico == Guid.Empty)
            throw new Exception("Id do medico inválido");
        IdMedico = idMedico;
        
        if (idProntuario == Guid.Empty)
            throw new Exception("Id do prontuario inválido");
        IdProntuario = idProntuario;
    }

    public void Update(string nome, DateOnly data, string tipo)
    {
        UpdateNome(nome);
        UpdateDate(data);
        UpdateTipo(tipo);
    }
    
    public void UpdateNome(string nome)
    {
        if (string.IsNullOrEmpty(nome))
            throw new Exception("Nome está vazio");
        Nome = nome.Trim();
    }

    public void UpdateDate(DateOnly data)
    {
        if (data.Year > DateTime.Now.Year || data.Equals(null))
            throw new Exception("Data inválida");
        Data = data;
    }
    
    public void  UpdateTipo(string tipo)
    {
        if (string.IsNullOrEmpty(tipo))
            throw new Exception("Tipo está vazio");
        Tipo = tipo;
    }
}
