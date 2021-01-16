using RoomMate.Data.Context;
using RoomMate.Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RoomMate.Data.Repository
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(RoomMateDbContext mainContext) : base(mainContext) { }
        public List<Room> GetAllActiveRooms(Guid userID)
        {
            List<Room> rooms = _context
                              .Room
                              .Where(r => r.User.UserID == userID && r.IsActive == true)
                              .Include(r => r.Address)
                              .Include(e => e.Equipment)
                              .ToList();

            return rooms;
        }
        public Room GetActiveRoomByID(Guid userID, Guid roomID)
        {
            Room room = _context
                        .Room
                        .Where(r => r.User.UserID == userID && r.IsActive == true && r.RoomID == roomID)
                        .Include(r => r.Address)
                        .Include(e => e.Equipment)
                        .FirstOrDefault();

            if(room != null)
            {
                return room;
            }

            return null;

        }

        public List<Room> GetRandomRooms(int amountRooms)
        {
            List<Room> rooms = _context
                               .Room
                               .Where(r => r.IsActive == true)
                               .Include(r => r.Address)
                               .Include(e => e.Equipment)
                               .OrderBy(x => Guid.NewGuid())
                               .Take(amountRooms)
                               .ToList();
            return rooms;
        }
        public List<Room> GetRandomRoomsByCity(string city, int amountRooms)
        {
            List<Room> rooms = _context
                   .Room
                   .Where(r => r.IsActive == true && r.Address.City.Equals(city))
                   .Include(r => r.Address)
                   .Include(e => e.Equipment)
                   .OrderBy(x => Guid.NewGuid())
                   .Take(amountRooms)
                   .ToList();

            return rooms;
        }
    }
}