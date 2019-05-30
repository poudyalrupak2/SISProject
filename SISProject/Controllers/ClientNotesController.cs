using HotelManagemant.Filters;
using SISProject.Data;
using SISProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SISProject.Controllers
{
    public class ClientNotesController : Controller
    {
        private SisDbContext db = new SisDbContext();
        [SessionCheck]
        [Authorize(Roles = "student")]
        public ActionResult Index()
        {
            List<Notice> notices = new List<Notice>();
            string category= Session["category"].ToString();
            if (category == "student")
            {
                string email = Session["userEmail"].ToString();
                int semister = db.students.Where(m => m.Email == email).Select(m => m.Semister.Id).FirstOrDefault();
                notices = db.notices.Where(m => m.NoticeType == "All").Union(db.notices.Where(m => m.NoticeType ==semister.ToString())).ToList();
            }
            return View(notices);

        }
        [Authorize(Roles = "teacher")]
        public ActionResult TeacherIndex()
        {
            List<Notice> notices = new List<Notice>();
            string category = Session["category"].ToString();
            if (category == "teacher")
            {
                string email = Session["userEmail"].ToString();
                notices = db.notices.Where(m => m.NoticeType == "All").Union(db.notices.Where(m => m.NoticeType =="teacher" )).ToList();
            }
            return View(notices);

        }
        public ActionResult DownLoad(string path)
        {
            string path1 =Server.MapPath(path);
            System.IO.FileInfo file = new System.IO.FileInfo(path1);
            if (file.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/octet-stream"; // download […]
            }
            byte[] filename = System.IO.File.ReadAllBytes(path1);
            return File(filename, Response.ContentType, file.Name);


        }
    }
}