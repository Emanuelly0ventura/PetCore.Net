using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class ExameResponse
{
    public Guid Id { get;  set; }
    public string Nome { get;  set; }
    public DateOnly Data { get;  set; }
    public string Tipo { get;  set; } 
    public Guid IdMedico { get; set; }
    public Guid IdProntuario { get; set; }

    public ExameResponse(Exame exame) {
        Id = exame.Id;
        Nome = exame.Nome;
        Data = exame.Data;
        Tipo = exame.Tipo;
        IdMedico = exame.IdMedico;
        IdProntuario = exame.IdProntuario;

    }
}