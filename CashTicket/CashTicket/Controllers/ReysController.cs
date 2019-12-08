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
    public class ReysController : Controller
    {
        private CashDeskEntities db = new CashDeskEntities();

        // GET: Reys
        public ActionResult Index(string search1, string search2)
        {
            List<Rey> listreys = db.Reys.ToList();
            return View(db.Reys.Where(x => x.start_point.StartsWith(search1) || search1 == null || x.end_point.StartsWith(search2) || search2 == null).ToList());
        }

        // GET: Reys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rey rey = db.Reys.Find(id);
            if (rey == null)
            {
                return HttpNotFound();
            }
            return View(rey);
        }

        // GET: Reys/Create
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Create()
        {
            ViewBag.train_id = new SelectList(db.Trains, "id_train", "id_train");
            return View();
        }

        // POST: Reys/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Create([Bind(Include = "id_reys,train_id,start_point,end_point,start_date,end_date")] Rey rey)
        {
            if (ModelState.IsValid)
            {
                db.Reys.Add(rey);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.train_id = new SelectList(db.Trains, "id_train", "id_train", rey.train_id);
            return View(rey);
        }

        // GET: Reys/Edit/5
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rey rey = db.Reys.Find(id);
            if (rey == null)
            {
                return HttpNotFound();
            }
            ViewBag.train_id = new SelectList(db.Trains, "id_train", "id_train", rey.train_id);
            return View(rey);
        }

        // POST: Reys/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Edit([Bind(Include = "id_reys,train_id,start_point,end_point,start_date,end_date")] Rey rey)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rey).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.train_id = new SelectList(db.Trains, "id_train", "id_train", rey.train_id);
            return View(rey);
        }

        // GET: Reys/Delete/5
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rey rey = db.Reys.Find(id);
            if (rey == null)
            {
                return HttpNotFound();
            }
            return View(rey);
        }

        // POST: Reys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Администратор, Менеджер")]
        public ActionResult DeleteConfirmed(int id)
        {
            Rey rey = db.Reys.Find(id);
            db.Reys.Remove(rey);
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
