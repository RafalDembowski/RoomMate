
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
        public int NumberOfGuests { get; set; }
        public float? TotalPrice { get; set; }
        [Required]
        public virtual Room Room { get; set; }
        [Required]
        public virtual User User { get; set; }
    }
}