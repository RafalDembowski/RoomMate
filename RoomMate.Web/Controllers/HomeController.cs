using Microsoft.Ajax.Utilities;
using RoomMate.Data.Context;
using RoomMate.Data.UnitOfWorks;
using RoomMate.Entities.HomeControllerViewModels;
using RoomMate.Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoomMate.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork unitOfWork;
        private HomeViewModel homeViewModel;
        public HomeController()
        {
            unitOfWork = new UnitOfWork(new RoomMateDbContext());
            homeViewModel = new HomeViewModel();
        }
        public ActionResult Index()
        {
            //take random cities
            string[] randomCities = getRandomCity();
            //take list of random rooms 
            homeViewModel.firstCityRandomRooms = getRandomRoomByCity(randomCities[0]);
            homeViewModel.secondCityRandomRooms = getRandomRoomByCity(randomCities[1]);
            homeViewModel.randomRooms = getRandomRooms();
            //take images for room
            var listWithAllRooms = homeViewModel.firstCityRandomRooms
                                   .Concat(homeViewModel.secondCityRandomRooms)
                                   .Concat(homeViewModel.randomRooms).ToList();
            homeViewModel.randomRoomsImages = getFirstImageForRandomRooms(listWithAllRooms);
            //take amount of guest orderby ascending
            homeViewModel.selectGuestList = setSelectListWithGuests();
            return View(homeViewModel);
        }
        public List<Room> shuffleRooms(List<Room> rooms)
        {
            var shuffledRooms = rooms
                    .OrderBy(a => Guid.NewGuid())
                    .Take(4)
                    .ToList();
            return shuffledRooms;
        }
        public List<Room> getRandomRooms()
        {
            var rooms = unitOfWork.RoomsRepository.Get(filter: r => r.IsActive == true,
                                                       orderBy: null,
                                                       includeProperties: "Address,Equipment"
                                                       ).ToList();

            var shuffledRooms = shuffleRooms(rooms);

            return shuffledRooms;
        }
        public List<RoomImage> getFirstImageForRandomRooms(List<Room> shuffledRooms)
        {
            List<RoomImage> roomImages = new List<RoomImage>();

            foreach(var room in shuffledRooms)
            {
                var images = unitOfWork.RoomImagesRepository.Get(
                                        filter: i => i.Room.RoomID == room.RoomID,
                                        orderBy: null,
                                        includeProperties: ""
                                        ).FirstOrDefault();
                roomImages.Add(images);
            }
            return roomImages;
        }
        public string[] getRandomCity()
        {
            var addresses = unitOfWork.AddressesRepository.GetAll().ToList();
            var shuffledAddresses = addresses
                                    .OrderBy(a => Guid.NewGuid())
                                    .ToList();
            var addressesDistinct = shuffledAddresses.Select(a => a.City).Distinct().ToArray();

            return addressesDistinct;
        }
        public List<Room> getRandomRoomByCity(string city)
        {
            var rooms = unitOfWork.RoomsRepository.Get(filter: r => r.IsActive == true 
                                           && r.Address.City.Equals(city),
                                           orderBy: null,
                                           includeProperties: "Address,Equipment"
                                           ).ToList();

            var shuffledRooms = shuffleRooms(rooms);

            return shuffledRooms;
        }
        public List<Room> getRoomDistinctByNumberOfGuestes()
        {
            var roomsDistinctByGuestNumber = unitOfWork.RoomsRepository.Get(filter: r => r.IsActive == true,
                                                       orderBy: r => r.OrderBy(o => o.NumberOfGuests),
                                                       includeProperties: ""
                                                       ).DistinctBy(r => r.NumberOfGuests).ToList();


            return roomsDistinctByGuestNumber;
        }
        public List<SelectListItem> setSelectListWithGuests()
        {
            //get amount of guest and sort ascending
            var roomsDistinctByGuestNumber = getRoomDistinctByNumberOfGuestes();

            List<SelectListItem> guestList = new List<SelectListItem>();
            guestList.Add(new SelectListItem() { Text = "Wybierz liczbę gości:" , Value = "0"});

            foreach (var room in roomsDistinctByGuestNumber)
            {
                guestList.Add(new SelectListItem() { Text = "Liczba gości: " + room.NumberOfGuests , Value = room.NumberOfGuests.ToString() }); 
            }

            return guestList;
        }



    }
}