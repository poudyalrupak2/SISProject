using HotelManagemant.ViewModels;
using SchoolInformationSystem.Data;
using SchoolInformationSystem.ViewModels;
using SISProject.Data;
using SISProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;


namespace HotelManagemant.Controllers
{
    public class AccountController : Controller
    {
        private readonly SisDbContext context=new SisDbContext();
        private IAuthRepository auth;
        public AccountController()
        {
            this.auth = new AuthRepository(context);
        }
        public ActionResult Index()
        {
            return View();  
        }


        public ActionResult Login()
        {
            return View();
        }
        Role role = new Role();
        [HttpPost]
        public ActionResult Login(LoginViewModel l, string ReturnUrl)
        {
            
            ViewBag.ReturnUrl = ReturnUrl;


            if (auth.IsUserExists(l.Email))
                {
                    var login = auth.Login(l.Email, l.Password);

                    string pass = context.login.FirstOrDefault(a => (a.Email == l.Email)).RandomPass;
                   

                  if (login!=null)
                    {
                        var Admin = context.login.FirstOrDefault(a => (a.Email == l.Email));

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                        var objAdmin = context.login.FirstOrDefault(a => (a.Email == l.Email));
                            
                        FormsAuthentication.SetAuthCookie(l.Email, false);
                        Session.Add("id", Admin.Id);
                        Session.Add("userEmail", Admin.Email);
                        Session.Add("category", Admin.Role);

                        return Redirect(ReturnUrl);
                            
                        }
                        else
                        {
                            Session.Add("id", Admin.Id);
                            Session.Add("userEmail", Admin.Email);
                            Session.Add("category", Admin.Role);
                            var objAdmin = context.login.FirstOrDefault(a => (a.Email == l.Email));
                            FormsAuthentication.SetAuthCookie(l.Email, false);
                            string[] roles = role.GetRolesForUser(objAdmin.Email);
                            if (roles.Contains("SuperAdmin"))
                            {
                                return RedirectToAction("Index", "Dashboard");

                            }
                            if (roles.Contains("teacher"))
                            {
                            return RedirectToAction("Indexs","UplodedFiles");
                            }
                            if (roles.Contains("student"))
                            {
                                
                                return RedirectToAction("Index", "Notes");
                            }
                        }
                       
                    }
                    else if (l.Password == pass)
                    {
                        TempData["message"] = l.Email;
                        return RedirectToAction("NewPassword");
                    }


                    else
                    {
                        ModelState.AddModelError("", "Invalid Password");
                    }

                }
                ModelState.AddModelError("", "Invalid User");

                return View();
            
           
        }

        public ActionResult NewPassword()
        {

            return PartialView();

        }
        [HttpPost]
        public ActionResult NewPassword(PasswordConform pass)
        {
            if (ModelState.IsValid)
            {
                string message = TempData["message"].ToString();
                var query = (from q in context.login
                             where q.Email == message
                             select q).First();
                string password = pass.Password;
                Login login = auth.registers(query, password);
                //query.RandomPass = null;

                return RedirectToAction("Login");



            }
            return PartialView();
        }
        public ActionResult ForgotPassword()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            var Admin = context.login.FirstOrDefault(a => (a.Email == email));
            if(Admin!=null)
            {
                Random generator = new Random();
                String password = generator.Next(0, 999999).ToString("D6");
                var message = new MailMessage();
                message.To.Add(new MailAddress(Admin.Email));
                message.Subject = "Forget password";
                message.Body = "Use this Password to login:" + password;
                using (var smtp = new SmtpClient())
                {
                    try
                    {

                        smtp.Send(message);
                        Admin.RandomPass = password;
                        context.SaveChanges();
                    }
                    catch (Exception e)
                    {

                        return new HttpStatusCodeResult(HttpStatusCode.RequestTimeout);
                    }
                }

                return RedirectToAction("Login");
            }
            ModelState.AddModelError("", "this email doesnot exist");
            return PartialView();
        }
        public ActionResult Logout()
        {
                FormsAuthentication.SignOut();
          
                Session.Abandon();
                return RedirectToAction("Login");
            
            
        }

    }
}