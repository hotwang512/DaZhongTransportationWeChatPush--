﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.ReportManagement.Controllers.RideCheckFeedbackReport.BusinessLogic;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.ReportManagement.Controllers.RideCheckFeedbackReport
{
    public class RideCheckFeedbackReportController : BaseController
    {
        RideCheckFeedbackReportLogic _logic;

        public RideCheckFeedbackReportController()
        {
            _logic = new RideCheckFeedbackReportLogic();
        }

        public ActionResult SelectionRatioReport(string startDate = "", string endDate = "")
        {
            if (string.IsNullOrEmpty(startDate))
            {
                startDate = DateTime.Now.ToString("yyyy-MM") + "-01";
                endDate = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");
            }
            var reportData = _logic.GetSelectionRatioReportData(startDate, endDate);
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            return View(reportData);
        }


        public ActionResult PersionSelectionRatioReport(string startDate = "", string endDate = "")
        {
            if (string.IsNullOrEmpty(startDate))
            {
                startDate = DateTime.Now.ToString("yyyy-MM") + "-01";
                endDate = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");
            }
            var reportData = _logic.GetPersionSelectionRatioReportData(startDate, endDate);
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            return View(reportData);
        }
    }
}