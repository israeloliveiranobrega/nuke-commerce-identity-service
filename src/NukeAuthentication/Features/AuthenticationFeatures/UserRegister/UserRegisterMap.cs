using NukeAuthentication.Entitys;
using NukeAuthentication.Features.AuthenticationFeatures.UserRegister.DTOs;
using NukeAuthentication.Shared.ValueObjects;
using NukeAuthentication.Shared.ValueObjects.Base;

namespace NukeAuthentication.Features.AuthenticationFeatures.UserRegister;

public static class UserRegisterMap
{
    public static User ToEntity(this RegisterUserRequest request)
    {
        Person person = new(new(request.Person.FirstName, request.Person.LastName),
            request.Person.BirthDate,new(request.Person.Cpf, false));

        Address address = new(request.Address.ZipCode,
            request.Address.Region,
            request.Address.State,
            request.Address.City,
            request.Address.Neighborhood,
            request.Address.Street,
            request.Address.Number,
            request.Address.Complement);

        VerificationCode emailVerification = default;
        Email email = new(request.Email.Address, request.Email.Domain, emailVerification);

        VerificationCode phoneVerification = default;
        Phone phone = new(request.Phone.RegionCode, request.Phone.Number, phoneVerification);

        Password password = new(request.Password);

        return new User(person, address, email, password, phone);
    }
}
