
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RoomMate.Entities.Rooms;
using RoomMate.Entities.Users;

namespace RoomMate.Entities.Bookings
{
    public class Booking
    {
        [Key]
        [Column(Order = 1)]
        public Guid BookingID { get; set; }
        [Required]
        public DateTime InDate { get; set; }
        [Required]
        public DateTime OutDate { get; set; }
        [Required]
        public Guid ClientID { get; set; }
        [Required]
        public Guid RoomID { get; set; }
        [Required]
        public int NumberOfGuests { get; set; }
        public float? TotalPrice { get; set; }
        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
    }
}