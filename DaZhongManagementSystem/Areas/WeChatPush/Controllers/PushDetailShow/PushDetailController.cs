using DaZhongManagementSystem.Areas.WeChatPush.Controllers.PushDetailShow.BusinessLogic;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using SyntacticSugar;
using System;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.WeChatExercise.BusinessLogic;
using DaZhongManagementSystem.Common;
using JQWidgetsSugar;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.PushDetailShow
{
    public class PushDetailController : Controller
    {
        //
        // GET: /WeChatPush/PushDetail/
        private readonly PushDetailLogic _pl;
        private readonly WeChatExerciseLogic _wl;
        public PushDetailController()
        {
            _wl = new WeChatExerciseLogic();
            _pl = new PushDetailLogic();
        }

        public ActionResult PushDetail(string code)
        {
            string pushContentVguid = Request.QueryString["Vguid"]; //"55ca3608-93d3-4245-b6f7-e4af76482edd";//
            var pushContentModel = _pl.GetPushDetail(pushContentVguid);
            string openHttpAddress = ConfigSugar.GetAppString("OpenHttpAddress");
            bool isValidTime = false;//未过有效期
            if (pushContentModel != null)
            {
                if (pushContentModel.PeriodOfValidity != null)
                {
                    if (DateTime.Now > pushContentModel.PeriodOfValidity)
                    {
                        isValidTime = true;//已过有效期
                    }
                }
            }
            string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
            string userInfoStr = Common.WeChatPush.WeChatTools.GetUserInfoByCode(accessToken, code);
            var userInfo = JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr);//用户ID
            _pl.UpdateIsRead(userInfo.UserId, pushContentVguid);//更新用户是否阅读推送
            if (pushContentModel.MessageType == 5) //培训推送
            {
                if (pushContentModel.RevenueType != 0)
                {
                    ViewBag.countDownConfig = pushContentModel.RevenueType;
                }
                else
                {
                    string countDownStr = _pl.GetCountDown();
                    double countDownConfig = double.Parse(countDownStr);//double.Parse(ConfigSugar.GetAppString("CountDown"));//倒计时配置
                    ViewBag.countDownConfig = countDownConfig;
                }
            }

            ViewBag.isValidTime = isValidTime;
            ViewBag.code = code;
            ViewBag.openHttpAddress = openHttpAddress;
            ViewData["PushContentModel"] = pushContentModel;
            return View();
        }
        /// <summary>
        /// 协议推送的详情页面
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult AgreementDetail(string code)
        {
            string pushContentVguid = Request.QueryString["Vguid"]; //"55ca3608-93d3-4245-b6f7-e4af76482edd";//
            var pushContentModel = _pl.GetPushDetail(pushContentVguid);
            bool isValidTime = false;//未过有效期
            if (pushContentModel != null)
            {
                if (pushContentModel.PeriodOfValidity != null)
                {
                    if (DateTime.Now > pushContentModel.PeriodOfValidity)
                    {
                        isValidTime = true;//已过有效期
                    }
                }
            }
            string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
            string userInfoStr = Common.WeChatPush.WeChatTools.GetUserInfoByCode(accessToken, code);
            var weChatUserInfo = JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr);//用户ID
            var userInfo = _wl.GetUserInfo(weChatUserInfo.UserId);
            ViewData["personVguid"] = userInfo.Vguid;
            _pl.UpdateIsRead(weChatUserInfo.UserId, pushContentVguid);//更新用户是否阅读推送
            ViewBag.isValidTime = isValidTime;
            ViewData["PushContentModel"] = pushContentModel;
            return View();
        }
        /// <summary>
        /// 通过vguid获取推送信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public Business_WeChatPush_Information GetPushDetail(string vguid)
        {
            var model = _pl.GetPushDetail(vguid);

            return model;
        }
        /// <summary>
        /// 通过vguid获取推送信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public JsonResult GetWeChatPushDetail(string vguid)
        {
            var model = _pl.GetPushDetail(vguid);
            return Json(model);
        }
        /// <summary>
        ///  新增协议操作信息
        /// </summary>
        /// <param name="agreementInfo"></param>
        /// <returns></returns>
        public JsonResult CreateAgreementOperationInfo(Business_ProtocolOperations_Information agreementInfo)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = _pl.CreateAgreementOperationInfo(agreementInfo);
            models.respnseInfo = models.isSuccess ? "1" : "0";
            return Json(models);
        }
        /// <summary>
        /// 用户是否已经操作过协议
        /// </summary>
        /// <param name="agreementInfo"></param>
        /// <returns></returns>
        public JsonResult IsExistAgreementOperationInfo(Business_ProtocolOperations_Information agreementInfo)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = _pl.IsExistAgreementOperationInfo(agreementInfo);
            models.respnseInfo = models.isSuccess ? "1" : "0";
            return Json(models);

        }
    }
}
