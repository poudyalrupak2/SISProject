using SISProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SISProject.ViewModel
{
    public class SimilarViewModel
    {
        public IEnumerable<UplodedFile> uplodedFiles { get; set; }
         public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
       public string path { get; set; }
      public List<double> point { get; set; }
        public string UplodedBy { get; set; }
        public string UplodedDate { get; set; }
        public string UpdatedFileName { get; set; }
      
    }
}