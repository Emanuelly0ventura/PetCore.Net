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
public class ReceitaController(IReceitaService receitaService, IPetService petService) : ControllerBase
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
            var query = ResourceAccess.ScopeRecordPageQuery(User, pagination.ToPageQuery(Request.Query), "TutorId", "IdMedicoResponsavel");
            return Ok(receitaService.FetchPage(query));
        }

        return Ok(ResourceAccess.FilterRecords(User, receitaService.FetchAll()));
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
        var item = receitaService.FetchById(id);
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
    public IActionResult Create([FromBody] ReceitaRequest request)
    {
        var medicoId = ResourceAccess.GetUserId(User);
        if (!medicoId.HasValue) return Forbid();
        request.IdMedicoResponsavel = medicoId.Value;
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

        var created = receitaService.Create(request);
        return CreatedAtAction(nameof(FetchById), new { id = created.Id }, created);
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
        var existing = receitaService.FetchById(id);
        if (existing is null) return NotFound();
        if (!ResourceAccess.CanAccessRecord(User, existing)) return Forbid();

        var deleted = receitaService.Delete(id);
        return deleted ? NoContent() : NotFound();
    }
}
