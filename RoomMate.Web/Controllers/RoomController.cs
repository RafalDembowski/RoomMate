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
using RoomMate.Entities.Users;
using RoomMate.Entities.Bookings;

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
                prepareRoomsAndRoomImagesAndGuestSelectList(id);

                if (roomViewModel.room != null && roomViewModel.roomImages != null)
                {
                    return View(roomViewModel);
                }

            }
           
            return RedirectToAction("Index", "Home");

        }
        [HttpPost]
        public ActionResult DisplayRoom(string id, RoomViewModel _roomViewModel)
        {
            prepareRoomsAndRoomImagesAndGuestSelectList(id);
            if (Session["UserID"] != null)
            {
                //get active user
                string userID = Session["UserID"].ToString();
                User activeUser = unitOfWork.UsersRepository.GetActiveUser(new Guid(userID));
                try
                {
                    if (ModelState.IsValid)
                    {
                        bool dateIsAvailable = unitOfWork.BookingRepository.checkRoomAvailability(_roomViewModel.booking.InDate, _roomViewModel.booking.OutDate, _roomViewModel.room.RoomID);
                        if (!dateIsAvailable)
                        {
                            //create booking
                            Booking booking = new Booking();
                            booking.BookingID = Guid.NewGuid();
                            booking.InDate = _roomViewModel.booking.InDate;
                            booking.OutDate = _roomViewModel.booking.OutDate;
                            booking.NumberOfGuests = _roomViewModel.booking.NumberOfGuests;
                            booking.Room = roomViewModel.room;
                            booking.TotalPrice = _roomViewModel.booking.countTotalCostForBooking(booking.InDate, booking.OutDate, booking.Room.Price, booking.NumberOfGuests);
                            booking.User = activeUser;

                            unitOfWork.BookingRepository.Insert(booking);
                            unitOfWork.Complete();

                            return RedirectToAction("BookingDetail", "Booking", new { id = booking.BookingID });
                        }
                        else
                        {
                            ModelState.AddModelError("isBooking", "Brak miejsc w tym terminie");
                            return View(roomViewModel);
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + e.Message);
                    ViewBag.Error = "Wystąpił błąd, proszę powtórzyć jeszcze raz.";
                    return View(roomViewModel);
                }
                return View(roomViewModel);
            }
            ViewBag.Error = "Twoja sesja wygasła, spróbuj jeszcze raz.";
            return View(roomViewModel);
        }

        [HttpGet]
        public ActionResult SearchRoom(string searchCity, int? page, string currentFilter, string numberOfGuest, DateTime? inDate, DateTime? outDate)
        {
            roomViewModel.sortSelectList = setSortList();

            roomViewModel.searchCity = searchCity;
            roomViewModel.currentFilter = currentFilter;
            roomViewModel.numberOfGuest = numberOfGuest;
            roomViewModel.inDate = inDate;
            roomViewModel.outDate = outDate;


            if (searchCity != null || !String.IsNullOrEmpty(searchCity))
            {

                if(!String.IsNullOrEmpty(numberOfGuest) && numberOfGuest != null)
                {
                    int numberOfGuestInt = int.Parse(numberOfGuest);

                    roomViewModel.rooms = unitOfWork.RoomsRepository.Get(
                                                 filter: r => r.Address.City == searchCity && r.IsActive == true && r.NumberOfGuests >= numberOfGuestInt,
                                                 orderBy: null,
                                                 includeProperties: "Address,Equipment"
                                                 ).ToList();
                }
                else
                {
                    roomViewModel.rooms = unitOfWork.RoomsRepository.Get(
                                                 filter: r => r.Address.City == searchCity && r.IsActive == true,
                                                 orderBy: null,
                                                 includeProperties: "Address,Equipment"
                                                 ).ToList();
                }
                //get only first image to display in page
                roomViewModel.roomImages = unitOfWork.RoomImagesRepository.GetFirstImageForRooms(roomViewModel.rooms);

                if (roomViewModel.rooms != null && roomViewModel.roomImages != null && roomViewModel.rooms.Any() && roomViewModel.roomImages.Any()) 
                {
                    //get rooms that are available on the given date
                    if (inDate != null && outDate != null)
                    {
                        List<Room> roomListToEnumartion = new List<Room>();
                        roomListToEnumartion.AddRange(roomViewModel.rooms);

                        foreach (var room in roomListToEnumartion)
                        {
                            bool dateIsAvailable = unitOfWork.BookingRepository.checkRoomAvailability((DateTime)inDate, (DateTime)outDate, room.RoomID);
                            if(dateIsAvailable == true)
                            {
                                roomViewModel.rooms.Remove(
                                                    roomViewModel.rooms
                                                    .Where(r => r.RoomID == room.RoomID)
                                                    .First());
                            }
                        }
                    }

                    //sort rooms
                    if (!String.IsNullOrEmpty(currentFilter) && currentFilter != null)
                    {
                        switch (currentFilter)
                        {
                            case "priceAscending":
                                roomViewModel.rooms = roomViewModel.rooms
                                                                   .OrderBy(r => r.Price)
                                                                   .ToList();
                                break;
                            case "priceDescending":
                                roomViewModel.rooms = roomViewModel.rooms
                                                                   .OrderByDescending(r => r.Price)
                                                                   .ToList();
                                break;
                            case "date":
                                break;
                        }
                    }

                    //set pagination
                    int pageSize = 2;
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
        public List<SelectListItem> setGuestSelectList(Guid roomID)
        {
            //get current room
            var room = unitOfWork.RoomsRepository.GetById(roomID);
            //set selectListItem
            List<SelectListItem> guestList = new List<SelectListItem>();
            guestList.Add(new SelectListItem() { Text = "Wybierz liczbę gości:", Value = "0" });

            for (int i = 1 ; i <= room.NumberOfGuests; i++)
            {
                guestList.Add(new SelectListItem() { Text = "Liczba gości: " + i, Value = i.ToString() }); 

            }

            return guestList;
        }
        public void prepareRoomsAndRoomImagesAndGuestSelectList(string id)
        {
            roomViewModel.room = unitOfWork.RoomsRepository.Get(filter: r => r.RoomID == new Guid(id),
                                              orderBy: null,
                                              includeProperties: "Address,Equipment")
                                              .FirstOrDefault();

            roomViewModel.roomImages = unitOfWork.RoomImagesRepository.Get(filter: i => i.Room.RoomID == new Guid(id),
                                                                        orderBy: null,
                                                                        includeProperties: "")
                                                                        .ToList();

            roomViewModel.guestSelectList = setGuestSelectList(roomViewModel.room.RoomID);
        }
    }
}