using MediatR;
using NukeAuthentication.Scr.Domain.ValueObjects.Base;
using NukeAuthentication.Scr.Features.AuthenticationFeatures.UserLogin.CpfLogin.DTOs;
using NukeAuthentication.Scr.Shared;

namespace NukeAuthentication.Scr.Features.AuthenticationFeatures.UserLogin.CpfLogin;

public record CpfLoginUserCommand (CpfLoginUserRequest LoginRequest, UserAgentInfo UserAgent) : IRequest<Result<LoginUserResponse>>;
