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
using RoomMate.Entities.UsersViewModels;

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
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(UserRegistrationViewModel userFromTheForm)
        {
            if (ModelState.IsValid)
            {
                var isEmailExist = unitOfWork.UsersRepository.IsUserWithEmailExist(userFromTheForm.Email);
                if (isEmailExist == false)
                {
                    User user = new User();
                    user.Email = userFromTheForm.Email;
                    user.UserName = userFromTheForm.UserName;
                    user.PasswordHash = Crypto.CreateMD5(userFromTheForm.PasswordHash);
                    user.UserID = Guid.NewGuid();
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
                    ViewBag.error = "Konto z podanym emailem zostało juz utworzone.";
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginViewModel userFromTheForm)
        {
            if (ModelState.IsValid)
            {
                var userPasswrodFromTheForm = Crypto.CreateMD5(userFromTheForm.PasswordHash);
                var user = unitOfWork.UsersRepository
                           .GetAll()
                           .Where(u => u.Email.Equals(userFromTheForm.Email) && u.PasswordHash.Equals(userPasswrodFromTheForm)).ToList();

                if (user.Count() > 0)
                {
                    Session["Email"] = user.FirstOrDefault().Email;
                    Session["UserID"] = user.FirstOrDefault().UserID;

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "Nie udało się zalogować.";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}