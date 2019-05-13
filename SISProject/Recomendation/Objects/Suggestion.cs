namespace UserBehavior.Objects
{
    public class Suggestion
    {
        public int UserID { get; set; }

        public int ArticleID { get; set; }
        public string Articlename { get; set; }
        public double Rating { get; set; }

        public Suggestion(int userId, int articleId,string ArticleName, double assurance)
        {
            UserID = userId;
            ArticleID = articleId;
            Rating = assurance;
            Articlename = ArticleName;
        }
    }
}
