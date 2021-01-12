using RoomMate.Data.Context;
using RoomMate.Data.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoomMate.Controllers
{
    public class BookingController : Controller
    {
        private UnitOfWork unitOfWork;
        public BookingController()
        {
            unitOfWork = new UnitOfWork(new RoomMateDbContext());
        }

        [HttpGet]
        public ActionResult BookingDetail(string id)
        {
            bool codeActivationCanBeGuid = Guid.TryParse(id, out var newGuid);
            if (!String.IsNullOrEmpty(id) && codeActivationCanBeGuid == true && !id.Equals("00000000-0000-0000-0000-000000000000"))
            {
                var booking = unitOfWork.BookingRepository.Get(filter: b => b.BookingID == new Guid(id),
                                                               orderBy: null,
                                                               includeProperties: "Room,User").FirstOrDefault();
                if (booking != null)
                {
                    return View(booking);
                }

            }
            return RedirectToAction("Index", "Home");
        }
    }
}