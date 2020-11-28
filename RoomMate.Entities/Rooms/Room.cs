using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RoomMate.Entities.Users;

namespace RoomMate.Entities.Rooms
{
    public class Room
    {
        [Key]
        [Column(Order = 1)]
        public Guid RoomID { get; set; }
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public int NumberOfGuests { get; set; }
        [Required]
        public Guid EquipmentID { get; set; }
        [Required]
        public Guid AddressID { get; set; }
        [Required]
        public Guid OwnerId { get; set; }
        public virtual User User { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<RoomImage> RoomImages { get; set; }

    }
}