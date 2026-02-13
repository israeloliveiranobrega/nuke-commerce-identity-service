using MediatR;
using NukeAuthentication.Entitys;
using NukeAuthentication.Features.AuthenticationFeatures.UserRegister.DTOs;
using NukeAuthentication.Services.Repositorys.Contracts;
using NukeAuthentication.Shared;
using UUIDNext;

namespace NukeAuthentication.Features.AuthenticationFeatures.UserRegister;
public class RegisterUserHandler(IUserRepository userRepository) : IRequestHandler<RegisterUserCommand, Result<RegisterUserResponse>>
{
    public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        #region Validations
        if (await userRepository.ExistsByCpfAsync(request.RegisterRequest.Person.Cpf, cancellationToken))
            return Result<RegisterUserResponse>.Failure(FailureType.BadRequest);

        if (await userRepository.ExistsByEmailAsync(request.RegisterRequest.Email.Address, request.RegisterRequest.Email.Domain, cancellationToken))
            return Result<RegisterUserResponse>.Failure(FailureType.BadRequest);

        if (request.RegisterRequest.Phone != null)
            if (await userRepository.ExistsByPhoneAsync(request.RegisterRequest.Phone.RegionCode, request.RegisterRequest.Phone.Number, cancellationToken))
                return Result<RegisterUserResponse>.Failure(FailureType.BadRequest);
        #endregion

        User user = request.RegisterRequest.ToEntity();

        user.Id = Uuid.NewSequential();

        user.CreatedBy = request.WhoCriated?? user.Id;

        Guid guidId = await userRepository.Create(user, cancellationToken);

        return Result<RegisterUserResponse>.Success(new RegisterUserResponse(user.Id, user.FirstName));
    } 
}
  