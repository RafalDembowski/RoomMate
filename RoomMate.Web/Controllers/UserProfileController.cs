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
            userProfileToDisplayView.user = getActiveUser();
            userProfileToDisplayView.rooms = getAllActiveRooms();
            userProfileToDisplayView.roomImages = unitOfWork.RoomImagesRepository.GetAll().ToList();

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            ViewBag.OnePageOfRooms = userProfileToDisplayView.rooms.ToPagedList(pageNumber, pageSize);

            return View(userProfileToDisplayView);
        }
        [HttpPost]
        public ActionResult Dashboard(UserProfileToDisplayViewModel userProfileToDisplayView)
        {
            try
            {
                UserSaveImageClient userSaveImageClient = new UserSaveImageClient();
                var userImage = userSaveImageClient.saveUserImageToDiskAndReturnCreatedObject(userProfileToDisplayView.images, userProfileToDisplayView.user.UserID);
                bool updateImageSuccesful = updateUserImageNameAndPath(userImage, userProfileToDisplayView.user.UserImage);

                if (!updateImageSuccesful && userImage != null)
                {
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
            userProfileToeditViewModel.user = getActiveUser();
            return View(userProfileToeditViewModel);
        }
        [HttpPost]
        public ActionResult AddRoom(UserProfileToEditViewModel _userProfileToeditViewModel)
        {
            userProfileToeditViewModel.user = getActiveUser();

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
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + e.Message);
                ViewBag.Error = "Wystąpił błąd, proszę powtórzyć jeszcze raz.";
                return View();
            }
            
        }
        public ActionResult DisplayRoom(string id)
        {
            bool codeActivationCanBeGuid = Guid.TryParse(id, out var newGuid);
            if (!String.IsNullOrEmpty(id) && codeActivationCanBeGuid == true)
            {
                userProfileToDisplayView.user = getActiveUser();
                userProfileToDisplayView.room = getActiveRoomByID(id);
                userProfileToDisplayView.room.RoomImages = getRoomImageByRoomID(id);

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
        //Update room 
        [HttpPost]
        public ActionResult DisplayRoom(UserProfileToDisplayViewModel userProfileToDisplayView)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    userProfileToDisplayView.user = getActiveUser();
                    unitOfWork.RoomsRepository.Update(userProfileToDisplayView.room);
                    unitOfWork.AddressesRepository.Update(userProfileToDisplayView.room.Address);
                    unitOfWork.EquipmentRepository.Update(userProfileToDisplayView.room.Equipment);
                    //delete old images
                    deleteOldRoomImagesFromDataBase(userProfileToDisplayView.room.RoomID);
                    //add new images 
                    addNewRoomImagesToDataBase(userProfileToDisplayView.images, userProfileToDisplayView.room.RoomID, userProfileToDisplayView.user.UserID, userProfileToDisplayView.room);
                    unitOfWork.Complete();

                    return RedirectToAction("DisplayRoom", new { id = userProfileToDisplayView.room.RoomID });
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
            userProfileToeditViewModel.user = getActiveUser();
            return View(userProfileToeditViewModel);
        }
        public User getActiveUser()
        {
            User user = new User();
            if (Session["UserID"] != null)
            {
                string userID = Session["UserID"].ToString();
                var users = unitOfWork.UsersRepository.Get(
                                          filter: U => U.UserID == new Guid(userID),
                                          orderBy: null,
                                          includeProperties: "UserImage"
                                          );
                user = users.ToList().FirstOrDefault();
            }
            return user;
        }
        public List<Room> getAllActiveRooms()
        {
            var rooms = unitOfWork.RoomsRepository.Get(
                                          filter: r => r.User.UserID == userProfileToDisplayView.user.UserID && r.IsActive == true,
                                          orderBy: null,
                                          includeProperties: "Address,Equipment"
                                          );
            return rooms.ToList();
        }
        public Room getActiveRoomByID(string id)
        {
            Room room = new Room();
            var roomResult = getAllActiveRooms();
            room = roomResult.Where(r => r.RoomID == new Guid(id)).FirstOrDefault();
            return room;
        }
        public List<RoomImage> getRoomImageByRoomID(string id)
        {
            var images = unitOfWork.RoomImagesRepository.Get(
                                             filter: i => i.Room.RoomID == new Guid(id),
                                             orderBy: null,
                                             includeProperties: "Room"
                                             );
            return images.ToList();
        }
        public bool updateUserImageNameAndPath(UserImage newUserImage, UserImage oldUserImage)
        {
            if(newUserImage != null)
            {
                oldUserImage.ImageName = newUserImage.ImageName;
                oldUserImage.ImagePath = newUserImage.ImagePath;
                unitOfWork.UserImageRepository.Update(oldUserImage);
                return true;
            }
            return false;
        }
        public void deleteOldRoomImagesFromDataBase(Guid id)
        {
            var oldRoomImages = getRoomImageByRoomID(id.ToString());
            if (oldRoomImages.Any() && oldRoomImages != null)
            {
                foreach (var oldImage in oldRoomImages)
                {
                    unitOfWork.RoomImagesRepository.Delete(oldImage.ImageRoomID);
                }
            }
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