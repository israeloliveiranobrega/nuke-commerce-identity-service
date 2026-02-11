namespace NukeAuthentication.Scr.Features.AuthenticationFeatures.UserRegister.DTOs;

public record RegisterUserResponse(Guid UserId, string Name, string EmailSecret);
