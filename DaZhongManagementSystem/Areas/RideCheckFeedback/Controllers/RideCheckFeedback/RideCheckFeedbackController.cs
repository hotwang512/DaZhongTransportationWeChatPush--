using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.RideCheckFeedback
{
    public class RideCheckFeedbackController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getRideCheckFeedback(string vguid)
        {

            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}