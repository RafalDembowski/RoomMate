using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMate.Data.Repository;

namespace RoomMate.Data.UnitOfWorks
{
    interface IUnitOfWork : IDisposable
    {
        IUserRepository UsersRepository { get; }
        int Complete();
    }
}
