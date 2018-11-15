using Aspose.Cells;
using DaZhongManagementSystem.Areas.QuestionManagement.Controllers.QuestionManagement.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Common.Tools;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using JQWidgetsSugar;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.QuestionManagement.Controllers.QuestionManagement
{
    public class QuestionManagementController : BaseController
    {
        //
        // GET: /QuestionManagement/QuestionManagement/

        public QuestionLogic _el;
        public AuthorityManageLogic _al;
        public QuestionManagementController()
        {
            _el = new QuestionLogic();
            _al = new AuthorityManageLogic();
        }

        /// <summary>
        /// 问卷列表界面
        /// </summary>
        /// <returns></returns>
        public ActionResult QuestionList()
        {
            //绑定问卷状态
            var exerciseStatus = new List<CS_Master_2>();
            exerciseStatus = _el.GetQuestionStatus();
            ViewData["QuestionStatus"] = exerciseStatus;

            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.QuestionModule);

            ViewBag.CurrentModulePermission = roleModuleModel;
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
            var model = _el.GetQuestionListBySearch(searchParam, para);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 问卷详情界面（新增/编辑）
        /// </summary>
        /// <returns></returns>
        public ActionResult QuestionDetail()
        {
            //绑定问卷类型
            var questionType = new List<CS_Master_2>();
            questionType = _el.GetQuestionType();
            ViewData["QuestionType"] = questionType;

            bool isEdit = Boolean.Parse(Request.QueryString["isEdit"]);
            string vguid = Request.QueryString["Vguid"];

            Business_Questionnaire questionMain = new Business_Questionnaire();
            if (isEdit)//编辑
            {
                questionMain = _el.GetQuestionByVguid(vguid);

            }
            else//新增
            {
                questionMain.Vguid = Guid.NewGuid();
            }
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.QuestionModule);

            ViewBag.CurrentModulePermission = roleModuleModel;
            ViewBag.isEdit = isEdit;
            ViewBag.questionMainModel = questionMain;
            return View();
        }

        /// <summary>
        /// 批量审核问卷
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public JsonResult CheckedQuestion(string[] vguidList)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = _el.CheckedQuestion(vguidList);
            models.respnseInfo = models.isSuccess == true ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批量删除问卷
        /// </summary>
        /// <param name="vguidList">勾选问卷Vguid数组</param>
        /// <returns></returns>
        public JsonResult DeletedQuestion(string[] vguidList)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = false;

            models.isSuccess = _el.DeletedQuestion(vguidList);
            models.respnseInfo = models.isSuccess == true ? "1" : "0";

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过问卷主信息的Vguid获取问卷详细列表
        /// </summary>
        /// <param name="Vguid"></param>
        /// <returns></returns>
        //[HttpPost]
        public JsonResult GetQuestionDetailListByMainVguid(string Vguid)
        {
            List<Business_QuestionnaireDetail> questionDetailList = new List<Business_QuestionnaireDetail>();
            questionDetailList = _el.GetQuestionDetailListByMainVguid(Vguid);
            return Json(questionDetailList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存问卷信息(主信息、详细信息)
        /// </summary>
        /// <param name="questionMainModel"></param>
        /// <returns></returns>
        public JsonResult SaveQuestionMain(Business_Questionnaire questionMainModel, bool isEdit, string questionData)
        {
            var models = new ActionResultModel<String>();
            models.isSuccess = false;
            models.isSuccess = _el.SaveQuestionMain(questionMainModel, isEdit, questionData);
            models.respnseInfo = models.isSuccess == true ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }

    }
}
