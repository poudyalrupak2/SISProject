using SISProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SISProject.Data
{
    public class SisDbContext : DbContext
    {
        public SisDbContext() : base("defaultconnection")
        {

        }
        public DbSet<student> students { get; set; }
        public DbSet<Semister> semisters { get; set; }
        public DbSet<Teacher> teachers {get; set;}
        public DbSet<Files> files { get; set; }
        public DbSet<Admin> admins { get; set; }
        public DbSet<Login> login { get; set; }
        public DbSet<Notice> notices { get; set; }
        public DbSet<UplodedFile> ufiles { get; set; }


    }
}