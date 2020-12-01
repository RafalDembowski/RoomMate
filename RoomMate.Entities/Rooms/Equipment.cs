using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RoomMate.Entities.Rooms
{
    public class Equipment
    {
        [Key]
        [Column(Order = 1)]
        public Guid EquipmentID { get; set; }
        public bool IsWifi { get; set; }
        public bool IsAirConditioning { get; set; }
        public bool IsParking { get; set; }
        public bool IsTelevision { get; set; }
        public bool IsKitchen { get; set; }
        public bool IsWashingMachine { get; set; }
        [Required] // fix this!
        public virtual Room Room { get; set; }
    }
}