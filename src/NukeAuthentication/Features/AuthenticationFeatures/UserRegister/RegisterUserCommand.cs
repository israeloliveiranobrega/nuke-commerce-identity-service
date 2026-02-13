using MediatR;
using NukeAuthentication.Features.AuthenticationFeatures.UserRegister.DTOs;
using NukeAuthentication.Shared;

namespace NukeAuthentication.Features.AuthenticationFeatures.UserRegister;

public record RegisterUserCommand(RegisterUserRequest RegisterRequest, Guid? WhoCriated) : IRequest<Result<RegisterUserResponse>>;
       
