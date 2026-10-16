namespace PetCore.Domain.Entities;

public class Endereco
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Cep { get; set; }
    public string Complemento { get; set; }

    //RELACIONAMENTO
    public Clinica Clinica { get; set; }

    private Endereco()
    {
    }
    
    public Endereco(string cep, string complemento)
    {
        UpdateCep(cep);

        UpdateComplemento(complemento);
    }

    public void UpdateCep(string cep)
    {
        if (string.IsNullOrWhiteSpace(cep))
            throw new Exception("Cep está vazia");
        Cep = cep.Trim();
    }

    public void UpdateComplemento(string complemento)
    {
        if (string.IsNullOrWhiteSpace(complemento))
            throw new Exception("Complemento está vazia");
        Complemento = complemento.Trim();
    }
}