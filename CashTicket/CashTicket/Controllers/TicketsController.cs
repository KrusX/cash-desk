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
    public class TicketsController : Controller
    {
        private CashDeskEntities db = new CashDeskEntities();

        [Authorize(Roles = "Администратор, Менеджер, Клиент")]
        public ActionResult AddToReserve(int id)
        {
            var tick = db.Tickets.Where(c => c.id_ticket == id).FirstOrDefault();
            tick.status_ticket_id = 2;
            string currentUserName = User.Identity.Name;
            Client client = db.Clients.FirstOrDefault(x => x.login == currentUserName);
            db.Reservations.Add(new Reservation { ticket_id = id, date_reservation = DateTime.Now, client_id = client.id_client, status_reservation_id = 2 });
            db.SaveChanges();

            return View();
        }

        // GET: Tickets
        [Authorize(Roles = "Администратор, Менеджер, Клиент")]
        public ActionResult Index(int id)
        {
            List<Ticket> listtickets = db.Tickets.ToList();
            var tickets = db.Tickets.Include(t => t.Place).Include(t => t.Rey).Include(t => t.Status_ticket);
            return View(db.Tickets.Where(x => x.reys_id == (id) && x.status_ticket_id.Equals(1)).ToList());
            // return View(tickets.ToList());
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Create()
        {
            ViewBag.place_id = new SelectList(db.Places, "id_place", "id_place");
            ViewBag.reys_id = new SelectList(db.Reys, "id_reys", "start_point");
            ViewBag.status_ticket_id = new SelectList(db.Status_ticket, "id_status_ticket", "name_status");
            return View();
        }

        // POST: Tickets/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Create([Bind(Include = "id_ticket,reys_id,place_id,price,status_ticket_id")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.place_id = new SelectList(db.Places, "id_place", "id_place", ticket.place_id);
            ViewBag.reys_id = new SelectList(db.Reys, "id_reys", "start_point", ticket.reys_id);
            ViewBag.status_ticket_id = new SelectList(db.Status_ticket, "id_status_ticket", "name_status", ticket.status_ticket_id);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.place_id = new SelectList(db.Places, "id_place", "id_place", ticket.place_id);
            ViewBag.reys_id = new SelectList(db.Reys, "id_reys", "start_point", ticket.reys_id);
            ViewBag.status_ticket_id = new SelectList(db.Status_ticket, "id_status_ticket", "name_status", ticket.status_ticket_id);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Edit([Bind(Include = "id_ticket,reys_id,place_id,price,status_ticket_id")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.place_id = new SelectList(db.Places, "id_place", "id_place", ticket.place_id);
            ViewBag.reys_id = new SelectList(db.Reys, "id_reys", "start_point", ticket.reys_id);
            ViewBag.status_ticket_id = new SelectList(db.Status_ticket, "id_status_ticket", "name_status", ticket.status_ticket_id);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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
