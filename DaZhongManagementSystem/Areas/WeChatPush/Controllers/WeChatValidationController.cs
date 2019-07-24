using DaZhongManagementSystem.Areas.WeChatPush.WeChatValidationBusiness;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using JQWidgetsSugar;
using System;
using System.Web.Mvc;
using SyntacticSugar;
using DaZhongManagementSystem.Models.WeChatPush;
using DaZhongManagementSystem.Entities.TableEntity.DaZhongPersonTable;
using DaZhongManagementSystem.Common.LogHelper;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers
{
    public class WeChatValidationController : Controller
    {
        //
        // GET: /WeChatPush/WeChatValidation/
        public WeChatValidationLogic _wl;
        public WeChatValidationController()
        {
            _wl = new WeChatValidationLogic();
        }

        /// <summary>
        /// 开启微信回调模式
        /// </summary>
        /// <returns></returns>
        public string OpenWeChatCallbackMode(U_WeChatCallbackParameter wcp)
        {
            //数据不为空，表示验证开启微信回调模式
            //数据为空，表示微信数据正常回调
            if (!string.IsNullOrEmpty(wcp.echostr))
            {
                string corpid = "";
                string echostr = Cryptography.AES_decrypt(wcp.echostr, ConfigSugar.GetAppString("WeChatCallbackEncodingAESKey"), ref corpid);
                return echostr;
            }
            else
            {
                WeChatHandle wch = WeChatEventFactory.GetWeChatHandle(wcp, Request.InputStream);
                return wch.ExecuteEventHandle();
            }

        }

        public ActionResult TestUser()
        {
            _wl.TestUser();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult TestAoauth(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                ViewBag.Code = "没有Code";
            }
            else
            {
                ViewBag.Code = "有Code啦:" + code;
            }
            return View();

        }

        /// <summary>
        /// 微信二次验证
        /// </summary>
        /// <param name="code">微信用户Code</param>
        /// <returns></returns>
        public ActionResult WeChatSecondValidation(string code)
        {
            U_WeChatUserID userInfo = new U_WeChatUserID();
            U_UserInfo userDetail = new U_UserInfo();
            string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
            //UserInfoUrl
            string userInfoStr = Common.WeChatPush.WeChatTools.GetUserInfoByCode(accessToken, code);
            userInfo = Common.JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr);//用户ID
            string GetUserInfoByUserID = Common.WeChatPush.WeChatTools.GetUserInfoByUserID(accessToken, userInfo.UserId);
            userDetail = Common.JsonHelper.JsonToModel<U_UserInfo>(GetUserInfoByUserID);//用户信息
            string openHttpAddress = ConfigSugar.GetAppString("OpenHttpAddress");
            Common.LogHelper.LogHelper.WriteLog("用户打开注册二次验证页面：Code为-" + code + "-UserID为-" + userInfo.UserId + "-打开注册页面时间为:" + DateTime.Now);

            ViewBag.openHttpAddress = openHttpAddress;
            ViewBag.accessToken = accessToken;
            ViewBag.Code = code;
            ViewBag.userInfo = userInfo;
            ViewBag.userDetail = userDetail;
            return View();
        }

        /// <summary>
        /// 获取部门信息列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOrganization()
        {
            var models = _wl.GetOrganization();
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 审核用户是否存在并保存至Person表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        public JsonResult CheckUser(AllEmployee userModel, string userID, string accessToken, string position, string mobilePhone)
        {
            var model = new ActionResultModel<string>();
            model.isSuccess = false;
            string checkUser = _wl.CheckUser(userModel, userID, position, mobilePhone);
            if (checkUser == "1")
            {
                model.isSuccess = true;
                string focusResult = Common.WeChatPush.WeChatTools.GetAuthSucee(accessToken, userID);
                U_FocusResult resultMsg = Common.JsonHelper.JsonToModel<U_FocusResult>(focusResult);
                if (resultMsg.errcode == 0)
                {
                    model.respnseInfo = "1";//关注成功
                    LogHelper.WriteLog("人员注册成功：" + userID + "   " + resultMsg.errmsg);
                }
                else
                {
                    model.respnseInfo = resultMsg.errmsg;
                    LogHelper.WriteLog("人员注册失败：" + userID + "   " + model.respnseInfo + "  " + resultMsg.errcode);
                    _wl.UpdateStatus(userModel.IDCard);  //更改状态
                }
            }
            else if (checkUser == "2")
            {
                model.isSuccess = false;
                model.respnseInfo = "人员注册失败！";
                LogHelper.WriteLog("注册时人员保存失败：" + model.respnseInfo);
            }
            else if (checkUser == "3")
            {
                model.isSuccess = false;
                model.respnseInfo = "人事库中不存在该人员！";
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }
}
