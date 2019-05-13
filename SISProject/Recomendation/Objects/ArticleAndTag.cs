using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserBehavior.Objects
{
    public class ArticleAndTag
    {
        public int ArticleID { get; set; }

        public string ArticleName { get; set; }

        public List<Tag> TagCounts { get; set; }

        public ArticleAndTag(int articleId,string Name, List<Tag> tag)
        {
            ArticleID = articleId;
            ArticleName = Name;
            TagCounts = tag;
        }
    
    }
}
