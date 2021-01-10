using RoomMate.Data.Context;
using RoomMate.Entities.Bookings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoomMate.Data.Repository
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(RoomMateDbContext mainContext) : base(mainContext) { }
        public bool checkRoomAvailability(DateTime InDate, DateTime OutDate, Guid RoomID)
        {
            bool dateIsAvailable = _context
                                   .Bookings
                                   .Where(b => b.Room.RoomID == RoomID)
                                   .Select(b => (InDate >= b.InDate && InDate <= b.OutDate
                                    && OutDate >= b.InDate && OutDate <= b.OutDate))
                                    .FirstOrDefault();
            return dateIsAvailable;
        }
    }
}