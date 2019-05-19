using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SISProject.Models
{
    public class Login
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string RandomPass { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set;}
        public string Role { get; set; }
        public DateTime? LoginTime { get; set; }
        public string  LogoutTime { get; set; }
    }
}