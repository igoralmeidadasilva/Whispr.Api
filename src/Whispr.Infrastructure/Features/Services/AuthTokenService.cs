using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Whispr.Application.Core.Options;
using Whispr.Application.Core.Services;
using Whispr.Domain.Features.Entities.Users;
using Whispr.Domain.Features.Models;

namespace Whispr.Infrastructure.Features.Services;

internal sealed class AuthTokenService : IAuthTokenService
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
            new(ClaimTypes.Sid, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name)
        ];

        DateTime tokenExpirationTime = DateTime.UtcNow.AddMinutes(_options.AccessTokenExpirationInMinutes);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = tokenExpirationTime,
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
        byte[] randomBytes = new byte[64];
        RandomNumberGenerator.Fill(randomBytes);
        string token = WebEncoders.Base64UrlEncode(randomBytes);

        return new TokenModel
        {
            Token = token,
            TokenExpirationAtUtc = DateTimeOffset.UtcNow.AddDays(_options.RefreshTokenExpirationInDays)
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

    public IEnumerable<Claim> GetClaimsFromAccessToken(string accessToken)
    {
        JwtSecurityTokenHandler handler = new();
        JwtSecurityToken jsonToken = handler.ReadJwtToken(accessToken);

        return jsonToken.Claims;
    }
}