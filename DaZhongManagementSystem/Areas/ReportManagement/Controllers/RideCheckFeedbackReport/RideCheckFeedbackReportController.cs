﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.ReportManagement.Controllers.RideCheckFeedbackReport.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.ReportManagement.Controllers.RideCheckFeedbackReport
{
    public class RideCheckFeedbackReportController : BaseController
    {
        AuthorityManageLogic _al;
        RideCheckFeedbackReportLogic _logic;

        public RideCheckFeedbackReportController()
        {
            _al = new AuthorityManageLogic();
            _logic = new RideCheckFeedbackReportLogic();
        }

        public ActionResult SelectionRatioReport(string startDate = "", string endDate = "")
        {
            var roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.SelectionRatioReport);
            ViewBag.CurrentModulePermission = roleModuleModel;
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



        public ActionResult PersionSelectionRatioReport()
        {
            var roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.PersionSelectionRatioReport);
            ViewBag.CurrentModulePermission = roleModuleModel;
            string startDate = "";
            string endDate = "";
            if (string.IsNullOrEmpty(startDate))
            {
                startDate = DateTime.Now.ToString("yyyy-MM") + "-01";
                endDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            return View();
        }
        public void ExportSelectionRatioReport(string startDate = "", string endDate = "")
        {

            _logic.ExportSelectionRatioReport(startDate, endDate);
        }

        public void ExportPersionSelectionRatioReport(string startDate = "", string endDate = "")
        {

            _logic.ExportPersionSelectionRatioReport(startDate, endDate);
        }

        public JsonResult getPersionSelectionRatioReport(string startDate = "", string endDate = "")
        {
            if (string.IsNullOrEmpty(startDate))
            {
                startDate = DateTime.Now.ToString("yyyy-MM") + "-01";
                endDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            var reportData = _logic.GetPersionSelectionRatioReportData(startDate, endDate);
            return Json(reportData, JsonRequestBehavior.AllowGet);
        }
    }
}