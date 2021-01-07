using RoomMate.Data.Context;
using RoomMate.Data.UnitOfWorks;
using RoomMate.Entities.RoomControllerViewModels;
using RoomMate.Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace RoomMate.Controllers
{
    public class RoomController : Controller
    {
        private UnitOfWork unitOfWork;
        private RoomViewModel roomViewModel;
        public RoomController()
        {
            unitOfWork = new UnitOfWork(new RoomMateDbContext());
            roomViewModel = new RoomViewModel();
        }
        [HttpGet]
        public ActionResult DisplayRoom(string id)
        {
            bool codeActivationCanBeGuid = Guid.TryParse(id, out var newGuid);

            if (!String.IsNullOrEmpty(id) && codeActivationCanBeGuid == true && !id.Equals("00000000-0000-0000-0000-000000000000"))
            {
                roomViewModel.room = unitOfWork.RoomsRepository.Get(filter: r => r.RoomID == new Guid(id),
                                                              orderBy: null,
                                                              includeProperties: "Address,Equipment")
                                                              .FirstOrDefault();

                roomViewModel.roomImages = unitOfWork.RoomImagesRepository.Get(filter: i => i.Room.RoomID == new Guid(id),
                                                                            orderBy: null,
                                                                            includeProperties: "")
                                                                            .ToList();

                if(roomViewModel.room != null && roomViewModel.roomImages != null)
                {
                    return View(roomViewModel);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("Index", "Home");

        }
        [HttpGet]
        public ActionResult SearchRoom(string searchCity, int? page, string currentFilter)
        {
            if (searchCity != null || !String.IsNullOrEmpty(searchCity))
            {

                roomViewModel.rooms = unitOfWork.RoomsRepository.Get(
                                                                 filter: r => r.Address.City == searchCity && r.IsActive == true,
                                                                 orderBy: null ,
                                                                 includeProperties: "Address,Equipment"
                                                                 ).ToList();

                roomViewModel.roomImages = getFirstImageForRooms(roomViewModel.rooms);
                roomViewModel.sortSelectList = setSortList();
                roomViewModel.searchCity = searchCity;
                roomViewModel.currentFilter = currentFilter;
               
                if (roomViewModel.rooms != null && roomViewModel.roomImages != null && roomViewModel.rooms.Any() && roomViewModel.roomImages.Any()) 
                {
                    //sort rooms
                    if(!String.IsNullOrEmpty(currentFilter) && currentFilter != null)
                    {
                        System.Diagnostics.Debug.WriteLine("jestem tutaj");
                        switch (currentFilter)
                        {
                            case "priceAscending":
                                roomViewModel.rooms = roomViewModel.rooms.OrderBy(r => r.Price).ToList();
                                break;
                            case "priceDescending":
                                roomViewModel.rooms = roomViewModel.rooms.OrderByDescending(r => r.Price).ToList();
                                break;
                            case "date":
                                break;
                        }
                    }
                    //set pagination
                    int pageSize = 4;
                    int pageNumber = (page ?? 1);
                    ViewBag.OnePageOfRooms = roomViewModel.rooms.ToPagedList(pageNumber, pageSize);

                    return View(roomViewModel);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public List<RoomImage> getFirstImageForRooms(List<Room> rooms)
        {
            List<RoomImage> roomImages = new List<RoomImage>();

            foreach (var room in rooms)
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

        public List<SelectListItem> setSortList()
        {
            List<SelectListItem> sortListItem = new List<SelectListItem>
            {
                new SelectListItem { Text = "Sortuj według: ceny rosnąco" , Value = "priceAscending"},
                new SelectListItem { Text = "Sortuj według: ceny malejąco", Value = "priceDescending"},
            };
            sortListItem.Insert(0, new SelectListItem { Text = "Sortuj według: najnowszy", Value = "date" });

            return sortListItem;
        }
    }
}