using System.ComponentModel.DataAnnotations;
using PetCore.Domain.Entities;

namespace PetCore.Application.DTOs;

public class ProtocoloRequest
{
    [StringLength(20)] public string? Id { get; set; }
    [Required, StringLength(150)] public string Titulo { get; set; }
    [Required, StringLength(2000)] public string Texto { get; set; }
    public Protocolo ToDomain() => new(Id ?? string.Empty, Titulo, Texto);
}
