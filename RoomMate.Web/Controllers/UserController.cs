using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoomMate.Data.Context;
using RoomMate.Data.Repository;
using RoomMate.Entities.Users;

namespace RoomMate.Controllers
{
    public class UserController : Controller
    {
        private IUserRepository userRepository = null;

        public UserController()
        {
            //?to mozna zmienic?
            this.userRepository = new UserRepository(new RoomMateDbContext());
        }

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                var isEmailExist = userRepository.IsUserWithEmailExist(user.Email);
            }
            return View();
        }
    }
}