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

                    ViewBag.Message = "Konto zostało utworzone, aktywuj je aby móc z niego korzystać.";

                    return View();
                }
                else
                {
                    ViewBag.Message = "Konto z podanym emailem zostało juz utworzone.";
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
                           .Where(u => u.Email.Equals(userFromTheForm.Email) 
                                  && u.PasswordHash.Equals(userPasswrodFromTheForm) 
                                  && u.IsEmailVerified == true).ToList();

                if (user.Count() > 0)
                {
                    Session["Email"] = user.FirstOrDefault().Email;
                    Session["UserID"] = user.FirstOrDefault().UserID;

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "Konto nie istnieje lub nie zostało aktywowane.";
                    return View();
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            ViewBag.ActivationStatus = false;
            bool codeActivationCanBeGuid = Guid.TryParse(id, out var newGuid);

            if (!String.IsNullOrEmpty(id) && codeActivationCanBeGuid == true)
            {
                
                var user = unitOfWork.UsersRepository
                           .GetAll()
                           .Where(u => u.CodeActivation == new Guid(id))
                           .FirstOrDefault();

                if (user != null)
                {
                    user.IsEmailVerified = true;
                    user.CodeActivation = Guid.Empty;
                    unitOfWork.Complete();

                    ViewBag.ActivationStatus = true;
                }
            }

            return View();
        }
        public ActionResult RemindPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RemindPassword(User userFromTheForm)
        {
            ViewBag.Message = "dupa";
            if (userFromTheForm != null)
            {
                string emailFromRemindView = userFromTheForm.Email;
                User user = unitOfWork.UsersRepository.GetUserByEmail(emailFromRemindView);

                if(user != null)
                {
                    //set new code reset password
                    user.CodeResetPassword = Guid.NewGuid();
                    unitOfWork.Complete();

                    //set link to reset password
                    var verifyUrl = "/User/ResetPassword/" + user.CodeResetPassword;
                    var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

                    //send email with reset password code
                    EmailClient emailClient = new EmailClient();
                    emailClient.SendResetPasswordCode(user.Email, link);

                    ViewBag.Message = "Link umożliwiający zmiane hasła został wysłany na podany adres e-mail.";
                }
                else
                {
                    ViewBag.Message = "Email jest nie poprawny lub nie istnieje takie konto.";
                }


            }
            else
            {
                ViewBag.Message = "Musisz wypełnić te pole.";
            }
            return View();
        }


    }
}