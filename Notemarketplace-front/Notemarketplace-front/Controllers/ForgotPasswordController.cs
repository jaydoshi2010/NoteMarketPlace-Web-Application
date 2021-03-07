using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Notemarketplace_front.Database;

namespace Notemarketplace_front.Controllers
{
    public class ForgotPasswordController : Controller
    {
        NotesMarketPlaceEntities dbobj = new NotesMarketPlaceEntities();

        private NotesMarketPlaceEntities _Context;

        public ForgotPasswordController()
        {
            _Context = new NotesMarketPlaceEntities();
        }

        // GET: ForgotPassword
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Forgot_password()
        {
            return View();
        }
    }
}