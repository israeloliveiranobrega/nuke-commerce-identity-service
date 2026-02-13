using MediatR;
using Microsoft.EntityFrameworkCore;
using NukeAuthentication.Data;
using NukeAuthentication.Entitys;
using NukeAuthentication.Features.AuthenticationFeatures.JasonWebTokenGenerator;
using NukeAuthentication.Services.Repositorys.Contracts;
using NukeAuthentication.Services.Repositorys.Implementations.DTOs;
using NukeProjectUtils.ContainerTypes;
using NukeProjectUtils.ValueObjects.Base;
using NukeProjectUtils.ValueObjects.Base.Enums;
using UAParser;

namespace NukeAuthentication.Features.AuthenticationFeatures.RefreshTokenVerification;

public class RefreshTokenHandler(DataContext dataContext, IJwtProvider jwtPreovider) : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
    private readonly DbSet<UserSession> _session = dataContext.UserSessions;
    private readonly DbSet<User> _user = dataContext.Users;
    public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (request.UserAgent.SpiderOrBot == true)
        {
            //Logg de login fraudulento
            return Result<RefreshTokenResponse>.Failure(FailureType.Fraud);
        }

        #region Confirm Valid Token
        Guid sessionId = await _session.AsNoTracking()
            .Where(x => x.UserId == request.TokenRequest.UserId && x.UserAgent.UserAgentComplete == request.UserAgent)
            .Select(x => (Guid?)x.Id).FirstOrDefaultAsync(cancellationToken)?? default;

        if(await _session.AsNoTracking().AnyAsync(x => x.Id == sessionId, cancellationToken))
        {
            //logg de login falhos
            if(await _session.AsNoTracking().Where(x => x.Id == sessionId).Select(x => x.Revoked).FirstOrDefaultAsync(cancellationToken))
            {
                //logg de login falhos
                return Result<RefreshTokenResponse>.Failure(FailureType.RevokedSession);
            }

            if (await _session.AsNoTracking().Where(x => x.Id == sessionId).Select(x => x.RefreshToken.ExpiresOn < DateTime.UtcNow).FirstOrDefaultAsync(cancellationToken))
            {
                //logg de login falhos
                return Result<RefreshTokenResponse>.Failure(FailureType.ExpiredToken);
            }
        }

        var storedTokenCode = await _session
            .AsNoTracking()
            .Where(x => x.Id == sessionId)
            .Select(x => x.RefreshToken.Token)
            .FirstOrDefaultAsync(cancellationToken);

        if (storedTokenCode == null)
            return Result<RefreshTokenResponse>.Failure(FailureType.Fraud);

        if (storedTokenCode != request.TokenRequest.CurrentToken)
        {
            //logg de login falhos
            return Result<RefreshTokenResponse>.Failure(FailureType.Fraud);
        }
        #endregion

        User? result = await _user.AsNoTracking().Where(u => u.Id == request.TokenRequest.UserId).FirstOrDefaultAsync(cancellationToken);

        if (result is null)
            return Result<RefreshTokenResponse>.Failure(FailureType.LoginIcorrect);

        UserAuthDTO userAuth = result;

        if (userAuth.Status != AccountStatus.Active)
        {
            //logg de login falhos
            return Result<RefreshTokenResponse>.Failure(FailureType.LoginIcorrect);
        }

        string accessToken = await jwtPreovider.GerateAccessToken(userAuth);
        RefreshToken refreshToken = new(await jwtPreovider.GerateRefreshToken(), DateTime.UtcNow.AddDays(7));

        await _session.Where(u => u.Id == sessionId).ExecuteUpdateAsync(u => u
                .SetProperty(u => u.RefreshToken.Token, refreshToken.Token)
                .SetProperty(u => u.RefreshToken.CreatedAt, DateTime.UtcNow)
                .SetProperty(u => u.RefreshToken.ExpiresOn, refreshToken.ExpiresOn)
                .SetProperty(u => u.Revoked, false),
                cancellationToken);

        return Result<RefreshTokenResponse>.Success(new(accessToken, refreshToken.Token));
    }
}
