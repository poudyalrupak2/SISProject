﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SISProject.Models
{
    public class Semister
    {
        public int Id { get; set; }
        [Display(Name = "Semester Name")]
        [Required(ErrorMessage = "Semester Name is required")]
        public string SemesterName { get; set; }
    }
}