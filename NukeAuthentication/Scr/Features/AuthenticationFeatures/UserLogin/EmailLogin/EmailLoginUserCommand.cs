using MediatR;
using NukeAuthentication.Scr.Domain.ValueObjects.Base;
using NukeAuthentication.Scr.Features.AuthenticationFeatures.UserLogin.EmailLogin.DTOs;
using NukeAuthentication.Scr.Shared;

namespace NukeAuthentication.Scr.Features.AuthenticationFeatures.UserLogin.EmailLogin
{
    public record EmailLoginUserCommand (EmailLoginUserRequest LoginRequest, UserAgentInfo UserAgent) : IRequest<Result<LoginUserResponse>>;
}
