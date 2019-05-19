using HotelManagemant.Filters;
using SISProject.Data;
using SISProject.Models;
using SISProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public ActionResult Index()
        {
            
            int id = Convert.ToInt32(Session["id"].ToString());
            string email = context.login.Where(m => m.Id == id).FirstOrDefault().Email;
            int realid = context.students.Where(m => m.Email == email).FirstOrDefault().Id;
            IRater rate = new LinearRater(-4, 2, 3, 1);
            IComparer compare = new CorrelationUserComparer();
            recommender = new UserCollaborativeFilterRecommender(compare, rate, 200);
            UserBehaviorDatabaseParser parser = new UserBehaviorDatabaseParser();
            UserBehaviorDatabase db1 = parser.LoadUserBehaviorDatabase("C:\\Users\\Rupak\\Desktop\\cp-user-behavior-master\\Data\\NewBehavior.txt");
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
                ViewBag.message = "\r\n" + suggestion.ArticleID + "" + "SUGGESTED ARTICLE Name=" + suggestion.Articlename;
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
          
            recomendedArticles.uplodedFiles = context.ufiles.Take(6).ToList();
            recomendedArticles.RecomendedArticles = rem;
            return View(recomendedArticles);
        }
        public ActionResult Details(int id)
        {
            
            UserBehaviorDatabaseParser parser = new UserBehaviorDatabaseParser();

            UserBehaviorDatabase db1 = parser.LoadUserBehaviorDatabase("C:\\Users\\Rupak\\Desktop\\cp-user-behavior-master\\Data\\NewBehavior.txt");

            UserBehaviorTransformer ubt = new UserBehaviorTransformer(db1);

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
            return View(sam);
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