using DaZhongManagementSystem.Areas.ReportManagement.Controllers.ScoreReport.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using System.Collections.Generic;
using System.Web.Mvc;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.ReportManagement.Controllers.ScoreReport
{
    public class ScoreReportController : BaseController
    {
        //
        // GET: /ReportManagement/ScoreReport/
        public AuthorityManageLogic _al;
        public ScoreReportLogic _sl;
        public ScoreReportController()
        {
            _sl = new ScoreReportLogic();
            _al = new AuthorityManageLogic();
        }

        public ActionResult ScoreReport()
        {
            
            List<Business_Exercises_Infomation> exerciseList = _sl.GetCheckedExerciseList();
            List<CS_Master_2> exportTypeList = _sl.GetExportTypeList();
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.ScoreRecordsModule);
            ViewData["currLoginName"] = CurrentUser.GetCurrentUser().LoginName;
            ViewBag.exportType = exportTypeList;
            ViewBag.exerciseCheckedList = exerciseList;
            ViewBag.CurrentModulePermission = roleModuleModel;
            return View();
        }

        public JsonResult GetCheckedExerciseList()
        {
            var exerciseList = _sl.GetCheckedExerciseList();

            return new ConfigurableJsonResult() { Data = exerciseList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// 查询习题报表信息列表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult GetAccountSource(Search_ScoreReport entity)
        {
            var model = new U_ScoreReport();
            model = _sl.GetScoreReport(entity);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取每一道习题的答题情况
        /// </summary>
        /// <param name="vguid"></param>
        /// <param name="departmentVguid">部门vguid</param>
        /// <returns></returns>
        public JsonResult GetExerciseDetail(string vguid, string departmentVguid)
        {
            var model = new List<ExerciseDetailReport>();
            model = _sl.GetExerciseDetail(vguid, departmentVguid);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取习题答题状况详情（饼图）
        /// </summary>
        /// <param name="vguid"></param>
        /// <param name="departmentVguid">部门vguid</param>
        /// <returns></returns>
        public JsonResult GetExerciseRateDetail(string vguid, string departmentVguid)
        {
            var model = new List<RateModel>();
            model = _sl.GetExerciseRateDetail(vguid, departmentVguid);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导出
        /// </summary>
        public void Export()
        {
            string exerciseVguid = Request.QueryString["exerciseVguid"];
            string exportType = Request.QueryString["exportType"];
            string departmentVguid = Request.QueryString["departmentVguid"];
            _sl.Export(exerciseVguid, exportType, departmentVguid);
        }

    }
}
