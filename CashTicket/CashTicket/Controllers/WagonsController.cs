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
    public class WagonsController : Controller
    {
        private CashDeskEntities db = new CashDeskEntities();

        // GET: Wagons
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Index(string search_wagon)
        {
            int? s_wagon = Convert.ToInt32(search_wagon);
            List<Wagon> listreys = db.Wagons.ToList();
            var wagons = db.Wagons.Include(w => w.Train).Include(w => w.Type_wagon);
            return View(db.Wagons.Where(x => x.train_id == s_wagon || s_wagon == 0).ToList());
        }

        // GET: Wagons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wagon wagon = db.Wagons.Find(id);
            if (wagon == null)
            {
                return HttpNotFound();
            }
            return View(wagon);
        }

        // GET: Wagons/Create
        public ActionResult Create()
        {
            ViewBag.train_id = new SelectList(db.Trains, "id_train", "id_train");
            ViewBag.type_wagon_id = new SelectList(db.Type_wagon, "id_type_wagon", "name_type");
            return View();
        }

        // POST: Wagons/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_wagon,train_id,type_wagon_id")] Wagon wagon)
        {
            if (ModelState.IsValid)
            {
                db.Wagons.Add(wagon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.train_id = new SelectList(db.Trains, "id_train", "id_train", wagon.train_id);
            ViewBag.type_wagon_id = new SelectList(db.Type_wagon, "id_type_wagon", "name_type", wagon.type_wagon_id);
            return View(wagon);
        }

        // GET: Wagons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wagon wagon = db.Wagons.Find(id);
            if (wagon == null)
            {
                return HttpNotFound();
            }
            ViewBag.train_id = new SelectList(db.Trains, "id_train", "id_train", wagon.train_id);
            ViewBag.type_wagon_id = new SelectList(db.Type_wagon, "id_type_wagon", "name_type", wagon.type_wagon_id);
            return View(wagon);
        }

        // POST: Wagons/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_wagon,train_id,type_wagon_id")] Wagon wagon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wagon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.train_id = new SelectList(db.Trains, "id_train", "id_train", wagon.train_id);
            ViewBag.type_wagon_id = new SelectList(db.Type_wagon, "id_type_wagon", "name_type", wagon.type_wagon_id);
            return View(wagon);
        }

        // GET: Wagons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wagon wagon = db.Wagons.Find(id);
            if (wagon == null)
            {
                return HttpNotFound();
            }
            return View(wagon);
        }

        // POST: Wagons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Wagon wagon = db.Wagons.Find(id);
            db.Wagons.Remove(wagon);
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
