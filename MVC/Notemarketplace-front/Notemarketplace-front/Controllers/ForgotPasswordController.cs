using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Notemarketplace_front.Database;
using Notemarketplace_front.Models;

namespace Notemarketplace_front.Controllers
{
    public class ForgotPasswordController : Controller
    {
        NotesMarketPlaceEntities dbobj = new NotesMarketPlaceEntities();

       
        // GET: ForgotPassword
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Forgot_password()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Forgot_password(string email)
        {
            var isUser = dbobj.tblUsers.Where(m => m.EmailID.Equals(email)).FirstOrDefault();
            if (isUser != null)
            {

                string newpassword = RandomString(6);
                isUser.Password = newpassword;
                string subject = "New Password has been created...";
                string body = "Hello, We generated a new password for you<br/> Passowrd: " + newpassword;
                Models.ForgotPass mailer = new Models.ForgotPass();
                mailer.sendMail(subject, body, email);

                dbobj.SaveChanges();

            }
            else
            {
                ViewBag.NotValidUser = "User not found...";
            }

            return RedirectToAction("Forgot_password");
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz@$_&*Z0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}