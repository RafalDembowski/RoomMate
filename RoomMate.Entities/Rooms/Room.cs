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
        [Column(Order = 0)]
        public Guid RoomID { get; set; }
        [Required(ErrorMessage = "Nazwa pokoju jest wymagana.")]
        [MaxLength(256)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Wymagane jest podanie ceny pokoju za jedeną noc.")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Opis pokoju jest wymagany.")]
        public string Description { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Wymagane jest podanie maksymalnej liczby gości.")]
        public int NumberOfGuests { get; set; }
        public virtual User User { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<RoomImage> RoomImages { get; set; }
    }
}