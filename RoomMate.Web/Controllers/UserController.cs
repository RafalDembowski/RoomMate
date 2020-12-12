using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoomMate.Common;
using RoomMate.Data.Context;
using RoomMate.Data.Repository;
using RoomMate.Data.UnitOfWorks;
using RoomMate.Entities.Users;

namespace RoomMate.Controllers
{
    public class UserController : Controller 
    {
        private UnitOfWork unitOfWork;
        public UserController()
        {
            unitOfWork = new UnitOfWork(new RoomMateDbContext());
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(User user)
        {
            if (ModelState.IsValid)
            {
                var isEmailExist = unitOfWork.UsersRepository.IsUserWithEmailExist(user.Email);
                if (isEmailExist == false)
                {
                    user.UserID = Guid.NewGuid();
                    user.PasswordHash = Crypto.CreateMD5(user.PasswordHash);
                    user.FirsName = "";
                    user.LastName = "";
                    user.IsEmailVerified = false;
                    user.CodeActivation = Guid.NewGuid();
                    user.CodeResetPassword = Guid.Empty;

                    unitOfWork.UsersRepository.Insert(user);
                    unitOfWork.Complete();

                    //set link to activation account
                    var verifyUrl = "/User/VerifyAccount/" + user.CodeActivation;
                    var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

                    EmailClient emailClient = new EmailClient();
                    emailClient.SendVerifyAccountCode(user.Email, link);
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
    }
}