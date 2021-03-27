using Notemarketplace_front.Database;
using Notemarketplace_front.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Notemarketplace_front.Controllers
{
    [Authorize(Roles = "Member")]
    public class ClientController : Controller
    {
        private NotesMarketPlaceEntities _Context;

        public ClientController()
        {
            _Context = new NotesMarketPlaceEntities();
        }

        private NotesMarketPlaceEntities dbobject = new NotesMarketPlaceEntities();


        // GET: Client
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Add_Notes()
        {
            NotesMarketPlaceEntities entities = new NotesMarketPlaceEntities();

            var Notecategory = entities.tblNoteCategories.ToList();
            SelectList list = new SelectList(Notecategory, "ID", "Name");
            ViewBag.NoteCategory = list;

            var Notetype = entities.tblNoteTypes.ToList();
            SelectList NoteTypelist = new SelectList(Notetype, "ID", "Name");
            ViewBag.NoteType = NoteTypelist;

            var Countryname = entities.tblCountries.ToList();
            SelectList CountryList = new SelectList(Countryname, "ID", "Name");
            ViewBag.Country = CountryList;

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_Notes(tblSellerNote model)
        {
            NotesMarketPlaceEntities entities = new NotesMarketPlaceEntities();
            var NoteCategory = entities.tblNoteCategories.ToList();
            SelectList list = new SelectList(NoteCategory, "ID", "Name");
            ViewBag.NoteCategory = list;


            var NoteType = entities.tblNoteTypes.ToList();
            SelectList NoteTypelist = new SelectList(NoteType, "ID", "Name");
            ViewBag.NoteType = NoteTypelist;


            var CountryName = entities.tblCountries.ToList();
            SelectList CountryList = new SelectList(CountryName, "ID", "Name");
            ViewBag.Country = CountryList;


            string name = User.Identity.Name;
            int access = (from user in dbobject.tblUsers where user.EmailID == name select user.ID).Single();
            string book_title = model.Title;
            string notename_fullpath = null;
            string picname_fullpath = null;
            string previewname_fullpath = null;

            tblSellerNote obj = new tblSellerNote();

            int lastid = (from row in dbobject.tblSellerNotes orderby row.ID descending select row.ID).FirstOrDefault();
            lastid += 1;
            string defaultpath = Server.MapPath(string.Format("~/Content/Files/{0}", lastid));
            if (!Directory.Exists(defaultpath))
            {
                Directory.CreateDirectory(defaultpath);
            }

            if (model.uploadNote != null)
            {
                string notename = Path.GetFileName(model.uploadNote.FileName);
                notename_fullpath = Path.Combine(defaultpath, notename);
                model.uploadNote.SaveAs(notename_fullpath);
            }
            else
            {
                ViewBag.Message = "Please upload note";
                return View();
            }

            if (model.displayPic != null)
            {
                string picname = Path.GetFileName(model.displayPic.FileName);
                picname_fullpath = Path.Combine(defaultpath, picname);
                model.displayPic.SaveAs(picname_fullpath);
                obj.DisplayPicture = picname_fullpath;
            }

            if (model.notePreview != null)
            {
                string previewname = Path.GetFileName(model.notePreview.FileName);
                previewname_fullpath = Path.Combine(defaultpath, previewname);
                model.notePreview.SaveAs(previewname_fullpath);
                obj.NotesPreview = previewname_fullpath;
            }

            obj.SellerID = access;
            obj.Title = model.Title;
            obj.Category = model.Category;
            obj.NoteType = model.NoteType;
            obj.NumberofPages = model.NumberofPages;
            obj.Description = model.Description;
            obj.UniversityName = model.UniversityName;
            obj.Country = model.Country;
            obj.Course = model.Course;
            obj.CourseCode = model.CourseCode;
            obj.Professor = model.Professor;
            obj.Status = 7;
            obj.CreatedDate = DateTime.Now;
            obj.CreatedBy = access;
            obj.IsActive = true;

            obj.IsPaid = model.IsPaid;
            if (obj.IsPaid)
            {
                obj.SellingPrice = model.SellingPrice;
            }
            else
            {
                obj.SellingPrice = 0;
            }

            dbobject.tblSellerNotes.Add(obj);
            dbobject.SaveChanges();
            //try
            //{
            //    dbobject.SaveChanges();
            //}
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }
            //}


            int book_id = (from record in dbobject.tblSellerNotes where record.SellerID == access && record.Title == book_title orderby record.ID descending select record.ID).First();

            tblSellerNotesAttachement sellerattachment = new tblSellerNotesAttachement();
            sellerattachment.NoteID = book_id;
            sellerattachment.FilePath = notename_fullpath;
            sellerattachment.FileName = book_title;
            sellerattachment.CreatedBy = access;
            sellerattachment.CreatedDate = DateTime.Now;
            sellerattachment.IsActive = true;
            dbobject.tblSellerNotesAttachements.Add(sellerattachment);
            dbobject.SaveChanges();
            ModelState.Clear();

            return View();
        }



        [AllowAnonymous]
        public ActionResult Display_book(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //tblSellerNote tblSeller = dbobj.tblSellerNotes.Find(id).;
            var user_id = dbobject.tblUsers.Where(m => m.EmailID == User.Identity.Name && m.RoleID != 103).FirstOrDefault();
            if (user_id != null)
                goto eligible;
            var tblSeller = dbobject.tblSellerNotes.Where(m => m.ID == id && m.Status == 9).FirstOrDefault();
            if (tblSeller == null)
                return HttpNotFound();
            eligible:
            List<tblSellerNote> tblSellerNotes = dbobject.tblSellerNotes.ToList();
            List<tblCountry> tblCountries = dbobject.tblCountries.ToList();
            List<tblNoteCategory> tblNoteCategories = dbobject.tblNoteCategories.ToList();

            var multiple = from c in tblSellerNotes
                           join t1 in tblCountries on c.Country equals t1.ID
                           join t2 in tblNoteCategories on c.Category equals t2.ID
                           where c.ID == id
                           select new datafile { sellerNote = c, Country = t1, NoteCategory = t2 };

            return View(multiple);

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //tblSellerNote tblSeller = dbobject.tblSellerNotes.Find(id);
            //if (tblSeller == null)
            //    return HttpNotFound();

            //List<tblSellerNote> tblSellerNotes = dbobject.tblSellerNotes.ToList();
            //List<tblCountry> tblCountries = dbobject.tblCountries.ToList();
            //List<tblNoteCategory> tblNoteCategories = dbobject.tblNoteCategories.ToList();

            //var display_book_data = from c in tblSellerNotes
            //                        join t1 in tblCountries on c.Country equals t1.ID
            //                        join t2 in tblNoteCategories on c.Category equals t2.ID
            //                        where c.ID == id
            //                        select new datafile { sellerNote = c, Country = t1, NoteCategory = t2 };

            //return View(display_book_data);
        }



        public ActionResult Free_downLoad(int? id)
        {
            var user_email = dbobject.tblUsers.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();

            var tblSeller = dbobject.tblSellerNotes.Where(m => m.ID == id).FirstOrDefault();
            var user_id = user_email.ID;

            /* if (id == null)
             {
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
             }
             else*/
            if (!tblSeller.IsPaid)
            {
                if (tblSeller == null || tblSeller.Status != 9)
                    return HttpNotFound();

                else if (tblSeller != null && tblSeller.Status == 9)
                {

                    string path = (from sa in dbobject.tblSellerNotesAttachements where sa.NoteID == tblSeller.ID select sa.FilePath).First().ToString();
                    string category = (from c in dbobject.tblNoteCategories where c.ID == tblSeller.Category select c.Name).First().ToString();
                    tblDownload obj = new tblDownload();
                    obj.NoteID = tblSeller.ID;
                    obj.Seller = tblSeller.SellerID;
                    obj.Downloader = user_id;
                    obj.IsSellerHasAllowedDownload = true;
                    obj.AttachmentPath = path;
                    obj.IsAttachmentDownloaded = true;
                    obj.IsPaid = false;
                    obj.PurchasedPrice = tblSeller.SellingPrice;
                    obj.NoteTitle = tblSeller.Title;
                    obj.NoteCategory = category;
                    obj.CreatedDate = DateTime.Now;
                    dbobject.tblDownloads.Add(obj);
                    dbobject.SaveChanges();

                    string file_name = (from sa in dbobject.tblSellerNotesAttachements where sa.NoteID == id select sa.FileName).First().ToString();
                    file_name += ".pdf";
                    byte[] fileBytes = System.IO.File.ReadAllBytes(path);

                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file_name);
                }
            }
            return HttpNotFound();
        }


        public ActionResult Paid_download(int id)
        {
            var user_email = dbobject.tblUsers.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();

            var tblSeller = dbobject.tblSellerNotes.Where(m => m.ID == id).FirstOrDefault();
            var user_id = user_email.ID;
            var checkdownload = dbobject.tblDownloads.Where(m => m.NoteID == id && m.Downloader == user_id).FirstOrDefault();

            if (checkdownload != null)
            {
                return Json(new { success = true, alertMsg = "already downloaded this note." }, JsonRequestBehavior.AllowGet);
            }

                //tblSellerNote tblSeller = dbobj.tblSellerNotes.Find(id).;
                if (tblSeller == null || tblSeller.Status != 9)
                {
                    return HttpNotFound();
                }

            else if (tblSeller != null && tblSeller.Status == 9)
            {
                var seller = dbobject.tblUsers.Where(m => m.ID == tblSeller.SellerID).FirstOrDefault();
                string path = (from sa in dbobject.tblSellerNotesAttachements where sa.NoteID == tblSeller.ID select sa.FilePath).First().ToString();
                string category = (from c in dbobject.tblNoteCategories where c.ID == tblSeller.Category select c.Name).First().ToString();
                string seller_name = seller.FirstName;
                seller_name += " " + seller.LastName;
                string buyer_name = user_email.FirstName;
                buyer_name += " " + user_email.LastName;
                string buyer_email = seller.EmailID;

                tblDownload obj = new tblDownload();
                obj.NoteID = tblSeller.ID;
                obj.Seller = tblSeller.SellerID;
                obj.Downloader = user_id;
                obj.IsSellerHasAllowedDownload = false;
                obj.AttachmentPath = path;
                obj.IsAttachmentDownloaded = false;
                obj.IsPaid = true;
                obj.PurchasedPrice = tblSeller.SellingPrice;
                obj.NoteTitle = tblSeller.Title;
                obj.NoteCategory = category;
                obj.CreatedDate = DateTime.Now;

                dbobject.tblDownloads.Add(obj);

                string subject = buyer_name + " wants to purchase your notes";
                string body = "Hello" + " " + seller_name + ",<br/><br/>We would like to inform you that, " + buyer_name + " wants to purchase your notes." +
                    " Please see Buyer Requests tab and allow download access to Buyer if you have received " +
                    "the payment from him.<br/><br/>Regards,<br/>Notes Marketplace";
                ForgotPass mailer = new ForgotPass();
                mailer.sendMail(subject, body, buyer_email);
                dbobject.SaveChanges();
                ViewBag.Msg = "Request Added";

                return Json(new { success = true, responseText = seller_name }, JsonRequestBehavior.AllowGet);
                //#codebyChandreshMendapara


            }

            return Json(new { success = false, responseText = "Error..." }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Buyer_requests(int? page)
        {
            //int pageSize = 10;
            //if (page != null)
            //    ViewBag.Count = page * pageSize - pageSize + 1;
            //else
            //    ViewBag.Count = 1;


            List<tblUser> tblUsersList = dbobject.tblUsers.ToList();
            List<tblDownload> tblDownloadList = dbobject.tblDownloads.ToList();
            List<tblUserProfile> tblUserProfilesList = dbobject.tblUserProfiles.ToList();

            int user_id = (from user in dbobject.tblUsers where user.EmailID == User.Identity.Name select user.ID).FirstOrDefault();

            var multiple = (from d in tblDownloadList
                            join t1 in tblUsersList on d.Downloader equals t1.ID
                            join t2 in tblUserProfilesList on d.Downloader equals t2.UserID
                            where d.Seller == user_id && d.IsSellerHasAllowedDownload == false
                            select new datafile { download = d, User = t1, userProfile = t2 });

            return View(multiple);
        }

        public ActionResult Buyer_Allowed(int id)
        {
            NotesMarketPlaceEntities a = new NotesMarketPlaceEntities();
            var obj = a.tblDownloads.Where(m => m.ID.Equals(id)).FirstOrDefault();
            int buyer_id = obj.Downloader;
            int seller_id = obj.Seller;
            var buyer = a.tblUsers.Where(m => m.ID.Equals(buyer_id)).FirstOrDefault();
            var seller = a.tblUsers.Where(m => m.ID.Equals(seller_id)).FirstOrDefault();
            if (obj != null)
            {
                // int id = admin_id.ID;
                obj.IsSellerHasAllowedDownload = true;

                string buyer_email = buyer.EmailID;
                string buyer_name = buyer.FirstName;
                buyer_name += " " + buyer.LastName;

                string seller_name = seller.FirstName;
                seller_name += " " + seller.LastName;
                string subject = seller_name + " Allows you to download a note";
                ForgotPass mailer = new ForgotPass();
                string body = "Hello " + buyer_name +
                    ",<br/><br/>We would like to inform you that," + seller_name + " Allows you to download a note." +
                    "Please login and see My Download tabs to download particular note." +
                    "<br/><br/>Regards,<br/>Notes Marketplace";
                mailer.sendMail(subject, body, buyer_email);

                a.SaveChanges();
            }
                return RedirectToAction("Buyer_requests", "Client");
        }


        public ActionResult My_download_notes(int ? page)
        {
            //int pageSize = 10;
            //if (page != null)
            //{
            //    ViewBag.Count = page * pageSize - pageSize + 1;
            //}
            //else
            //{
            //    ViewBag.Count = 1;
            //} 

            List<tblUser> tblUsersList = dbobject.tblUsers.ToList();
            List<tblDownload> tblDownloadList = dbobject.tblDownloads.ToList();
            List<tblUserProfile> tblUserProfilesList = dbobject.tblUserProfiles.ToList();

            int user_id = (from user in dbobject.tblUsers where user.EmailID == User.Identity.Name select user.ID).FirstOrDefault();

            var download_details = (from d in tblDownloadList
                            join t1 in tblUsersList on d.Downloader equals t1.ID
                            join t2 in tblUserProfilesList on d.Downloader equals t2.UserID
                            where d.Downloader == user_id && d.IsSellerHasAllowedDownload == true
                            select new datafile { download = d, User = t1, userProfile = t2 }).ToList();

            return View(download_details);
        }

        //buyer allow then download process...
        public ActionResult downloadBook(int id)
        {
            NotesMarketPlaceEntities a = new NotesMarketPlaceEntities();
            var user_id = dbobject.tblUsers.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();
            var download = dbobject.tblDownloads.Where(m => m.ID == id && m.Downloader.Equals(user_id.ID)).FirstOrDefault();

            if (download == null || download.IsSellerHasAllowedDownload != true)
            {
                return HttpNotFound();
            }
            else if (download != null)
            {
                string path = download.AttachmentPath;
                string filename = download.NoteTitle;
                filename += ".pdf";

                download.IsAttachmentDownloaded = true;
                download.AttachmentDownloadedDate = DateTime.Now;

                dbobject.SaveChanges();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);

            }
            return HttpNotFound();
        }



        public ActionResult My_sold_notes(int ? page)
        {
            //int pageSize = 10;
            //if (page != null)
            //{
            //    ViewBag.Count = page * pageSize - pageSize + 1;
            //}
            //else
            //{
            //    ViewBag.Count = 1;
            //}

            List<tblUser> tblUsersList = dbobject.tblUsers.ToList();
            List<tblDownload> tblDownloadList = dbobject.tblDownloads.ToList();
            List<tblUserProfile> tblUserProfilesList = dbobject.tblUserProfiles.ToList();

            int user_id = (from user in dbobject.tblUsers where user.EmailID == User.Identity.Name select user.ID).FirstOrDefault();

            var sold_notes_detail = (from d in tblDownloadList
                            join t1 in tblUsersList on d.Downloader equals t1.ID
                            join t2 in tblUserProfilesList on d.Downloader equals t2.UserID
                            where d.Seller == user_id && d.IsSellerHasAllowedDownload == true
                            select new datafile { download = d, User = t1, userProfile = t2 }).ToList();

            return View(sold_notes_detail);
        }     
   


        public ActionResult My_rejected_notes(int? page)
        {
            //int pageSize = 10;

            List<tblUser> tblUsersList = dbobject.tblUsers.ToList();
            List<tblSellerNote> tblSellerNotes = dbobject.tblSellerNotes.ToList();
            List<tblNoteCategory> tblNoteCategories = dbobject.tblNoteCategories.ToList();

            int user_id = (from user in dbobject.tblUsers where user.EmailID == User.Identity.Name select user.ID).FirstOrDefault();
            //if (page != null)
            //    ViewBag.Count = page * pageSize - pageSize + 1;
            //else
            //    ViewBag.Count = 1;
            var rejected_details = (from d in tblSellerNotes
                            join t1 in tblUsersList on d.SellerID equals t1.ID
                            join t2 in tblNoteCategories on d.Category equals t2.ID
                            where d.SellerID == user_id && d.Status == 10
                            select new datafile { sellerNote = d, User = t1, NoteCategory = t2 }).ToList();

            return View(rejected_details);
        }



        public ActionResult rejected(int id)
        {
            var user_id = dbobject.tblUsers.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();
            var download = dbobject.tblDownloads.Where(m => m.NoteID.Equals(id) && m.Seller == user_id.ID).FirstOrDefault();

            var attachments = dbobject.tblSellerNotesAttachements.Where(m => m.NoteID == id).FirstOrDefault();
            var seller = dbobject.tblSellerNotes.Where(m => m.ID == id && m.SellerID == user_id.ID && m.Status == 10).FirstOrDefault();
            if (seller != null || download != null)
            {
                string path = attachments.FilePath;
                string filename = attachments.FileName + ".pdf";
                byte[] fileBytes = System.IO.File.ReadAllBytes(path);

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
            }
            else
            {
                return HttpNotFound();
            }
        }



        public ActionResult Dashboard()
        {
            var tbldownload = dbobject.tblDownloads;
            var tblseller = dbobject.tblSellerNotes;
            var user_id = dbobject.tblUsers.Where(m => m.EmailID == User.Identity.Name).FirstOrDefault();

            ViewBag.No_soldNote = tbldownload.Where(m => m.IsSellerHasAllowedDownload == true && m.Seller == user_id.ID).Count();
            ViewBag.No_download = tbldownload.Where(m => m.IsSellerHasAllowedDownload == true && m.Downloader == user_id.ID).Count();
            ViewBag.Earnings = tbldownload.Where(m => m.IsSellerHasAllowedDownload == true && m.Seller == user_id.ID).Sum(m => m.PurchasedPrice);
            ViewBag.Rejectednote = tblseller.Count(m => m.SellerID == user_id.ID && m.Status == 10);
            ViewBag.BuyerRequests = tbldownload.Where(m => m.IsSellerHasAllowedDownload == false && m.Seller == user_id.ID).Count();
            return View();
        }



        public ActionResult My_profile()
        {
            return View();
        }
    }
}