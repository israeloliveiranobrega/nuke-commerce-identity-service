using NukeAuthentication.Scr.Domain.ValueObjects.Base.Enums;

namespace NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Implementations.DTOs;

public record UserAuthDTO(Guid Id, string FirsName, string LastName, string EmailAddress, 
    string EmailDomain, string Cpf, string PasswordHash, AccountStatus Status, AccessLevel Level);
