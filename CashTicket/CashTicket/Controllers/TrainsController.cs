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
    public class TrainsController : Controller
    {
        private CashDeskEntities db = new CashDeskEntities();

        // GET: Trains
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Index()
        {
            var trains = db.Trains.Include(t => t.Type_train);
            return View(trains.ToList());
        }

        // GET: Trains/Create
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Create()
        {
            ViewBag.type_train_id = new SelectList(db.Type_train, "id_type_train", "name_type");
            return View();
        }

        // POST: Trains/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Create([Bind(Include = "id_train,type_train_id")] Train train)
        {
            if (ModelState.IsValid)
            {
                db.Trains.Add(train);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.type_train_id = new SelectList(db.Type_train, "id_type_train", "name_type", train.type_train_id);
            return View(train);
        }

        // GET: Trains/Edit/5
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Train train = db.Trains.Find(id);
            if (train == null)
            {
                return HttpNotFound();
            }
            ViewBag.type_train_id = new SelectList(db.Type_train, "id_type_train", "name_type", train.type_train_id);
            return View(train);
        }

        // POST: Trains/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Edit([Bind(Include = "id_train,type_train_id")] Train train)
        {
            if (ModelState.IsValid)
            {
                db.Entry(train).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.type_train_id = new SelectList(db.Type_train, "id_type_train", "name_type", train.type_train_id);
            return View(train);
        }

        // GET: Trains/Delete/5
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Train train = db.Trains.Find(id);
            if (train == null)
            {
                return HttpNotFound();
            }
            return View(train);
        }

        // POST: Trains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult DeleteConfirmed(int id)
        {
            Train train = db.Trains.Find(id);
            db.Trains.Remove(train);
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
