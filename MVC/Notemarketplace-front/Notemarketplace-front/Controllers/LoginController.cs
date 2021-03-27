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
        public ActionResult Login(Login model, string returnUrl)
        {
            bool isvalid = _Context.tblUsers.Any(db => db.EmailID == model.Email && db.Password == model.Password);

            if (isvalid)
            {
                var result = _Context.tblUsers.Where(db => db.EmailID == model.Email).FirstOrDefault();
                if (result.RoleID == 101 || result.RoleID == 102)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);

                    returnUrl = Request.QueryString["ReturnUrl"];
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        Response.Redirect(returnUrl);
                    }

                    return RedirectToAction("Admin_dashboard", "Admin");
                }

                else if (result.RoleID == 103)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        Response.Redirect(returnUrl);
                    }
                    return RedirectToAction("Dashboard", "Client");
                }
                else
                {
                    ViewBag.NotValidUser = "Something went wrong";
                }
            }
            else
            {
                ViewBag.NotValidUser = "Incorrect Email or Password";
            }
            return View("Login");
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("", "Login");
        }

    }
}