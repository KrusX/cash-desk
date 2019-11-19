using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CashTicket.Models;

namespace CashTicket.Controllers
{
    public class PlacesController : Controller
    {
        private CashDeskEntities db = new CashDeskEntities();

        // GET: Places
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Index(int? id)
        {
            List<Place> listtickets = db.Places.ToList();
            var places = db.Places.Include(p => p.Wagon);
            return View(db.Places.Where(x => x.wagon_id == (id)).ToList());
        }

        // GET: Places/Details/5
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place place = db.Places.Find(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            return View(place);
        }

        // GET: Places/Create
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Create()
        {
            ViewBag.wagon_id = new SelectList(db.Wagons, "id_wagon", "id_wagon");
            return View();
        }

        // POST: Places/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Create([Bind(Include = "id_place,wagon_id")] Place place)
        {
            if (ModelState.IsValid)
            {
                db.Places.Add(place);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.wagon_id = new SelectList(db.Wagons, "id_wagon", "id_wagon", place.wagon_id);
            return View(place);
        }

        // GET: Places/Edit/5
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place place = db.Places.Find(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            ViewBag.wagon_id = new SelectList(db.Wagons, "id_wagon", "id_wagon", place.wagon_id);
            return View(place);
        }

        // POST: Places/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Edit([Bind(Include = "id_place,wagon_id")] Place place)
        {
            if (ModelState.IsValid)
            {
                db.Entry(place).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.wagon_id = new SelectList(db.Wagons, "id_wagon", "id_wagon", place.wagon_id);
            return View(place);
        }

        // GET: Places/Delete/5
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place place = db.Places.Find(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            return View(place);
        }

        // POST: Places/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult DeleteConfirmed(int id)
        {
            Place place = db.Places.Find(id);
            db.Places.Remove(place);
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
