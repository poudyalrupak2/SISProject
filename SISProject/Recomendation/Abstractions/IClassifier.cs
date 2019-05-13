﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserBehavior.Objects;
using UserBehavior.Parsers;

namespace UserBehavior.Recommenders
{
    public interface IRecommender
    {
        void Train(UserBehaviorDatabase db);
        
        List<Suggestion> GetSuggestions(int userId, int numSuggestions);
        List<Tag> gettags(int userId);
        double GetRating(int userId, int articleId);
        
        void Save(string file);
        List<SuggestedArticlePoints> suggestingArticle(List<ArticleAndTag> art, int userid);
        void Load(string file);
    }
}
