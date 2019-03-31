using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SISProject.Models
{
    public class Files
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string FileType { get; set; }
        [NotMapped]
        public HttpPostedFileBase File { get; set; }
    }
}