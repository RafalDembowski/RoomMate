using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoomMate.Entities.UsersViewModels
{
    public class UserLoginViewModel
    {
        [Display(Name = "Identyfikator użytkownika")]
        [Required(ErrorMessage = "Login lub e-mail jest wymagany.")]
        public string EmailOrLogin { get; set; }
        [Display(Name = "Hasło")]
        [Required(ErrorMessage = "Hasło jest wymagane.")]
        public string PasswordHash { get; set; }
    }
}