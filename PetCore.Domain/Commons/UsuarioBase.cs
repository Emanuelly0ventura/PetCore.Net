using PetCore.Domain.Enums;

namespace PetCore.Domain.Commons;

public class UsuarioBase
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nome { get;  set; }
    public DateOnly DataNascimento { get;  set; }
    public string Telefone { get;  set; }
    public string Email{ get;  set; }
    
    public GeneroSexoEnum Sexo { get;  set; }
    
    public string Senha { get; set; }
    
    // A imagem é opcional no cadastro. Um valor vazio mantém a coluna não nula
    // e permite criar tutor e médico apenas com os campos do Swagger.
    public string UrlImg { get; set; } = string.Empty;
}
