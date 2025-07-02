using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LifeTrack.Core.Interfaces.Services.Security;
using LifeTrack.Core.Models.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace LifeTrack.Services.Security;

public class JwtService : IJwtService
{
    private readonly JwtSettings _settings;

    public JwtService(JwtSettings settings)
    {
        _settings = settings;
    }
    
    public Result<string> GenerateToken(Guid userId)
    {
        try
        {
            var claims = new Claim[]
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, userId.ToString())
            };
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(_settings.ExpiresInHours),
                signingCredentials: creds,
                issuer: _settings.Issuer,
                audience: _settings.Audience
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Result<string>.Success(tokenString);
        }
        catch (Exception e)
        {
            return Result<string>.Failure(e.Message);
        }
    }
}