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
    public bool ObitoInformado { get; private set; }
    public string ComedouroStatus { get; private set; } = "vazio";
    
    public string UrlImg { get; private set; }
    //RELACIONAMENTO
    public Historico? Historico { get; private set; }
    public List<Tutor> Tutores { get; set; } = [];

    private Pet()
    {
    }
    
    public Pet(string nome, string especie, string raca, DateOnly dataNasc, string pelagem, string porte, GeneroSexoEnum sexo, bool status, string urlImg, bool obitoInformado = false, string comedouroStatus = "vazio")
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

        if (dataNasc == default || dataNasc > DateOnly.FromDateTime(DateTime.UtcNow))
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
        
        UrlImg = urlImg?.Trim() ?? string.Empty;
        ObitoInformado = obitoInformado;
        UpdateComedouroStatus(comedouroStatus);
        
    }

    public void UpdateStatus(bool status)
    {
        Status = status;
    }

    public void UpdateComedouroStatus(string status)
    {
        if (status is not ("cheio" or "vazio")) throw new ArgumentException("Status do comedouro deve ser 'cheio' ou 'vazio'.", nameof(status));
        ComedouroStatus = status;
    }

    public void UpdateObitoInformado(bool informado) => ObitoInformado = informado;

    public void UpdateDados(string nome, string especie, string raca, DateOnly dataNasc, string pelagem, string porte, GeneroSexoEnum sexo, bool obitoInformado, string comedouroStatus)
    {
        if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(especie) || string.IsNullOrWhiteSpace(raca) || string.IsNullOrWhiteSpace(pelagem) || string.IsNullOrWhiteSpace(porte))
            throw new ArgumentException("Nome, espécie, raça, pelagem e porte são obrigatórios.");
        Nome = nome.Trim(); Especie = especie.Trim(); Raca = raca.Trim(); DataNasc = dataNasc;
        Pelagem = pelagem.Trim(); Porte = porte.Trim(); Sexo = sexo;
        ObitoInformado = obitoInformado; UpdateComedouroStatus(comedouroStatus);
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
