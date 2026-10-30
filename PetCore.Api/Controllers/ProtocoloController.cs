using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PetCore.Authentication;
using PetCore.Application.DTOs;
using PetCore.Application.Services.Interfaces;
using PetCore.Extensions;

namespace PetCore.Controllers;

[Authorize]
[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class ProtocoloController(IProtocoloService protocoloService) : ControllerBase
{
    [HttpGet] public IActionResult FetchAll([FromQuery] PaginationOptions pagination) => Ok(protocoloService.FetchPage(pagination.ToPageQuery(Request.Query)));
    [HttpGet("{id}")] public IActionResult FetchById(string id) => protocoloService.FetchById(id) is { } item ? Ok(item) : NotFound();
    [HttpPost] public IActionResult Create([FromBody] ProtocoloRequest request) { var created = protocoloService.Create(request); return CreatedAtAction(nameof(FetchById), new { id = created.Id }, created); }
    [HttpPatch("{id}")]
    [HttpPut("{id}/patch")] public IActionResult Patch(string id, ProtocoloDadosRequest request) => protocoloService.Patch(id, request) is { } item ? Ok(item) : NotFound();
    [HttpDelete("{id}")] public IActionResult Delete(string id) => protocoloService.Delete(id) ? NoContent() : NotFound();
}




