using BlindBoxWebsite.Interfaces;
using BlindBoxWebsite.Models;

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
    }
}
