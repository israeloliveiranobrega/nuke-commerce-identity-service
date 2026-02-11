using MediatR;
using NukeAuthentication.Scr.Domain.Entitys;
using NukeAuthentication.Scr.Domain.ValueObjects.Base;
using NukeAuthentication.Scr.Domain.ValueObjects.Base.Enums;
using NukeAuthentication.Scr.Features.AuthenticationFeatures.JasonWebTokenGenerator;
using NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Contracts;
using NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Implementations.DTOs;
using NukeAuthentication.Scr.Shared;
using UUIDNext;

namespace NukeAuthentication.Scr.Features.AuthenticationFeatures.UserLogin.EmailLogin;
public class EmailLoginUserHandler(IUserRepository userRepository, IJwtProvider jwtPreovider, IUserSessionRepository userSessionRepository) : IRequestHandler<EmailLoginUserCommand, Result<LoginUserResponse>>
{
    public async Task<Result<LoginUserResponse>> Handle(EmailLoginUserCommand request, CancellationToken cancellationToken)
    {
        #region Validations 
        if (request.UserAgent.SpiderOrBot == true)
        {
            //Logg de login fraudulento
            return Result<LoginUserResponse>.Failure(FailureType.Fraud);
        }

        UserAuthDTO userAuth = await userRepository.GetUserAuthByEmailAsync(request.LoginRequest.EmailAddress, request.LoginRequest.EmailDomain, cancellationToken);

        if (userAuth is null)
        {
            //logg de login falhos
            return Result<LoginUserResponse>.Failure(FailureType.LoginIcorrect);
        }

        if (userAuth.Status != AccountStatus.Active)
        {
            //logg de login falhos
            return Result<LoginUserResponse>.Failure(FailureType.LoginIcorrect);
        }

        if (!Password.Validate(request.LoginRequest.Password, userAuth.PasswordHash))
        {
            //logg de login falhos
            return Result<LoginUserResponse>.Failure(FailureType.LoginIcorrect);
        }
        #endregion

        #region Authentication



        #endregion

        #region Session
        Guid sessionId = await userSessionRepository.GetSessionId(userAuth.Id, request.UserAgent.UserAgentComplete, cancellationToken)?? Uuid.NewSequential();

        string accessToken = await jwtPreovider.GerateAccessToken(userAuth);
        RefreshToken refreshToken = new(await jwtPreovider.GerateRefreshToken(), DateTime.UtcNow.AddDays(7));

        if (await userSessionRepository.ExistSessionById(sessionId, cancellationToken))
        {
            await userSessionRepository.UpdateRefreshTokenAsync(sessionId, refreshToken, cancellationToken);
            //log de login bem sucedido com sessão existente
        }
        else
        {
            UserSession userSession = new(userAuth.Id, request.UserAgent, refreshToken);
            await userSessionRepository.Create(userSession, cancellationToken);
            //log de login bem sucedido com nova sessão
        }

        return Result<LoginUserResponse>.Success(new LoginUserResponse(userAuth.Id, accessToken, refreshToken.Code));
        #endregion
    }
}
