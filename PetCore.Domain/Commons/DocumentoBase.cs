namespace PetCore.Domain.Commons;

public abstract class DocumentoBase
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nome { get;  set; }
}