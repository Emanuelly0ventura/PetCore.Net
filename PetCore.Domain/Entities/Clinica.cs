using PetCore.Domain.Exceptions;
namespace PetCore.Domain.Entities;

public class Clinica
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; private set; }
    public string Cnpj { get; private set; }
    //1:1 Endereco
    public Guid IdEndereco { get; private set; }
 
    //RELACIONAMENTO
    public Endereco Endereco { get; private set; }
    public List<Relatorio> Relatorios { get; private set; }

    private Clinica()
    {
    }
    
    public Clinica(string nome, string cnpj, Guid idEndereco)
    {
        UpdateNome(nome);
        
        if (string.IsNullOrWhiteSpace(cnpj))
            throw new DomainValidationException("Cnpj está vazia");
        Cnpj = cnpj.Trim();
        
        UpdateEndereco(idEndereco);
    }

    public void Update(string nome, Guid endereco)
    {
        UpdateNome(nome);
        UpdateEndereco(endereco);
    }

    public void UpdateNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainValidationException("Nome está vazia");
        Nome = nome.Trim();
    }

    public void UpdateEndereco(Guid idEndereco)
    {
        if (idEndereco == Guid.Empty)
            throw new DomainValidationException("Id do Endereco está vazio");
        IdEndereco = idEndereco;
    }
}