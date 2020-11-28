using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RoomMate.Entities.Rooms;

namespace RoomMate.Entities.Users
{
    public class User
    {
        [Key]
        [Column(Order = 1)]
        public Guid UserID { get; set; }
        [Required]
        [MaxLength(256)]
        public string Email { get; set; }
        [Required]
        [MaxLength(256)]
        public string PasswordHash { get; set; }
        [Required]
        [MaxLength(128)]
        public string UserName { get; set; }
        [MaxLength(128)]
        public string FirsName { get; set; }
        [MaxLength(128)]
        public string LastName { get; set; }
        public bool IsEmailVerified { get; set; }
        public Guid CodeActivation { get; set; }
        public Guid CodeResetPassword { get; set; }
        public virtual UserImage UserImage { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}