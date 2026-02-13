namespace NukeAuthentication.Features.AuthenticationFeatures.RefreshTokenVerification;

public record RefreshTokenRequest(Guid UserId, string CurrentToken);
