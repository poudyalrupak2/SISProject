using SISProject.Data;
using SISProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SISProject.Controllers
{
    public class DashboardController : Controller
    {
        private SisDbContext db = new SisDbContext();

        // GET: Dashboard
        public ActionResult Index()
        {
            Counts c = new Counts();
            c.Student = db.students.Where(m=>m.Status==true).Count();
            c.Teacher = db.teachers.Where(m => m.status == true).Count();
            c.DisabledStudent = db.students.Where(m => m.Status == false).Count();
            return View(c);
        }

          }
}
