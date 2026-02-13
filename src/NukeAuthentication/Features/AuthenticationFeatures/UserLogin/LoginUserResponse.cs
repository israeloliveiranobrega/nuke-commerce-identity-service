namespace NukeAuthentication.Features.AuthenticationFeatures.UserLogin;

public record LoginUserResponse(Guid UserId, string FirstName, string AccessToken, string RefreshToken);
