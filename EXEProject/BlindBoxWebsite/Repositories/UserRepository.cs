using BlindBoxWebsite.Interfaces;
using BlindBoxWebsite.Models;
using BlindBoxWebsite.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BlindBoxWebsite.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(BlindBoxContext context) : base(context)
        {

        }

        public User GetByEmail(string email)
        {
            return _dbSet.SingleOrDefault(x => x.Email == email);
        }

        public bool UserExist(string email)
        {
            return _dbSet.Any(x => x.Email == email);
        }

        public User GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == userId);
        }

        public bool IsAdmin(User user)
        {
            return user.Role == UserRole.Admin;
        }

        public async Task<int> AddAccount(User user)
        {
            using var context = new BlindBoxContext();
            var lastuser = await context.Users.OrderByDescending(x => x.UserId).FirstOrDefaultAsync();
            var lastId = lastuser.UserId + 1;
            user.UserId = lastId;
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user.UserId;
        }
    }
}
