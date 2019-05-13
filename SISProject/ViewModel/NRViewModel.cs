using SISProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SISProject.ViewModel
{
    public class NRViewModel
    {
        public IEnumerable<UplodedFile>  uplodedFiles { get; set; }
        public IEnumerable<RecomendedArticles> RecomendedArticles { get; set; }
    }
}