﻿using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.WeChatExercise.BusinessLogic;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using JQWidgetsSugar;
using SyntacticSugar;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.WeChatExercise
{
    public class WeChatExerciseController : Controller
    {
        //
        // GET: /BasicDataManagement/WeChatExercise/
        public WeChatExerciseLogic _wl;
        public WeChatExerciseController()
        {
            _wl = new WeChatExerciseLogic();
        }

        public ActionResult Index(string code)
        {
            bool isValid = false;
            string vguidStr = Request.QueryString["exerciseVguid"];
            string pushContentVguid = string.Empty;
            string exerciseVguid = vguidStr.Split(',')[0]; //"A5EF3FB4-96E7-4DF3-9BDD-1D172D338149";
            var effectiveDate = _wl.GetExerciseInfo(exerciseVguid).EffectiveDate;
            if (effectiveDate < DateTime.Now)
            {
                isValid = true;
            }
            ViewData["isValid"] = isValid;
            string wechatMain = string.Empty;
            ViewData["isHistory"] = "0";
            if (vguidStr.Split(',').Count() == 2)
            {
                pushContentVguid = vguidStr.Split(',')[1];//Request.QueryString["pushContentVguid"];
                ViewData["isHistory"] = "0";
            }
            else if (vguidStr.Split(',').Count() == 3)
            {
                ViewData["isHistory"] = "1";    //从消息历史界面跳转而来
                wechatMain = vguidStr.Split(',')[2];
            }
            string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
            U_WeChatUserID userInfo = new U_WeChatUserID();
            string userInfoStr = Common.WeChatPush.WeChatTools.GetUserInfoByCode(accessToken, code);
            userInfo = Common.JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr);//用户ID
            string answerCount = _wl.GetAnswerCount();
            Business_Personnel_Information personInfoModel = GetUserInfo(userInfo.UserId);//获取人员表信息

            bool isRead = false;
            if (pushContentVguid != string.Empty)
            {
                isRead = UpdateIsRead(pushContentVguid, userInfo.UserId);//更新是否查看推送
            }
            ViewData["wechatMain"] = wechatMain;
            ViewBag.answerCount = answerCount;
            ViewBag.pushContentVguid = pushContentVguid;
            ViewBag.exerciseVguid = exerciseVguid;
            ViewBag.PersonInfo = personInfoModel;
            ViewBag.code = code;
            return View();
        }

        /// <summary>
        /// 更新是否查看推送
        /// </summary>
        /// <param name="pushVguid"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool UpdateIsRead(string pushVguid, string userID)
        {
            bool isRead = false;
            isRead = _wl.UpdateIsRead(pushVguid, userID);
            return isRead;
        }

        /// <summary>
        /// 获取人员详细信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Business_Personnel_Information GetUserInfo(string userID)
        {
            Business_Personnel_Information personModel = new Business_Personnel_Information();
            personModel = _wl.GetUserInfo(userID);
            return personModel;
        }

        /// <summary>
        /// 上传照片
        /// </summary>
        /// <param name="exerciseVguid"></param>
        /// <param name="personVguid"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult LoadUserPic(string exerciseVguid, string personVguid)
        {
            UploadImage ui = new UploadImage();
            ui.SetAllowSize = 1000;//允许上传图片的大小（M）
            ui.SetAllowFormat = ".jpeg,.jpg,.bmp,.gif,.png";//允许图片上传格式
            string url = "/Areas/PersonalCenterManagement/Views/_img/";//文件保存路径
            string saveFolder = Server.MapPath(url);
            HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];
            var reponseMessage = ui.FileSaveAs(file, saveFolder);//保存图片
            //System.Threading.Thread.Sleep(10000);
            if (!reponseMessage.IsError)
            {
                url = System.IO.Path.Combine(url, reponseMessage.FileName);
                bool imgResult = _wl.UpLoadImg(url, exerciseVguid, personVguid);
            }
            return Json(reponseMessage);
        }

        /// <summary>
        /// 查询习题信息（主信息、详细信息）
        /// </summary>
        /// <param name="Vguid"></param>
        /// <param name="personVguid"></param>
        /// <returns></returns>
        public JsonResult GetExerciseAll(string Vguid, string personVguid)
        {
            //两个地方用到（一个是这里。另一个是阅卷GetShortAnswerDetail方法）
            JsonResultEntity<V_Business_ExercisesAndAnswer_Infomation, V_Business_ExercisesDetailAndExercisesAnswerDetail_Information> exerciseAll = new JsonResultEntity<V_Business_ExercisesAndAnswer_Infomation, V_Business_ExercisesDetailAndExercisesAnswerDetail_Information>();
            exerciseAll = _wl.GetExerciseDoneAllMsg(Vguid, personVguid);
            //exerciseAll.MainRow[0].EffectiveDate
            return Json(exerciseAll, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 提交用户答案 （用户点击选项的时候同时提交）
        /// </summary>
        /// <param name="userAnswer"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public bool SubmitUserAnswer(U_ExercisesAnswerDetail userAnswer)
        {
            bool result = false;
            result = _wl.SaveUserAnswer(userAnswer);
            return result;
        }

        /// <summary>
        /// 判断答案是否正确并计算得分
        /// </summary>
        /// <param name="exerciseDetailVguid"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public int CheckAnswer(string exerciseDetailVguid, string answer)
        {
            return _wl.CheckAnswer(exerciseDetailVguid, answer);
        }

        /// <summary>
        /// 提交全部习题（更新总分数，批改状态和习题完成状态）
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult SubmitAllExercise(string businessPersonnelVguid, string businessExercisesVguid)
        {
            var model = new ActionResultModel<String>();
            U_ExerciseResult exerciseResult = new U_ExerciseResult();
            //bool isExistShortMsg = false;
            //isExistShortMsg = _wl.IsExistShortMsg(businessExercisesVguid);
            //if (!isExistShortMsg)
            //{
            exerciseResult = _wl.SubmitAllExercise(businessPersonnelVguid, businessExercisesVguid);
            model.isSuccess = exerciseResult.isComplete;
            if (model.isSuccess)
            {
                model.respnseInfo = exerciseResult.score.ToString();
            }
            //}
            //else
            //{
            //    model.isSuccess = true;
            //    model.respnseInfo = "-1";//存在简答题，提示答题已完成
            //}
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 给阅读消息历史并且没有答过题的人重新推送习题
        /// </summary>
        /// <param name="businessPersonnelVguid"></param>
        /// <param name="wechatMainVguid"></param>
        /// <returns></returns>
        public JsonResult ReWechatPushExercise(string businessPersonnelVguid, string wechatMainVguid)
        {
            var model = new ActionResultModel<String>();
            model.isSuccess = _wl.ReWechatPushExercise(businessPersonnelVguid, wechatMainVguid);
            model.respnseInfo = model.isSuccess ? "1" : "0";
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 当前人是否推送过
        /// </summary>
        /// <param name="businessPersonnelVguid"></param>
        /// <param name="wechatMainVguid"></param>
        /// <returns></returns>
        public JsonResult IsPushed(string businessPersonnelVguid, string wechatMainVguid)
        {
            var model = new ActionResultModel<String>();
            model.isSuccess = _wl.IsPushed(businessPersonnelVguid, wechatMainVguid);
            model.respnseInfo = model.isSuccess ? "1" : "0";
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
