using PetCore.Domain.Exceptions;
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
            throw new DomainValidationException("Nome está vazio");
        Nome = nome;

        if (dataNasc > DateOnly.FromDateTime(DateTime.Now.AddYears(-18)))
            throw new DomainValidationException("Tutor deve ser maior de 18 anos");
        DataNascimento = dataNasc;

        UpdateTelefone(telefone);

        UpdateEmail(email);

        if (!Enum.IsDefined(sexo))
            throw new DomainValidationException("Sexo está vazio");
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
            throw new DomainValidationException("Email está vazio");
        Email = email.Trim();
    }

    public void UpdateTelefone(string telefone)
    {
        if (string.IsNullOrEmpty(telefone))
            throw new DomainValidationException("Telefone está vazio");
        Telefone = telefone.Trim();
    }

    public void UpdateSenha(string senha)
    {
        if (string.IsNullOrEmpty(senha))
            throw new DomainValidationException("Senha está vazia");
        Senha = senha.Trim();
    }

    public void UpdateUrlImg(string url)
    {
        if (string.IsNullOrEmpty(url))
            throw new DomainValidationException("Url está vazia");
        UrlImg = url.Trim();
    }
}