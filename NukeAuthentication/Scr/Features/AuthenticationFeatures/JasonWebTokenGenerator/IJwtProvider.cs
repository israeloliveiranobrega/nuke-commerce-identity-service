using NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Implementations.DTOs;

namespace NukeAuthentication.Scr.Features.AuthenticationFeatures.JasonWebTokenGenerator;

public interface IJwtProvider
{
    Task<string> GerateAccessToken(UserAuthDTO user);
    Task<string> GerateRefreshToken();
}
