using DaZhongManagementSystem.Areas.WeChatPush.Controllers.CheckedList.BusinessLogic;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using JQWidgetsSugar;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.DraftList.BusinessLogic;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Common.Tools;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.CheckedList
{
    public class CheckedListController : BaseController
    {
        //
        // GET: /WeChatPush/CheckedList/
        public CheckedListLogic _cl;
        public DraftInfoLogic _dl;
        public CheckedListController()
        {
            _cl = new CheckedListLogic();
            _dl = new DraftInfoLogic();
        }

        public ActionResult CheckedList()
        {
            List<CS_Master_2> pushType = new List<CS_Master_2>();
            pushType = _cl.GetPushTypeList();
            ViewData["PushType"] = pushType;

            return View();
        }

        public ActionResult CheckedDetail()
        {
            Business_WeChatPush_Information weChatMainModel = new Business_WeChatPush_Information();
            bool isEdit = Boolean.Parse(Request.QueryString["isEdit"]);
            string vguid = Request.QueryString["Vguid"];
            List<CS_Master_2> pushType = new List<CS_Master_2>();
            pushType = _cl.GetPushTypeList();
            ViewData["PushType"] = pushType;

            List<CS_Master_2> weChatPush = new List<CS_Master_2>();
            weChatPush = _cl.GetWeChatPushType();
            ViewData["WeChatPushType"] = weChatPush;

            List<Business_Exercises_Infomation> exerciseList = new List<Business_Exercises_Infomation>();
            exerciseList = _cl.GetExerciseList();
            ViewData["ExerciseList"] = exerciseList;
            ViewData["RevenueTypeList"] = _dl.GetRevenueType();
            weChatMainModel = _cl.GetWeChatMainByVguid(vguid);
            var pushObject = _dl.GetPushObjectStr(vguid);
            ViewBag.WeChatModel = weChatMainModel;
            ViewBag.pushObject = pushObject;
            ViewBag.listCountersignType = _dl.GetMasterDataType(MasterVGUID.CountersignType);
            ViewBag.listRedPacketType = _dl.GetMasterDataType(MasterVGUID.RedPacketType);
            return View();
        }

        /// <summary>
        /// 通过vguid获取推送信息详情
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public JsonResult GetPushMessage(string vguid)
        {
            var model = new ActionResultModel<string>();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批量提交推送信息
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public JsonResult PushSubmitList(string[] vguidList)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = false;

            models.isSuccess = _cl.PushSubmitList(vguidList);
            models.respnseInfo = models.isSuccess == true ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过查询条件获取推送信息列表（已审核）
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

            //List<DeparTment_1> departmenteList = _dl.GetDepartmentList(searchParam, para);
            var model = _cl.GetWeChatPushListBySearch(searchParam, para);
            return new ConfigurableJsonResult { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
