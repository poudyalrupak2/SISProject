using SISProject.Data;
using SISProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SISProject.Controllers
{
    public class PDetailsController : Controller
    {
        // GET: PDetails
        private SisDbContext db = new SisDbContext();

        public ActionResult Teacher(int id)
        {
            Login teacher = db.login.Find(id);

            Teacher email = db.teachers.Where(m => m.Email == teacher.Email).FirstOrDefault();
            return View(email);
        }
        public ActionResult Student(int id)
        {
            Login teacher = db.login.Find(id);
            student email = db.students.Where(m => m.Email == teacher.Email).FirstOrDefault();
            return View(email);
        }
    }
}