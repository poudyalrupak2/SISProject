using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserBehavior.Objects
{
    public class SuggestedArticlePoints
    {
        public int ArticleId { get; set; }
        public string ArticleName { get; set; }
        public double Points { get; set; }
        public SuggestedArticlePoints(int id, string name,double Point)
        {
            ArticleId = id;
            ArticleName = name;
            Points = Point;
        }

    }
}
