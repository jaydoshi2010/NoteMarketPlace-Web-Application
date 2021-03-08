using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Notemarketplace_front.Database;
using Notemarketplace_front.Models;

namespace Notemarketplace_front.Controllers
{
    public class LoginController : Controller
    {
        NotesMarketPlaceEntities dbobject = new NotesMarketPlaceEntities();

        private NotesMarketPlaceEntities _Context;

        public LoginController()
        {
            _Context = new NotesMarketPlaceEntities();
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login model)
        {
            bool isvalid = _Context.tblUsers.Any(db => db.EmailID == model.Email && db.Password == model.Password);

            if (isvalid)
            {
                var result = _Context.tblUsers.Where(db => db.EmailID == model.Email).FirstOrDefault();
                if (result.RoleID == 101 || result.RoleID == 102)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    return RedirectToAction("Admin_dashboard", "Admin");
                }

                else if (result.RoleID == 103)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    ViewBag.NotValidUser = "Something went wrong";
                }
            }
            else
            {
                ViewBag.NotValidUser = "Incorrect Email or Password";

                /*if (model.Password == result.Password)
                {

                  /*  if (User.Identity.IsAuthenticated)
                    {
                        string name = User.Identity.Name;
                    }
                    if()
                    FormsAuthentication.SetAuthCookie(result.EmailID,true);
                    return RedirectToAction("", "user");
                }
                else
                {
                    ViewBag.NotValidPassword = "Passowrd is Incorrect";
                }*/
            }
            return View("Login");
        }

    }
}