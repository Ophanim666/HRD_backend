using DTO.Usuario;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;

    public TokenService(string secretKey, string issuer, string audience)
    {
        _secretKey = secretKey;
        _issuer = issuer;
        _audience = audience;
    }

    public string GenerateJwtToken(UsuarioTokenDTO usuario)
    {
        var claims = new List<Claim>
    {
        new Claim("user_id", usuario.Id.ToString()), //esta calim es la supuesata calim que nos debe ayudar a poner el id del usaurio qeu ingresa para busacar donde estan los grupo de tarea que le corresponde
        new Claim(ClaimTypes.NameIdentifier, usuario.Email),
        new Claim("es_administrador", usuario.EsAdministrador == 1 ? "True" : "False") // Se compara con 1
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddMinutes(60);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: expiry,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    // Obtener los datos del token
    public string GetUserEmailFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        try
        {
            var jwtToken = handler.ReadJwtToken(token);  // Usar ReadJwtToken para JWT
            var emailClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return emailClaim;
        }
        catch (Exception ex)
        {
            // Si hay un error al leer el token, puedes manejarlo de alguna manera
            // Ejemplo: Registrar el error o lanzar una excepción personalizada
            throw new InvalidOperationException("Token inválido o malformado.", ex);
        }
    }

    // Nueva función para obtener el Id del usuario desde el token
    public int? GetUserIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        try
        {
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "user_id")?.Value;

            // Verificar si el user_id es un valor válido y convertirlo
            if (int.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            return null;
        }
        catch (Exception ex)
        {
            // Manejar el error de forma apropiada
            throw new InvalidOperationException("Token inválido o malformado.", ex);
        }
    }

}
