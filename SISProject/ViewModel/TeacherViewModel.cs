using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SISProject.ViewModel
{
    public class TeacherViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "first Name is required")]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

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