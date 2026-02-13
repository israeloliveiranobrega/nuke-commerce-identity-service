using Microsoft.EntityFrameworkCore;
using NukeAuthentication.Entitys;
using NukeAuthentication.Services.Repositorys.Implementations.DTOs;
using NukeAuthentication.Shared.ValueObjects.Base.Enums;

namespace NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Implementations.UserRepo;

public partial class UserRepository
{
    #region Existence checkers
    public Task<bool> ExistsByEmailAsync(string address, string domain, CancellationToken cancellationToken = default)
    {
        return _user
           .AsNoTracking()
           .AnyAsync(x => x.Email.Address == address && x.Email.Domain == domain, cancellationToken);
    }
    public Task<bool> ExistsByCpfAsync(string cpf, CancellationToken cancellationToken = default)
    {
        var test = _user
            .AsNoTracking()
            .AnyAsync(user => user.Person.Cpf.UnformattedCpf == cpf, cancellationToken);

        return test;
    }
    public Task<bool> ExistsByPhoneAsync(string countryCode, string number, CancellationToken cancellationToken = default)
    {
        return _user
            .AsNoTracking()
            .AnyAsync(u => u.Phone.RegionCode == countryCode && u.Phone.Number == number, cancellationToken);
    }
    #endregion

    public Task<UserAuthDTO?> GetUserAuthByEmailAsync(string address, string domain, CancellationToken cancellationToken = default)
    {
        return _user
            .AsNoTracking()
            .Where(u => u.Email.Address == address && u.Email.Domain == domain)
            .Select(u => new UserAuthDTO(
                u.Id,
                u.Person.FirstName,
                u.Person.LastName,
                u.Email.Address,
                u.Email.Domain,
                u.Person.Cpf.UnformattedCpf,
                u.Password.Hash,
                u.Status,
                u.Role))
            .FirstOrDefaultAsync(cancellationToken);
        throw new NotImplementedException();
    }
    public Task<UserAuthDTO?> GetUserAuthByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _user
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new UserAuthDTO(
                u.Id,
                u.Person.FirstName,
                u.Person.LastName,
                u.Email.Address,
                u.Email.Domain,
                u.Person.Cpf.UnformattedCpf,
                u.Password.Hash,
                u.Status,
                u.Role))
            .FirstOrDefaultAsync(cancellationToken);
        throw new NotImplementedException();
    }
    public Task<UserAuthDTO?> GetUserAuthByCpfAsync(string cpf, CancellationToken cancellationToken = default)
    {
        return _user
            .AsNoTracking()
            .Where(u => u.Person.Cpf.UnformattedCpf == cpf)
            .Select(u => new UserAuthDTO(
                u.Id,
                u.Person.FirstName,
                u.Person.LastName,
                u.Email.Address,
                u.Email.Domain,
                u.Person.Cpf.UnformattedCpf,
                u.Password.Hash,
                u.Status,
                u.Role))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<User?> GetUserByEmailAsync(string address, string domain, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    public Task<User?> GetUserByCpfAsync(string cpf, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<AccountStatus?> GetStatusAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
