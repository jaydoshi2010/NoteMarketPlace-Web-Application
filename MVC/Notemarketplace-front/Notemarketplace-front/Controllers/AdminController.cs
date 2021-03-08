using Notemarketplace_front.Database;
using Notemarketplace_front.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Notemarketplace_front.Controllers
{
    [Authorize(Roles = "Admin, Super Admin")]
    public class AdminController : Controller
    {

        // GET: Admin
        //public ActionResult Index()
        //{
        //    return View();
        //}

        private NotesMarketPlaceEntities _Context;
        public AdminController()
        {
            _Context = new NotesMarketPlaceEntities();
        }

        public ActionResult Admin_dashboard(tblNoteType model)
        {
            List<tblNoteType> tblNoteTypes = _Context.tblNoteTypes.ToList(); //new List<tblNoteCategory>();

            List<tblUser> tblUser = _Context.tblUsers.ToList(); //new List<tblNoteCategory>();

            var multipledata_notetype = from c in tblNoteTypes
                           join t1 in tblUser on c.CreatedBy equals t1.ID
                           select new datafile { NoteType = c, User = t1 };
            return View(multipledata_notetype);

            List<tblNoteCategory> tblNoteCategories = _Context.tblNoteCategories.ToList(); //new List<tblNoteCategory>();
            //List<tblUser> tblUser = _Context.tblUsers.ToList(); //new List<tblNoteCategory>();

            var multipledata_notecategories = from c in tblNoteCategories
                                              join t1 in tblUser on c.CreatedBy equals t1.ID
                                              select new datafile { NoteCategory = c, User = t1 };
            return View(multipledata_notecategories);

            List<tblCountry> tblCountries = _Context.tblCountries.ToList(); //new List<tblNoteCategory>();
            //List<tblUser> tblUser = _Context.tblUsers.ToList(); //new List<tblNoteCategory>();

            var multipledata_contries = from c in tblCountries
                                        join t1 in tblUser on c.CreatedBy equals t1.ID
                                        select new datafile { Country = c, User = t1 };
            return View(multipledata_contries);
        }

      


        public ActionResult Admin_system_config()
        {
            return View();
        }

        public ActionResult Admin_member_details()
        {
            return View();
        }

        public ActionResult Admin_profile()
        {
            return View();
        }



        [Authorize(Roles = "Super Admin")]
        public ActionResult Admin_add_admin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin")]
        public ActionResult Admin_add_admin(UserInfo model)
        {
            NotesMarketPlaceEntities entities = new NotesMarketPlaceEntities();
            var CountryCode = entities.tblCountries.ToList();
            SelectList list = new SelectList(CountryCode, "CountryCode", "CountryCode");
            ViewBag.CountryCode = list;
            string name = User.Identity.Name;
            int access = (from user in _Context.tblUsers where user.EmailID == name select user.ID).Single();

            if (User.Identity.IsAuthenticated)
            {
                NotesMarketPlaceEntities dbobj = new NotesMarketPlaceEntities();
                tblUser obj = new tblUser();
                obj.FirstName = model.FirstName;
                obj.LastName = model.LastName;
                obj.EmailID = model.EmailID;
                obj.Password = "Admin@123";
                obj.CreatedDate = DateTime.Now;
                obj.CreatedBy = access;
                obj.IsActive = true;
                obj.IsEmailVerified = true;
                obj.RoleID = 102;
                dbobj.tblUsers.Add(obj);
                dbobj.SaveChanges();

                int id = (from record in dbobj.tblUsers orderby record.ID descending select record.ID).First();

                tblUserProfile userobj = new tblUserProfile();
                userobj.UserID = id;
                userobj.PhoneNumber_CountryCode = model.CountryCode;
                userobj.PhoneNumber = model.PhnNo;
                userobj.AddressLine1 = "addressline1";
                userobj.AddressLine2 = "addressline2";
                userobj.City = "city";
                userobj.State = "State";
                userobj.ZipCode = "123321";
                userobj.Country = "India";
                dbobj.tblUserProfiles.Add(userobj);
                dbobj.SaveChanges();
                ModelState.Clear();
                return RedirectToAction("Admin_dashboard", "Admin");
            }
            return View();
        }



        public ActionResult Admin_add_category()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Admin_add_category(tblNoteCategory model)
        {
            if (User.Identity.IsAuthenticated)
            {
                string name = User.Identity.Name;
                int access = (from user in _Context.tblUsers where user.EmailID == name select user.ID).Single();
                bool isvalid = _Context.tblNoteCategories.Any(m => m.Name == model.Name);

                if (!isvalid)
                {
                    tblNoteCategory obj = new tblNoteCategory();
                    obj.Name = model.Name;
                    obj.Description = model.Description;
                    obj.CreatedDate = DateTime.Now;
                    obj.CreatedBy = access;
                    obj.IsActive = true;

                    if (ModelState.IsValid)
                    {
                        _Context.tblNoteCategories.Add(obj);
                        try
                        {
                            _Context.SaveChanges();
                            ModelState.Clear();
                            return RedirectToAction("Admin_dashboard", "Admin");
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
                    ViewBag.Message = "Note Category already exists";
                }
            }
            return View();
        }



        public ActionResult Admin_add_country()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Admin_add_country(tblCountry model)
        {
            if (User.Identity.IsAuthenticated)
            {
                string name = User.Identity.Name;
                int access = (from user in _Context.tblUsers where user.EmailID == name select user.ID).Single();
                bool isvalid = _Context.tblCountries.Any(m => m.CountryCode == model.CountryCode);

                if (!isvalid)
                {
                    tblCountry obj = new tblCountry();
                    obj.CountryCode = model.CountryCode;
                    obj.Name = model.Name;
                    obj.CreatedDate = DateTime.Now;
                    obj.CreatedBy = access;
                    obj.IsActive = true;
                    if (ModelState.IsValid)
                    {
                        _Context.tblCountries.Add(obj);
                        try
                        {
                            _Context.SaveChanges();
                            ModelState.Clear();
                            return RedirectToAction("Admin_dashboard", "Admin");
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
                    ViewBag.Message = "Country is already exists in list";
                }
            }
            return View();
        }



        public ActionResult Admin_add_type()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Admin_add_type(tblNoteType model)
        {
            if (User.Identity.IsAuthenticated)
            {
                string name = User.Identity.Name;
                int access = (from user in _Context.tblUsers where user.EmailID == name select user.ID).Single();

                bool isvalid = _Context.tblNoteTypes.Any(m => m.Name == model.Name);

                if (!isvalid)
                {
                    tblNoteType obj = new tblNoteType();
                    obj.Name = model.Name;
                    obj.Description = model.Description;
                    obj.CreatedDate = DateTime.Now;
                    obj.CreatedBy = access;
                    obj.IsActive = true;

                    if (ModelState.IsValid)
                    {
                        _Context.tblNoteTypes.Add(obj);
                        try
                        {
                            _Context.SaveChanges();
                            ModelState.Clear();
                            return RedirectToAction("Admin_dashboard", "Admin");
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
                    ViewBag.Message = "Note Type already exists";
                }
            }
            return View();
        }

        public ActionResult ContactUsinfo()
        {
            List<ContactUs> contactus = new List<ContactUs>();
            return View(contactus);
        }
    }
}