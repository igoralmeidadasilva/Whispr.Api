using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Whispr.Application.Core.Options;
using Whispr.Domain.Core.Services;
using Whispr.Domain.Features.Entities.User;
using Whispr.Domain.Features.Models;

namespace Whispr.Infrastructure.Features.Services;

public sealed class AuthTokenService : IAuthTokenService
{
    private readonly JwtAuthenticationOptions _options;

    public AuthTokenService(IOptions<JwtAuthenticationOptions> jwtAuthenticationOptions)
    {
        _options = jwtAuthenticationOptions.Value;
    }

    public TokenModel GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        Claim[] claims = 
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name)
        };

        DateTimeOffset tokenExpirationTime = DateTimeOffset.UtcNow.AddMinutes(_options.TokenExpirationInMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = tokenExpirationTime.DateTime,
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return new TokenModel
        {
            AccessToken = jwt,
            AccessTokenExpirationAtUtc = tokenExpirationTime
        };
    }
}