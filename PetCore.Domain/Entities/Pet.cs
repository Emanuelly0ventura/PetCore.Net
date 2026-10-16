using PetCore.Domain.Enums;

namespace PetCore.Domain.Entities;

public class Pet
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nome { get; private set; }
    public string Especie { get; private set; }
    public string Raca { get; private set; }
    public DateOnly DataNasc { get; private set; }
    public string Pelagem { get; private set; }
    public string Porte { get; private set; }
    public GeneroSexoEnum Sexo { get; private set; }
    public bool Status { get; private set; }
    
    public string UrlImg { get; private set; }
    //RELACIONAMENTO
    public Historico? Historico { get; private set; }
    public List<Tutor> Tutores { get; set; } = [];

    private Pet()
    {
    }
    
    public Pet(string nome, string especie, string raca, DateOnly dataNasc, string pelagem, string porte, GeneroSexoEnum sexo, bool status, string urlImg)
    {
        
        if (string.IsNullOrWhiteSpace(nome))
            throw new Exception("Nome está vazia");
        Nome = nome.Trim();
        
        if (string.IsNullOrWhiteSpace(especie))
            throw new Exception("Especie está vazia");
        Especie = especie.Trim();
        
        if (string.IsNullOrWhiteSpace(raca))
            throw new Exception("Raça está vazia");
        Raca = raca.Trim();

        if (dataNasc.Equals(null))
            throw new Exception("Data de nascimento do Pet inválida");
        DataNasc = dataNasc;
        
        if (string.IsNullOrWhiteSpace(pelagem))
            throw new Exception("Pelagem está vazia");
        Pelagem = pelagem.Trim();
        
        if (string.IsNullOrWhiteSpace(porte))
            throw new Exception("Porte está vazia");
        Porte = porte.Trim();
        
        if (!Enum.IsDefined(sexo))
            throw new Exception("Sexo está vazio");
        Sexo = sexo;
        
        Status = status;
        
        if (string.IsNullOrWhiteSpace(urlImg))
            throw new Exception("UrlImg está vazia");
        UrlImg = urlImg.Trim();
        
    }

    public void UpdateStatus(bool status)
    {
        Status = status;
    }
    
    public void updateUrlImg(string urlImg)
    {
        if (string.IsNullOrWhiteSpace(urlImg))
            throw new Exception("UrlImg está vazia");
        UrlImg = urlImg.Trim();
    }

    public void Update(string url, bool status)
    {
        UpdateStatus(status);
        updateUrlImg(url);
    }
}
