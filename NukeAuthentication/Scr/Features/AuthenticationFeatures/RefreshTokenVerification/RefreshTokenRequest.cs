namespace NukeAuthentication.Scr.Features.AuthenticationFeatures.RefreshTokenVerification;

public record RefreshTokenRequest(Guid UserId, string CurrentToken);
