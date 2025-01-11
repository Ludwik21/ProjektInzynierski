using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Infrastructure.Models;

namespace ProjektInzynierski.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ProjektContext _context;

        public UserRepository(ProjektContext context)
        {
            _context = context;
        }

        public Task<User> GetUser(string username)
        {
            return _context.Users.SingleAsync(x => x.UserName == username);
        }
    }
}
