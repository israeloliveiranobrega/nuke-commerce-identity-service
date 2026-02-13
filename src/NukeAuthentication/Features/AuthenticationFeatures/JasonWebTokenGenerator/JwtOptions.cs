namespace NukeAuthentication.Features.AuthenticationFeatures.JasonWebTokenGenerator;

public record JwtOptions
{
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public int ExpireInMinutes { get; init; } 
}
