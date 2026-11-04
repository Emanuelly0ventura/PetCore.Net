using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PetCore.Authentication;
using PetCore.Application.DTOs;
using PetCore.Application.Services.Interfaces;
using PetCore.Extensions;
using PetCore.Authorization;

namespace PetCore.Controllers;

[Authorize(Roles = "tutor,medico,veterinario")]
[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class ProtocoloController(IProtocoloService protocoloService, IPetService petService) : ControllerBase
{
    [HttpGet]
    public IActionResult FetchAll([FromQuery] PaginationOptions pagination)
    {
        if (Request.Query.IsPaginationRequested())
        {
            if (ResourceAccess.IsVeterinarian(User))
            {
                var veterinarianId = ResourceAccess.GetUserId(User);
                if (!veterinarianId.HasValue) return Forbid();
                return Ok(protocoloService.FetchPageForVeterinarian(
                    pagination.ToPageQuery(Request.Query), veterinarianId.Value));
            }

            var query = ResourceAccess.ScopeRecordPageQuery(User, pagination.ToPageQuery(Request.Query), "TutorId", "VeterinarioId");
            return Ok(protocoloService.FetchPage(query));
        }

        return Ok(protocoloService.FetchAll().Where(CanReadProtocol).ToList());
    }

    [HttpGet("{id}")]
    public IActionResult FetchById(string id)
    {
        var item = protocoloService.FetchById(id);
        if (item is null) return NotFound();
        return CanReadProtocol(item) ? Ok(item) : Forbid();
    }

    [HttpPost]
    [Authorize(Roles = "medico,veterinario")]
    public IActionResult Create([FromBody] ProtocoloRequest request)
    {
        var medicoId = ResourceAccess.GetUserId(User);
        if (!medicoId.HasValue) return Forbid();
        request.VeterinarioId = medicoId.Value;
        if (request.PetId.HasValue)
        {
            var pet = petService.FetchById(request.PetId.Value);
            if (pet is null) return NotFound();
            if (request.TutorId.HasValue && request.TutorId != pet.TutorId) return Forbid();
            request.TutorId = pet.TutorId;
            request.PetNome = pet.Nome;
        }
        else
        {
            request.TutorId = null;
            request.TutorNome = null;
        }

        var created = protocoloService.Create(request);
        return CreatedAtAction(nameof(FetchById), new { id = created.Id }, created);
    }

    [HttpPatch("{id}")]
    [HttpPut("{id}/patch")]
    [Authorize(Roles = "medico,veterinario")]
    public IActionResult Patch(string id, ProtocoloDadosRequest request)
    {
        var existing = protocoloService.FetchById(id);
        if (existing is null) return NotFound();
        if (!ResourceAccess.CanAccessRecord(User, existing)) return Forbid();
        return protocoloService.Patch(id, request) is { } item ? Ok(item) : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "medico,veterinario")]
    public IActionResult Delete(string id)
    {
        var existing = protocoloService.FetchById(id);
        if (existing is null) return NotFound();
        if (!ResourceAccess.CanAccessRecord(User, existing)) return Forbid();
        return protocoloService.Delete(id) ? NoContent() : NotFound();
    }

    private bool CanReadProtocol(ProtocoloResponse protocol) =>
        ResourceAccess.CanAccessRecord(User, protocol) ||
        (ResourceAccess.IsVeterinarian(User) && IsUnassignedProtocolTemplate(protocol));

    private static bool IsUnassignedProtocolTemplate(ProtocoloResponse protocol) =>
        string.Equals(protocol.TipoRegistro, "protocolo", StringComparison.OrdinalIgnoreCase) &&
        protocol.TutorId is null &&
        protocol.PetId is null &&
        protocol.VeterinarioId is null &&
        protocol.DataEnvio is null;
}
