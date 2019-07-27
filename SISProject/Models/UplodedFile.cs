using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SISProject.Models
{
    public class UplodedFile
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string FIlesType { get; set; }
        [NotMapped]
        public string[] tag { get; set; }
        public string Tags { get; set; }
        public string UplodedBy { get; set; }
        public string UplodedDate { get; set; }
        public string UpdatedFileName { get; set; }
        public string imagepath { get; set; }
        public string Filename { get; set; }
        [NotMapped]
        public HttpPostedFileBase File { get; set; }
    }
}