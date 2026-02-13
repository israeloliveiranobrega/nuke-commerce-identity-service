using Microsoft.EntityFrameworkCore;
using NukeAuthentication.Data;
using NukeAuthentication.Entitys;
using NukeAuthentication.Services.Repositorys.Contracts;

namespace NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Implementations.UserRepo;

public partial class UserRepository(DataContext dataContext) : IUserRepository
{
    private readonly DbSet<User> _user = dataContext.Users;
}
