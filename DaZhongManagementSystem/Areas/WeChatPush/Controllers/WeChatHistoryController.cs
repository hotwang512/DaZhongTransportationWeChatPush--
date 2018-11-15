using System;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.WeChatExercise.BusinessLogic;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.PushHistory.BusinessLogic;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers
{
    public class WeChatHistoryController : Controller
    {
        //
        // GET: /WeChatPush/WeChatHistory/
        private WeChatExerciseLogic _wl;
        private PushHistoryLogic _pushHistoryLogic;
        public WeChatHistoryController()
        {
            _wl = new WeChatExerciseLogic();
            _pushHistoryLogic = new PushHistoryLogic();
        }

        public ActionResult History(string code)
        {
            string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
            string userInfoStr = Common.WeChatPush.WeChatTools.GetUserInfoByCode(accessToken, code);
            var userInfo = Common.JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr);//用户ID
            Business_Personnel_Information personInfoModel = _wl.GetUserInfo(userInfo.UserId);//获取人员表信息
            ViewBag.PersonInfo = personInfoModel;
            return View();
        }
        /// <summary>
        /// 手机端分页获取消息历史记录
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        ///  <param name="personVguid">当前浏览人的vguid</param>
        /// <returns></returns>
        public JsonResult GetWeChatPushList(int pageIndex, Guid personVguid)
        {
            var list = _pushHistoryLogic.GetWeChatPushList(pageIndex, personVguid);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 消息历史详细界面
        /// </summary>
        /// <returns></returns>
        public ActionResult HistoryDetail()
        {
            ViewData["Vguid"] = Request["Vguid"];
            ViewData["personVguid"] = Request["personVguid"];
            return View();
        }
        /// <summary>
        /// 手机端获取消息历史的详细信息
        /// </summary>
        /// <param name="vguid">消息历史主键</param>
        ///  <param name="personVguid">用户主键</param>
        /// <returns></returns>
        public JsonResult GetWeChatMainByVguid(string vguid, Guid personVguid)
        {
            var model = _pushHistoryLogic.GetWeChatDetail(vguid, personVguid);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
