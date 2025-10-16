using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Booking.Models;

namespace Booking.Controllers
{
    public class RoomsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rooms
        [AllowAnonymous]
        public ActionResult Index()
        {
            var rooms = db.Rooms
                  .Include(r => r.Hotel)          
                  .Include("Hotel.Town");         

            return View(rooms.ToList());
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var room = db.Rooms
                 .Include(r => r.Hotel.Town) 
                 .FirstOrDefault(r => r.Id == id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Number,Capacity,ImageURL,Price,Status,HotelId")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name", room.HotelId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name", room.HotelId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Number,Capacity,ImageURL,Price,Status,HotelId")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name", room.HotelId);
            return View(room);
        }

        
        public ActionResult Delete(int id)
        {
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult Search(string town, DateTime? startDate, DateTime? endDate, int? capacity)
        {

            ViewBag.Town = town;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.Capacity = capacity;



            var rooms = db.Rooms.Include(roomss => roomss.Hotel.Town).AsQueryable();

            if (!string.IsNullOrEmpty(town))
            {
                rooms = rooms.Where(roomss => roomss.Hotel.Town.Name.Contains(town));
            }

            if (capacity.HasValue)
            {
                rooms = rooms.Where(roomss => roomss.Capacity >= capacity.Value);
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                if (startDate.Value >= endDate.Value)
                {
                    TempData["DateError"] = "Крајниот датум мора да биде после почетниот датум!";
                    return View("~/Views/Home/Index.cshtml");
                }

                rooms = rooms.Where(roomss => !db.Reservations.Any(res =>
                    res.RoomId == roomss.Id &&
                    (
                        (startDate >= res.DateOfReservation && startDate < res.EndOfReservation) ||
                        (endDate > res.DateOfReservation && endDate <= res.EndOfReservation) ||
                        (startDate <= res.DateOfReservation && endDate >= res.EndOfReservation)
                    )
                ));
            }

            var results = rooms.ToList();
            return View("~/Views/Home/Index.cshtml", results);
        }

    }
}
