using NukeAuthentication.Src.Shared.Base_DTOs;

namespace NukeAuthentication.Scr.Features.AuthenticationFeatures.UserRegister.DTOs;

public record RegisterUserRequest
{
    public PersonDTO Person { get; set; }
    public AddressDTO Address { get; set; }
    public EmailDTO Email { get; set; }
    public PhoneDTO? Phone { get; set; }
    public string Password { get; set; }
}
