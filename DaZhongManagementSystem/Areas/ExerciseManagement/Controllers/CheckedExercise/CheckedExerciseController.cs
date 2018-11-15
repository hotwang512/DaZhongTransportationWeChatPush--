using DaZhongManagementSystem.Areas.ExerciseManagement.Controllers.CheckedExercise.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using JQWidgetsSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.ExerciseManagement.Controllers.CheckedExercise
{
    public class CheckedExerciseController : BaseController
    {
        //
        // GET: /ExerciseManagement/CheckedExercise/
        public CheckedExerciseLogic _cl;
        public AuthorityManageLogic _al;
        public CheckedExerciseController()
        {
            _cl = new CheckedExerciseLogic();
            _al = new AuthorityManageLogic();
        }

        public ActionResult CheckedList()
        {
            //绑定习题状态
            var exerciseStatus = new List<CS_Master_2>();
            exerciseStatus = _cl.GetExerciseStatus();
            ViewData["ExerciseStatus"] = exerciseStatus;

            //绑定录入类型
            var inputType = new List<CS_Master_2>();
            inputType = _cl.GetInputType();
            ViewData["InputType"] = inputType;
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.CheckedExerciseModule);
            
            ViewBag.CurrentModulePermission = roleModuleModel;
            return View();
        }

        public ActionResult CheckedDetail()
        {
            //绑定习题类型
            var exerciseType = new List<CS_Master_2>();
            exerciseType = _cl.GetExerciseType();
            ViewData["ExerciseType"] = exerciseType;

            string vguid = Request.QueryString["Vguid"];
            ViewData["Type"] = Request.QueryString["Type"];
            Business_Exercises_Infomation exerciseMain = new Business_Exercises_Infomation();
            exerciseMain = _cl.GetExerciseByVguid(vguid);
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.CheckedExerciseModule);

            ViewBag.CurrentModulePermission = roleModuleModel;
            ViewBag.exerciseMainModel = exerciseMain;
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
            var model = _cl.GetExerciseListBySearch(searchParam, para);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批量删除习题
        /// </summary>
        /// <param name="vguidList">勾选习题Vguid数组</param>
        /// <returns></returns>
        //public JsonResult DeletedExercise(string[] vguidList)
        //{
        //    var models = new ActionResultModel<string>();
        //    models.isSuccess = false;

        //    models.isSuccess = _cl.DeletedExercise(vguidList);
        //    models.respnseInfo = models.isSuccess == true ? "1" : "0";

        //    return Json(models, JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// 通过习题主信息的Vguid获取习题详细列表
        /// </summary>
        /// <param name="Vguid"></param>
        /// <returns></returns>
        //[HttpPost]
        public JsonResult GetExerciseDetailListByMainVguid(string Vguid)
        {
            List<Business_ExercisesDetail_Infomation> exerciseDetailList = new List<Business_ExercisesDetail_Infomation>();
            exerciseDetailList = _cl.GetExerciseDetailListByMainVguid(Vguid);
            return Json(exerciseDetailList, JsonRequestBehavior.AllowGet);
        }

    }
}
