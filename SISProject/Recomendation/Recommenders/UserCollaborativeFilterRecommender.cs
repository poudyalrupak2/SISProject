﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserBehavior.Abstractions;
using UserBehavior.Comparers;
using UserBehavior.Mathematics;
using UserBehavior.Objects;
using UserBehavior.Parsers;

namespace UserBehavior.Recommenders
{
    public class UserCollaborativeFilterRecommender : IRecommender
    {
        private IComparer comparer;
        private IRater rater;
        private UserArticleRatingsTable ratings;
       

        private int neighborCount;
        private int latentUserFeatureCount;
        public UserCollaborativeFilterRecommender(IComparer userComparer, IRater implicitRater, int numberOfNeighbors)
            : this(userComparer, implicitRater, numberOfNeighbors, 10)
        {
        }

            public UserCollaborativeFilterRecommender(IComparer userComparer, IRater implicitRater, int numberOfNeighbors, int latentFeatures)
        {
            comparer = userComparer;
            rater = implicitRater;
            neighborCount = numberOfNeighbors;
            latentUserFeatureCount = latentFeatures;
        }

        public void Train(UserBehaviorDatabase db)
        {
            UserBehaviorTransformer ubt = new UserBehaviorTransformer(db);
            ratings = ubt.GetUserArticleRatingsTable(rater);
            ratings.art = ubt.Angualr2();
            List<ArticleAndTag> articles1 = ubt.Angualr2();
            
            //if (latentUserFeatureCount > 0)
            //{
            //    SingularValueDecomposition svd = new SingularValueDecomposition(latentUserFeatureCount, 100);
            //    SvdResult results = svd.FactorizeMatrix(ratings);

            //    ratings.AppendUserFeatures(results.UserFeatures);
            //}
        }
        public List<Tag> gettags(int id)
        {
            throw new NotImplementedException();


        }

        public double GetRating(int userId, int articleId)
        {
            UserArticleRatings user = ratings.Users.FirstOrDefault(x => x.UserID == userId);
            List<UserArticleRatings> neighbors = GetNearestNeighbors(user, neighborCount);

            return GetRating(user, neighbors, articleId);
        }

        private double GetRating(UserArticleRatings user, List<UserArticleRatings> neighbors, int articleId)
        {
            int articleIndex = ratings.ArticleIndexToID.IndexOf(articleId);

            var nonZero = user.ArticleRatings.Where(x => x != 0);
            double avgUserRating = nonZero.Count() > 0 ? nonZero.Average() : 0.0;

            double score = 0.0;
            int count = 0;
            for (int u = 0; u < neighbors.Count; u++)
            {
                var nonZeroRatings = neighbors[u].ArticleRatings.Where(x => x != 0);
                double avgRating = nonZeroRatings.Count() > 0 ? nonZeroRatings.Average() : 0.0;

                if (neighbors[u].ArticleRatings[articleIndex] != 0)
                {
                    score += neighbors[u].ArticleRatings[articleIndex] - avgRating;
                    count++;
                }
            }
            if (count > 0)
            {
                score /= count;
                score += avgUserRating;
            }

            return score;
        }
        public List<SuggestedArticlePoints> suggestingArticle(List<ArticleAndTag> art, int userid)
        {
            var tags = art[userid + 1].TagCounts;
            List<string> name = new List<string>();
            List<string> name1 = new List<string>();
            List<double> tag1 = new List<double>();
            List<SuggestedArticlePoints> sun = new List<SuggestedArticlePoints>();
            double similarities = 0;
            for (int i = 0; i < tags.Count; i++)
            {
                string a = tags[i].Name;
                name.Add(a);
            }
            for (int j = 0; j <= 5; j++)
            {
                if (j == 3)
                {
                    continue;
                }
                else
                {
                    var tang = art[j].TagCounts;
                    for (int k = 0; k < tang.Count; k++)
                    {
                        var z = tang[k].Name;
                        name1.Add(z);
                    }
                    HashSet<string> arr2Set = new HashSet<string>(name);
                    double e = name.Count() + name1.Count();
                    double d = name1.Where(x => arr2Set.Contains(x)).Count();
                    similarities = d / e;
                    sun.Add(new SuggestedArticlePoints(art[j].ArticleID, art[j].ArticleName, similarities));
                }
                name1.Clear();

            }
            sun.Sort((c, n) => n.Points.CompareTo(c.Points));
            var sugg = sun.Take(2).ToList();
            return sun.Take(2).ToList();



        }



