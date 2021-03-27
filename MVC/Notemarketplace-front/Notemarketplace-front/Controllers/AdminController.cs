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

        public ActionResult Admin_dashboard()
        {
            return View();
        }

        public ActionResult Manage_Category() 
        {
            List<tblNoteCategory> tblNoteCategories = _Context.tblNoteCategories.ToList(); //new List<tblNoteCategory>();
            List<tblUser> tblUser = _Context.tblUsers.ToList(); //new List<tblNoteCategory>();

            var multipledata_notecategories = from c2 in tblNoteCategories
                                              join t2 in tblUser on c2.CreatedBy equals t2.ID
                                              select new datafile { NoteCategory = c2, User = t2 };
            return View(multipledata_notecategories);
        }

        public ActionResult Manage_Country()
        {
            List<tblCountry> tblCountries = _Context.tblCountries.ToList(); //new List<tblNoteCategory>();
            List<tblUser> tblUser = _Context.tblUsers.ToList(); //new List<tblNoteCategory>();

            var multipledata_contries = from c3 in tblCountries
                                        join t3 in tblUser on c3.CreatedBy equals t3.ID
                                        select new datafile { Country = c3, User = t3 };
            return View(multipledata_contries);
        }

        public ActionResult Manage_Type()
        {
            List<tblNoteType> tblNoteTypes = _Context.tblNoteTypes.ToList(); //new List<tblNoteCategory>();
            List<tblUser> tblUser = _Context.tblUsers.ToList(); //new List<tblNoteCategory>();

            var multipledata_notetype = from c in tblNoteTypes
                                        join t1 in tblUser on c.CreatedBy equals t1.ID
                                        select new datafile { NoteType = c, User = t1 };
            return View(multipledata_notetype);
        }



        public ActionResult Manage_Admin()
        {
            return View();
        }



        [Authorize(Roles = "Super Admin")]
        public ActionResult Admin_add_admin()
        {
            NotesMarketPlaceEntities entities = new NotesMarketPlaceEntities();
            var CountryCode = entities.tblCountries.ToList();
            SelectList list = new SelectList(CountryCode, "CountryCode", "CountryCode");
            ViewBag.CountryCode = list;
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
            int u = (from user in _Context.tblUsers where user.EmailID == name select user.ID).Single();


            if (User.Identity.IsAuthenticated)
            {
                NotesMarketPlaceEntities dbobj = new NotesMarketPlaceEntities();
                tblUser obj = new tblUser();
                obj.FirstName = model.FirstName;
                obj.LastName = model.LastName;
                obj.EmailID = model.EmailID;
                obj.Password = "Admin@12345";
                obj.CreatedDate = DateTime.Now;
                obj.CreatedBy = u;
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
                userobj.ZipCode = "364710";
                userobj.Country = "India";
                dbobj.tblUserProfiles.Add(userobj);
                dbobj.SaveChanges();
                ModelState.Clear();
                return RedirectToAction("ManageAdmin", "Admin");
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
                    ViewBag.Message = "Note type already exists";
                }
            }
            return View();
        }



        public ActionResult ContactUsinfo()
        {
            List<ContactUs> contactus = new List<ContactUs>();
            return View(contactus);
        }

        public ActionResult Admin_notes_under_review()
        {
            var SellerList = _Context.tblUsers.ToList();
            SelectList list = new SelectList(SellerList, "Id", "FirstName");
            ViewBag.SellerList = list;

            List<tblSellerNote> tblSellerNotesList = _Context.tblSellerNotes.ToList();
            List<tblUser> tblUserList = _Context.tblUsers.ToList();
            List<tblNoteCategory> tblNoteCategoriesList = _Context.tblNoteCategories.ToList();
            List<tblReferenceData> tblReferenceDataList = _Context.tblReferenceDatas.ToList();

            var multiple = from c in tblSellerNotesList
                           join t1 in tblUserList on c.SellerID equals t1.ID

                           join t2 in tblReferenceDataList on c.Status equals t2.ID
                           join t3 in tblNoteCategoriesList on c.Category equals t3.ID
                           where c.Status == 7 || c.Status == 8
                           select new datafile { sellerNote = c, User = t1, referenceData = t2, NoteCategory = t3 };

            return View(multiple);
        }

        [HttpPost]
        public ActionResult Rejected(int noteId, string rejectRemark)
        {
            NotesMarketPlaceEntities a = new NotesMarketPlaceEntities();
            var obj = a.tblSellerNotes.Where(m => m.ID.Equals(noteId)).FirstOrDefault();

            try
            {
                var admin_id = a.tblUsers.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();
                int id = admin_id.ID;
                obj.Status = 10;
                obj.AdminRemarks = rejectRemark;
                obj.ActionBy = id;
                a.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.PropertyName + ": " + x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
            return RedirectToAction("Admin_notes_under_review", "Admin");
        }

        [HttpPost]
        public ActionResult Approved(int noteId)
        {
            NotesMarketPlaceEntities a = new NotesMarketPlaceEntities();
            var obj = a.tblSellerNotes.Where(m => m.ID.Equals(noteId)).FirstOrDefault();

            try
            {
                var admin_id = a.tblUsers.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();
                int id = admin_id.ID;
                obj.Status = 9;
                obj.ActionBy = id;
                obj.PublishedDate = DateTime.Now;
                a.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.PropertyName + ": " + x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
            return RedirectToAction("Admin_notes_under_review", "Admin");
        }

        [HttpPost]
        public ActionResult InReview(int noteId)
        {
            NotesMarketPlaceEntities a = new NotesMarketPlaceEntities();
            var obj = a.tblSellerNotes.Where(m => m.ID.Equals(noteId)).FirstOrDefault();

            try
            {
                var admin_id = a.tblUsers.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();
                int id = admin_id.ID;
                obj.Status = 8;
                obj.ActionBy = id;
                a.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.PropertyName + ": " + x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
            return RedirectToAction("Admin_notes_under_review", "Admin");
        }

        public ActionResult AdminDownload(int id)
        {

            var tblSeller = _Context.tblSellerNotes.Where(m => m.ID == id).FirstOrDefault();

            var user_id = _Context.tblUsers.Where(m => m.EmailID == User.Identity.Name && m.RoleID != 103).FirstOrDefault();
            if (user_id != null)
            {
                string path = (from sa in _Context.tblSellerNotesAttachements where sa.NoteID == tblSeller.ID select sa.FilePath).First().ToString();


                string filename = (from sa in _Context.tblSellerNotesAttachements where sa.NoteID == id select sa.FileName).First().ToString();
                filename += ".pdf";
                byte[] fileBytes = System.IO.File.ReadAllBytes(path);

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
            }
            return HttpNotFound();
        }
        

        public ActionResult Admin_member_details(int ? page)
        {
            //int pageSize = 5;
            //if (page != null)
            //    ViewBag.Count = page * pageSize - pageSize + 1;
            //else
            //    ViewBag.Count = 1;

            var SellerList = _Context.tblUsers.ToList();
            SelectList list = new SelectList(SellerList, "Id", "FirstName");
            ViewBag.SellerList = list;


            List<tblSellerNote> tblSellerNotesList = _Context.tblSellerNotes.ToList();
            List<tblUser> tblUserList = _Context.tblUsers.ToList();
            List<tblNoteCategory> tblNoteCategoriesList = _Context.tblNoteCategories.ToList();
            List<tblReferenceData> tblReferenceDataList = _Context.tblReferenceDatas.ToList();

            var member_details = (from c in tblSellerNotesList
                                  join t1 in tblUserList on c.SellerID equals t1.ID
                                  join t3 in tblNoteCategoriesList on c.Category equals t3.ID
                                  where c.Status == 7 || c.Status == 8
                                  select new datafile { sellerNote = c, User = t1, NoteCategory = t3 });

            return View(member_details);
        }


        public ActionResult Admin_system_config()
        {
            return View();
        }


        public ActionResult Admin_profile()
        {
            return View();
        }
    }
}