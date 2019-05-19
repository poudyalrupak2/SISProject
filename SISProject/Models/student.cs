using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SISProject.Models
{
    public class student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int RollNo { get; set; }
        public Semister Semister { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string StudentRole { get; set; }
        public bool Status { get; set; }
        public string photopath { get; set; }
        [NotMapped]
        public HttpPostedFileBase photo { get; set; }
    }
}