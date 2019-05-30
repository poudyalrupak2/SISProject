using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelManagemant.Filters;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Office.Interop.Word;
using SISProject.Data;
using SISProject.Models;

namespace SISProject.Controllers
{
    [SessionCheck]
    [Authorize(Roles = "teacher,SuperAdmin")]

    public class NoticesController : Controller
    {
        private SisDbContext db = new SisDbContext();

        // GET: Notices
        public ActionResult Index()
        {
            string name = Session["userEmail"].ToString();
            return View(db.notices.Where(m=>m.CreatedBy==name).ToList());
        }

        // GET: Notices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notice notice = db.notices.Find(id);
            if (notice == null)
            {
                return HttpNotFound();
            }
            return View(notice);
        }

        // GET: Notices/Create
        public ActionResult Create()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();

            if (Session["category"].ToString() == "SuperAdmin")
            {

                foreach (var item in db.semisters)
                {
                    listItems.Add(new SelectListItem
                    {
                        Text = item.SemisterName,
                        Value = item.Id.ToString()
                    });
                }
                listItems.Add(new SelectListItem
                {
                    Text = "teacher",
                    Value = "teacher"
                });
                listItems.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "All"
                });

            }
            else
            {
                foreach (var item in db.semisters)
                {
                    listItems.Add(new SelectListItem
                    {
                        Text = item.SemisterName,
                        Value = item.Id.ToString()
                    });
                }
                listItems.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "All"
                });
            }
            ViewBag.SemId = listItems;

            return View();
        }

        public string texttopdf(string name,string rol,string nig, string pat,int ids=0)
        {


            //Create a New instance on Document Class

            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            iTextSharp.text.Font titleFont = FontFactory.GetFont("Arial", 36, iTextSharp.text.Font.UNDERLINE);
            iTextSharp.text.Font regularFont = FontFactory.GetFont("Arial", 16);
            iTextSharp.text.Paragraph title;
            iTextSharp.text.Paragraph text;
            iTextSharp.text.Paragraph text2;
            iTextSharp.text.Paragraph title2;
            int fileno;

            if (ids == 0)
            {
                int id;
                try
                {
                    id = db.notices.OrderByDescending(m => m.NoticeId).FirstOrDefault().NoticeId;
                }
                catch (Exception ex)
                {
                    id = 0;
                }
                if (id == 0)
                {
                    fileno = 1;
                }
                else
                {
                    fileno = id + 1;
                }
            }
            else
            {
                fileno = ids;
            }

            //Create a New instance of PDFWriter Class for Output File
            string firstpath1 = "/Path/";
            bool exists1 = System.IO.Directory.Exists(Server.MapPath(firstpath1));
            string firstpath2 = "/Path/"+fileno+"/";
            bool exists2 = System.IO.Directory.Exists(Server.MapPath(firstpath2));
            if (!exists1)
            {
                System.IO.Directory.CreateDirectory(Server.MapPath(firstpath1));

            }
            if (!exists2)
            {
                System.IO.Directory.CreateDirectory(Server.MapPath(firstpath2));

            }
            
            string path1 = "/Path/"+fileno+"/"+"Test.pdf";


            var path = Server.MapPath("/Path/"+ fileno+"/"+"Test.pdf");

            PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));

            //Open the Document

            doc.Open();

            //Add the content of Text File to PDF File
            title = new iTextSharp.text.Paragraph(name, titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            text = new iTextSharp.text.Paragraph(nig, regularFont);
            //title2 = new Paragraph(rol);
            // title2.Alignment = Element.ALIGN_CENTER;
            //text2 = new Paragraph("Consectetur adipiscing elit", regularFont);
            doc.Add(title);
            doc.Add(text);
            //doc.Add(text);
            //doc.Add(text2);
            //doc.Add(title2);
            if (System.IO.File.Exists(Server.MapPath(pat)))
            {
                Bitmap bitmap = new Bitmap(Server.MapPath(pat));
                iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(bitmap, System.Drawing.Imaging.ImageFormat.Jpeg);




                doc.Add(pic);
            }
        


            //Close the Document

                doc.Close();
           
            //Open the Converted PDF File

            return path1;

        }



        // POST: Notices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NoticeId,Title,ShortDescription,LongDescription,NoticeType,Image")] Notice notice,HttpPostedFileBase img)
        {
            if (ModelState.IsValid)
            {
                string path1 = "";
                if (img != null && img.ContentLength > 0)
                {
                    int id;
                    try
                    {
                        id = db.notices.OrderByDescending(m => m.NoticeId).FirstOrDefault().NoticeId;
                    }
                    catch (Exception e)
                    {
                        id = 0;
                    }
                    int fileno;
                    if (id == 0)
                    {
                        fileno = 1;
                    }
                    else
                    {
                        fileno = id + 1;
                    }

                    //Create a New instance of PDFWriter Class for Output File
                    string firstpath1 = "/Path/";
                    bool exists1 = System.IO.Directory.Exists(Server.MapPath(firstpath1));
                    string firstpath2 = "/Path/" + fileno + "/";
                    bool exists2 = System.IO.Directory.Exists(Server.MapPath(firstpath2));
                    if (!exists1)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(firstpath1));

                    }
                    if (!exists2)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(firstpath2));

                    }

                     path1 = "/Path/" + fileno + "/" + img.FileName;
                    img.SaveAs(Server.MapPath(path1));
                    notice.Image = path1;
                    
                }
                string a = texttopdf(notice.Title, notice.ShortDescription, notice.LongDescription, path1);
                notice.path = a;
                notice.Createdate = DateTime.Now;

                notice.CreatedBy = Session["userEmail"].ToString();
                db.notices.Add(notice);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(notice);
        }

        // GET: Notices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notice notice = db.notices.Where(m=>m.NoticeId==id).FirstOrDefault();
            if (notice == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> listItems = new List<SelectListItem>();

            if (Session["category"].ToString() == "SuperAdmin")
            {

                foreach (var item in db.semisters)
                {
                    listItems.Add(new SelectListItem
                    {
                        Text = item.SemisterName,
                        Value = item.Id.ToString()
                    });
                }
                listItems.Add(new SelectListItem
                {
                    Text = "teacher",
                    Value = "teacher"
                });
                listItems.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "All"
                });

            }
            else
            {
                foreach (var item in db.semisters)
                {
                    listItems.Add(new SelectListItem
                    {
                        Text = item.SemisterName,
                        Value = item.Id.ToString()
                    });
                }
                listItems.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "All"
                });
            }
            ViewBag.SemId = listItems;

            return View(notice);
        }

        // POST: Notices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NoticeId,Title,ShortDescription,LongDescription,NoticeType,Image")] Notice notice,HttpPostedFileBase img)
        {
            if (ModelState.IsValid)
            {
                var noti = db.notices.Find(notice.NoticeId);
                string path1 = "";
                if (img != null && img.ContentLength > 0)
                {
                 
                    int fileno=notice.NoticeId;
              

                    //Create a New instance of PDFWriter Class for Output File
                    string firstpath1 = "/Path/";
                    bool exists1 = System.IO.Directory.Exists(Server.MapPath(firstpath1));
                    string firstpath2 = "/Path/" + fileno + "/";
                    bool exists2 = System.IO.Directory.Exists(Server.MapPath(firstpath2));
                    if (!exists1)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(firstpath1));

                    }
                    if (!exists2)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(firstpath2));

                    }

                    path1 = "/Path/" + fileno + "/" + img.FileName;
                    img.SaveAs(Server.MapPath(path1));
                    noti.Image = path1;


                }
                else
                {
                    path1 = db.notices.Where(m => m.NoticeId == notice.NoticeId).FirstOrDefault().Image;
                }
                string a = texttopdf(notice.Title, notice.ShortDescription, notice.LongDescription, path1,notice.NoticeId);
                noti.path = a;
                noti.Title = notice.Title;
                noti.ShortDescription = notice.ShortDescription;
                noti.LongDescription = notice.LongDescription;
                noti.NoticeType = notice.NoticeType;
                db.Entry(noti).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(notice);
        }

        // GET: Notices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notice notice = db.notices.Find(id);
            if (notice == null)
            {
                return HttpNotFound();
            }
            return View(notice);
        }

        // POST: Notices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Notice notice = db.notices.Find(id);
            db.notices.Remove(notice);
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
