using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class MedicamentoResponse
{
    public Guid Id { get;  set; }
    public string Nome { get;  set; } = string.Empty;
    public string Dosagem { get;  set; } = string.Empty;
    public string Instrucao { get;  set; } = string.Empty;

    public MedicamentoResponse(Medicamento medicamento)
    {
        Id = medicamento.Id;
        Nome = medicamento.Nome;
        Dosagem = medicamento.Dosagem;
        Instrucao = medicamento.Instrucao;
    }
}