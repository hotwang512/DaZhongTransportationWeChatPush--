using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.ReportManagement.Controllers.RideCheckFeedbackReport
{
    public class RideCheckFeedbackReportController : BaseController
    {
        public ActionResult SelectionRatioReport()
        {
            return View();
        }
    }
}