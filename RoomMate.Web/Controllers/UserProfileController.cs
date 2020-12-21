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
        private UserProfileToEditViewModel userProfileToeditViewModel;
        public UserProfileController()
        {
            unitOfWork = new UnitOfWork(new RoomMateDbContext());
            userProfileToeditViewModel = new UserProfileToEditViewModel();
        }
        public ActionResult Dashboard()
        {
            prepareUserProfileViewModel();
            return View(userProfileToeditViewModel);
        }
        public ActionResult AddRoom()
        {
            prepareUserProfileViewModel();
            return View(userProfileToeditViewModel);
        }
        [HttpPost]
        public ActionResult AddRoom(UserProfileToEditViewModel userProfileToeditViewModel)
        {
            System.Diagnostics.Debug.WriteLine("cos nie dziala ziombelku");
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("Testowe sprawdzanko");
            }
            return View();
        }
        public ActionResult Customers()
        {
            prepareUserProfileViewModel();
            return View(userProfileToeditViewModel);
        }
        public void prepareUserProfileViewModel()
        {
            if (Session["UserID"] != null)
            {
                User user = unitOfWork.UsersRepository.GetById((Guid)Session["UserID"]);
                userProfileToeditViewModel.user = user;
            }
        }
    }
}