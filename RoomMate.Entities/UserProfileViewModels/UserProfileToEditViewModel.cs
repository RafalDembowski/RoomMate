using RoomMate.Entities.Rooms;
using RoomMate.Entities.RoomsViewModels;
using RoomMate.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoomMate.Entities.UserProfileViewModels
{
    public class UserProfileToEditViewModel
    {
        public RoomToEditViewModel roomToEdit { get; set; }
        public User user { get; set; }
        public IEnumerable<HttpPostedFileBase> images { get; set; }
        public EquipmentToEditViewModel equipmentToEdit { get; set; }
        public AddressToEditViewModel addressToEdit { get; set; }
    }
}