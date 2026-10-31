using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PetCore.Authentication;

public static class MobileLoginResponse
{
    public static object Create(object user, string email, Guid userId, string role, IConfiguration configuration)
    {
        var secret = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key não configurada.");
        if (Encoding.UTF8.GetByteCount(secret) < 32) throw new InvalidOperationException("Jwt:Key deve ter ao menos 32 bytes.");
        var expires = DateTime.UtcNow.AddMinutes(configuration.GetValue("Jwt:ExpirationMinutes", 60));
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role)
        };
        var token = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Audience"], claims,
            expires: expires,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)), SecurityAlgorithms.HmacSha256));
        var encoded = new JwtSecurityTokenHandler().WriteToken(token);
        var response = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object?>>(System.Text.Json.JsonSerializer.Serialize(user, new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web)))!;
        response["token"] = encoded;
        response["accessToken"] = encoded;
        response["tokenType"] = "Bearer";
        response["expiresAtUtc"] = token.ValidTo;
        return response;
    }
}
