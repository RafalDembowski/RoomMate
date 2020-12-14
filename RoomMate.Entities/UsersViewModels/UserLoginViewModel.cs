using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoomMate.Entities.UsersViewModels
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Email jest wymagany.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Hasło jest wymagany.")]
        public string PasswordHash { get; set; }
    }
}