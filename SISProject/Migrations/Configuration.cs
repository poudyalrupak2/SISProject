namespace SISProject.Migrations
{
    using SISProject.Data;
    using SISProject.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SisDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SisDbContext context)
        {
            byte[] passwordhash;
            byte[] passwordsalt;
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                string password = "nepalnepal1";
                passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                passwordsalt = hmac.Key;


            }
            context.teachers.AddOrUpdate(x => x.Id,
               new Teacher() { Id=1,Name="Rajesh",Address="ilam",Email="john123@gmail.com"},
              new Teacher() { Id = 2, Name = "Ram", Address = "Jhapa", Email = "john23@gmail.com" }





                );
            context.students.AddOrUpdate(x => x.Id,
             new student() { Id = 2, FirstName = "raju", LastName = "Standard 2", Address = "Second Standard", Email = "raj@gmail.com" },
           new student() { Id = 2, FirstName = "raju", LastName = "Standard 2", Address = "Second Standard", Email = "raj2@gmail.com" },
            new student() { Id = 3, FirstName = "ramu", LastName = "Standard 3", Address = "Third Standard", Email = "raj3@gmail.com" },
            new student() { Id = 4, FirstName = "raman", LastName = "Standard 3", Address = "Third Standard", Email = "raj4@gmail.com" },
            new student() { Id = 5, FirstName = "ramjan", LastName = "Raju", Address = "Third Standard", Email = "raj5@gmail.com" },
            new student() { Id = 6, FirstName = "sham", LastName = "Rajan", Address = "Third Standard", Email = "ram@gmail.com" },
           new student() { Id = 7, FirstName = "sayam", LastName = "Raan", Address = "Third Standard", Email = "ram1@gmail.com" },
            new student() { Id = 8, FirstName = "Ganu", LastName = "Raa", Address = "Third Standard", Email = "ram2@gmail.com" },
           new student() { Id = 9, FirstName = "ramya", LastName = "Raaj", Address = "Third Standard", Email = "ram3@gmail.com" },
            new student() { Id = 10, FirstName = "rayan", LastName = "Raanu", Address = "Third Standard", Email = "ram4@gmail.com"});
            context.login.AddOrUpdate(x => x.Id,
           new Login
           {
               Id = 1,
               Email = "poudyalrupak2@gmail.com",
               Role = "SuperAdmin",
               PasswordHash = passwordhash,
               PasswordSalt = passwordsalt
           },

            new Login
            {
                Id = 2,
                Email = "raj2@gmail.com",
                Role = "student",
                PasswordHash = passwordhash,
                PasswordSalt = passwordsalt
            },
             new Login
             {
                 Id = 3,
                 Email = "raj3@gmail.com",
                 Role = "student",
                 PasswordHash = passwordhash,
                 PasswordSalt = passwordsalt
             },
              new Login
              {
                  Id = 4,
                  Email = "raj4@gmail.com",
                  Role = "student",
                  PasswordHash = passwordhash,
                  PasswordSalt = passwordsalt
              },
               new Login
               {
                   Id = 5,
                   Email = "raj5@gmail.com",
                   Role = "student",
                   PasswordHash = passwordhash,
                   PasswordSalt = passwordsalt
               },
                new Login
                {
                    Id = 6,
                    Email = "ram@gmail.com",
                    Role = "student",
                    PasswordHash = passwordhash,
                    PasswordSalt = passwordsalt
                },
                 new Login
                 {
                     Id = 7,
                     Email = "raj1@gmail.com",
                     Role = "student",
                     PasswordHash = passwordhash,
                     PasswordSalt = passwordsalt
                 },
                  new Login
                  {
                      Id = 8,
                      Email = "ram2@gmail.com",
                      Role = "student",
                      PasswordHash = passwordhash,
                      PasswordSalt = passwordsalt
                  },
                  new Login
                  {
                      Id = 8,
                      Email = "john123@gmail.com",
                      Role = "teacher",
                      PasswordHash = passwordhash,
                      PasswordSalt = passwordsalt
                  }







            );

        }
    }
}
