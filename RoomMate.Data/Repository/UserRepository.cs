using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using RoomMate.Data.Context;
using RoomMate.Entities.Users;

namespace RoomMate.Data.Repository
{
    public class UserRepository : GenericRepository<User> , IUserRepository
    {
        public UserRepository(RoomMateDbContext mainContext) : base(mainContext) { }
        public bool IsUserWithEmailExist(string email)
        {
            User user = _context
                        .Users
                        .FirstOrDefault(u => u.Email.ToLower()
                        .Equals(email.ToLower()));

            if(user != null)
            {
                return true;
            }
            return false;
        }

        public User GetUserByEmail(string email)
        {
            User user = _context
                        .Users
                        .FirstOrDefault(u => u.Email.ToLower()
                        .Equals(email.ToLower()));

            if (user != null)
            {
                return user; 
            }
            return null;
        }
        public User GetActiveUser(Guid userID)
        {
            User user = _context
                       .Users
                       .Where(u => u.UserID == userID)
                       .Include(i => i.UserImage)
                       .FirstOrDefault();

            if (user != null)
            {
                return user;
            }           
            return null;
        }
    }
}