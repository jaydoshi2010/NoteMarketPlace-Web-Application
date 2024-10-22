﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Notemarketplace_front.Database;

namespace Notemarketplace_front.Controllers
{
    public class SignUpController : Controller
    {
        NotesMarketPlaceEntities dbobject = new NotesMarketPlaceEntities();

        private NotesMarketPlaceEntities _Context;

        public SignUpController()
        {
            _Context = new NotesMarketPlaceEntities();
        }

        // GET: SignUp
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(tblUser model)
        {

            var connectionDB = new NotesMarketPlaceEntities();
            string email = model.EmailID;
            if (IsValidEmail(email))
            {
                if (IsValidPassword(model.Password, model.RePassword))
                {
                    var result = connectionDB.tblUsers.Where(db => db.EmailID == email).FirstOrDefault();
                    if (result == null)
                    {

                        tblUser obj = new tblUser();

                        obj.FirstName = model.FirstName;
                        obj.LastName = model.LastName;
                        obj.EmailID = model.EmailID;
                        obj.Password = model.Password;
                        obj.IsEmailVerified = false;
                        obj.IsActive = true;
                        obj.RePassword = "0";
                        obj.ModifiedBy = null;
                        obj.ModifiedDate = null;
                        obj.CreatedDate = DateTime.Now;
                        obj.CreatedBy = null;
                        obj.RoleID = 103;

                        if (ModelState.IsValid)
                        {
                            dbobject.tblUsers.Add(obj);
                            try
                            {
                                // Your code...
                                // Could also be before try if you know the exception occurs in SaveChanges

                                dbobject.SaveChanges();
                                ModelState.Clear();
                                FormsAuthentication.SetAuthCookie(model.EmailID, true);
                                return RedirectToAction("My_profile", "Client");
                            }
                            catch (DbEntityValidationException e)
                            {
                                foreach (var eve in e.EntityValidationErrors)
                                {
                                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                    foreach (var ve in eve.ValidationErrors)
                                    {
                                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                            ve.PropertyName, ve.ErrorMessage);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ViewBag.NotValidEmail = "Email is already exists";
                    }
                }
                else
                {
                    ViewBag.NotValidPassword = "Password and Re-enter password must be same";
                }
            }
            else
            {
                ViewBag.NotValidEmail = "Email is not valid";
            }
            return View("SignUp");
        }
        public static bool IsValidPassword(string pswd, string repswd)
        {
            if (pswd == repswd && pswd != "")
            {
                return true;
            }
            return false;
        }
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}