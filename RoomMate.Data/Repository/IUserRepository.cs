using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMate.Entities.Users;

namespace RoomMate.Data.Repository
{
    interface IUserRepository : IGenericRepository<User>
    {
        bool IsUserWithEmailExist(string email);
    }
}
