using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Implementations.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UUIDNext;

namespace NukeAuthentication.Scr.Features.AuthenticationFeatures.JasonWebTokenGenerator;
public sealed class JwtProvider(IOptions<JwtOptions> jwtOptions) : IJwtProvider
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public Task<string> GerateRefreshToken()
    {
        var randomNumber = new byte[32];
        RandomNumberGenerator.Fill(randomNumber);
        return Task.FromResult(Convert.ToBase64String(randomNumber));
    }

    public Task<string> GerateAccessToken(UserAuthDTO user)
    {
        var claims = new List<Claim>()
        {
            new (JwtRegisteredClaimNames.Sub, $"{user.Id}"),
            new (JwtRegisteredClaimNames.Jti, $"{Uuid.NewSequential()}"),
            new (JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),

            new (ClaimTypes.Role, user.Level.ToString())
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)), SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            null,
            DateTime.UtcNow.AddMinutes(_jwtOptions.ExpireInMinutes),
            signingCredentials);

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwtToken));
    }

}
