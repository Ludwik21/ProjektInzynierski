using ProjektInzynierski.Infrastructure.Models;

namespace ProjektInzynierski.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUser(string username);
    }
}