        public List<Suggestion> GetSuggestions(int userId, int numSuggestions)
        {
            
            int userIndex = ratings.UserIndexToID.IndexOf(userId);
        
            UserArticleRatings user = ratings.Users[userIndex];
            var art1 = ratings.art.OrderBy(m => m.ArticleID).ToList();
            var user1 = ratings.Users.OrderBy(m => m.UserID).ToList();
            List<Suggestion> suggestions = new List<Suggestion>();

            var neighbors = GetNearestNeighbors(user, neighborCount);

            for (int articleIndex = 0; articleIndex < ratings.ArticleIndexToID.Count; articleIndex++)
            {
                // If the user in question hasn't rated the given article yet

                if (user.ArticleRatings[articleIndex] == 0)
                {
                    double score = 0.0;
                    int count = 0;
                    for (int u = 0; u < neighbors.Count; u++)
                    {
                        if (neighbors[u].ArticleRatings[articleIndex] != 0)
                        {
                            // Calculate the weighted score for this article   
                            score += neighbors[u].ArticleRatings[articleIndex] - ((u + 1.0) / 100.0);
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        score /= count;
                    }
                    suggestions.Add(new Suggestion(userId, art1[articleIndex].ArticleID, art1[articleIndex].ArticleName, score));

                   // suggestions.Add(new Suggestion(userId, ratings.ArticleIndexToID[articleIndex],ratings.Ang1[articleIndex], score));
                }
            }

            suggestions.Sort((c, n) => n.Rating.CompareTo(c.Rating));
           
            return suggestions.Take(numSuggestions).ToList();
        }

        private List<UserArticleRatings> GetNearestNeighbors(UserArticleRatings user, int numUsers)
        {
            List<UserArticleRatings> neighbors = new List<UserArticleRatings>();

            for (int i = 0; i < ratings.Users.Count; i++)
            {
                if (ratings.Users[i].UserID == user.UserID)
                {
                    ratings.Users[i].Score = double.NegativeInfinity;
                }
                else
                {
                  // IComparer comparers=new CoRatedCosineUserComparer
                    ratings.Users[i].Score = comparer.CompareVectors(ratings.Users[i].ArticleRatings, user.ArticleRatings);
                }
            }

            var similarUsers = ratings.Users.OrderByDescending(x => x.Score);

            return similarUsers.Take(numUsers).ToList();
        }

        public void Save(string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Create))
            using (GZipStream zip = new GZipStream(fs, CompressionMode.Compress))
            using (StreamWriter w = new StreamWriter(zip))
            {
                w.WriteLine(ratings.Users.Count);
                w.WriteLine(ratings.Users[0].ArticleRatings.Length);

                foreach (UserArticleRatings t in ratings.Users)
                {
                    w.WriteLine(t.UserID);

                    foreach (double v in t.ArticleRatings)
                    {
                        w.WriteLine(v);
                    }
                }

                w.WriteLine(ratings.UserIndexToID.Count);

                foreach (int i in ratings.UserIndexToID)
                {
                    w.WriteLine(i);
                }

                w.WriteLine(ratings.ArticleIndexToID.Count);

                foreach (int i in ratings.ArticleIndexToID)
                {
                    w.WriteLine(i);
                }
            }
        }

        public void Load(string file)
        {
            ratings = new UserArticleRatingsTable();

            using (FileStream fs = new FileStream(file, FileMode.Open))
            using (GZipStream zip = new GZipStream(fs, CompressionMode.Decompress))
            using (StreamReader r = new StreamReader(zip))
            {
                long total = long.Parse(r.ReadLine());
                int features = int.Parse(r.ReadLine());
                
                for (long i = 0; i < total; i++)
                {
                    int userId = int.Parse(r.ReadLine());
                    UserArticleRatings uat = new UserArticleRatings(userId, features);

                    for (int x = 0; x < features; x++)
                    {
                        uat.ArticleRatings[x] = double.Parse(r.ReadLine());
                    }

                    ratings.Users.Add(uat);
                }

                total = int.Parse(r.ReadLine());

                for (int i = 0; i < total; i++)
                {
                    ratings.UserIndexToID.Add(int.Parse(r.ReadLine()));
                }

                total = int.Parse(r.ReadLine());

                for (int i = 0; i < total; i++)
                {
                    ratings.ArticleIndexToID.Add(int.Parse(r.ReadLine()));
                }
            }
        }
    }
}
