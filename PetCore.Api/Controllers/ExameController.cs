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
public class ExameController(IExameService exameService, IPetService petService) : ControllerBase
{
     /// <summary>
    /// Lista todos os registros.
    /// </summary>
    /// <response code="200">Registros retornados com sucesso.</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult FetchAll([FromQuery] PaginationOptions pagination)
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            if (Request.Query.IsPaginationRequested())
            {
                var page = exameService.FetchPage(pagination.ToPageQuery(Request.Query));
                var catalogPage = new PagedData<ExameCatalogResponse>(
                    page.Items.Select(item => new ExameCatalogResponse(item.Nome)).ToList(),
                    page.Page, page.PageSize, page.TotalItems, page.TotalPages);
                return Ok(catalogPage);
            }

            return Ok(exameService.FetchAll()
                .Select(item => new ExameCatalogResponse(item.Nome))
                .ToList());
        }

        if (ResourceAccess.IsTutor(User))
        {
            if (Request.Query.IsPaginationRequested())
            {
                var query = ResourceAccess.ScopeRecordPageQuery(User, pagination.ToPageQuery(Request.Query), "TutorId", "IdMedico");
                return Ok(exameService.FetchPage(query));
            }

            return Ok(ResourceAccess.FilterRecords(User, exameService.FetchAll()));
        }

        if (!ResourceAccess.IsVeterinarian(User)) return Forbid();
        if (Request.Query.IsPaginationRequested())
        {
            var query = ResourceAccess.ScopeRecordPageQuery(User, pagination.ToPageQuery(Request.Query), "TutorId", "IdMedico");
            return Ok(exameService.FetchPage(query));
        }

        return Ok(ResourceAccess.FilterRecords(User, exameService.FetchAll()));
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
        var item = exameService.FetchById(id);
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
    public IActionResult Create([FromBody] ExameRequest request)
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

        var created = exameService.Create(request);
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
    public IActionResult Patch(Guid id, [FromBody] ExameDadosRequest request)
    {
        var existing = exameService.FetchById(id);
        if (existing is null) return NotFound();
        if (!ResourceAccess.CanAccessRecord(User, existing)) return Forbid();

        var updated = exameService.Patch(id, request);
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
        var existing = exameService.FetchById(id);
        if (existing is null) return NotFound();
        if (!ResourceAccess.CanAccessRecord(User, existing)) return Forbid();

        var deleted = exameService.Delete(id);
        return deleted ? NoContent() : NotFound();
    }
}
