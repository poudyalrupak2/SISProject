using HotelManagemant.ViewModels;
using SchoolInformationSystem.Data;
using SchoolInformationSystem.ViewModels;
using SISProject.Data;
using SISProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpPost]
        public ActionResult Login(LoginViewModel l, string ReturnUrl = "")
        {
            try
            {

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
                            if (objAdmin.Role == "Admin")
                            {
                                FormsAuthentication.SetAuthCookie(l.Email, false);

                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                FormsAuthentication.SetAuthCookie(l.Email, false);


                                return Redirect(ReturnUrl);
                            }
                        }
                        else
                        {
                            Session.Add("id", Admin.Id);
                            Session.Add("userEmail", Admin.Email);
                            Session.Add("category", Admin.Role);
                            var objAdmin = context.login.FirstOrDefault(a => (a.Email == l.Email));
                            FormsAuthentication.SetAuthCookie(l.Email, false);
                            return RedirectToAction("Index", "Home");



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
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.ServiceUnavailable);
            }
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
        //public ActionResult Logout()
        //{

        //    var Sesson = Session["userEmail"].ToString();
        //    string category = db.Login.Where(t => t.Email == Sesson).Select(t => t.Category).FirstOrDefault();
        //    if (category != null)
        //    {
        //        FormsAuthentication.SignOut();

        //        Session.Abandon();
        //        return RedirectToAction("Login");
        //    }
        //    else
        //    {
        //        FormsAuthentication.SignOut();

        //        Session.Abandon();
        //        return RedirectToAction("Login");
        //    }
        //}

    }
}