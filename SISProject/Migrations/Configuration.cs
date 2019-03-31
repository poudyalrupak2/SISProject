namespace SISProject.Migrations
{
    using SISProject.Data;
    using System;
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
            context.login.AddOrUpdate(x => x.Id, new Models.Login()
            {
                Id = 1,
                Email = "poudyalrupak2@gmail.com",
                Role = "SuperAdmin",
                PasswordHash = passwordhash,
                PasswordSalt = passwordsalt


            });

        }
    }
}
