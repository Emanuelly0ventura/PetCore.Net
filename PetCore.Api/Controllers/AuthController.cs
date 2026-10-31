using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetCore.Application.Services.Interfaces;
using PetCore.Authentication;

namespace PetCore.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(ITutorService tutors, IMedicoService medicos, IConfiguration configuration) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login(LoginRequest request)
    {
        var role = request.Role.Trim().ToLowerInvariant();
        Guid? userId = role switch
        {
            "tutor" => tutors.FetchByEmail(request.Email, request.Password)?.Id,
            "medico" or "veterinario" => medicos.FetchByEmail(request.Email, request.Password)?.Id,
            _ => null
        };
        if (userId is null) return Unauthorized(new ProblemDetails { Status = 401, Title = "Credenciais inválidas" });
        var secret = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key não configurada.");
        if (Encoding.UTF8.GetByteCount(secret) < 32) throw new InvalidOperationException("Jwt:Key deve ter ao menos 32 bytes.");
        var claims = new[] { new Claim(JwtRegisteredClaimNames.Sub, userId.Value.ToString()), new Claim(JwtRegisteredClaimNames.Email, request.Email), new Claim(ClaimTypes.Role, role) };
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var token = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddMinutes(configuration.GetValue("Jwt:ExpirationMinutes", 60)), signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
        return Ok(new { accessToken = new JwtSecurityTokenHandler().WriteToken(token), tokenType = "Bearer", expiresAtUtc = token.ValidTo });
    }
}
