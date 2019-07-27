using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SISProject.Models
{
    public class student
    {
        public int Id { get; set; }
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
     
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [Display(Name = "Roll No")]
        [Required(ErrorMessage = "Roll Number is required")]
        public int RollNo { get; set; }
        public Semister Semister { get; set; }
        public int SemisterId { get; set; }
        public string Faculty { get; set; }
     
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        public string Address { get; set; }
       
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
        public DateTime? EnrollDate { get; set; }
        public string StudentRole { get; set; }
        public string NotificationSeenDate { get; set; }
        public bool Status { get; set; }
        public string photopath { get; set; }
        [NotMapped]
        public HttpPostedFileBase photo { get; set; }
    }
}