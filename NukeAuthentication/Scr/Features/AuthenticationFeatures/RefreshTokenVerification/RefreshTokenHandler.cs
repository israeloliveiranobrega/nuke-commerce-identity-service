using MediatR;
using NukeAuthentication.Scr.Domain.ValueObjects.Base;
using NukeAuthentication.Scr.Domain.ValueObjects.Base.Enums;
using NukeAuthentication.Scr.Features.AuthenticationFeatures.JasonWebTokenGenerator;
using NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Contracts;
using NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Implementations.DTOs;
using NukeAuthentication.Scr.Shared;

namespace NukeAuthentication.Scr.Features.AuthenticationFeatures.RefreshTokenVerification;

public class RefreshTokenHandler(IUserRepository userRepository, IUserSessionRepository userSessionRepository, IJwtProvider jwtPreovider) : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
    public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (request.UserAgent.SpiderOrBot == true)
        {
            //Logg de login fraudulento
            return Result<RefreshTokenResponse>.Failure(FailureType.Fraud);
        }

        #region Confirm Valid Token
        Guid sessionId = await userSessionRepository.GetSessionId(request.TokenRequest.UserId, request.UserAgent.UserAgentComplete, cancellationToken)?? default;

        if(await userSessionRepository.ExistSessionById(sessionId,cancellationToken))
        {
            //logg de login falhos
            if(await userSessionRepository.IsRevoked(sessionId, cancellationToken))
            {
                //logg de login falhos
                return Result<RefreshTokenResponse>.Failure(FailureType.RevokedSession);
            }

            if (await userSessionRepository.IsExpired(sessionId, cancellationToken))
            {
                //logg de login falhos
                return Result<RefreshTokenResponse>.Failure(FailureType.ExpiredToken);
            }
        }

        if (await userSessionRepository.IsDiferenToken(sessionId, request.TokenRequest.CurrentToken, cancellationToken))
        {
            //logg de login falhos
            return Result<RefreshTokenResponse>.Failure(FailureType.Fraud);
        }
        #endregion

        var result = await userRepository.GetUserAuthByIdAsync(request.TokenRequest.UserId, cancellationToken);

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

        await userSessionRepository.UpdateRefreshTokenAsync(sessionId, refreshToken, cancellationToken);

        return Result<RefreshTokenResponse>.Success(new(accessToken, refreshToken.Code));
    }
}
