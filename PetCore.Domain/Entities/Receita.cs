using PetCore.Domain.Commons;

namespace PetCore.Domain.Entities;

public class Receita : DocumentoBase
{
    public DateOnly Validade { get; private set; }
    //N:1 Medico
    public Guid IdMedicoResponsavel { get; private set; }
    //N:1 Prontuario
    public Guid IdProntuario { get; private set; }
    
    //RELACIONAMENTOS
    public List<Medicamento> Medicamentos { get; set; } = [];
    public Medico Medico { get; private set; } 
    public Prontuario Prontuario { get; private set; }

    
    private Receita()
    {
    }
    public Receita(String nome, DateOnly validade, Guid idMedicoResponsavel, Guid idProntuario)
    {
        if (string.IsNullOrEmpty(nome))
            throw new Exception("Nome está vazio");
        Nome = nome;

        if (validade.Year > DateTime.Now.Year || validade.Equals(null))
            throw new Exception("Validade inválida");
        Validade = validade;
        
        if (idMedicoResponsavel == Guid.Empty)
            throw new Exception("Id do Médico está vazio");
        IdMedicoResponsavel = idMedicoResponsavel;

        if (idProntuario == Guid.Empty)
            throw new Exception("Id do Prontuario está vazio");
        IdProntuario = idProntuario;
    }
}
