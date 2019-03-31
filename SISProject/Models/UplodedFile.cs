using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SISProject.Models
{
    public class UplodedFile
    {
        public int Id { get; set; }
        public  Files Files { get; set; }
        public string FIlesType { get; set; }
        public string Tags { get; set; }
        public string UplodedBy { get; set; }
        public Semister semister { get; set; }
    }
}