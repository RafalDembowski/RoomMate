using RoomMate.Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoomMate.Entities.HomeControllerViewModels
{
    public class HomeViewModel
    {
        public List<Room> randomRooms { get; set; }
        public List<Room> firstCityRandomRooms { get; set; }
        public List<Room> secondCityRandomRooms { get; set; }
        public List<RoomImage> randomRoomsImages { get; set; }
        /*to search room*/
        public string searchCity { get; set; }
        public string numberOfGuest { get; set; }
        public DateTime? inDate { get; set; }
        public DateTime? outDate { get; set; }
        public List<SelectListItem> selectGuestList { get; set; }
    }
}