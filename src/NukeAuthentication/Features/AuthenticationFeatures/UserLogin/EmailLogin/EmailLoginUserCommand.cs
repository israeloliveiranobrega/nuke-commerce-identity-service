using MediatR;
using NukeAuthentication.Features.AuthenticationFeatures.UserLogin;
using NukeAuthentication.Features.AuthenticationFeatures.UserLogin.EmailLogin.DTOs;
using NukeAuthentication.Shared;
using NukeAuthentication.Shared.ValueObjects.Base;

namespace NukeAuthentication.Features.AuthenticationFeatures.UserLogin.EmailLogin
{
    public record EmailLoginUserCommand (EmailLoginUserRequest LoginRequest, UserAgentInfo UserAgent) : IRequest<Result<LoginUserResponse>>;
}
