using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using JQWidgetsSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Controllers;
using DaZhongManagementSystem.Areas.QuestionManagement.Controllers.CheckedQuestion.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;

namespace DaZhongManagementSystem.Areas.QuestionManagement.Controllers.CheckedQuestion
{
    public class CheckedQuestionController : BaseController
    {
        //
        // GET: /QuestionManagement/CheckedQuestion/
        public CheckedQuestionLogic _cl;
        public AuthorityManageLogic _al;
        public CheckedQuestionController()
        {
            _cl = new CheckedQuestionLogic();
            _al = new AuthorityManageLogic();
        }

        public ActionResult CheckedList()
        {
            //绑定问卷状态
            var questionStatus = new List<CS_Master_2>();
            questionStatus = _cl.GetQuestionStatus();
            ViewData["QuestionStatus"] = questionStatus;

            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.CheckedQuestionModule);

            ViewBag.CurrentModulePermission = roleModuleModel;
            return View();
        }

        public ActionResult CheckedDetail()
        {
            //绑定问卷类型
            var questionType = new List<CS_Master_2>();
            questionType = _cl.GetQuestionType();
            ViewData["QuestionType"] = questionType;

            string vguid = Request.QueryString["Vguid"];
            ViewData["Type"] = Request.QueryString["Type"];
            Business_Questionnaire questionMain = new Business_Questionnaire();
            questionMain = _cl.GetQuestionByVguid(vguid);
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.CheckedQuestionModule);

            ViewBag.CurrentModulePermission = roleModuleModel;
            ViewBag.questionMainModel = questionMain;
            return View();
        }

        /// <summary>
        /// 分页查询问卷信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResult GetQuestionListBySearch(Business_Questionnaire_Search searchParam, GridParams para)
        {
            if (para.sortdatafield == null)
            {
                para.sortdatafield = "CreatedDate";
                para.sortorder = "desc";
            }
            para.pagenum = para.pagenum + 1;
            var model = _cl.GetQuestionListBySearch(searchParam, para);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批量删除问卷
        /// </summary>
        /// <param name="vguidList">勾选问卷Vguid数组</param>
        /// <returns></returns>
        //public JsonResult DeletedQuestion(string[] vguidList)
        //{
        //    var models = new ActionResultModel<string>();
        //    models.isSuccess = false;

        //    models.isSuccess = _cl.DeletedQuestion(vguidList);
        //    models.respnseInfo = models.isSuccess == true ? "1" : "0";

        //    return Json(models, JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// 通过问卷主信息的Vguid获取问卷详细列表
        /// </summary>
        /// <param name="Vguid"></param>
        /// <returns></returns>
        //[HttpPost]
        public JsonResult GetQuestionDetailListByMainVguid(string Vguid)
        {
            List<Business_QuestionnaireDetail> questionDetailList = new List<Business_QuestionnaireDetail>();
            questionDetailList = _cl.GetQuestionDetailListByMainVguid(Vguid);
            return Json(questionDetailList, JsonRequestBehavior.AllowGet);
        }

    }
}
