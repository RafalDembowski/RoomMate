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
            string[] randomCities = unitOfWork.AddressesRepository.GetRandomDistinctCity();
            //take list of random rooms
            homeViewModel.firstCityRandomRooms = unitOfWork.RoomsRepository.GetRandomRoomsByCity(randomCities[0], 4);
            homeViewModel.secondCityRandomRooms = unitOfWork.RoomsRepository.GetRandomRoomsByCity(randomCities[1], 4);
            
            homeViewModel.randomRooms = unitOfWork.RoomsRepository.GetRandomRooms(4);
            //take images for room
            var listWithAllRooms = homeViewModel.firstCityRandomRooms
                                   .Concat(homeViewModel.secondCityRandomRooms)
                                   .Concat(homeViewModel.randomRooms).ToList();
            homeViewModel.randomRoomsImages = unitOfWork.RoomImagesRepository.GetFirstImageForRooms(listWithAllRooms);
            //take amount of guest orderby ascending
            homeViewModel.selectGuestList = setSelectListWithGuests();
            return View(homeViewModel);
        }
        public List<SelectListItem> setSelectListWithGuests()
        {
            List<SelectListItem> guestList = new List<SelectListItem>();
            guestList.Add(new SelectListItem() { Text = "Wybierz liczbę gości:" , Value = "0"});

            for(int numberOfGuest = 1; numberOfGuest <= 15; numberOfGuest++)
            {
                guestList.Add(new SelectListItem() { Text = "Liczba gości: " + numberOfGuest, Value = numberOfGuest.ToString() }); 
            }

            return guestList;
        }

    }
}