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
    public class ReservationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reservations
        public ActionResult Index()
        {
            var reservations = db.Reservations.Include(r => r.Room);
            return View(reservations.ToList());
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult Create(int? id)
        {
            var reservation = new Reservation();

            if (id.HasValue)
            {
                reservation.RoomId = id.Value;
            }
            var room = db.Rooms.Include(r => r.Hotel).FirstOrDefault(r => r.Id == id);
            var hotelot = room.Hotel.Name;

            ViewBag.RoomId = new SelectList(db.Rooms, "Id", "Number", reservation.RoomId);
            ViewBag.BrojSoba = room.Number;
            ViewBag.Hotelot = hotelot;
            ViewBag.RoomPrice = db.Rooms.Find(id).Price;
            return View(); 
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Surname,DateOfReservation,EndOfReservation,RoomId,HotelId")] Reservation new_reservation)
        {
            if (new_reservation.EndOfReservation <= new_reservation.DateOfReservation)
            {
                ModelState.AddModelError("", "Крајниот датум мора да биде после почетниот датум.");

            }

            bool vekjeRez = db.Reservations.Any(rez =>
                rez.RoomId == new_reservation.RoomId &&
                (
                    (new_reservation.DateOfReservation >= rez.DateOfReservation && new_reservation.DateOfReservation < rez.EndOfReservation) ||
                    (new_reservation.EndOfReservation > rez.DateOfReservation && new_reservation.EndOfReservation <= rez.EndOfReservation) ||
                    (new_reservation.DateOfReservation <= rez.DateOfReservation && new_reservation.EndOfReservation >= rez.EndOfReservation)
                    ));

            if (vekjeRez)
            {
                ModelState.AddModelError("", "Оваа соба е веќе резервирана во избраниот период.");
            }

            if (!ModelState.IsValid)
            {
                var room = db.Rooms.Include(r => r.Hotel).FirstOrDefault(r => r.Id == new_reservation.RoomId);
                ViewBag.RoomId = new SelectList(db.Rooms, "Id", "Number", new_reservation.RoomId);
                if (room != null)
                {
                    ViewBag.BrojSoba = room.Number;
                    ViewBag.Hotelot = room.Hotel.Name;
                }

                return View(new_reservation);
            }

            var roomPrice = db.Rooms.Find(new_reservation.RoomId).Price;
            var numDays = (new_reservation.EndOfReservation - new_reservation.DateOfReservation).Days;
            new_reservation.TotalPrice = numDays * roomPrice;


            db.Reservations.Add(new_reservation);
            db.SaveChanges();

            return RedirectToAction("Index", "Rooms");
        }




        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomId = new SelectList(db.Rooms, "Id", "ImageURL", reservation.RoomId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,DateOfReservation,EndOfReservation,RoomId")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomId = new SelectList(db.Rooms, "Id", "ImageURL", reservation.RoomId);
            return View(reservation);
        }

        
        public ActionResult Delete(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
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
    }
}
