using RoomMate.Entities.Bookings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoomMate.Data.Repository
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        bool checkRoomAvailability(DateTime InDate, DateTime OutDate, Guid RoomID);
    }
}