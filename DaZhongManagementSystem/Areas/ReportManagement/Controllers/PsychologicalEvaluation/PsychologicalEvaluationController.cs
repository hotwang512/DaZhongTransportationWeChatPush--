using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.ReportManagement.Controllers.QuestionReport.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.ReportManagement.Controllers.PsychologicalEvaluation
{
    public class PsychologicalEvaluationController : BaseController
    {
        public AuthorityManageLogic _al;
        public QuestionReportLogic _sl;
        public PsychologicalEvaluationController()
        {
            _sl = new QuestionReportLogic();
            _al = new AuthorityManageLogic();
        }
        public ActionResult Index()
        {
            var roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.PsychologicalEvaluation);
            ViewBag.CurrentModulePermission = roleModuleModel;
            ViewBag.ExerciseCheckedList = _sl.GetCheckedQuestionList();
            return View();
        }

        public JsonResult GetPsychologicalEvaluationSource(string vguid, string start, string end)
        {
            var source = _sl.GetPsychologicalEvaluationSource(vguid, start, end);
            return Json(source, JsonRequestBehavior.AllowGet);
        }
        public string ExportPsychologicalEvaluationSource(string vguid, string start, string end)
        {
            return _sl.ExportPsychologicalEvaluationSource(vguid, start, end);
        }

    }
}