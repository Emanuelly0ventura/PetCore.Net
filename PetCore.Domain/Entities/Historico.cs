namespace PetCore.Domain.Entities;

public class Historico
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    
    public DateOnly DataAbertura { get; private set; }
    
    public bool Status { get; private set; }

    public Guid? IdPet { get; private set; }
    
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
    
}