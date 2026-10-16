using System.ComponentModel.DataAnnotations;

namespace PetCore.Application.DTOs;

public class ProtocoloDadosRequest
{
    [Required, StringLength(150)] public string Titulo { get; set; }
    [Required, StringLength(2000)] public string Texto { get; set; }
}
