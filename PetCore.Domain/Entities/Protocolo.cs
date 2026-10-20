namespace PetCore.Domain.Entities;

public class Protocolo
{
    public string Id { get; private set; }
    public string Titulo { get; private set; }
    public string Texto { get; private set; }
    public Guid? TutorId { get; private set; }
    public string TutorNome { get; private set; } = string.Empty;
    public Guid? PetId { get; private set; }
    public string PetNome { get; private set; } = string.Empty;
    public Guid? VeterinarioId { get; private set; }
    public string VeterinarioNome { get; private set; } = string.Empty;
    public string TipoRegistro { get; private set; } = "protocolo";
    public DateTime? DataEnvio { get; private set; }
    private Protocolo() { }
    public Protocolo(string id, string titulo, string texto, Guid? tutorId = null, string? tutorNome = null,
        Guid? petId = null, string? petNome = null, Guid? veterinarioId = null, string? veterinarioNome = null,
        string? tipoRegistro = null, DateTime? dataEnvio = null)
    {
        Id = string.IsNullOrWhiteSpace(id)
            ? $"PROTO-{Guid.NewGuid():N}"[..20].ToUpperInvariant()
            : id.Trim().ToUpperInvariant();
        Update(titulo, texto);
        if (tutorId == Guid.Empty || petId == Guid.Empty || veterinarioId == Guid.Empty) throw new ArgumentException("IDs inválidos no protocolo.");
        TutorId = tutorId; TutorNome = tutorNome?.Trim() ?? string.Empty; PetId = petId; PetNome = petNome?.Trim() ?? string.Empty;
        VeterinarioId = veterinarioId; VeterinarioNome = veterinarioNome?.Trim() ?? string.Empty;
        TipoRegistro = string.IsNullOrWhiteSpace(tipoRegistro) ? "protocolo" : tipoRegistro.Trim(); DataEnvio = dataEnvio;
    }
    public void Update(string titulo, string texto)
    {
        if (string.IsNullOrWhiteSpace(titulo)) throw new Exception("Título do protocolo está vazio");
        if (string.IsNullOrWhiteSpace(texto)) throw new Exception("Texto do protocolo está vazio");
        Titulo = titulo.Trim();
        Texto = texto.Trim();
    }
}
