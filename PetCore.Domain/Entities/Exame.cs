using PetCore.Domain.Exceptions;
using PetCore.Domain.Commons;

namespace PetCore.Domain.Entities;

public class Exame : DocumentoBase
{
    public DateOnly Data { get; private set; }
    public String Tipo { get; private set; }
    //N:1 Medico
    public Guid IdMedico { get; set; }
    //N:1 Prontuario
    public Guid? IdProntuario { get; set; }
    public Guid? PetId { get; private set; }
    public Guid? TutorId { get; private set; }
    public string PetNome { get; private set; } = string.Empty;
    public string TutorNome { get; private set; } = string.Empty;
    public string VeterinarioNome { get; private set; } = string.Empty;
    public string ArquivoSolicitacao { get; private set; } = string.Empty;
    public DateTime? DataEnvio { get; private set; }
    
    //RELACIONAMENTOS
    public Medico Medico { get; private set; } 
    public Prontuario Prontuario { get; private set; }

    private Exame()
    {
    }
    
    public Exame(string nome, DateOnly data, string tipo, Guid idMedico, Guid? idProntuario,
        Guid? petId = null, Guid? tutorId = null, string? petNome = null, string? tutorNome = null,
        string? veterinarioNome = null, string? arquivoSolicitacao = null, DateTime? dataEnvio = null)
    {
        UpdateNome(nome);
        
        UpdateDate(data);
        
        UpdateTipo(tipo);
        
        if (idMedico == Guid.Empty)
            throw new DomainValidationException("Id do medico inválido");
        IdMedico = idMedico;
        
        if (idProntuario == Guid.Empty) throw new DomainValidationException("Id do prontuário inválido");
        IdProntuario = idProntuario;
        if (petId == Guid.Empty || tutorId == Guid.Empty) throw new DomainValidationException("Pet/tutor inválidos");
        PetId = petId; TutorId = tutorId; PetNome = petNome?.Trim() ?? string.Empty;
        TutorNome = tutorNome?.Trim() ?? string.Empty; VeterinarioNome = veterinarioNome?.Trim() ?? string.Empty;
        ArquivoSolicitacao = arquivoSolicitacao?.Trim() ?? string.Empty; DataEnvio = dataEnvio;
    }

    public void Update(string nome, DateOnly data, string tipo)
    {
        UpdateNome(nome);
        UpdateDate(data);
        UpdateTipo(tipo);
    }
    
    public void UpdateNome(string nome)
    {
        if (string.IsNullOrEmpty(nome))
            throw new DomainValidationException("Nome está vazio");
        Nome = nome.Trim();
    }

    public void UpdateDate(DateOnly data)
    {
        if (data == default || data > DateOnly.FromDateTime(DateTime.Today))
            throw new DomainValidationException("Data inválida");
        Data = data;
    }
    
    public void  UpdateTipo(string tipo)
    {
        if (string.IsNullOrEmpty(tipo))
            throw new DomainValidationException("Tipo está vazio");
        Tipo = tipo;
    }
}
