using Microsoft.AspNetCore.Mvc;
using PetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using PetCore.Application.DTOs;
using PetCore.Application.Services.Interfaces;
using PetCore.Extensions;
using PetCore.Authorization;

namespace PetCore.Controllers;

[Authorize]
[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class MedicoController(IMedicoService medicoService, IConfiguration configuration) : ControllerBase
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
        if (ResourceAccess.IsVeterinarian(User) != true)
        {
            if (Request.Query.IsPaginationRequested())
            {
                var page = medicoService.FetchPage(pagination.ToPageQuery(Request.Query));
                var catalogPage = new PagedData<MedicoCatalogResponse>(
                    page.Items.Select(item => new MedicoCatalogResponse(item.Especializacao)).ToList(),
                    page.Page, page.PageSize, page.TotalItems, page.TotalPages);
                return Ok(catalogPage);
            }

            return Ok(medicoService.FetchAll()
                .Select(item => new MedicoCatalogResponse(item.Especializacao))
                .ToList());
        }

        return Request.Query.IsPaginationRequested() ? Ok(medicoService.FetchPage(pagination.ToPageQuery(Request.Query))) : Ok(medicoService.FetchAll());
    }

    /// <summary>Autentica um médico para clientes Mobile legados.</summary>
    [AllowAnonymous]
    [HttpGet("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromQuery] string email, [FromQuery] string senha)
    {
        var user = medicoService.FetchByEmail(email, senha);
        if (user is null) return Unauthorized(new ProblemDetails { Status = 401, Title = "Credenciais inválidas" });
        return Ok(MobileLoginResponse.Create(user, user.Email, user.Id, "veterinario", configuration));
    }
    /// <summary>
    /// Busca um registro por Id.
    /// </summary>
    /// <param name="id">Identificador único da avaliação.</param>
    /// <response code="200">Registro encontrado.</response>
    /// <response code="404">Nenhum registro foi encontrado para o Id informado.</response>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "medico,veterinario")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult FetchById(Guid id)
    {
        if (!ResourceAccess.IsOwnProfile(User, id))
            return Forbid();

        var item = medicoService.FetchById(id);
        return item is null ? NotFound() : Ok(item);
    }

    /// <summary>
    /// Cria um novo registro.
    /// </summary>
    /// <param name="request">Dados para criação de avaliação.</param>
    /// <response code="201">Registro criado com sucesso.</response>
    /// <response code="400">Payload inválido para criação.</response>
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] MedicoRequest request)
    {
        var created = medicoService.Create(request);
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
    public IActionResult Patch(Guid id, [FromBody] UserDadosRequest request)
    {
        if (!ResourceAccess.IsOwnProfile(User, id))
            return Forbid();

        var updated = medicoService.Patch(id, request);
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
        if (!ResourceAccess.IsOwnProfile(User, id))
            return Forbid();

        var deleted = medicoService.Delete(id);
        return deleted ? NoContent() : NotFound();
    }
    
}
