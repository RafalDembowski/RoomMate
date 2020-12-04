using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoomMate.Data.Context;
using RoomMate.Entities.Users;

namespace RoomMate.Data.Repository
{
    public class UserRepository : GenericRepository<User> , IUserRepository
    {
        public UserRepository(RoomMateDbContext context) : base(context)
        {

        }
        public bool IsUserWithEmailExist(string email)
        {
            User user = _context.Users.FirstOrDefault(u => u.Email.Equals(email));
            return user != null;
        }
    }
}