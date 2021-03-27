using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using Notemarketplace_front.Database;
using System.IO;
using System.Data.Entity.Validation;
using System.Net;
using Notemarketplace_front.Models;
using System.Net.Mail;

namespace Notemarketplace_front.Controllers
{

    //[Authorize(Roles = "Member")]
    public class HomeController : Controller
    {

        private NotesMarketPlaceEntities _Context;

        public HomeController()
        {
            _Context = new NotesMarketPlaceEntities();
        }

        private NotesMarketPlaceEntities dbobject = new NotesMarketPlaceEntities();

        //public ActionResult Add_Notes()
        //{
        //    NotesMarketPlaceEntities entities = new NotesMarketPlaceEntities();

        //    var Notecategory = entities.tblNoteCategories.ToList();
        //    SelectList list = new SelectList(Notecategory, "ID", "Name");
        //    ViewBag.NoteCategory = list;

        //    var Notetype = entities.tblNoteTypes.ToList();
        //    SelectList NoteTypelist = new SelectList(Notetype, "ID", "Name");
        //    ViewBag.NoteType = NoteTypelist;

        //    var Countryname = entities.tblCountries.ToList();
        //    SelectList CountryList = new SelectList(Countryname, "ID", "Name");
        //    ViewBag.Country = CountryList;

        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Add_Notes(tblSellerNote model)
        //{
        //    NotesMarketPlaceEntities entities = new NotesMarketPlaceEntities();
        //    var NoteCategory = entities.tblNoteCategories.ToList();
        //    SelectList list = new SelectList(NoteCategory, "ID", "Name");
        //    ViewBag.NoteCategory = list;


        //    var NoteType = entities.tblNoteTypes.ToList();
        //    SelectList NoteTypelist = new SelectList(NoteType, "ID", "Name");
        //    ViewBag.NoteType = NoteTypelist;


        //    var CountryName = entities.tblCountries.ToList();
        //    SelectList CountryList = new SelectList(CountryName, "ID", "Name");
        //    ViewBag.Country = CountryList;


        //    string name = User.Identity.Name;
        //    int access = (from user in dbobject.tblUsers where user.EmailID == name select user.ID).Single();
        //    string book_title = model.Title;
        //    string notename_fullpath = null;
        //    string picname_fullpath = null;
        //    string previewname_fullpath = null;

        //    tblSellerNote obj = new tblSellerNote();

        //    int lastid = (from row in dbobject.tblSellerNotes orderby row.ID descending select row.ID).FirstOrDefault();
        //    lastid += 1;
        //    string defaultpath = Server.MapPath(string.Format("~/Content/Files/{0}", lastid));
        //    if (!Directory.Exists(defaultpath))
        //    {
        //        Directory.CreateDirectory(defaultpath);
        //    }

        //    if (model.uploadNote != null)
        //    {
        //        string notename = Path.GetFileName(model.uploadNote.FileName);
        //        notename_fullpath = Path.Combine(defaultpath, notename);
        //        model.uploadNote.SaveAs(notename_fullpath);
        //    }
        //    else
        //    {
        //        ViewBag.Message = "Please upload note";
        //        return View();
        //    }

        //    if (model.displayPic != null)
        //    {
        //        string picname = Path.GetFileName(model.displayPic.FileName);
        //        picname_fullpath = Path.Combine(defaultpath, picname);
        //        model.displayPic.SaveAs(picname_fullpath);
        //        obj.DisplayPicture = picname_fullpath;
        //    }

        //    if (model.notePreview != null)
        //    {
        //        string previewname = Path.GetFileName(model.notePreview.FileName);
        //        previewname_fullpath = Path.Combine(defaultpath, previewname);
        //        model.notePreview.SaveAs(previewname_fullpath);
        //        obj.NotesPreview = previewname_fullpath;
        //    }

        //    obj.SellerID = access;
        //    obj.Title = model.Title;
        //    obj.Category = model.Category;
        //    obj.NoteType = model.NoteType;
        //    obj.NumberofPages = model.NumberofPages;
        //    obj.Description = model.Description;
        //    obj.UniversityName = model.UniversityName;
        //    obj.Country = model.Country;
        //    obj.Course = model.Course;
        //    obj.CourseCode = model.CourseCode;
        //    obj.Professor = model.Professor;
        //    obj.Status = 7;
        //    obj.CreatedDate = DateTime.Now;
        //    //obj.CreatedBy = access;
        //    obj.IsActive = true;

