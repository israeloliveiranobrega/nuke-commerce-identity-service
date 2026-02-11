using Microsoft.EntityFrameworkCore;
using NukeAuthentication.Scr.Domain.Entitys;
using NukeAuthentication.Scr.Domain.ValueObjects.Base;
using NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Contracts;

namespace NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Implementations;

public class UserSessionRepository(DataContext dataContext) : IUserSessionRepository
{
    private readonly DbSet<UserSession> _session = dataContext.UserSessions;

    public async Task<int> UpdateRefreshTokenAsync(Guid sessionId, RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        return await _session
            .Where(u => u.Id == sessionId)
            .ExecuteUpdateAsync(u => u
                .SetProperty(u => u.RefreshToken.Code, refreshToken.Code)
                .SetProperty(u => u.RefreshToken.CreatedAt, DateTime.UtcNow)
                .SetProperty(u => u.RefreshToken.ExpiresOn, refreshToken.ExpiresOn)
                .SetProperty(u => u.Revoked, false),
                cancellationToken);
    }
    public async Task<int> RevokeRefreshTokenAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        return await _session
            .Where(u => u.Id == sessionId)
            .ExecuteUpdateAsync(u => u
                .SetProperty(u => u.RefreshToken.Code, string.Empty)
                .SetProperty(u => u.RefreshToken.CreatedAt, null)
                .SetProperty(u => u.RefreshToken.ExpiresOn, null)
                .SetProperty(u => u.Revoked, true),
                cancellationToken);
    }

    public async Task<Guid> Create(UserSession userSession, CancellationToken cancellationToken = default)
    {
        await _session.AddAsync(userSession, cancellationToken);
        await dataContext.SaveChangesAsync(cancellationToken);
        return userSession.Id;
    }
    public async Task<Guid?> GetSessionId(Guid userId, string UserAgent, CancellationToken cancellationToken = default)
    {
        Guid? result = await _session
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.UserAgent.UserAgentComplete == UserAgent)
            .Select(x => (Guid?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if(result == default)
            result = null;

        return result;
    }

    public async Task<bool> ExistSessionById(Guid sessionId, CancellationToken cancellationToken = default)
    {
        return await _session
            .AsNoTracking()
            .AnyAsync(x => x.Id == sessionId, cancellationToken);
    }
    public async Task<bool> IsRevoked(Guid sessionId, CancellationToken cancellationToken = default)
    {
        return await _session
            .AsNoTracking()
            .Where(x => x.Id == sessionId)
            .Select(x => x.Revoked)
            .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<bool> IsExpired(Guid sessionId, CancellationToken cancellationToken = default)
    {
        return await _session
            .AsNoTracking()
            .Where(x => x.Id == sessionId)
            .Select(x => x.RefreshToken.ExpiresOn < DateTime.UtcNow)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsDiferenToken(Guid sessionId, string code, CancellationToken cancellationToken = default)
    {
        var storedTokenCode = await _session
        .AsNoTracking()
        .Where(x => x.Id == sessionId)
        .Select(x => x.RefreshToken.Code) 
        .FirstOrDefaultAsync(cancellationToken);

        if (storedTokenCode == null)
            return true;

        return storedTokenCode != code;
    }

    public async Task<Guid?> GetUserId(string RefreshToken, CancellationToken cancellationToken = default)
    {
        return await _session
            .AsNoTracking()
            .Where(x => x.RefreshToken.Code == RefreshToken)
            .Select(x => (Guid?)x.UserId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<Guid?> GetUserId(Guid sessionId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
