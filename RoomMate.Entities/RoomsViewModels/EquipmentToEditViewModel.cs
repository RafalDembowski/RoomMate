using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoomMate.Entities.RoomsViewModels
{
    public class EquipmentToEditViewModel
    {
        public bool IsWifi { get; set; }
        public bool IsAirConditioning { get; set; }
        public bool IsParking { get; set; }
        public bool IsTelevision { get; set; }
        public bool IsKitchen { get; set; }
        public bool IsWashingMachine { get; set; }
    }
}