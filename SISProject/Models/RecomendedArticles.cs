using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SISProject.Models
{
    public class RecomendedArticles
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FIlesType { get; set; }
        public string Tags { get; set; }
        public string UplodedBy { get; set; }
        public string UplodedDate { get; set; }
        public string UpdatedFileName { get; set; }
        public string imagepath { get; set; }
        public string Filename { get; set; }
        public double Rating { get; set; }




    }
}