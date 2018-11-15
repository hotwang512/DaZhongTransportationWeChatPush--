using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using System.Collections.Generic;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.DraftList.BusinessLogic;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.PushedList.BusinessLogic;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.PushHistory.BusinessLogic;
using DaZhongManagementSystem.Common.Tools;
using DaZhongManagementSystem.Controllers;
using JQWidgetsSugar;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.PushHistory
{
    public class HistoryController : BaseController
    {
        //
        private readonly PushHistoryLogic _historyLogic;
        private readonly PushedListLogic _pushedListLogic;
        private readonly DraftInfoLogic _dl;
        private readonly AuthorityManageLogic _al;
        public HistoryController()
        {
            _dl = new DraftInfoLogic();
            _pushedListLogic = new PushedListLogic();
            _historyLogic = new PushHistoryLogic();
            _al = new AuthorityManageLogic();
        }

        public ActionResult History()
        {
            List<CS_Master_2> pushType = new List<CS_Master_2>();
            pushType = _pushedListLogic.GetPushTypeList();
            ViewData["PushType"] = pushType;
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.PushedHistory);
            ViewBag.CurrentModulePermission = roleModuleModel;

            return View();
        }

        ///// <summary>
        ///// 获取推送详细信息
        ///// </summary>
        ///// <returns></returns>
        public ActionResult HistoryDetail()
        {

            string vguid = Request.QueryString["Vguid"];
            var pushType = _pushedListLogic.GetPushTypeList();
            ViewData["PushType"] = pushType;


            var weChatPush = _pushedListLogic.GetWeChatPushType();
            ViewData["WeChatPushType"] = weChatPush;
            ViewData["RevenueTypeList"] = _dl.GetRevenueType();

            var exerciseList = _pushedListLogic.GetExerciseList();
            ViewData["ExerciseList"] = exerciseList;
            var weChatMainModel = _historyLogic.GetWeChatMainByVguid(vguid);
            ViewBag.WeChatModel = weChatMainModel;
            var pushObj = _dl.GetPushObjectStr(vguid);
            ViewBag.pushObj = pushObj;
            ViewBag.listCountersignType = _dl.GetMasterDataType(MasterVGUID.CountersignType);
            return View();
        }

        /// <summary>
        /// 通过查询条件获取推送信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResult GetWeChatPushListBySearch(SearchWeChatPushList searchParam, GridParams para)
        {
            if (para.sortdatafield == null)
            {
                para.sortdatafield = "CreatedDate";
                para.sortorder = "desc";
            }
            para.pagenum = para.pagenum + 1;//页0，+1
            var model = _historyLogic.GetWeChatPushListBySearch(searchParam, para);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 批量删除推送信息
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public JsonResult DeletePushHistory(string[] vguidList)
        {
            var models = new ActionResultModel<string> {isSuccess = false};

            models.isSuccess = _historyLogic.DeletePushHistory(vguidList);
            models.respnseInfo = models.isSuccess ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }

    }
}
