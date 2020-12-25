using RoomMate.Entities.Users;
using RoomMate.Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoomMate.Entities.UserProfileViewModels
{
    public class UserProfileToDisplayViewModel
    {
        public User user { get; set; }
        public Room room { get; set; }
        public Equipment equipment { get; set; }
        public Address address { get; set; }
    }
}