using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SISProject.Models
{
    public class Notice
    {
        public int NoticeId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string NoticeType { get; set; }
        public string Image { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Createdate { get; set; }
        [NotMapped]
        public HttpPostedFileBase Images { get; set; }
        public string path { get; set; }
    }
}