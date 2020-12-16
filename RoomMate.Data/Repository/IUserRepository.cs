using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMate.Data.Context;
using RoomMate.Entities.Users;

namespace RoomMate.Data.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        bool IsUserWithEmailExist(string email);
        User GetUserByEmail(string email);
    }
}
