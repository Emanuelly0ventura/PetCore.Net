using Microsoft.AspNetCore.Mvc;
using PetCore.Application.DTOs;
using PetCore.Application.Services.Interfaces;

namespace PetCore.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class TutorController(ITutorService tutorService) : ControllerBase
{
    /// <summary>
    /// Lista todos os registros.
    /// </summary>
    /// <response code="200">Registros retornados com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult FetchAll()
    {
        return Ok(tutorService.FetchAll());
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
        var item = tutorService.FetchById(id);
        return item is null ? NotFound() : Ok(item);
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
    public IActionResult Create([FromBody] TutorRequest request)
    {
        var created = tutorService.Create(request);
        return CreatedAtAction(nameof(FetchById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Atualiza somente a nota da avaliação.
    /// </summary>
    /// <param name="id">Identificador único da avaliação.</param>
    /// <param name="request">Nova nota da avaliação.</param>
    /// <response code="200">Nota atualizada com sucesso.</response>
    /// <response code="404">Nenhum registro foi encontrado para o Id informado.</response>
    [HttpPut("{id:guid}/patch")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Patch(Guid id, [FromBody] UserDadosRequest request)
    {
        var updated = tutorService.Patch(id, request);
        return updated is null ? NotFound() : Ok(updated);
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
        var deleted = tutorService.Delete(id);
        return deleted ? NoContent() : NotFound();
    }
    
    /// <summary>
    /// Busca um tutor por email e senha.
    /// </summary>
    /// <param name="email">Email do tutor.</param>
    /// <param name="senha">Senha do tutor.</param>
    /// <response code="200">Tutor encontrado com sucesso.</response>
    /// <response code="404">Nenhum tutor encontrado com as credenciais informadas.</response>
    [HttpGet("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult FetchByEmail([FromQuery] string email, [FromQuery] string senha)
    {
        var tutor = tutorService.FetchByEmail(email, senha);

        return tutor is null ? NotFound() : Ok(tutor);
    }
}