using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using JQWidgetsSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.ExerciseManagement.Controllers.ExerciseManagement.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.Exercise
{
    public class ExerciseManagementController : BaseController
    {
        //
        // GET: /BasicDataManagement/ExerciseManagement/
        public ExerciseLogic _el;
        public AuthorityManageLogic _al;
        public ExerciseManagementController()
        {
            _el = new ExerciseLogic();
            _al = new AuthorityManageLogic();
        }

        /// <summary>
        /// 习题列表界面
        /// </summary>
        /// <returns></returns>
        public ActionResult ExerciseList()
        {
            //绑定习题状态
            var exerciseStatus = new List<CS_Master_2>();
            exerciseStatus = _el.GetExerciseStatus();
            ViewData["ExerciseStatus"] = exerciseStatus;

            //绑定录入类型
            var inputType = new List<CS_Master_2>();
            inputType = _el.GetInputType();
            ViewData["InputType"] = inputType;

            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.ExerciseModule);

            ViewBag.CurrentModulePermission = roleModuleModel;
            return View();
        }

        /// <summary>
        /// 分页查询习题信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResult GetExerciseListBySearch(Business_Exercises_Infomation_Search searchParam, GridParams para)
        {
            if (para.sortdatafield == null)
            {
                para.sortdatafield = "CreatedDate";
                para.sortorder = "desc";
            }
            para.pagenum = para.pagenum + 1;
            var model = _el.GetExerciseListBySearch(searchParam, para);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 习题详情界面（新增/编辑）
        /// </summary>
        /// <returns></returns>
        public ActionResult ExerciseDetail()
        {
            //绑定习题类型
            var exerciseType = new List<CS_Master_2>();
            exerciseType = _el.GetExerciseType();
            ViewData["ExerciseType"] = exerciseType;

            bool isEdit = Boolean.Parse(Request.QueryString["isEdit"]);
            string vguid = Request.QueryString["Vguid"];
            Business_Exercises_Infomation exerciseMain = new Business_Exercises_Infomation();
            if (isEdit)//编辑
            {
                exerciseMain = _el.GetExerciseByVguid(vguid);

            }
            else//新增
            {
                exerciseMain.Vguid = Guid.NewGuid();
            }
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.ExerciseModule);

            ViewBag.CurrentModulePermission = roleModuleModel;
            ViewBag.isEdit = isEdit;
            ViewBag.exerciseMainModel = exerciseMain;
            return View();
        }

        /// <summary>
        /// 通过习题主信息的Vguid获取习题详细列表
        /// </summary>
        /// <param name="Vguid"></param>
        /// <returns></returns>
        //[HttpPost]
        public JsonResult GetExerciseDetailListByMainVguid(string Vguid)
        {
            List<Business_ExercisesDetail_Infomation> exerciseDetailList = new List<Business_ExercisesDetail_Infomation>();
            exerciseDetailList = _el.GetExerciseDetailListByMainVguid(Vguid);
            return Json(exerciseDetailList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存习题信息(主信息、详细信息)
        /// </summary>
        /// <param name="exerciseMainModel"></param>
        /// <returns></returns>
        public JsonResult SaveExerciseMain(Business_Exercises_Infomation exerciseMainModel, bool isEdit, string exerciseData)
        {
            var models = new ActionResultModel<String>();
            models.isSuccess = false;
            models.isSuccess = _el.SaveExerciseMain(exerciseMainModel, isEdit, exerciseData);
            models.respnseInfo = models.isSuccess == true ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }
    }
}
