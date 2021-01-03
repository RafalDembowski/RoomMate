﻿using RoomMate.Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoomMate.Entities.HomeViewModels
{
    public class HomeViewModel
    {
        public List<Room> randomRooms { get; set; }
        public List<Room> firstCityRandomRooms { get; set; }
        public List<Room> secondCityRandomRooms { get; set; }
        public List<RoomImage> randomRoomsImages { get; set; }

    }
}