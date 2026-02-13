using NukeAuthentication.Services.Repositorys.Implementations.DTOs;

namespace NukeAuthentication.Features.AuthenticationFeatures.JasonWebTokenGenerator;

public interface IJwtProvider
{
    Task<string> GerateAccessToken(UserAuthDTO user);
    Task<string> GerateRefreshToken();
}
