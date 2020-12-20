using RoomMate.Entities.Rooms;
using RoomMate.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoomMate.Entities.UserProfileViewModels
{
    public class UserProfileViewModel
    {
        /*only for test*/
        public UserProfileViewModel()
        {
            amountRooms = 0;
        }
        public Room room { get; set; }
        public User user { get; set; }
        public List<Room> rooms { get; set; }
        public List<RoomImage> roomImages { get; set; }
        public IEnumerable<HttpPostedFileBase> images { get; set; }
        public int amountRooms { get; set; }
        public Equipment equipment { get; set; }
        public Address address { get; set; }
    }
}