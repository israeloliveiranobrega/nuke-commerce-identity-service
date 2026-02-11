using MediatR;
using NukeAuthentication.Scr.Domain.Entitys;
using NukeAuthentication.Scr.Features.AuthenticationFeatures.UserRegister.DTOs;
using NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Contracts;
using NukeAuthentication.Scr.Shared;
using UUIDNext;

namespace NukeAuthentication.Scr.Features.AuthenticationFeatures.UserRegister;
public class RegisterUserHandler(IUserRepository userRepository) : IRequestHandler<RegisterUserCommand, Result<RegisterUserResponse>>
{
    public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        #region Validations
        if (await userRepository.ExistsByCpfAsync(request.RegisterRequest.Person.Cpf, cancellationToken))
            return Result<RegisterUserResponse>.Failure(FailureType.LoginIcorrect);

        if (await userRepository.ExistsByEmailAsync(request.RegisterRequest.Email.Address, request.RegisterRequest.Email.Domain, cancellationToken))
            return Result<RegisterUserResponse>.Failure(FailureType.LoginIcorrect);

        if (request.RegisterRequest.Phone != null)
            if (await userRepository.ExistsByPhoneAsync(request.RegisterRequest.Phone.CountryCode, request.RegisterRequest.Phone.Number, cancellationToken))
                return Result<RegisterUserResponse>.Failure(FailureType.LoginIcorrect);
        #endregion

        User user = request.RegisterRequest.ToEntity();
        user.Id = Uuid.NewSequential();

        user.CreatedBy = request.WhoCriated?? user.Id;

        Guid guidId = await userRepository.Create(user, cancellationToken);

        return Result<RegisterUserResponse>.Success(new RegisterUserResponse(guidId, user.FullName, user.MaskedEmail));
    } 
}
  