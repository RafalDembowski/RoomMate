using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RoomMate.Entities.Rooms
{
    public class RoomImage
    {
        [Key]
        [Column(Order = 1)]
        public Guid ImageRoomID { get; set; }
        [Required]
        [MaxLength(256)]
        public string Path { get; set; }
        [Required]
        public virtual Room Room { get; set; }
    }
}