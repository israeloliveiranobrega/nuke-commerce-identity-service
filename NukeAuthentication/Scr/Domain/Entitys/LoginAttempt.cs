using NukeAuthentication.Scr.Domain.ValueObjects.Base;
using UUIDNext;

namespace NukeAuthentication.Scr.Domain.Entitys;

public record LoginAttempt
{
    public Guid Id { get; set; } = Uuid.NewSequential();
    public Guid UserId { get; set; }
    public DateTime SessionTime { get; set; }
    public bool IsSuccess { get; set; }

    public bool IsBot { get; set; }
    public string? ASN { get; set; }
    public string? IpAddress { get; set; }
    public UserAgentInfo? UserAgent { get; set; }
    public Coordinates? Coordinates { get; set; }
}
