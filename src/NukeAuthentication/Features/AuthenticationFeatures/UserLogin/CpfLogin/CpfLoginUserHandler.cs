using MediatR;
using NukeAuthentication.Entitys;
using NukeAuthentication.Features.AuthenticationFeatures.JasonWebTokenGenerator;
using NukeAuthentication.Features.AuthenticationFeatures.UserLogin;
using NukeAuthentication.Services.Repositorys.Contracts;
using NukeAuthentication.Services.Repositorys.Implementations.DTOs;
using NukeAuthentication.Shared;
using NukeAuthentication.Shared.ValueObjects.Base;
using NukeAuthentication.Shared.ValueObjects.Base.Enums;
using UUIDNext;

namespace NukeAuthentication.Features.AuthenticationFeatures.UserLogin.CpfLogin;

public class CpfLoginUserHandler(IUserRepository userRepository, IJwtProvider jwtPreovider, IUserSessionRepository userSessionRepository) : IRequestHandler<CpfLoginUserCommand, Result<LoginUserResponse>>
{
    public async Task<Result<LoginUserResponse>> Handle(CpfLoginUserCommand request, CancellationToken cancellationToken)
    {
        #region Validations 
        if (request.UserAgent.SpiderOrBot == true)
        {
            //Logg de login fraudulento
            return Result<LoginUserResponse>.Failure(FailureType.Fraud);
        }

        UserAuthDTO userAuth = await userRepository.GetUserAuthByCpfAsync(request.LoginRequest.Cpf, cancellationToken);

        if (userAuth is null)
        {
            //logg de login falhos
            return Result<LoginUserResponse>.Failure(FailureType.LoginIcorrect);
        }

        if(userAuth.Status != AccountStatus.Active)
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
        Guid sessionId = await userSessionRepository.GetSessionId(userAuth.Id, request.UserAgent.UserAgentComplete, cancellationToken) ?? Uuid.NewSequential();

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

        return Result<LoginUserResponse>.Success(new LoginUserResponse(userAuth.Id, userAuth.FirsName, accessToken, refreshToken.Token));
        #endregion
    }
}