        //    obj.IsPaid = model.IsPaid;
        //    if (obj.IsPaid)
        //    {
        //        obj.SellingPrice = model.SellingPrice;
        //    }
        //    else
        //    {
        //        obj.SellingPrice = 0;
        //    }

        //    dbobject.tblSellerNotes.Add(obj);
        //    dbobject.SaveChanges();
        //    //try
        //    //{
        //    //    dbobject.SaveChanges();
        //    //}
        //    //catch (DbEntityValidationException e)
        //    //{
        //    //    foreach (var eve in e.EntityValidationErrors)
        //    //    {
        //    //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //    //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //    //        foreach (var ve in eve.ValidationErrors)
        //    //        {
        //    //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //    //                ve.PropertyName, ve.ErrorMessage);
        //    //        }
        //    //    }
        //    //}


        //    int book_id = (from record in dbobject.tblSellerNotes where record.SellerID == access && record.Title == book_title orderby record.ID descending select record.ID).First();

        //    tblSellerNotesAttachement sellerattachment = new tblSellerNotesAttachement();
        //    sellerattachment.NoteID = book_id;
        //    sellerattachment.FilePath = notename_fullpath;
        //    sellerattachment.FileName = book_title;
        //    sellerattachment.CreatedBy = access;
        //    sellerattachment.CreatedDate = DateTime.Now;
        //    sellerattachment.IsActive = true;
        //    dbobject.tblSellerNotesAttachements.Add(sellerattachment);
        //    dbobject.SaveChanges();
        //    ModelState.Clear();
            
        //    return View();
        //}


        public ActionResult Search_notes()
        {
            ViewBag.Message = "Your note page.";

            List<tblSellerNote> tblSellerNotes = _Context.tblSellerNotes.ToList();
            List<tblCountry> tblCountries = _Context.tblCountries.ToList();

            var search_details = from c in tblSellerNotes
                           join t1 in tblCountries on c.Country equals t1.ID
                           where c.Status == 9
                           select new datafile { sellerNote = c, Country = t1 };

            ViewBag.Count = (from c in tblSellerNotes
                             join t1 in tblCountries on c.Country equals t1.ID
                             where c.Status == 9
                             select c).Count();

            return View(search_details);
            //ViewBag.Message = "Your note page.";

            //List<tblSellerNote> tblSellerNotes = _Context.tblSellerNotes.ToList();
            //List<tblCountry> tblCountries = _Context.tblCountries.ToList();

            //var multiple = from c in tblSellerNotes
            //               join t1 in tblCountries on c.Country equals t1.ID
            //               select new datafile { sellerNote = c, Country = t1 };
            //return View(multiple);
        }

        // [AllowAnonymous]
        //public ActionResult Display_book(int? id)
        // {
        //     if (id == null)
        //     {
        //         return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //     }
        //     //tblSellerNote tblSeller = dbobj.tblSellerNotes.Find(id).;
        //     var user_id = dbobject.tblUsers.Where(m => m.EmailID == User.Identity.Name && m.RoleID != 103).FirstOrDefault();
        //     if (user_id != null)
        //         goto eligible;
        //     var tblSeller = dbobject.tblSellerNotes.Where(m => m.ID == id && m.Status == 9).FirstOrDefault();
        //     if (tblSeller == null)
        //         return HttpNotFound();
        //     eligible:
        //     List<tblSellerNote> tblSellerNotes = dbobject.tblSellerNotes.ToList();
        //     List<tblCountry> tblCountries = dbobject.tblCountries.ToList();
        //     List<tblNoteCategory> tblNoteCategories = dbobject.tblNoteCategories.ToList();

        //     var multiple = from c in tblSellerNotes
        //                    join t1 in tblCountries on c.Country equals t1.ID
        //                    join t2 in tblNoteCategories on c.Category equals t2.ID
        //                    where c.ID == id
        //                    select new datafile { sellerNote = c, Country = t1, NoteCategory = t2 };

        //     return View(multiple);

        //     //if (id == null)
        //     //{
        //     //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //     //}
        //     //tblSellerNote tblSeller = dbobject.tblSellerNotes.Find(id);
        //     //if (tblSeller == null)
        //     //    return HttpNotFound();

