using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SISProject.Models
{
    public class Notice
    {
        public int NoticeId { get; set; }
        [Required(ErrorMessage ="Title is required")]
        public string Title { get; set; }
        [Display(Name ="Short Description")]
        [Required(ErrorMessage = "Short description is required")]
        public string ShortDescription { get; set; }
        [Display(Name = "Long Description")]
        [Required(ErrorMessage = "Long description is required")]
      
        public string LongDescription { get; set; }
        [Display(Name = "Notice Type")]
        [Required(ErrorMessage = "Notice Type is required")]
        public string NoticeType { get; set; }
        public string Image { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Createdate { get; set; }
        [NotMapped]
        public HttpPostedFileBase Images { get; set; }
        public string path { get; set; }
    }
}