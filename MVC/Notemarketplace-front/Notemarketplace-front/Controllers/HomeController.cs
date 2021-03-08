using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace Notemarketplace_front.Controllers
{
    public class HomeController : Controller
    {
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
        public ActionResult Contact()
        {
            ModelState.Clear();
            return View();
        }

        //[AllowAnonymous]
        public ActionResult Faq()
        {
            return View();
        }

        public ActionResult Search_notes()
        {
            return View();
        }

  
        public ActionResult Dashboard()
        {
            return View();
        }

      
        public ActionResult Buyer_requests()
        {
            return View();
        }

       
        public ActionResult My_download_notes()
        {
            return View();
        }

       
        public ActionResult My_sold_notes()
        {
            return View();
        }

     
        public ActionResult My_rejected_notes()
        {
            return View();
        }

       
        public ActionResult My_profile()
        {
            return View();
        }
    }
}