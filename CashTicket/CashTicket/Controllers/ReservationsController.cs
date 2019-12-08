using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CashTicket.Models;
using Microsoft.AspNet.Identity;

namespace CashTicket.Controllers
{
    public class ReservationsController : Controller
    {
        private CashDeskEntities db = new CashDeskEntities();

        [Authorize(Roles="Администратор, Менеджер, Клиент")]
        // GET: Reservations
        public ActionResult Index()
        {
            var reservations = db.Reservations.Include(r => r.Client).Include(r => r.Status_res).Include(r => r.Ticket);
            if (User.IsInRole("Клиент"))
            {
                string currentUserName = User.Identity.Name;
                Client client = db.Clients.FirstOrDefault(x => x.login == currentUserName);
                return View(db.Reservations.Where(x => x.client_id == client.id_client).ToList());
            }
            else
            {
                return View(reservations.ToList());
            }
        }

        [Authorize(Roles = "Администратор, Менеджер, Клиент")]
        public ActionResult Pay(int id)
        {
            var reserve = db.Reservations.Where(c => c.id_reservation == id).FirstOrDefault();
            if (reserve.status_reservation_id == 2)
            {
                reserve.status_reservation_id = 1;
                db.SaveChanges();
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Администратор, Менеджер, Клиент")]
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

        [Authorize(Roles = "Администратор, Менеджер")]
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

            ViewBag.client_id = new SelectList(db.Clients, "id_client", "surname", reservation.client_id);
            ViewBag.status_reservation_id = new SelectList(db.Status_res, "id_status_reservation", "name_status", reservation.status_reservation_id);
            ViewBag.ticket_id = new SelectList(db.Tickets, "id_ticket", "id_ticket", reservation.ticket_id);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_reservation,ticket_id,date_reservation,client_id,status_reservation_id")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.client_id = new SelectList(db.Clients, "id_client", "surname", reservation.client_id);
            ViewBag.status_reservation_id = new SelectList(db.Status_res, "id_status_reservation", "name_status", reservation.status_reservation_id);
            ViewBag.ticket_id = new SelectList(db.Tickets, "id_ticket", "id_ticket", reservation.ticket_id);
            return View(reservation);
        }

        [Authorize(Roles = "Администратор, Менеджер, Клиент")]
        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
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

        [Authorize(Roles = "Администратор, Менеджер, Клиент")]
        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            var tick = db.Tickets.Where(c => c.id_ticket == reservation.ticket_id).FirstOrDefault();
            tick.status_ticket_id = 1;
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
