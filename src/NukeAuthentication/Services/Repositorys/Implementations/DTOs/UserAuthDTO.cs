using NukeProjectUtils.ValueObjects.Base.Enums;

namespace NukeAuthentication.Services.Repositorys.Implementations.DTOs;

public record UserAuthDTO(Guid Id, string FirsName, string LastName, string EmailAddress, 
    string EmailDomain, string Cpf, string PasswordHash, AccountStatus Status, Role Level);
