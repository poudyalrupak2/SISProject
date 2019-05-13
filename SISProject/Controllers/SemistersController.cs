using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelManagemant.Filters;
using SISProject.Data;
using SISProject.Models;

namespace SISProject.Controllers
{
    [SessionCheck]
    public class SemistersController : Controller
    {
        private SisDbContext db = new SisDbContext();

        // GET: Semisters
        public ActionResult Index()
        {
            return View(db.semisters.ToList());
        }

        // GET: Semisters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semister semister = db.semisters.Find(id);
            if (semister == null)
            {
                return HttpNotFound();
            }
            return View(semister);
        }

        // GET: Semisters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Semisters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SemisterName")] Semister semister)
        {
            if (ModelState.IsValid)
            {
                db.semisters.Add(semister);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(semister);
        }

        // GET: Semisters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semister semister = db.semisters.Find(id);
            if (semister == null)
            {
                return HttpNotFound();
            }
            return View(semister);
        }

        // POST: Semisters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SemisterName")] Semister semister)
        {
            if (ModelState.IsValid)
            {
                db.Entry(semister).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(semister);
        }

        // GET: Semisters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semister semister = db.semisters.Find(id);
            if (semister == null)
            {
                return HttpNotFound();
            }
            return View(semister);
        }

        // POST: Semisters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Semister semister = db.semisters.Find(id);
            db.semisters.Remove(semister);
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
