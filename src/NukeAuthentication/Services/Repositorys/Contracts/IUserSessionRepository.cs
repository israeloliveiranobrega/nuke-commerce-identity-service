using NukeAuthentication.Entitys;
using NukeAuthentication.Shared.ValueObjects.Base;

namespace NukeAuthentication.Services.Repositorys.Contracts;

public interface IUserSessionRepository
{
    Task<Guid> Create(UserSession userSession, CancellationToken cancellationToken = default);
    Task<Guid?> GetSessionId(Guid userId, string UserAgent, CancellationToken cancellationToken = default);
    Task<bool> IsDiferenToken(Guid sessionId, string RefreshToken, CancellationToken cancellationToken = default);
    Task<Guid?> GetUserId(Guid sessionId, CancellationToken cancellationToken = default);
    Task<int> UpdateRefreshTokenAsync(Guid sessionId, RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task<int> RevokeRefreshTokenAsync(Guid usersessionIdId, CancellationToken cancellationToken = default);
    Task<bool> ExistSessionById(Guid sessionId, CancellationToken cancellationToken = default);
    Task<bool> IsRevoked(Guid sessionId, CancellationToken cancellationToken = default);
    Task<bool> IsExpired(Guid sessionId, CancellationToken cancellationToken = default);
}
