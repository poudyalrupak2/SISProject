using HotelManagemant.Filters;
using SISProject.Data;
using SISProject.Models;
using SISProject.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using UserBehavior.Abstractions;
using UserBehavior.Comparers;
using UserBehavior.Objects;
using UserBehavior.Parsers;
using UserBehavior.Raters;
using UserBehavior.Recommenders;

namespace SISProject.Controllers
{
    [SessionCheck]
    [Authorize(Roles = "Student")]

    public class NotesController : Controller
    {
        private IRecommender recommender;

        SisDbContext context = new SisDbContext();
        public ActionResult Index(string search="")
        {
            if (search == "")
            {
                int id = Convert.ToInt32(Session["id"].ToString());
                string email = context.login.Where(m => m.Id == id).FirstOrDefault().Email;
                int realid = context.students.Where(m => m.Email == email).FirstOrDefault().Id;
                IRater rate = new LinearRater(-4, 2, 1, 2);
                IComparer compare = new CorrelationUserComparer();
                recommender = new UserCollaborativeFilterRecommender(compare, rate, 200);
                UserBehaviorDatabaseParser parser = new UserBehaviorDatabaseParser();
                UserBehaviorDatabase db1 = parser.LoadUserBehaviorDatabase("/Data/NewBehavior.txt");
                UserBehaviorTransformer ubt = new UserBehaviorTransformer(db1);
                recommender.Train(db1);



                int userId;
                int ratings;
                userId = realid;
                ratings = 2;
                List<Suggestion> result = new List<Suggestion>();
                List<RecomendedArticles> rem = new List<RecomendedArticles>();
                RecomendedArticles recom;
                if (ratings >= 1 && ratings <= 100)
                {
                    new GetRecommendation { UserID = userId, Ratings = ratings };
                    result = recommender.GetSuggestions(userId, ratings);
                }

                foreach (Suggestion suggestion in result)
                {
                    var ye = context.ufiles.Where(m => m.Id == suggestion.ArticleID).FirstOrDefault();
                    recom = new RecomendedArticles()
                    {
                        Name = ye.Name,
                        UpdatedFileName = ye.UpdatedFileName,
                        UplodedBy = ye.UplodedBy,
                        Description = ye.Description,
                        Filename = ye.Filename,
                        imagepath = ye.imagepath,
                        UplodedDate = ye.UplodedDate,
                        Id = ye.Id,
                    };
                    rem.Add(recom);

                }
                NRViewModel recomendedArticles = new NRViewModel();

                recomendedArticles.uplodedFiles = context.ufiles.OrderByDescending(m => m.Id).Take(6).ToList();
                recomendedArticles.RecomendedArticles = rem;
                return View(recomendedArticles);
            }
            else
            {
                NRViewModel recomendedArticles = new NRViewModel();

                recomendedArticles.uplodedFiles = context.ufiles.OrderByDescending(m => m.Id).Where(m=>m.Name.Contains(search)).Take(6).ToList();
                if(recomendedArticles.uplodedFiles==null)
                {
                    ViewBag.messagea = "no item found";
                }
                ViewBag.message = "search";
                return View(recomendedArticles);


            }
        }
        public ActionResult Details(int id)
        {
            
            UserBehaviorDatabaseParser parser = new UserBehaviorDatabaseParser();

            UserBehaviorDatabase db1 = parser.LoadUserBehaviorDatabase("/Data/NewBehavior.txt");

            UserBehaviorTransformer ubt = new UserBehaviorTransformer(db1);
            int userid = Convert.ToInt32(Session["id"].ToString());
            string email = context.login.Where(m => m.Id == userid).FirstOrDefault().Email;
            int realid = context.students.Where(m => m.Email == email).FirstOrDefault().Id;

            string name = context.students.Where(m => m.Id == realid).FirstOrDefault().FirstName;
            UplodedFile admin = context.ufiles.Find(id);
            SimilarViewModel sam = new SimilarViewModel();
            sam.Description = admin.Description;
            sam.Id = admin.Id;
            sam.Name = admin.Name;
            sam.UpdatedFileName = admin.UpdatedFileName;
            sam.UplodedBy = admin.UplodedBy;
            sam.UplodedDate = admin.UplodedDate;
            UserArticleRatingsTable ratings1;
                   IRater rate = new LinearRater(-4, 2, 3, 1);

            ratings1 = ubt.GetUserArticleRatingsTable(rate);
            List<SuggestedArticlePoints> SAT = ratings1.suggestArticle(ratings1.art, id);
            List<UplodedFile> up = new List<UplodedFile>();
            UplodedFile upa=new UplodedFile();
            foreach (var item in SAT)
            {
                if (item.ArticleId != 1)
                {
                    upa = context.ufiles.Where(m => m.Id == item.ArticleId).FirstOrDefault();
                    up.Add(upa);
                }
            }
            sam.uplodedFiles = up;
            var text = System.IO.File.ReadAllText("/Data/NewBehavior.txt");
            List<string> lines = System.IO.File.ReadAllLines("/Data/NewBehavior.txt").ToList();
            int index = text.IndexOf("# End");
            text = text.Insert(index,"1,View,"+realid+","+name+","+admin.Id+","+admin.Name+ Environment.NewLine);
            System.IO.File.WriteAllText("/Data/NewBehavior.txt", text);
            



            return View(sam);
        }
        public ActionResult Download(string path,int id)
        {

            int userid = Convert.ToInt32(Session["id"].ToString());
            string email = context.login.Where(m => m.Id == userid).FirstOrDefault().Email;
            int realid = context.students.Where(m => m.Email == email).FirstOrDefault().Id;

            string name = context.students.Where(m => m.Id == realid).FirstOrDefault().FirstName;

            UplodedFile admin = context.ufiles.Find(id);
            string path1 = Server.MapPath(path);
            byte[] filedata = System.IO.File.ReadAllBytes(path1);
            System.IO.FileInfo file = new System.IO.FileInfo(path1);
            string contentType = MimeMapping.GetMimeMapping(path1);

            if (file.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachment; filename="+file.Name );

                if (file.Extension == ".pdf")
                {
                    Response.ContentType = "application/pdf"; // download […]
                }
                else if(file.Extension==".doc"||file.Extension== ".docx")
                {
                    Response.ContentType= "Application/msword";
                }
                //else if(file.Extension==".pptx")
                //{
                //    Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                //}
             
                else
                {
                    Response.ContentType = "application/vnd.ms-powerpoint";
                }
            }
            byte[] filename = System.IO.File.ReadAllBytes(path1);
            var text = System.IO.File.ReadAllText("/Data/NewBehavior.txt");
            List<string> lines = System.IO.File.ReadAllLines("/Data/NewBehavior.txt").ToList();
            int index = text.IndexOf("# End");
            text = text.Insert(index, "1,Download," + realid + "," + name + "," + admin.Id + "," + admin.Name + Environment.NewLine);
            System.IO.File.WriteAllText("/Data/NewBehavior.txt", text);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = file.Name,
                Inline = true,
            };

           // return File(filename, cd.ToString());

           return File(filename, Response.ContentType, file.Name);

        }
    }

    







    class GetRating
    {
        public int UserID { get; set; }
        public int ArticleID { get; set; }
    }

    class GetRecommendation
    {
        public int UserID { get; set; }
        public int Ratings { get; set; }
    }
}