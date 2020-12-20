using RoomMate.Data.Context;
using RoomMate.Data.UnitOfWorks;
using RoomMate.Entities.UserProfileViewModels;
using RoomMate.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoomMate.Controllers
{
    public class UserProfileController : Controller
    {
        private UnitOfWork unitOfWork;
        private UserProfileViewModel userProfileViewModel;
        public UserProfileController()
        {
            unitOfWork = new UnitOfWork(new RoomMateDbContext());
            userProfileViewModel = new UserProfileViewModel();
        }
        public ActionResult Dashboard()
        {
            prepareUserProfileViewModel();
            return View(userProfileViewModel);
        }
        public ActionResult AddRoom()
        {
            prepareUserProfileViewModel();
            return View(userProfileViewModel);
        }
        [HttpPost]
        public ActionResult AddRoom(UserProfileViewModel userProfileViewModel)
        {
            return View();
        }
        public ActionResult Customers()
        {
            prepareUserProfileViewModel();
            return View(userProfileViewModel);
        }
        public void prepareUserProfileViewModel()
        {
            User user = unitOfWork.UsersRepository.GetById((Guid)Session["UserID"]);
            userProfileViewModel.user = user;
        }
    }
}