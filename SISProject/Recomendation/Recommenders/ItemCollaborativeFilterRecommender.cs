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
    public class ItemCollaborativeFilterRecommender : IRecommender
    {
        private IComparer comparer;
        private IRater rater;
        private UserArticleRatingsTable ratings;
        //private ArticleTag tags;
       //    private UserBehaviorTransformer transformer;//ADD recently
        private double[][] transposedRatings;

        private int neighborCount;

        public ItemCollaborativeFilterRecommender(IComparer itemComparer, IRater implicitRater, int numberOfNeighbors)
        {
            comparer = itemComparer;
            rater = implicitRater;
            neighborCount = numberOfNeighbors;
        }

        private void FillTransposedRatings()
        {
            int features = ratings.Users.Count;
            transposedRatings = new double[ratings.ArticleIndexToID.Count][];

            // Precompute a transposed ratings matrix where each row becomes an article and each column a user or tag
            for (int a = 0; a < ratings.ArticleIndexToID.Count; a++)
            {
                transposedRatings[a] = new double[features];

                for (int f = 0; f < features; f++)
                {
                    transposedRatings[a][f] = ratings.Users[f].ArticleRatings[a];
                }
            }
        }
       
        private List<int> GetHighestRatedArticlesForUser(int userIndex)
        {
            List<Tuple<int, double>> items = new List<Tuple<int, double>>();
           
           
                for (int articleIndex = 0; articleIndex < ratings.ArticleIndexToID.Count; articleIndex++)
            {
                // Create a list of every article this user has viewed
                if (ratings.Users[userIndex].ArticleRatings[articleIndex] != 0)
                {
                    items.Add(new Tuple<int, double>(articleIndex, ratings.Users[userIndex].ArticleRatings[articleIndex]));
                }
            }

            // Sort the articles by rating
            items.Sort((c, n) => n.Item2.CompareTo(c.Item2));

            return items.Select(x => x.Item1).ToList();
        }

        public void Train(UserBehaviorDatabase db)
        {
            UserBehaviorTransformer ubt = new UserBehaviorTransformer(db);
            ratings = ubt.GetUserArticleRatingsTable(rater);

            List<ArticleTagCounts> articleTags = ubt.GetArticleTagCounts();
            //train article
            List<ArticleAndTag> articles1 = ubt.Angualr2();
            ratings.AppendArticleFeatures(articleTags);
           // ratings.suggestArticle(articles1, 3);

            FillTransposedRatings();
        }
        
        public double GetRating(int userId, int articleId)
        {
            int userIndex = ratings.UserIndexToID.IndexOf(userId);
            int articleIndex = ratings.ArticleIndexToID.IndexOf(articleId);

            var userRatings = ratings.Users[userIndex].ArticleRatings.Where(x => x != 0);
            var articleRatings = ratings.Users.Select(x => x.ArticleRatings[articleIndex]).Where(x => x != 0);

            double averageUser = userRatings.Count() > 0 ? userRatings.Average() : 0;
            double averageArticle = articleRatings.Count() > 0 ? articleRatings.Average() : 0;

            // For now, just return the average rating across this user and article
            return averageArticle > 0 && averageUser > 0 ? (averageUser + averageArticle) / 2.0 : Math.Max(averageUser, averageArticle);
        }

        public List<Suggestion> GetSuggestions(int userId, int numSuggestions)
        {
            int userIndex =ratings.UserIndexToID.IndexOf(userId);
            List<int> articles = GetHighestRatedArticlesForUser(userIndex).Take(5).ToList();
            //List<ArticleTagCounts> articlecount = transformer.GetArticleTagCounts();    
            List<Suggestion> suggestions = new List<Suggestion>();

            foreach (int articleIndex in articles)
            {
                int articleId = ratings.ArticleIndexToID[articleIndex];
                List<ArticleRating> neighboringArticles = GetNearestNeighbors(articleId, neighborCount);

                foreach (ArticleRating neighbor in neighboringArticles)
                {
                    int neighborArticleIndex = ratings.ArticleIndexToID.IndexOf(neighbor.ArticleID);

                    double averageArticleRating = 0.0;
                    int count = 0;
                    for (int userRatingIndex = 0; userRatingIndex < ratings.UserIndexToID.Count; userRatingIndex++)
                    {
                        if (transposedRatings[neighborArticleIndex][userRatingIndex] != 0)
                        {
                            averageArticleRating += transposedRatings[neighborArticleIndex][userRatingIndex];
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        averageArticleRating /= count;
                    }

                    //suggestions.Add(new Suggestion(userId, neighbor.ArticleID, averageArticleRating));
                }
            }

            suggestions.Sort((c, n) => n.Rating.CompareTo(c.Rating));

            return suggestions.Take(numSuggestions).ToList();
        }

        private List<ArticleRating> GetNearestNeighbors(int articleId, int numArticles)
        {
            List<ArticleRating> neighbors = new List<ArticleRating>();
            int mainArticleIndex = ratings.ArticleIndexToID.IndexOf(articleId);
            
            for (int articleIndex = 0; articleIndex < ratings.ArticleIndexToID.Count; articleIndex++)
            {
                int searchArticleId = ratings.ArticleIndexToID[articleIndex];

                double score = comparer.CompareVectors(transposedRatings[mainArticleIndex], transposedRatings[articleIndex]);

                neighbors.Add(new ArticleRating(searchArticleId, score));
            }

            neighbors.Sort((c, n) => n.Rating.CompareTo(c.Rating));

            return neighbors.Take(numArticles).ToList();
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
            
            FillTransposedRatings();
        }

        public List<Tag> gettags(int userId)
        {
            throw new NotImplementedException();
        }

        public List<SuggestedArticlePoints> suggestingArticle(List<ArticleAndTag> art, int userid)
        {
            throw new NotImplementedException();
        }
    }
}
