using DaZhongManagementSystem.Areas.WeChatPush.Controllers.PushedList.BusinessLogic;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.DraftList.BusinessLogic;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Common.Tools;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.PushedList
{
    public class PushedListController : BaseController
    {
        //
        // GET: /WeChatPush/PushedList/
        private readonly PushedListLogic _pl;
        private readonly DraftInfoLogic _dl;
        public PushedListController()
        {
            _pl = new PushedListLogic();
            _dl = new DraftInfoLogic();
        }

        public ActionResult PushedList()
        {
            var pushType = _pl.GetPushTypeList();
            ViewData["PushType"] = pushType;
            return View();
        }

        public ActionResult PushedDetail()
        {
            string vguid = Request.QueryString["Vguid"];
            var pushType = _pl.GetPushTypeList();
            ViewData["PushType"] = pushType;

            var weChatPush = _pl.GetWeChatPushType();
            ViewData["WeChatPushType"] = weChatPush;


            var exerciseList = _pl.GetExerciseList();
            ViewData["ExerciseList"] = exerciseList;
            ViewData["RevenueTypeList"] = _dl.GetRevenueType();
            var weChatMainModel = _pl.GetWeChatMainByVguid(vguid);

            ViewBag.WeChatModel = weChatMainModel;
            var pushObj = _dl.GetPushObjectStr(vguid);
            ViewBag.pushObj = pushObj;
            ViewBag.listCountersignType = _dl.GetMasterDataType(MasterVGUID.CountersignType);
            ViewBag.listRedPacketType = _dl.GetMasterDataType(MasterVGUID.RedPacketType);
            return View();
        }

        /// <summary>
        /// 通过查询条件获取推送信息列表（已推送）
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

            var model = _pl.GetWeChatPushListBySearch(searchParam, para);
            return new ConfigurableJsonResult() { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}
