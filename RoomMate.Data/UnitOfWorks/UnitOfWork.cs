using RoomMate.Data.Context;
using RoomMate.Data.Repository;
using RoomMate.Entities.Rooms;
using RoomMate.Entities.Users;
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
        public IRoomRepository RoomsRepository { get; }
        public IGenericRepository<Equipment> EquipmentRepository { get;  }
        public IGenericRepository<Address> AddressesRepository { get;  }
        public IRoomImageRepository RoomImagesRepository { get;  }
        public IGenericRepository<UserImage> UserImageRepository { get; }
        public IBookingRepository BookingRepository { get; }
        public UnitOfWork(RoomMateDbContext context)
        {
            _context = context;
            UsersRepository = new UserRepository(_context);
            RoomsRepository = new RoomRepository(_context);
            EquipmentRepository = new GenericRepository<Equipment>(_context);
            AddressesRepository = new GenericRepository<Address>(_context);
            RoomImagesRepository = new RoomImageRepository(_context);
            UserImageRepository = new GenericRepository<UserImage>(_context);
            BookingRepository = new BookingRepository(_context);
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