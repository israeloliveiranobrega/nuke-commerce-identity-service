using NukeAuthentication.Entitys;

namespace NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Implementations.UserRepo;

public partial class UserRepository
{
    public async Task<Guid> Create(User user, CancellationToken cancellationToken = default)
    {
        await _user.AddAsync(user, cancellationToken);
        await dataContext.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
    public async Task<int> Change(User user, Guid whoEdited, CancellationToken cancellationToken = default)
    {
        user.LastUpdateBy = whoEdited;
        user.LastUpdateDate = DateTime.UtcNow;

        _user.Update(user);

        return await dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> Active(User user, Guid whoEdited, CancellationToken cancellationToken = default)
    {
        user.ActivedBy = whoEdited;
        user.ActivedDate = DateTime.UtcNow;

        await Change(user, whoEdited, cancellationToken);

        return await dataContext.SaveChangesAsync(cancellationToken);
    }
    public async Task<int> Suspend(User user, Guid whoEdited, CancellationToken cancellationToken = default)
    {
        user.SuspendedBy = whoEdited;
        user.SuspendedDate = DateTime.UtcNow;

        await Change(user, whoEdited, cancellationToken);

        return await dataContext.SaveChangesAsync(cancellationToken);
    }
    public async Task<int> Delete(User user, Guid whoEdited, CancellationToken cancellationToken = default)
    {
        user.DeletedBy = whoEdited;
        user.DeletedDate = DateTime.UtcNow;

        await Change(user, whoEdited, cancellationToken);

        return await dataContext.SaveChangesAsync(cancellationToken);
    }
}
