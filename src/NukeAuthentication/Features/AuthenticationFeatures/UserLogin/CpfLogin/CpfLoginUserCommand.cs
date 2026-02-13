using MediatR;
using NukeAuthentication.Features.AuthenticationFeatures.UserLogin;
using NukeAuthentication.Features.AuthenticationFeatures.UserLogin.CpfLogin.DTOs;
using NukeAuthentication.Shared;
using NukeAuthentication.Shared.ValueObjects.Base;

namespace NukeAuthentication.Features.AuthenticationFeatures.UserLogin.CpfLogin;

public record CpfLoginUserCommand (CpfLoginUserRequest LoginRequest, UserAgentInfo UserAgent) : IRequest<Result<LoginUserResponse>>;