        //     //List<tblSellerNote> tblSellerNotes = dbobject.tblSellerNotes.ToList();
        //     //List<tblCountry> tblCountries = dbobject.tblCountries.ToList();
        //     //List<tblNoteCategory> tblNoteCategories = dbobject.tblNoteCategories.ToList();

        //     //var display_book_data = from c in tblSellerNotes
        //     //                        join t1 in tblCountries on c.Country equals t1.ID
        //     //                        join t2 in tblNoteCategories on c.Category equals t2.ID
        //     //                        where c.ID == id
        //     //                        select new datafile { sellerNote = c, Country = t1, NoteCategory = t2 };

        //     //return View(display_book_data);
        // }


        //public ActionResult FreeDownLoad(int? id)
        //{

        //    var user_email = dbobject.tblUsers.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();

        //    var tblSeller = dbobject.tblSellerNotes.Where(m => m.ID == id).FirstOrDefault();
        //    var user_id = user_email.ID;

        //    /* if (id == null)
        //     {
        //         return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //     }
        //     else*/
        //    if (!tblSeller.IsPaid)
        //    {
        //        if (tblSeller == null || tblSeller.Status != 9)
        //            return HttpNotFound();

        //        else if (tblSeller != null && tblSeller.Status == 9)
        //        {

        //            string path = (from sa in dbobject.tblSellerNotesAttachements where sa.NoteID == tblSeller.ID select sa.FilePath).First().ToString();
        //            string category = (from c in dbobject.tblNoteCategories where c.ID == tblSeller.Category select c.Name).First().ToString();
        //            tblDownload obj = new tblDownload();
        //            obj.NoteID = tblSeller.ID;
        //            obj.Seller = tblSeller.SellerID;
        //            obj.Downloader = user_id;
        //            obj.IsSellerHasAllowedDownload = true;
        //            obj.AttachmentPath = path;
        //            obj.IsAttachmentDownloaded = true;
        //            obj.IsPaid = false;
        //            obj.PurchasedPrice = tblSeller.SellingPrice;
        //            obj.NoteTitle = tblSeller.Title;
        //            obj.NoteCategory = category;

        //            obj.CreatedDate = DateTime.Now;
        //            dbobject.tblDownloads.Add(obj);
        //            dbobject.SaveChanges();

        //            string filename = (from sa in dbobject.tblSellerNotesAttachements where sa.NoteID == id select sa.FileName).First().ToString();
        //            filename += ".pdf";
        //            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

        //            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);

        //        }
        //    }
        //    return HttpNotFound();
        //}


        //public ActionResult AskforDownload(int id)
        //{
        //    var user_email = dbobject.tblUsers.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();

        //    var tblSeller = dbobject.tblSellerNotes.Where(m => m.ID == id).FirstOrDefault();
        //    var user_id = user_email.ID;

        //    //tblSellerNote tblSeller = dbobj.tblSellerNotes.Find(id).;
        //    if (tblSeller == null || tblSeller.Status != 9)
        //        return HttpNotFound();

        //    else if (tblSeller != null && tblSeller.Status == 9)
        //    {


        //        string path = (from sa in dbobject.tblSellerNotesAttachements where sa.NoteID == tblSeller.ID select sa.FilePath).First().ToString();
        //        string category = (from c in dbobject.tblNoteCategories where c.ID == tblSeller.Category select c.Name).First().ToString();
        //        string seller_name = (from c in dbobject.tblUsers where c.ID == tblSeller.SellerID select c.FirstName).First().ToString();
        //        string seller_lname = (from c in dbobject.tblUsers where c.ID == tblSeller.SellerID select c.LastName).First().ToString();
        //        seller_name += " " + seller_lname;
        //        tblDownload obj = new tblDownload();
        //        obj.NoteID = tblSeller.ID;
        //        obj.Seller = tblSeller.SellerID;
        //        obj.Downloader = user_id;
        //        obj.IsSellerHasAllowedDownload = false;
        //        obj.AttachmentPath = path;
        //        obj.IsAttachmentDownloaded = false;
        //        obj.IsPaid = true;
        //        obj.PurchasedPrice = tblSeller.SellingPrice;
        //        obj.NoteTitle = tblSeller.Title;
        //        obj.NoteCategory = category;
        //        obj.CreatedDate = DateTime.Now;

