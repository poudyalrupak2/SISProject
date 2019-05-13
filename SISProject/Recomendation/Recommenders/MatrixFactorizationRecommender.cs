﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserBehavior.Abstractions;
using UserBehavior.Mathematics;
using UserBehavior.Objects;
using UserBehavior.Parsers;

namespace UserBehavior.Recommenders
{
    public class MatrixFactorizationRecommender : IRecommender
    {
        private UserArticleRatingsTable ratings;
        private SvdResult svd;
        private IRater rater;

        private int numUsers;
        private int numArticles;

        private int numFeatures;
        private int learningIterations;        

        public MatrixFactorizationRecommender(IRater implicitRater)
            : this(20, implicitRater)
        {
        }

        public MatrixFactorizationRecommender(int features, IRater implicitRater)
        {
            numFeatures = features;
            learningIterations = 100;
            rater = implicitRater;
        }

        public void Train(UserBehaviorDatabase db)
        {
            UserBehaviorTransformer ubt = new UserBehaviorTransformer(db);
            ratings = ubt.GetUserArticleRatingsTable(rater);

            SingularValueDecomposition factorizer = new SingularValueDecomposition(numFeatures, learningIterations);
            svd = factorizer.FactorizeMatrix(ratings);

            numUsers = ratings.UserIndexToID.Count;
            numArticles = ratings.ArticleIndexToID.Count;
        }
        
        public double GetRating(int userId, int articleId)
        {
            int userIndex = ratings.UserIndexToID.IndexOf(userId);
            int articleIndex = ratings.ArticleIndexToID.IndexOf(articleId);

            return GetRatingForIndex(userIndex, articleIndex);
        }

        private double GetRatingForIndex(int userIndex, int articleIndex)
        {
            return svd.AverageGlobalRating + svd.UserBiases[userIndex] + svd.ArticleBiases[articleIndex] + Matrix.GetDotProduct(svd.UserFeatures[userIndex], svd.ArticleFeatures[articleIndex]);
        }

        public List<Suggestion> GetSuggestions(int userId, int numSuggestions)
        {
            int userIndex = ratings.UserIndexToID.IndexOf(userId);
            UserArticleRatings user = ratings.Users[userIndex];
            List<Suggestion> suggestions = new List<Suggestion>();

            for (int articleIndex = 0; articleIndex < ratings.ArticleIndexToID.Count; articleIndex++)
            {
                // If the user in question hasn't rated the given article yet
                if (user.ArticleRatings[articleIndex] == 0)
                {
                    double rating = GetRatingForIndex(userIndex, articleIndex);

                    //suggestions.Add(new Suggestion(userId, ratings.ArticleIndexToID[articleIndex], rating));
                }
            }

            suggestions.Sort((c, n) => n.Rating.CompareTo(c.Rating));

            return suggestions.Take(numSuggestions).ToList();
        }
        
        public void Save(string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Create))
            using (GZipStream zip = new GZipStream(fs, CompressionMode.Compress))
            using (StreamWriter w = new StreamWriter(zip))
            {
                w.WriteLine(numUsers);
                w.WriteLine(numArticles);
                w.WriteLine(numFeatures);

                w.WriteLine(svd.AverageGlobalRating);

                for (int userIndex = 0; userIndex < numUsers; userIndex++)
                {
                    w.WriteLine(svd.UserBiases[userIndex]);
                }

                for (int articleIndex = 0; articleIndex < numArticles; articleIndex++)
                {
                    w.WriteLine(svd.ArticleBiases[articleIndex]);
                }

                for (int userIndex = 0; userIndex < numUsers; userIndex++)
                {
                    for (int featureIndex = 0; featureIndex < numFeatures; featureIndex++)
                    {
                        w.WriteLine(svd.UserFeatures[userIndex][featureIndex]);
                    }
                }
                
                for (int articleIndex = 0; articleIndex < numUsers; articleIndex++)
                {
                    for (int featureIndex = 0; featureIndex < numFeatures; featureIndex++)
                    {
                        w.WriteLine(svd.ArticleFeatures[articleIndex][featureIndex]);
                    }
                }

                foreach (UserArticleRatings t in ratings.Users)
                {
                    w.WriteLine(t.UserID);

                    foreach (double v in t.ArticleRatings)
                    {
                        w.WriteLine(v);
                    }
                }
                
                foreach (int i in ratings.UserIndexToID)
                {
                    w.WriteLine(i);
                }
                
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
                numUsers = int.Parse(r.ReadLine());
                numArticles = int.Parse(r.ReadLine());
                numFeatures = int.Parse(r.ReadLine());

                double averageGlobalRating = double.Parse(r.ReadLine());

                double[] userBiases = new double[numUsers];
                for (int userIndex = 0; userIndex < numUsers; userIndex++)
                {
                    userBiases[userIndex] = double.Parse(r.ReadLine());
                }

                double[] articleBiases = new double[numArticles];
                for (int articleIndex = 0; articleIndex < numArticles; articleIndex++)
                {
                    articleBiases[articleIndex] = double.Parse(r.ReadLine());
                }

                double[][] userFeatures = new double[numUsers][];
                for (int userIndex = 0; userIndex < numUsers; userIndex++)
                {
                    userFeatures[userIndex] = new double[numFeatures];

                    for (int featureIndex = 0; featureIndex < numFeatures; featureIndex++)
                    {
                        userFeatures[userIndex][featureIndex] = double.Parse(r.ReadLine());
                    }
                }

                double[][] articleFeatures = new double[numArticles][];
                for (int articleIndex = 0; articleIndex < numUsers; articleIndex++)
                {
                    articleFeatures[articleIndex] = new double[numFeatures];

                    for (int featureIndex = 0; featureIndex < numFeatures; featureIndex++)
                    {
                        articleFeatures[articleIndex][featureIndex] = double.Parse(r.ReadLine());
                    }
                }

                svd = new SvdResult(averageGlobalRating, userBiases, articleBiases, userFeatures, articleFeatures);

                for (int i = 0; i < numUsers; i++)
                {
                    int userId = int.Parse(r.ReadLine());
                    UserArticleRatings uat = new UserArticleRatings(userId, numArticles);

                    for (int x = 0; x < numArticles; x++)
                    {
                        uat.ArticleRatings[x] = double.Parse(r.ReadLine());
                    }

                    ratings.Users.Add(uat);
                }

                for (int i = 0; i < numUsers; i++)
                {
                    ratings.UserIndexToID.Add(int.Parse(r.ReadLine()));
                }
                
                for (int i = 0; i < numArticles; i++)
                {
                    ratings.ArticleIndexToID.Add(int.Parse(r.ReadLine()));
                }
            }
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
