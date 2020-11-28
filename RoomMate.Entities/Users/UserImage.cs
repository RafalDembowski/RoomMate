using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomMate.Entities.Users
{
    public class UserImage
    {
        [Key]
        [Column(Order = 1)]
        public Guid UserID { get; set; }
        [Required]
        [MaxLength(256)]
        public string ImageName {get; set;}
        [Required]
        [MaxLength(256)]
        public string ImagePath { get; set; }
        public virtual User user { get; set; }
    }
}