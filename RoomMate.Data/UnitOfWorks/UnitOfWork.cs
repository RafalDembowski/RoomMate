using RoomMate.Data.Context;
using RoomMate.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoomMate.Data.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RoomMateDbContext _context;
        public IUserRepository UsersRepository { get; }

        public UnitOfWork(RoomMateDbContext context)
        {
            _context = context;
            UsersRepository = new UserRepository(_context);
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}