namespace NukeAuthentication.Scr.Domain.ValueObjects.Base;
public record RefreshToken
{
    public string Code { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime ExpiresOn { get; init; }

    private RefreshToken() { }

    public RefreshToken(string code, DateTime expiresOn)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("O Refresh Token não pode ser vazio.", nameof(code));

        if (expiresOn <= CreatedAt)
            throw new ArgumentException($"A data de expiração ({expiresOn}) deve ser maior que agora ({CreatedAt}).", nameof(expiresOn));

        Code = code;
        ExpiresOn = expiresOn;
    }
}
