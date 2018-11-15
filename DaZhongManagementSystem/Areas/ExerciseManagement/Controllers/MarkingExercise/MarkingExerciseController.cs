using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.WeChatExercise.BusinessLogic;
using DaZhongManagementSystem.Areas.ExerciseManagement.Controllers.MarkingExercise.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using JQWidgetsSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.ExerciseManagement.Controllers.MarkingExercise
{
    public class MarkingExerciseController : BaseController
    {
        //
        // GET: /ExerciseManagement/MarkingExercise/
        public MarkingExerciseLogic _markingExerciseLogic;
        public WeChatExerciseLogic _weChatExerciseLogic;
        public AuthorityManageLogic _authorityManageLogic;
        public MarkingExerciseController()
        {
            _markingExerciseLogic = new MarkingExerciseLogic();
            _weChatExerciseLogic = new WeChatExerciseLogic();
            _authorityManageLogic = new AuthorityManageLogic();
        }

        public ActionResult MarkingExercise()
        {
            Sys_Role_Module roleModuleModel = _authorityManageLogic.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.MarkingShortQuestionModule);
            List<V_ExercisesUserInformation> exerciseList = new List<V_ExercisesUserInformation>();
            exerciseList = _markingExerciseLogic.GetExerciseList();

            ViewBag.CurrentModulePermission = roleModuleModel;
            ViewBag.exerciseList = exerciseList;
            return View();
        }

        /// <summary>
        /// 判断此习题是否阅过
        /// </summary>
        /// <param name="exerciseVguid">习题Vguid</param>
        /// <param name="personVguid">人员Vguid</param>
        /// <returns></returns>
        public JsonResult IsExerciseMarked(string exerciseVguid, string personVguid)
        {
            var models = new ActionResultModel<String>();
            models.isSuccess = _markingExerciseLogic.IsExerciseMarked(exerciseVguid, personVguid);
            models.respnseInfo = models.isSuccess == true ? "1" : "0";

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取答过本套习题人员列表
        /// </summary>
        /// <param name="vguid">习题主Vguid</param>
        /// <param name="isMarking">是否阅卷</param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResult GetAnsweredPersonList(string vguid, string isMarking, GridParams para)
        {
            para.pagenum = para.pagenum + 1;
            //List<U_MarkingPerson> markingPersonList = new List<U_MarkingPerson>();
            var model = _markingExerciseLogic.GetAnsweredPersonList(vguid,isMarking, para);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取人员简答题目以及答案
        /// </summary>
        /// <param name="exerciseVguid"></param>
        /// <param name="personVguid"></param>
        /// <returns></returns>
        public JsonResult GetShortAnswerDetail(string exerciseVguid, string personVguid)
        {
            JsonResultEntity<V_Business_ExercisesAndAnswer_Infomation, V_Business_ExercisesDetailAndExercisesAnswerDetail_Information> exerciseAll = new JsonResultEntity<V_Business_ExercisesAndAnswer_Infomation, V_Business_ExercisesDetailAndExercisesAnswerDetail_Information>();
            exerciseAll = _weChatExerciseLogic.GetExerciseDoneAllMsg(exerciseVguid, personVguid);//获取用户答题的所有信息（包括题目信息以及答题信息）
            List<V_Business_ExercisesDetailAndExercisesAnswerDetail_Information> shortAnswerList = new List<V_Business_ExercisesDetailAndExercisesAnswerDetail_Information>();
            foreach (var item in exerciseAll.DetailRow)
            {
                if (item.ExerciseType == 4)
                {
                    shortAnswerList.Add(item);
                }
            }
            return Json(shortAnswerList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存简答题分数
        /// </summary>
        /// <param name="personVguid">答题人Vguid</param>
        /// <param name="vguid">习题Vguid</param>
        /// <param name="exerciseDetailVguid">具体习题Vguid</param>
        /// <param name="score">得分</param>
        /// <returns></returns>
        public JsonResult SaveShortAnswerMarking(string personVguid, string vguid, string exerciseDetailVguid, string score)
        {
            var models = new ActionResultModel<String>();
            models.isSuccess = false;
            models.isSuccess = _markingExerciseLogic.SaveShortAnswerMarking(personVguid, vguid, exerciseDetailVguid, score);
            models.respnseInfo = models.isSuccess == true ? "1" : "0";

            return Json(models, JsonRequestBehavior.AllowGet);
        }

    }
}
