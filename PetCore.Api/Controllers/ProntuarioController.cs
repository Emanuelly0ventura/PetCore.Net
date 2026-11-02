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
public class ProntuarioController(IProntuarioService prontuarioService, IPetService petService) : ControllerBase
{
    /// <summary>
    /// Lista todos os registros.
    /// </summary>
    /// <response code="200">Registros retornados com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult FetchAll([FromQuery] PaginationOptions pagination)
    {
        if (Request.Query.IsPaginationRequested())
        {
            var query = ResourceAccess.ScopeRecordPageQuery(User, pagination.ToPageQuery(Request.Query), "TutorId", "IdMedico");
            return Ok(prontuarioService.FetchPage(query));
        }

        return Ok(ResourceAccess.FilterRecords(User, prontuarioService.FetchAll()));
    }

    /// <summary>
    /// Busca um registro por Id.
    /// </summary>
    /// <param name="id">Identificador único da avaliação.</param>
    /// <response code="200">Registro encontrado.</response>
    /// <response code="404">Nenhum registro foi encontrado para o Id informado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult FetchById(Guid id)
    {
        var item = prontuarioService.FetchById(id);
        if (item is null) return NotFound();
        return ResourceAccess.CanAccessRecord(User, item) ? Ok(item) : Forbid();
    }

    /// <summary>
    /// Cria um novo registro.
    /// </summary>
    /// <param name="request">Dados para criação de avaliação.</param>
    /// <response code="201">Registro criado com sucesso.</response>
    /// <response code="400">Payload inválido para criação.</response>
    [HttpPost]
    [Authorize(Roles = "medico,veterinario")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] ProntuarioRequest request)
    {
        var medicoId = ResourceAccess.GetUserId(User);
        if (!medicoId.HasValue) return Forbid();
        request.IdMedico = medicoId.Value;
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

        var created = prontuarioService.Create(request);
        return CreatedAtAction(nameof(FetchById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Atualiza somente a nota da avaliação.
    /// </summary>
    /// <param name="id">Identificador único da avaliação.</param>
    /// <param name="request">Nova nota da avaliação.</param>
    /// <response code="200">Nota atualizada com sucesso.</response>
    /// <response code="404">Nenhum registro foi encontrado para o Id informado.</response>
    [HttpPatch("{id:guid}")]
    [HttpPut("{id:guid}/patch")]
    [Authorize(Roles = "medico,veterinario")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Patch(Guid id, [FromBody] ProntuarioDadosRequest request)
    {
        var existing = prontuarioService.FetchById(id);
        if (existing is null) return NotFound();
        if (!ResourceAccess.CanAccessRecord(User, existing)) return Forbid();
        if (request.PetId.HasValue && request.PetId != existing.PetId)
            return Forbid();
        if (request.TutorId.HasValue && request.TutorId != existing.TutorId)
            return Forbid();

        // Vínculos são mantidos conforme o registro persistido; IDs e nomes enviados pelo cliente
        // não podem transferir o prontuário para outro tutor ou pet.
        request.PetId = existing.PetId;
        request.TutorId = existing.TutorId;
        request.PetNome = existing.PetNome;
        request.TutorNome = existing.TutorNome;
        if (request.PetId.HasValue)
        {
            var pet = petService.FetchById(request.PetId.Value);
            if (pet is null) return NotFound();
            if (pet.TutorId != existing.TutorId) return Forbid();
            request.TutorId = pet.TutorId;
            request.PetNome = pet.Nome;
        }

        var updated = prontuarioService.Patch(id, request);
        return updated is null ? NotFound() : Ok(updated);
    }

    /// <summary>
    /// Exclui um registro por Id.
    /// </summary>
    /// <param name="id">Identificador único da avaliação.</param>
    /// <response code="204">Registro excluído com sucesso.</response>
    /// <response code="404">Nenhum registro foi encontrado para o Id informado.</response>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "medico,veterinario")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        var existing = prontuarioService.FetchById(id);
        if (existing is null) return NotFound();
        if (!ResourceAccess.CanAccessRecord(User, existing)) return Forbid();

        var deleted = prontuarioService.Delete(id);
        return deleted ? NoContent() : NotFound();
    }
}
