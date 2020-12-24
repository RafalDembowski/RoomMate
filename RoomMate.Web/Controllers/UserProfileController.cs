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
        public ActionResult AddRoom(UserProfileToEditViewModel _userProfileToeditViewModel)
        {
            prepareUserProfileViewModel();

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

                unitOfWork.RoomsRepository.Insert(room);

                //create room images objetcs
                List<RoomImage> roomImages = new List<RoomImage>();
                RoomSaveImageClient roomSaveImageClient = new RoomSaveImageClient();
                roomImages = roomSaveImageClient.saveRoomImageToDiskAndReturnListWithRoomImages(_userProfileToeditViewModel.images, room.RoomID, room.User.UserID , room);


                if ((roomImages != null) && (!roomImages.Any()))
                {
                    foreach(var image in roomImages)
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
                equipment.Room = room;

                unitOfWork.EquipmentRepository.Insert(equipment);

                //create address object
                Address address = new Address();
                address.AddressID = Guid.NewGuid();
                address.City = _userProfileToeditViewModel.addressToEdit.City;
                address.Street = _userProfileToeditViewModel.addressToEdit.Street;
                address.PostCode = _userProfileToeditViewModel.addressToEdit.PostCode;
                address.Lon = _userProfileToeditViewModel.addressToEdit.Lon;
                address.Lat = _userProfileToeditViewModel.addressToEdit.Lat;
                address.Room = room;

                unitOfWork.AddressesRepository.Insert(address);

                //save to db
                unitOfWork.Complete();

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