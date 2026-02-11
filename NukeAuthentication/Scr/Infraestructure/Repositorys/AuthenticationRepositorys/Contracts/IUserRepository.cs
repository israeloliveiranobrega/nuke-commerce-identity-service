using NukeAuthentication.Scr.Domain.Entitys;
using NukeAuthentication.Scr.Domain.ValueObjects.Base.Enums;
using NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Implementations.DTOs;

namespace NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Contracts;

public interface IUserRepository
{
    Task<Guid> Create(User user, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string address, string domain, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCpfAsync(string cpf, CancellationToken cancellationToken = default);
    Task<bool> ExistsByPhoneAsync(string countryCode, string number, CancellationToken cancellationToken = default);

    Task<UserAuthDTO?> GetUserAuthByEmailAsync(string address, string domain, CancellationToken cancellationToken = default);
    Task<UserAuthDTO?> GetUserAuthByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserAuthDTO?> GetUserAuthByCpfAsync(string cpf, CancellationToken cancellationToken = default);

    Task<User?> GetUserByEmailAsync(string address, string domain, CancellationToken cancellationToken = default);
    Task<User?> GetUserByCpfAsync(string cpf, CancellationToken cancellationToken = default);

    Task<AccountStatus?> GetStatusAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<int> Change(User user, Guid whoEdited, CancellationToken cancellationToken = default);
    Task<int> Delete(User user, Guid whoEdited, CancellationToken cancellationToken = default);
    Task<int> Suspend(User user, Guid whoEdited, CancellationToken cancellationToken = default);
    Task<int> Active(User user, Guid whoEdited, CancellationToken cancellationToken = default);
}
