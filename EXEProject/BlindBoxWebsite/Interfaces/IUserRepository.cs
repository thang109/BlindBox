﻿using BlindBoxWebsite.Models;

namespace BlindBoxWebsite.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User GetByEmail(string email);
        bool UserExist(string email);
    }
}