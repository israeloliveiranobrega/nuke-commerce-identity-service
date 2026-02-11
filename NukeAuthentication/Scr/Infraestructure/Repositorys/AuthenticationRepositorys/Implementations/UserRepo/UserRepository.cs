using Microsoft.EntityFrameworkCore;
using NukeAuthentication.Scr.Domain.Entitys;
using NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Contracts;

namespace NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Implementations.UserRepo;

public partial class UserRepository(DataContext dataContext) : IUserRepository
{
    private readonly DbSet<User> _user = dataContext.Users;
}
