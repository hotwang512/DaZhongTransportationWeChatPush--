using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.ExerciseLibraryManagement.Controllers.CheckedExerciseLibrary.BusinessLogic;
using DaZhongManagementSystem.Areas.ExerciseLibraryManagement.Controllers.ExerciseLibraryManagement.BusinessLogic;
using DaZhongManagementSystem.Areas.ExerciseManagement.Controllers.ExerciseManagement.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Controllers;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using JQWidgetsSugar;

namespace DaZhongManagementSystem.Areas.ExerciseLibraryManagement.Controllers.CheckedExerciseLibrary
{
    public class CheckedExerciseLibraryController : BaseController
    {
        //
        // GET: /ExerciseLibraryManagement/CheckedExerciseLibrary/
        private CheckedExerciseLibraryLogic _checkedLogic;
        private ExerciseLibraryLogic _exerciseLibraryLogic;
        private ExerciseLogic _el;
        private AuthorityManageLogic _al;

        public CheckedExerciseLibraryController()
        {
            _exerciseLibraryLogic = new ExerciseLibraryLogic();
            _al = new AuthorityManageLogic();
            _el = new ExerciseLogic();
            _checkedLogic = new CheckedExerciseLibraryLogic();
        }

        public ActionResult CheckedList()
        {
            //绑定习题状态
            var exerciseStatus = new List<CS_Master_2>();
            exerciseStatus = _el.GetExerciseStatus();
            ViewData["ExerciseStatus"] = exerciseStatus;

            //绑定录入类型
            var inputType = new List<CS_Master_2>();
            inputType = _exerciseLibraryLogic.GetInputType();
            ViewData["InputType"] = inputType;

            var exerciseType = _el.GetExerciseType();
            ViewData["exerciseType"] = exerciseType;
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.ExerciseLibraryFormalModule);
            ViewBag.CurrentModulePermission = roleModuleModel;
            return View();
        }


        /// <summary>
        /// 分页查询正式习题信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResult GetCheckedExerciseListBySearch(Business_Exercises_Infomation_Search searchParam, GridParams para)
        {
            if (para.sortdatafield == null)
            {
                para.sortdatafield = "CreatedDate";
                para.sortorder = "desc";
            }
            para.pagenum = para.pagenum + 1;
            var model = _checkedLogic.GetCheckedExerciseListBySearch(searchParam, para);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 随机生成习题
        /// </summary>
        /// <returns></returns>
        public JsonResult RandomExercise()
        {
            var model = _checkedLogic.RandomExercise();
            return Json(model);
        }

        /// <summary>
        /// 获取正式习题的分值数量
        /// </summary>
        /// <returns></returns>
        public JsonResult GetExerciseScoreInfo()
        {
            var model = _checkedLogic.GetExerciseScoreInfo();
            return Json(model);
        }

        /// <summary>
        /// 将习题状态变成草稿
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public JsonResult BackToDraftStatus(List<Guid> vguidList)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = _checkedLogic.BackToDraftStatus(vguidList);
            models.respnseInfo = models.isSuccess ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }
    }
}
