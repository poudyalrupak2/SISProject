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

    public class studentsController : Controller
    {
        private SisDbContext db = new SisDbContext();

        // GET: students
        public ActionResult Index()
        {
            return View(db.students.Where(m=>m.Status==true).ToList());
        }
        public ActionResult DisabledStudent()
        {
            return View(db.students.Where(m => m.Status == false).ToList());
        }

        // GET: students/Details/5E:\SISProject\SISProject\Controllers\TeachersController.cs
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
            List<SelectListItem> listItems = new List<SelectListItem>();


            foreach (var item in db.semisters)
            {
                listItems.Add(new SelectListItem
                {
                    Text = item.SemesterName,
                    Value = item.Id.ToString()
                });
            }
          

            ViewBag.SemId = listItems;
            return View();
        }

        // POST: students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( student student,HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {

                if (student.EnrollDate < DateTime.Now && student.EnrollDate > Convert.ToDateTime("2000/10/18"))
                {
                    var email = db.login.Where(m => m.Email == student.Email).ToList();
                    if (email.Count == 0)
                    {
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
                                name = db.students.OrderByDescending(m => m.Id).FirstOrDefault().Id;
                            }
                            catch
                            {
                                name = 1;
                            }
                            string firstpath1 = "/StuPhoto/";
                            string secondpath = "/StuPhoto/" + name + "/";
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
                            var path = Server.MapPath("/StuPhoto/" + name + "/" + fileName1 + ext);

                            photo.SaveAs(path);
                            student.photopath = "/StuPhoto/" + name + "/" + fileName1 + ext;


                        }
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
                                    Role = "student",
                                    RandomPass = password,
                                    LoginTime = DateTime.Now


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
                        var text = System.IO.File.ReadAllText("/Data/NewBehavior.txt");
                        List<string> lines = System.IO.File.ReadAllLines("/Data/NewBehavior.txt").ToList();
                        int index = text.IndexOf("# User actions");
                        text = text.Insert(index, id + "," + student.FirstName + Environment.NewLine);
                        System.IO.File.WriteAllText("/Data/NewBehavior.txt", text);

                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "email already exists");
                }
                else
                { 
                ModelState.AddModelError("", "Date is not valid");
            }
            }
            List<SelectListItem> listItems = new List<SelectListItem>();


            foreach (var item in db.semisters)
            {
                listItems.Add(new SelectListItem
                {
                    Text = item.SemesterName,
                    Value = item.Id.ToString()
                });
            }
           
            ViewBag.SemId = listItems;
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
            List<SelectListItem> listItems = new List<SelectListItem>();


            foreach (var item in db.semisters)
            {
                listItems.Add(new SelectListItem
                {
                    Text = item.SemesterName,
                    Value = item.Id.ToString()
                });
            }

            ViewBag.SemId = listItems;

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
        public ActionResult Edit( student student, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                if (student.EnrollDate < DateTime.Now && student.EnrollDate > Convert.ToDateTime("2000/10/18"))
                {
                    if (photo != null && photo.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(photo.FileName);
                        var fileName1 = Path.GetFileNameWithoutExtension(photo.FileName);
                        fileName1 = fileName1.Replace(" ", "_");

                        // extract only the fielname
                        var ext = Path.GetExtension(fileName.ToLower());            //extract only the extension of filename and then convert it into lower case.


                        int name = student.Id;

                        string firstpath1 = "/StuPhoto/";
                        string secondpath = "/StuPhoto/" + name + "/";
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
                        var path = Server.MapPath("/StuPhoto/" + name + "/" + fileName1 + ext);

                        photo.SaveAs(path);
                        student.photopath = "/StuPhoto/" + name + "/" + fileName1 + ext;


                    }
                    else
                    {
                        student.photopath = db.students.Where(m => m.Id == student.Id).FirstOrDefault().photopath;
                    }
                    student std = db.students.Where(m => m.Id == student.Id).FirstOrDefault();
                    std.FirstName = student.FirstName;
                    std.MiddleName = student.LastName;
                    std.LastName = student.LastName;
                    std.photopath = student.photopath;
                    std.RollNo = student.RollNo;
                    std.Gender = student.Gender;
                    std.SemisterId = student.SemisterId;
                    std.Status = true;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Date is not valid");
            }
            List<SelectListItem> listItems = new List<SelectListItem>();


            foreach (var item in db.semisters)
            {
                listItems.Add(new SelectListItem
                {
                    Text = item.SemesterName,
                    Value = item.Id.ToString()
                });
            }

            ViewBag.SemId = listItems;
            return View(student);
        }
        public ActionResult status(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = db.students.Find(id);
            if(student.Status==true)
            {
                student.Status = false;
            }
            else
            {
                student.Status = true;
            }
            if (student == null)
            {
                return HttpNotFound();
            }
            db.SaveChanges();
            return RedirectToAction("Index");
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
