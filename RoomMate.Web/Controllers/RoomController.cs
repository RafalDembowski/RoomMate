using RoomMate.Data.Context;
using RoomMate.Data.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoomMate.Controllers
{
    public class RoomController : Controller
    {
        private UnitOfWork unitOfWork;
        public RoomController()
        {
            unitOfWork = new UnitOfWork(new RoomMateDbContext());

        }
        [HttpGet]
        public ActionResult DisplayRoom(string id)
        {
            bool codeActivationCanBeGuid = Guid.TryParse(id, out var newGuid);

            if (!String.IsNullOrEmpty(id) && codeActivationCanBeGuid == true && !id.Equals("00000000-0000-0000-0000-000000000000"))
            {
                var room = unitOfWork.RoomsRepository.Get(filter: r => r.RoomID == new Guid(id),
                                                          orderBy: null,
                                                          includeProperties: "Address,Equipment")
                                                          .FirstOrDefault();
                return View(room);
            }

            return RedirectToAction("Index", "Home");

        }


    }
}