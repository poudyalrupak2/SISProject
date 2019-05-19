using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SISProject.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public bool status { get; set; }
        public string photopath { get; set; }
        [NotMapped]
        public HttpPostedFileBase photo { get; set; }

    }
}