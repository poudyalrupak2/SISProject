using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using HotelManagemant.Filters;
using SISProject.Data;
using SISProject.Models;

namespace SISProject.Controllers
{
    [SessionCheck]
    public class TeachersController : Controller
    {
        private SisDbContext db = new SisDbContext();

        // GET: Teachers
        public ActionResult Index()
        {
            return View(db.teachers.ToList());
        }

        // GET: Teachers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // GET: Teachers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,PhoneNo,Address")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                int data = db.login.Where(t => t.Email == teacher.Email).Count();
                if (data > 0)
                {
                    ModelState.AddModelError("", "Email already exists");
                    return View();
                }
                Random generator = new Random();
                String password = generator.Next(0, 999999).ToString("D6");



                var message = new MailMessage();
                message.To.Add(new MailAddress(teacher.Email));
                message.Subject = "Account has been created";
                message.Body = "Use this Password to login:" + password;
                using (var smtp = new SmtpClient())
                {
                    try
                    {

                        smtp.Send(message);
                        db.login.Add(new Login
                        {
                            Email = teacher.Email,
                            Role = "teacher",
                            RandomPass = password,


                        });

                        TempData["Message"] = "Teacher Created Successfully.";


                    }
                    catch (Exception e)
                    {

                        return new HttpStatusCodeResult(HttpStatusCode.RequestTimeout);
                    }
                }
                db.teachers.Add(teacher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,PhoneNo,Address")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Teacher teacher = db.teachers.Find(id);
            db.teachers.Remove(teacher);
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
