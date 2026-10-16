using PetCore.Domain.Commons;
using PetCore.Domain.Enums;

namespace PetCore.Domain.Entities;

public class Tutor : UsuarioBase
{
    //RELACIONAMENTO
    public List<Pet> Pets { get; set; }

    
    private Tutor()
    {
    }
    public Tutor(string nome, DateOnly dataNasc, string telefone, string email, GeneroSexoEnum sexo, string senha)
    {
        if (string.IsNullOrEmpty(nome))
            throw new Exception("Nome está vazio");
        Nome = nome;

        if (dataNasc > DateOnly.FromDateTime(DateTime.Now.AddYears(-18)))
            throw new Exception("Tutor deve ser maior de 18 anos");
        DataNascimento = dataNasc;

        UpdateTelefone(telefone);

        UpdateEmail(email);

        if (!Enum.IsDefined(sexo))
            throw new Exception("Sexo está vazio");
        Sexo = sexo;

        UpdateSenha(senha);
    }

    public void Update(string telefone, string email, string senha, string url)
    {
        UpdateEmail(email);
        UpdateTelefone(telefone);
        UpdateSenha(senha);
        UpdateUrlImg(url);
    }

    public void UpdateEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            throw new Exception("Email está vazio");
        Email = email.Trim();
    }

    public void UpdateTelefone(string telefone)
    {
        if (string.IsNullOrEmpty(telefone))
            throw new Exception("Telefone está vazio");
        Telefone = telefone.Trim();
    }

    public void UpdateSenha(string senha)
    {
        if (string.IsNullOrEmpty(senha))
            throw new Exception("Senha está vazia");
        Senha = senha.Trim();
    }

    public void UpdateUrlImg(string url)
    {
        if (string.IsNullOrEmpty(url))
            throw new Exception("Url está vazia");
        UrlImg = url.Trim();
    }
}