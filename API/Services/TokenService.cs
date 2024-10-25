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
        new Claim(ClaimTypes.NameIdentifier, usuario.Email),
        new Claim(ClaimTypes.Role, usuario.Rol_id.ToString()),
        new Claim("es_administrador", usuario.EsAdministrador ? "True" : "False") // Agregar claim de administrador
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


}
