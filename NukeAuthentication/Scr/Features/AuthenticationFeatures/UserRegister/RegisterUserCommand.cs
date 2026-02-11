using MediatR;
using NukeAuthentication.Scr.Features.AuthenticationFeatures.UserRegister.DTOs;
using NukeAuthentication.Scr.Shared;

namespace NukeAuthentication.Scr.Features.AuthenticationFeatures.UserRegister;

public record RegisterUserCommand(RegisterUserRequest RegisterRequest, Guid? WhoCriated) : IRequest<Result<RegisterUserResponse>>;
       
