using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoomMate.Entities.UsersViewModels
{
    public class UserRegistrationViewModel
    {
        [Required(ErrorMessage = "Login jest wymagany.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email jest wymagany.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Hasło jest wymagane.")]
        public string PasswordHash { get; set; }
    }
}