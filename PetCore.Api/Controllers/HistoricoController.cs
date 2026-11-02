using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PetCore.Authentication;
using PetCore.Application.DTOs;
using PetCore.Application.Services.Interfaces;
using PetCore.Extensions;
using PetCore.Authorization;

namespace PetCore.Controllers;

[Authorize(Roles = "tutor")]
[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class HistoricoController(IHistoricoService historicoService, IPetService petService) : ControllerBase
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
            var query = ResourceAccess.ScopeRecordPageQuery(
                User, pagination.ToPageQuery(Request.Query), "TutorId", "TutorId");
            return Ok(historicoService.FetchPage(query));
        }

        var tutorId = ResourceAccess.GetUserId(User);
        return tutorId.HasValue
            ? Ok(historicoService.FetchAllByTutorId(tutorId.Value))
            : Forbid();
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
        var item = historicoService.FetchById(id);
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] HistoricoRequest request)
    {
        var tutorId = ResourceAccess.GetUserId(User);
        if (!tutorId.HasValue) return Forbid();

        var petId = request.PetIdMobile ?? request.IdPet;
        var pet = petService.FetchById(petId);
        if (pet is null) return NotFound();
        if (pet.TutorId != tutorId.Value) return Forbid();

        request.IdPet = petId;
        request.PetIdMobile = petId;
        request.TutorId = tutorId;
        request.PetNome = pet.Nome;

        var created = historicoService.Create(request);
        return CreatedAtAction(nameof(FetchById), new { id = created.Id }, created);
    }
    
    /// <summary>
    /// Exclui um registro por Id.
    /// </summary>
    /// <param name="id">Identificador único da avaliação.</param>
    /// <response code="204">Registro excluído com sucesso.</response>
    /// <response code="404">Nenhum registro foi encontrado para o Id informado.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        var existing = historicoService.FetchById(id);
        if (existing is null) return NotFound();
        if (!ResourceAccess.CanAccessRecord(User, existing)) return Forbid();

        var deleted = historicoService.Delete(id);
        return deleted ? NoContent() : NotFound();
    }
}
