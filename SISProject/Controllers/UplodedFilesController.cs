using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
    public class UplodedFilesController : Controller
    {
        private SisDbContext db = new SisDbContext();

        // GET: UplodedFiles
        public ActionResult Index()
        {
            return View(db.ufiles.ToList());
        }

        // GET: UplodedFiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UplodedFile uplodedFile = db.ufiles.Find(id);
            if (uplodedFile == null)
            {
                return HttpNotFound();
            }
            return View(uplodedFile);
        }

        // GET: UplodedFiles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UplodedFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UplodedFile uplodedFile, string[] tagInput, HttpPostedFileBase Files)
        {
            if (ModelState.IsValid)
            {
                if (Files != null && Files.ContentLength > 0)
                {
                    string FinalPath = null;
                    var fileName = Path.GetFileName(Files.FileName);
                    var fileName1 = Path.GetFileNameWithoutExtension(Files.FileName);

                    // extract only the fielname
                    var ext = Path.GetExtension(fileName.ToLower());            //extract only the extension of filename and then convert it into lower case.


                    int name;
                    try
                    {
                        name = db.ufiles.OrderByDescending(m => m.Id).FirstOrDefault().Id;
                    }
                    catch
                    {
                        name = 1;
                    }
                    string firstpath1 = "/Files/";
                    string secondpath = "/files/" + name + "/";
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
                    var path = Server.MapPath("/Files/" + name + "/" + fileName1 + ext);
                    uplodedFile.FIlesType = ext;
                    for (int i = 0; i < tagInput.Count(); i++)
                    {
                        uplodedFile.Tags = tagInput[i];
                    }
                    Files.SaveAs(path);
                    uplodedFile.Filename = "/Files/" + name + "/" + fileName1 + ext;

                    if (ext == ".doc" || ext == ".docx" || ext == ".rtf")
                    {
                        string inputFilePath = ""; string outputFilePath = "";
                        string firstpath = "/word/";
                        string secondpath1 = "/word/" + name + "/";
                        bool exists = System.IO.Directory.Exists(Server.MapPath(firstpath1));
                        bool exists3 = System.IO.Directory.Exists(Server.MapPath(secondpath1));


                        if (!exists)
                        {
                            System.IO.Directory.CreateDirectory(Server.MapPath(firstpath));

                        }
                        if (!exists3)
                        {
                            System.IO.Directory.CreateDirectory(Server.MapPath(secondpath1));

                        }
                        inputFilePath = path;
                        outputFilePath = Server.MapPath("/word/" + name + "/" + fileName1 + ".pdf");
                        FinalPath = Server.MapPath("/word/" + name + "/" + fileName1 + ".pdf");
                        Upload up = new Upload();

                        bool t = up.ExportWordFileToPdf(inputFilePath, outputFilePath);
                        string file1 = "";
                        if (t)
                        {
                            uplodedFile.UpdatedFileName = "/word/" + name + "/" + fileName1 + ".pdf";
                            file1 = "/word/" + name + "/";
                        }
                        string imagename = up.ConvertSingleImage(FinalPath, file1,fileName1);
                        uplodedFile.imagepath = imagename;

                    }


                    else if (ext == ".ppt" || ext == ".pptx")
                    {
                        string inputFilePath = ""; string outputFilePath = "";
                        string firstpath = "/ppt/";

                        string secondpath1 = "/ppt/" + name + "/";
                        bool exists = System.IO.Directory.Exists(Server.MapPath(firstpath));
                        bool exists3 = System.IO.Directory.Exists(Server.MapPath(secondpath1));


                        if (!exists)
                        {
                            System.IO.Directory.CreateDirectory(Server.MapPath(firstpath));

                        }
                        if (!exists3)
                        {
                            System.IO.Directory.CreateDirectory(Server.MapPath(secondpath1));

                        }





                        inputFilePath = Server.MapPath("/ppt/" + name + "/" + fileName1 + ext);
                        outputFilePath = Server.MapPath("/ppt/" + name + "/" + fileName1 + ".pdf");
                        FinalPath = Server.MapPath("/ppt/" + name + "/" + fileName1 + ".pdf");
                        Upload up = new Upload();
                        bool t = up.ExportPptFileToPdf(path, outputFilePath);
                        string file1 = "";
                        if (t)
                        {
                            uplodedFile.UpdatedFileName = "/ppt/" + name + "/" + fileName1 + ".pdf";
                             file1 = "/ppt/" + name + "/";

                        }

                        uplodedFile.imagepath=up.ConvertSingleImage(FinalPath,file1, fileName1);

                    }

                    //If the file is a "PDF" file.
                    else if (ext == ".pdf")
                    {
                        string firstpath = "/pdf/";
                        string secondpath1 = "/pdf/" + name + "/";
                        bool exists = System.IO.Directory.Exists(Server.MapPath(firstpath));
                        bool exists3 = System.IO.Directory.Exists(Server.MapPath(secondpath1));


                        if (!exists)
                        {
                            System.IO.Directory.CreateDirectory(Server.MapPath(firstpath));

                        }
                        if (!exists3)
                        {
                            System.IO.Directory.CreateDirectory(Server.MapPath(secondpath1));

                        }
                        Files.SaveAs(Server.MapPath("/pdf/" + name + "/" + fileName1 + ext));

                        FinalPath = "/pdf/" + name + "/" + fileName1 + ext;
                        Upload up = new Upload();
                        uplodedFile.UpdatedFileName = FinalPath;
                        string file1 = "/pdf/" + name + "/";
                        uplodedFile.imagepath=up.ConvertSingleImage(Server.MapPath(FinalPath),file1, fileName1);           //Convert the pdf file into "jpg" file, the code is written in FileUploaderModel.cs

                    }
                    else
                    {
                        ModelState.AddModelError("", "file not supported");
                        return View();
                    }
                }


                db.ufiles.Add(uplodedFile);
                db.SaveChanges();
                int id = uplodedFile.Id;
                var text = System.IO.File.ReadAllText("C:\\Users\\Rupak\\Desktop\\cp-user-behavior-master\\Data\\NewBehavior.txt");
                List<string> lines = System.IO.File.ReadAllLines("C:\\Users\\Rupak\\Desktop\\cp-user-behavior-master\\Data\\NewBehavior.txt").ToList();
                int index = text.IndexOf("# Articles");
                text = text.Insert(index, "," + uplodedFile.Tags + Environment.NewLine);
                System.IO.File.WriteAllText("C:\\Users\\Rupak\\Desktop\\cp-user-behavior-master\\Data\\NewBehavior.txt", text);
                int index1 = text.IndexOf("# Users");
                text = text.Insert(index1, id + "," + uplodedFile.Name + "," + uplodedFile.Tags + Environment.NewLine);
                System.IO.File.WriteAllText("C:\\Users\\Rupak\\Desktop\\cp-user-behavior-master\\Data\\NewBehavior.txt", text);

                //DownLoad(uplodedFile.UpdatedFileName);
                return View("index", db.ufiles.ToList());

                //void DownLoad(string FName)
                //{
                //    string path = FName;
                //    System.IO.FileInfo file = new System.IO.FileInfo(path);
                //    if (file.Exists)
                //    {
                //        Response.Clear();
                //        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                //        Response.AddHeader("Content-Length", file.Length.ToString());
                //        Response.ContentType = "application/octet-stream"; // download […]
                //    }

                //}
            }
            return View();

        }

        // GET: UplodedFiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UplodedFile uplodedFile = db.ufiles.Find(id);
            if (uplodedFile == null)
            {
                return HttpNotFound();
            }
            return View(uplodedFile);
        }

        // POST: UplodedFiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,FIlesType,Tags,UplodedBy")] UplodedFile uplodedFile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uplodedFile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(uplodedFile);
        }

        // GET: UplodedFiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UplodedFile uplodedFile = db.ufiles.Find(id);
            if (uplodedFile == null)
            {
                return HttpNotFound();
            }
            return View(uplodedFile);
        }

        // POST: UplodedFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UplodedFile uplodedFile = db.ufiles.Find(id);
            db.ufiles.Remove(uplodedFile);
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
