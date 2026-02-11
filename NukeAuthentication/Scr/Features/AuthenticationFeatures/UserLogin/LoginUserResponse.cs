namespace NukeAuthentication.Scr.Features.AuthenticationFeatures.UserLogin;

public record LoginUserResponse(Guid UserId, string AccessToken, string RefreshToken);
