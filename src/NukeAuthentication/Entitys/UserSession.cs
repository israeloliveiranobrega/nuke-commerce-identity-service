using NukeProjectUtils.ValueObjects.Base;
using UUIDNext;

namespace NukeAuthentication.Entitys;

public record UserSession
{
    public Guid Id { get; set; } = Uuid.NewSequential();
    public Guid UserId { get; init; }
    //public Guid? LoginAttemptId { get; in
    public UserAgentInfo UserAgent { get; init; }
    public RefreshToken RefreshToken { get; init; }

    private UserSession() { }

    public UserSession(Guid userId, UserAgentInfo userAgent, RefreshToken refreshToken, bool revoked = false)
    {
        UserId = userId;
        UserAgent = userAgent;
        RefreshToken = refreshToken;
        Revoked = revoked;

        if (DateTime.UtcNow >= ExpiresOn)
            IsExpired = true;

        if (!Revoked && !IsExpired)
            IsActive = true;
    }

    #region Refresh Token
    public string Token => RefreshToken.Token;
    public DateTime CreatedAt => RefreshToken.CreatedAt;
    public DateTime ExpiresOn => RefreshToken.ExpiresOn;

    public string? LastToken { get; set; }
    public DateTime RenewedAt { get; set; }

    public bool Revoked { get; set; }

    public bool IsExpired { get; set; }
    public bool IsActive { get; set; }
    #endregion

    #region UserAgent
    public string UserAgentComplete => UserAgent.UserAgentComplete;
    public string? Browser => UserAgent.Browser;
    public string? BrowserMajor => UserAgent.BrowserMajor;
    public string? System => UserAgent.System;
    public string? SystemMajor => UserAgent.SystemMajor;
    public string? Device => UserAgent.Device;
    public string? DeviceBrand => UserAgent.DeviceBrand;
    #endregion
}
