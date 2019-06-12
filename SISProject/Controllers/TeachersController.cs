using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
    [Authorize(Roles = "SuperAdmin")]

    public class TeachersController : Controller
    {
        private SisDbContext db = new SisDbContext();

        // GET: Teachers
        public ActionResult Index()
        {
            return View(db.teachers.Where(m=>m.status==true).ToList());
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
        public ActionResult Create( Teacher teacher,HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                int data = db.login.Where(t => t.Email == teacher.Email).Count();
                if (data > 0)
                {
                    ModelState.AddModelError("", "Email already exists");
                    return View();
                }
                if (photo != null && photo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(photo.FileName);
                    var fileName1 = Path.GetFileNameWithoutExtension(photo.FileName);
                    fileName1 = fileName1.Replace(" ", "_");

                    // extract only the fielname
                    var ext = Path.GetExtension(fileName.ToLower());            //extract only the extension of filename and then convert it into lower case.


                    int name;
                    try
                    {
                        name = db.teachers.OrderByDescending(m => m.Id).FirstOrDefault().Id;
                    }
                    catch
                    {
                        name = 1;
                    }
                    string firstpath1 = "/TeaPhoto/";
                    string secondpath = "/TeaPhoto/" + name + "/";
                    bool exists1 = System.IO.Directory.Exists(Server.MapPath(firstpath1));
                    bool exists2 = System.IO.Directory.Exists(Server.MapPath(secondpath));
                    if (!exists1)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(firstpath1));

                    }
                    if (!exists2)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(secondpath));

                    }
                    var path = Server.MapPath("/TeaPhoto/" + name + "/" + fileName1 + ext);

                    photo.SaveAs(path);
                    teacher.photopath = "/TeaPhoto/" + name + "/" + fileName1 + ext;


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
                teacher.status = true;
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
        public ActionResult Edit( Teacher teacher,HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null && photo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(photo.FileName);
                    var fileName1 = Path.GetFileNameWithoutExtension(photo.FileName);
                    fileName1 = fileName1.Replace(" ", "_");

                    // extract only the fielname
                    var ext = Path.GetExtension(fileName.ToLower());            //extract only the extension of filename and then convert it into lower case.


                    int name;
                    name = teacher.Id;
                    string firstpath1 = "/TeaPhoto/";
                    string secondpath = "/TeaPhoto/" + name + "/";
                    bool exists1 = System.IO.Directory.Exists(Server.MapPath(firstpath1));
                    bool exists2 = System.IO.Directory.Exists(Server.MapPath(secondpath));
                    if (!exists1)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(firstpath1));

                    }
                    if (!exists2)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(secondpath));

                    }
                    var path = Server.MapPath("/TeaPhoto/" + name + "/" + fileName1 + ext);

                    photo.SaveAs(path);
                    teacher.photopath = "/TeaPhoto/" + name + "/" + fileName1 + ext;


                }
                else
                {
                    teacher.photopath = db.teachers.Where(m => m.Id == teacher.Id).FirstOrDefault().photopath;
                }
                Teacher tea = db.teachers.Where(m => m.Id == teacher.Id).FirstOrDefault();
                tea.Name = teacher.Name;
                tea.HireDate = teacher.HireDate;
                tea.PhoneNo = teacher.PhoneNo;
                tea.photopath = teacher.photopath;
                tea.Gender = teacher.Gender;
                tea.Address = tea.Address;
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teacher);
        }

        public ActionResult status(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher student = db.teachers.Find(id);
            if (student.status == true)
            {
                student.status = false;
            }
            else
            {
                student.status = true;
            }
            if (student == null)
            {
                return HttpNotFound();
            }
            db.SaveChanges();
            return RedirectToAction("Index");
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