        //        dbobject.tblDownloads.Add(obj);
        //        dbobject.SaveChanges();
        //        ViewBag.Msg = "Request Added";

        //        return Json(new { success = true, responseText = seller_name }, JsonRequestBehavior.AllowGet);
        //    }

        //    return Json(new { success = false, responseText = "Not Completed." }, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult download(int? id)
        //{

        //    string path = (from sa in dbobject.tblSellerNotesAttachements where sa.NoteID == id select sa.FilePath).First().ToString();

        //    string filename = (from sa in dbobject.tblSellerNotesAttachements where sa.NoteID == id select sa.FileName).First().ToString();
        //    filename += ".pdf";
        //    byte[] fileBytes = System.IO.File.ReadAllBytes(path);

        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        //}

        //public ActionResult Buyer_requests()
        //{
        //    List<tblUser> tblUsersList = dbobject.tblUsers.ToList();
        //    List<tblDownload> tblDownloadList = dbobject.tblDownloads.ToList();
        //    List<tblUserProfile> tblUserProfilesList = dbobject.tblUserProfiles.ToList();

        //    int user_id = (from user in dbobject.tblUsers where user.EmailID == User.Identity.Name select user.ID).FirstOrDefault();

        //    var multiple = from d in tblDownloadList
        //                   join t1 in tblUsersList on d.Downloader equals t1.ID
        //                   join t2 in tblUserProfilesList on d.Downloader equals t2.UserID
        //                   where d.Seller == user_id && d.IsSellerHasAllowedDownload == false
        //                   select new datafile { download = d, User = t1, userProfile = t2 };

        //    return View(multiple);
        //}

        //public ActionResult BuyerAllowed(int id)
        //{
        //    NotesMarketPlaceEntities a = new NotesMarketPlaceEntities();
        //    var obj = a.tblDownloads.Where(m => m.ID.Equals(id)).FirstOrDefault();

        //    if (obj != null)
        //    {
        //        // int id = admin_id.ID;
        //        obj.IsSellerHasAllowedDownload = true;

        //        a.SaveChanges();
        //    }

        //    return RedirectToAction("Buyer_requests", "Home");
        //}


        //public ActionResult My_download_notes()
        //{
        //    List<tblUser> tblUsersList = dbobject.tblUsers.ToList();
        //    List<tblDownload> tblDownloadList = dbobject.tblDownloads.ToList();
        //    List<tblUserProfile> tblUserProfilesList = dbobject.tblUserProfiles.ToList();

        //    int user_id = (from user in dbobject.tblUsers where user.EmailID == User.Identity.Name select user.ID).FirstOrDefault();

        //    var multiple = from d in tblDownloadList
        //                   join t1 in tblUsersList on d.Downloader equals t1.ID
        //                   join t2 in tblUserProfilesList on d.Downloader equals t2.UserID
        //                   where d.Downloader == user_id && d.IsSellerHasAllowedDownload == true
        //                   select new datafile { download = d, User = t1, userProfile = t2 };

        //    return View(multiple);
        //}


        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page.";
            ModelState.Clear();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactUs model)
        {
            if (ModelState.IsValid)
            {
                using (MailMessage mm = new MailMessage("jaydoshi2010@gmail.com", model.Email))
                {
                    mm.Subject = model.Subject;
                    string body = "Hello " + model.Name + ",";
                    // body += "<br /><br />Please click the following link to activate your account";
                    //body += "<br /><a href = '" + string.Format("{0}://{1}/Home/Activation/{2}", Request.Url.Scheme, Request.Url.Authority, activationCode) + "'>Click here to activate your account.</a>";
                    //body += "<br /><br />Thanks";

                    mm.Body = model.Message;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("jaydoshi2010@gmail.com", "uxtfgkmimlolaktg");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }
            }
            ModelState.Clear();
            return View();
        }





        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

  
       
        //[AllowAnonymous]
        public ActionResult Faq()
        {
            return View();
        }




        //public ActionResult Dashboard()
        //{
        //    return View();
        //}


        //public ActionResult My_sold_notes()
        //{
        //    return View();
        //}

     
        //public ActionResult My_rejected_notes()
        //{
        //    return View();
        //}

       
        //public ActionResult My_profile()
        //{
        //    return View();
        //}
    }
}