using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PetCore.Authentication;
using PetCore.Application.DTOs;
using PetCore.Application.Services.Interfaces;
using PetCore.Extensions;
using PetCore.Authorization;

namespace PetCore.Controllers;

[Authorize]
[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class PetController(IPetService petService) : ControllerBase
{
    /// <summary>
    /// Lista todos os registros.
    /// </summary>
    /// <response code="200">Registros retornados com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult FetchAll([FromQuery] PaginationOptions pagination, [FromQuery] Guid? tutorId = null)
    {
        if (ResourceAccess.IsTutor(User))
        {
            var authenticatedTutorId = ResourceAccess.GetUserId(User);
            if (!authenticatedTutorId.HasValue) return Forbid();
            if (tutorId.HasValue && tutorId.Value != authenticatedTutorId.Value) return Forbid();
            if (Request.Query.IsPaginationRequested())
            {
                var query = pagination.ToPageQuery(Request.Query);
                var filters = new Dictionary<string, string>(query.Filters, StringComparer.OrdinalIgnoreCase)
                {
                    ["tutorId"] = authenticatedTutorId.Value.ToString()
                };
                query = new PageQuery
                {
                    Page = query.Page,
                    PageSize = query.PageSize,
                    SortBy = query.SortBy,
                    Descending = query.Descending,
                    Filters = filters
                };
                return Ok(petService.FetchPage(query));
            }

            return Ok(petService.FetchAll(authenticatedTutorId));
        }

        if (!ResourceAccess.IsVeterinarian(User)) return Forbid();
        return Request.Query.IsPaginationRequested() ? Ok(petService.FetchPage(pagination.ToPageQuery(Request.Query))) : Ok(petService.FetchAll(tutorId));
    }
    
    /// <summary>
    /// Lista todos os registros.
    /// </summary>
    /// <response code="200">Registros retornados com sucesso.</response>
    [HttpGet("menu")]
    [Authorize(Roles = "medico,veterinario")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult FetchMenuAll([FromQuery] PaginationOptions pagination)
    {
        return Request.Query.IsPaginationRequested() ? Ok(petService.FetchMenuPage(pagination.ToPageQuery(Request.Query))) : Ok(petService.FetchMenuAll());
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
        var item = petService.FetchById(id);
        if (item is null) return NotFound();
        return ResourceAccess.IsVeterinarian(User) ||
            (ResourceAccess.IsTutor(User) && item.TutorId == ResourceAccess.GetUserId(User))
            ? Ok(item)
            : Forbid();
    }

    /// <summary>
    /// Cria um novo registro.
    /// </summary>
    /// <param name="request">Dados para criação de avaliação.</param>
    /// <response code="201">Registro criado com sucesso.</response>
    /// <response code="400">Payload inválido para criação.</response>
    [HttpPost]
    [Authorize(Roles = "tutor")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] PetRequest request)
    {
        var tutorId = ResourceAccess.GetUserId(User);
        if (!tutorId.HasValue) return Forbid();
        request.TutorIdMobile = tutorId.Value;
        request.IdTutores = [tutorId.Value];
        var created = petService.Create(request);
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
    [Authorize(Roles = "tutor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Patch(Guid id, [FromBody] PetDadosRequest request)
    {
        var existing = petService.FetchById(id);
        if (existing is null) return NotFound();
        if (existing.TutorId != ResourceAccess.GetUserId(User)) return Forbid();

        var updated = petService.Patch(id, request);
        return updated is null ? NotFound() : Ok(updated);
    }

    /// <summary>
    /// Exclui um registro por Id.
    /// </summary>
    /// <param name="id">Identificador único da avaliação.</param>
    /// <response code="204">Registro excluído com sucesso.</response>
    /// <response code="404">Nenhum registro foi encontrado para o Id informado.</response>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "tutor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        var existing = petService.FetchById(id);
        if (existing is null) return NotFound();
        if (existing.TutorId != ResourceAccess.GetUserId(User)) return Forbid();

        var deleted = petService.Delete(id);
        return deleted ? NoContent() : NotFound();
    }
}
