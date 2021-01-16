using RoomMate.Data.Context;
using RoomMate.Data.UnitOfWorks;
using RoomMate.Entities.UserProfileViewModels;
using RoomMate.Entities.Users;
using RoomMate.Common.SaveImageClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoomMate.Entities.Rooms;
using PagedList;

namespace RoomMate.Controllers
{
    public class UserProfileController : Controller
    {
        private UnitOfWork unitOfWork;
        private UserProfileToEditViewModel userProfileToeditViewModel;
        private UserProfileToDisplayViewModel userProfileToDisplayView;
        public UserProfileController()
        {
            unitOfWork = new UnitOfWork(new RoomMateDbContext());
            userProfileToeditViewModel = new UserProfileToEditViewModel();
            userProfileToDisplayView = new UserProfileToDisplayViewModel();
        }
        public ActionResult Dashboard(int? page)
        {
            if (Session["UserID"] != null)
            {
                string userID = Session["UserID"].ToString();
                userProfileToDisplayView.user = unitOfWork.UsersRepository.GetActiveUser(new Guid(userID));
                //get all active rooms and include images
                userProfileToDisplayView.rooms = unitOfWork.RoomsRepository.GetAllActiveRooms(new Guid(userID));
                userProfileToDisplayView.roomImages = unitOfWork.RoomImagesRepository.GetAll().ToList();
                //set pagination 
                int pageSize = 2;
                int pageNumber = (page ?? 1);
                ViewBag.OnePageOfRooms = userProfileToDisplayView.rooms.ToPagedList(pageNumber, pageSize);

                return View(userProfileToDisplayView);
            }
            return RedirectToAction("Login", "User");
        }
        [HttpPost]
        public ActionResult Dashboard(UserProfileToDisplayViewModel userProfileToDisplayView)
        {
            try
            {
                UserSaveImageClient userSaveImageClient = new UserSaveImageClient();
                var userImage = userSaveImageClient.saveUserImageToDiskAndReturnCreatedObject(userProfileToDisplayView.images, userProfileToDisplayView.user.UserID);
                //update image or insert new image
                if (userImage != null && userProfileToDisplayView.user.UserImage.UserImageID != Guid.Empty)
                {
                    userProfileToDisplayView.user.UserImage.ImageName = userImage.ImageName;
                    userProfileToDisplayView.user.UserImage.ImagePath = userImage.ImagePath;
                    unitOfWork.UserImageRepository.Update(userProfileToDisplayView.user.UserImage);
                }
                else
                {
                    userImage.UserImageID = userProfileToDisplayView.user.UserID;
                    unitOfWork.UserImageRepository.Insert(userImage);
                    userProfileToDisplayView.user.UserImage = userImage;
                }

                unitOfWork.UsersRepository.Update(userProfileToDisplayView.user);
                unitOfWork.Complete();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + e.Message);
                ViewBag.Error = "Wystąpił błąd, proszę powtórzyć jeszcze raz.";
                return RedirectToAction("Dashboard", "UserProfile");
            }
            return RedirectToAction("Dashboard", "UserProfile");
        }
        //delete room
        [HttpPost, ActionName("Delete")]
        public ActionResult Dashboard(string id)
        {
            try
            {
               var room = unitOfWork.RoomsRepository.GetById(new Guid(id));
               room.IsActive = false;
               unitOfWork.RoomsRepository.Update(room);
               unitOfWork.Complete();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + e.Message);
                ViewBag.Error = "Wystąpił błąd, proszę powtórzyć jeszcze raz.";
                return RedirectToAction("Dashboard", "UserProfile");
            }
            return RedirectToAction("Dashboard", "UserProfile");
        }
        public ActionResult AddRoom()
        {
            if (Session["UserID"] != null)
            {
                string userID = Session["UserID"].ToString();
                userProfileToeditViewModel.user = unitOfWork.UsersRepository.GetActiveUser(new Guid(userID));

                return View(userProfileToeditViewModel);
            }
            return RedirectToAction("Login", "User");
        }
        [HttpPost]
        public ActionResult AddRoom(UserProfileToEditViewModel _userProfileToeditViewModel)
        {
            if (Session["UserID"] != null)
            {
                string userID = Session["UserID"].ToString();
                userProfileToeditViewModel.user = unitOfWork.UsersRepository.GetActiveUser(new Guid(userID));

                try
                {

                    if (ModelState.IsValid)
                    {
                        //create room object
                        Room room = new Room();
                        room.RoomID = Guid.NewGuid();
                        room.Name = _userProfileToeditViewModel.roomToEdit.Name;
                        room.Price = _userProfileToeditViewModel.roomToEdit.Price;
                        room.Description = _userProfileToeditViewModel.roomToEdit.Description;
                        room.IsActive = true;
                        room.NumberOfGuests = _userProfileToeditViewModel.roomToEdit.NumberOfGuests;
                        room.User = userProfileToeditViewModel.user;

                        //create room images objetcs
                        addNewRoomImagesToDataBase(_userProfileToeditViewModel.images, room.RoomID, room.User.UserID, room);

                        //create equipment object
                        Equipment equipment = new Equipment();
                        equipment.EquipmentID = Guid.NewGuid();
                        equipment.IsWifi = _userProfileToeditViewModel.equipmentToEdit.IsWifi;
                        equipment.IsAirConditioning = _userProfileToeditViewModel.equipmentToEdit.IsAirConditioning;
                        equipment.IsParking = _userProfileToeditViewModel.equipmentToEdit.IsParking;
                        equipment.IsTelevision = _userProfileToeditViewModel.equipmentToEdit.IsTelevision;
                        equipment.IsKitchen = _userProfileToeditViewModel.equipmentToEdit.IsKitchen;
                        equipment.IsWashingMachine = _userProfileToeditViewModel.equipmentToEdit.IsWashingMachine;

                        unitOfWork.EquipmentRepository.Insert(equipment);

                        //create address object
                        Address address = new Address();
                        address.AddressID = Guid.NewGuid();
                        address.City = _userProfileToeditViewModel.addressToEdit.City;
                        address.Street = _userProfileToeditViewModel.addressToEdit.Street;
                        address.Flat = _userProfileToeditViewModel.addressToEdit.Flat;
                        address.PostCode = _userProfileToeditViewModel.addressToEdit.PostCode;
                        address.Lon = _userProfileToeditViewModel.addressToEdit.Lon;
                        address.Lat = _userProfileToeditViewModel.addressToEdit.Lat;

                        unitOfWork.AddressesRepository.Insert(address);


                        room.Address = address;
                        room.Equipment = equipment;
                        unitOfWork.RoomsRepository.Insert(room);

                        //save to db
                        unitOfWork.Complete();

                        return RedirectToAction("DisplayRoom", new { id = room.RoomID });
                    }
                    return View();

                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + e.Message);
                    ViewBag.Error = "Wystąpił błąd, proszę powtórzyć jeszcze raz.";
                    return View();
                }
            }
            return RedirectToAction("Login", "User");
        }
        public ActionResult DisplayRoom(string id)
        {
            bool codeActivationCanBeGuid = Guid.TryParse(id, out var newGuid);
            if (!String.IsNullOrEmpty(id) && codeActivationCanBeGuid == true)
            {
                if (Session["UserID"] != null)
                {
                    string userID = Session["UserID"].ToString();
                    userProfileToDisplayView.user = unitOfWork.UsersRepository.GetActiveUser(new Guid(userID));
                    userProfileToDisplayView.room = unitOfWork.RoomsRepository.GetActiveRoomByID(new Guid(userID), new Guid(id));
                    userProfileToDisplayView.room.RoomImages = unitOfWork.RoomImagesRepository.GetRoomImageByRoomIDIncludeRoom(new Guid(id));

                    if (userProfileToDisplayView.room != null)
                    {
                        return View(userProfileToDisplayView);
                    }
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    return RedirectToAction("Dashboard");
                }
            }
            return RedirectToAction("Login", "User");
        }
        //Update room 
        [HttpPost]
        public ActionResult DisplayRoom(UserProfileToDisplayViewModel userProfileToDisplayView)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Session["UserID"] != null)
                    {
                        string userID = Session["UserID"].ToString();
                        userProfileToDisplayView.user = unitOfWork.UsersRepository.GetActiveUser(new Guid(userID));

                        unitOfWork.RoomsRepository.Update(userProfileToDisplayView.room);
                        unitOfWork.AddressesRepository.Update(userProfileToDisplayView.room.Address);
                        unitOfWork.EquipmentRepository.Update(userProfileToDisplayView.room.Equipment);
                        //delete old images
                        unitOfWork.RoomImagesRepository.DeleteRoomImagesByRoomID(userProfileToDisplayView.room.RoomID);
                        //add new images 
                        addNewRoomImagesToDataBase(userProfileToDisplayView.images, userProfileToDisplayView.room.RoomID, userProfileToDisplayView.user.UserID, userProfileToDisplayView.room);
                        unitOfWork.Complete();

                        return RedirectToAction("DisplayRoom", new { id = userProfileToDisplayView.room.RoomID });
                    }
                    return RedirectToAction("Login", "User");
                }
                return RedirectToAction("DisplayRoom", new { id = userProfileToDisplayView.room.RoomID });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + e.Message);
                ViewBag.Error = "Wystąpił błąd, proszę powtórzyć jeszcze raz.";
                return RedirectToAction("DisplayRoom", new { id = userProfileToDisplayView.room.RoomID });
            }
        }
        public ActionResult Customers()
        {

            if (Session["UserID"] != null)
            {
                string userID = Session["UserID"].ToString();
                userProfileToeditViewModel.user = unitOfWork.UsersRepository.GetActiveUser(new Guid(userID));
                return View(userProfileToeditViewModel);
            }
            return RedirectToAction("Login", "User");
        }
        public void addNewRoomImagesToDataBase(IEnumerable<HttpPostedFileBase> images, Guid roomID, Guid userID, Room room  )
        {
            List<RoomImage> roomImages = new List<RoomImage>();
            RoomSaveImageClient roomSaveImageClient = new RoomSaveImageClient();
            roomImages = roomSaveImageClient.saveRoomImageToDiskAndReturnListWithRoomImages(images , roomID, userID, room);

            if (roomImages.Any() && roomImages != null)
            {
                foreach (var image in roomImages)
                {
                    unitOfWork.RoomImagesRepository.Insert(image);
                }
            }
        }
    }
}