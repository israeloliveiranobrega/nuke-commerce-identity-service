using MediatR;
using NukeAuthentication.Scr.Domain.ValueObjects.Base;
using NukeAuthentication.Scr.Shared;

namespace NukeAuthentication.Scr.Features.AuthenticationFeatures.RefreshTokenVerification;

public record RefreshTokenCommand(RefreshTokenRequest TokenRequest, UserAgentInfo UserAgent) : IRequest<Result<RefreshTokenResponse>>;
