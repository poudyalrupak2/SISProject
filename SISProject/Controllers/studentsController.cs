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
    public class studentsController : Controller
    {
        private SisDbContext db = new SisDbContext();

        // GET: students
        public ActionResult Index()
        {
            return View(db.students.ToList());
        }

        // GET: students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = db.students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,Address,Status")] student student)
        {
            if (ModelState.IsValid)
            {
                int data = db.login.Where(t => t.Email == student.Email).Count();
                if (data > 0)
                {
                    ModelState.AddModelError("", "Email already exists");
                    return View();

                }
                Random generator = new Random();
                String password = generator.Next(0, 999999).ToString("D6");



                var message = new MailMessage();
                message.To.Add(new MailAddress(student.Email));
                message.Subject = "Account has been created";
                message.Body = "Use this Password to login:" + password;
                using (var smtp = new SmtpClient())
                {
                    try
                    {

                        smtp.Send(message);
                        db.login.Add(new Login
                        {
                            Email = student.Email,
                            Role = "Student",
                            RandomPass = password,


                        });


                        TempData["Message"] = "Student Created Successfully.";


                    }
                    catch (Exception e)
                    {

                        return new HttpStatusCodeResult(HttpStatusCode.RequestTimeout);
                    }
                }
                student.Status = true;
                db.students.Add(student);
                db.SaveChanges();
                int id = student.Id;
                var text = System.IO.File.ReadAllText("C:\\Users\\Rupak\\Desktop\\cp-user-behavior-master\\Data\\NewBehavior.txt");
                List<string> lines = System.IO.File.ReadAllLines("C:\\Users\\Rupak\\Desktop\\cp-user-behavior-master\\Data\\NewBehavior.txt").ToList();
                int index = text.IndexOf("# User actions");
                text = text.Insert(index,id+" "+student.FirstName  + Environment.NewLine);
                System.IO.File.WriteAllText("C:\\Users\\Rupak\\Desktop\\cp-user-behavior-master\\Data\\NewBehavior.txt", text);

                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = db.students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,Address,Status")] student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = db.students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            student student = db.students.Find(id);
            db.students.Remove(student);
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
