using RoomMate.Entities.Users;
using RoomMate.Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoomMate.Entities.Bookings;

namespace RoomMate.Entities.UserProfileViewModels
{
    public class UserProfileToDisplayViewModel
    {
        public User user { get; set; }
        public Room room { get; set; }
        public List<Room> rooms { get; set; }
        public List<RoomImage> roomImages { get; set; }
        public IEnumerable<HttpPostedFileBase> images { get; set; }
        public List<Booking> bookings { get; set; }
    }
}