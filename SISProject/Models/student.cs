using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SISProject.Models
{
    public class student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Semister Semister { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string StudentRole { get; set; }
        public bool Status { get; set; }
    }
}