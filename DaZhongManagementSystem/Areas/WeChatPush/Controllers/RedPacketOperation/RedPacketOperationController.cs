using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.RedPacketOperation.Business;
using DaZhongManagementSystem.Controllers;
using DaZhongManagementSystem.Entities.UserDefinedEntity;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.RedPacketOperation
{
    public class RedPacketOperationController : BaseController
    {
        //
        // GET: /WeChatPush/RedPacketOperation/
        private readonly RedPacketLogic _redPacketLogic;

        public RedPacketOperationController()
        {
            _redPacketLogic = new RedPacketLogic();
        }
        public ActionResult RedPacketOperationList()
        {
            ViewBag.ListRedPacketStatus = _redPacketLogic.GetRedPacketStatus();
            return View();
        }


        /// <summary>
        /// 获取协议操作历史记录
        /// </summary>
        /// <param name="searchParas"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResult GetRedPacketHistoryList(Search_RedPacketHistory searchParas, GridParams para)
        {
            para.pagenum = para.pagenum + 1;//页0，+1
            var list = _redPacketLogic.GetRedPacketHistoryList(searchParas, para);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
