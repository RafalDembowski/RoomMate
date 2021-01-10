using RoomMate.Entities.BookingsViewModels;
using RoomMate.Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoomMate.Entities.RoomControllerViewModels
{
    public class RoomViewModel
    {
        //for display room and booking 
        public Room room { get; set; }
        public BookingViewModel booking { get; set; }
        public List<SelectListItem> guestSelectList { get; set; }
        //for search rooms 
        public string searchCity { get; set; }
        public string numberOfGuest { get; set; }
        public List<RoomImage> roomImages { get; set; }
        public List<Room> rooms { get; set; }
        public List<SelectListItem> sortSelectList { get; set; }
        public string currentFilter { get; set; }
    }
}