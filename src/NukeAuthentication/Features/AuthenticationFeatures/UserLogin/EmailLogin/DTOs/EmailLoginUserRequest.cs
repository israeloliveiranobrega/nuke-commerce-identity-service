namespace NukeAuthentication.Features.AuthenticationFeatures.UserLogin.EmailLogin.DTOs
{
    public record EmailLoginUserRequest(string EmailAddress, string EmailDomain, string Password);
}
