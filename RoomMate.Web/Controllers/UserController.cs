using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoomMate.Data.Repository;
using RoomMate.Entities.Users;

namespace RoomMate.Controllers
{
    public class UserController : Controller
    {
        private IGenericRepository<User> userRepository = null;

        public UserController()
        {
            this.userRepository = new GenericRepository<User>();
        }
        public UserController(IGenericRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
    }
}