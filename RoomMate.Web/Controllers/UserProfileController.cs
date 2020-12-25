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
        public ActionResult Dashboard()
        {
            userProfileToeditViewModel.user = getActiveUserID();
            return View(userProfileToeditViewModel);
        }
        public ActionResult AddRoom()
        {
            userProfileToeditViewModel.user = getActiveUserID();
            return View(userProfileToeditViewModel);
        }
        [HttpPost]
        public ActionResult AddRoom(UserProfileToEditViewModel _userProfileToeditViewModel)
        {
            userProfileToeditViewModel.user = getActiveUserID();

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
                    List<RoomImage> roomImages = new List<RoomImage>();
                    RoomSaveImageClient roomSaveImageClient = new RoomSaveImageClient();
                    roomImages = roomSaveImageClient.saveRoomImageToDiskAndReturnListWithRoomImages(_userProfileToeditViewModel.images, room.RoomID, room.User.UserID, room);

                    if (roomImages.Any() && roomImages != null)
                    {
                        foreach (var image in roomImages)
                        {
                            unitOfWork.RoomImagesRepository.Insert(image);
                        }
                    }

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
                userProfileToDisplayView.user = getActiveUserID();
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
        [HttpPost]
        public ActionResult DisplayRoom(UserProfileToDisplayViewModel userProfileToDisplayView)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    System.Diagnostics.Debug.WriteLine("Jestem tutaj");
                    unitOfWork.RoomsRepository.Update(userProfileToDisplayView.room);
                    unitOfWork.AddressesRepository.Update(userProfileToDisplayView.room.Address);
                    unitOfWork.EquipmentRepository.Update(userProfileToDisplayView.room.Equipment);
                    unitOfWork.Complete();
                    //zrobić edycje zdjęć!

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
            userProfileToeditViewModel.user = getActiveUserID();
            return View(userProfileToeditViewModel);
        }
        public User getActiveUserID()
        {
            User user = new User();
            if (Session["UserID"] != null)
            {
                user = unitOfWork.UsersRepository.GetById((Guid)Session["UserID"]);
            }
            return user;
        }
        public Room getActiveRoomByID(string id)
        {
            Room room = new Room();
            var rooms = unitOfWork.RoomsRepository.Get(
                                                      filter: r => r.User.UserID == userProfileToDisplayView.user.UserID && r.IsActive == true && r.RoomID == new Guid(id),
                                                      orderBy: null,
                                                      includeProperties: "Address,Equipment"
                                                      );
            var roomResult = rooms.ToList();
            room = roomResult.FirstOrDefault();
            return room;
        }
        public List<RoomImage> getRoomImageByRoomID(string id)
        {
            var images = unitOfWork.RoomImagesRepository.Get(
                                             filter: i => i.Room.RoomID == new Guid(id),
                                             orderBy: null,
                                             includeProperties: ""
                                             );
            return images.ToList();
        }
    }
}