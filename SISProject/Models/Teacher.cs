using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SISProject.Models
{
    public class Teacher
    {
        public int Id { get; set; }
       
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
      
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Display(Name = "Phone No")]
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNo { get; set; }

        public string Address { get; set; }
      
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
        public DateTime? HireDate { get; set; }
        public bool status { get; set; }
        public string NotificationSeenDate { get; set; }
        public string photopath { get; set; }
        [NotMapped]
        public HttpPostedFileBase photo { get; set; }

    }
}