using MediatR;
using NukeAuthentication.Shared;
using NukeAuthentication.Shared.ValueObjects.Base;

namespace NukeAuthentication.Features.AuthenticationFeatures.RefreshTokenVerification;

public record RefreshTokenCommand(RefreshTokenRequest TokenRequest, UserAgentInfo UserAgent) : IRequest<Result<RefreshTokenResponse>>;
