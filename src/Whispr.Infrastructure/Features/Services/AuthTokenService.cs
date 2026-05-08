using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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

    public TokenModel GenerateAccessToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        Claim[] claims =
        [
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name)
        ];

        DateTimeOffset tokenExpirationTime = DateTimeOffset.UtcNow.AddMinutes(_options.AccessTokenExpirationInMinutes);

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
            Token = jwt,
            TokenExpirationAtUtc = tokenExpirationTime
        };
    }

    public TokenModel GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var token = Convert.ToBase64String(randomNumber);

        return new TokenModel
        {
            Token = token,
            TokenExpirationAtUtc = DateTimeOffset.UtcNow.AddMinutes(_options.RefreshTokenExpirationInMinutes)
        };
    }

    public ClaimsPrincipal? GetPrincipalFromAccessToken(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Key!)),
            ValidateLifetime = false
        };

        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtToken
            || !jwtToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}