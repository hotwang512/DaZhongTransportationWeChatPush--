using DaZhongManagementSystem.Areas.ReportManagement.Controllers.PushMsgReport.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.ReportManagement.Controllers.PushMsgReport
{
    public class PushMsgReportController : BaseController
    {
        //
        // GET: /ReportManagement/PushMsgReport/
        public AuthorityManageLogic _authorityManageLogic;
        public PushMsgReportLogic _pushMsgReportLogic;
        public PushMsgReportController()
        {
            _authorityManageLogic = new AuthorityManageLogic();
            _pushMsgReportLogic = new PushMsgReportLogic();
        }


        public ActionResult PushMsgReport()
        {
            Sys_Role_Module roleModuleModel = _authorityManageLogic.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.PushRecordsModule);

            ViewBag.CurrentModulePermission = roleModuleModel;
            return View();
        }

        /// <summary>
        /// 获取推送列表
        /// </summary>
        /// <param name="pushMsgName">推送名称</param>
        /// <param name="para">分页</param>
        /// <returns></returns>
        public JsonResult GetPushMsgReportList(string pushMsgName)
        {
            var model = _pushMsgReportLogic.GetPushMsgReportList(pushMsgName);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导出
        /// </summary>
        public void Export()
        {
            string title = Request.QueryString["title"];
            _pushMsgReportLogic.Export(title);
        }

    }
}
