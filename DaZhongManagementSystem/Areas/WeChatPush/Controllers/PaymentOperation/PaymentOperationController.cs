using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.RedPacketOperation.Business;
using DaZhongManagementSystem.Controllers;
using DaZhongManagementSystem.Entities.UserDefinedEntity;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.PaymentOperation
{
    public class PaymentOperationController : BaseController
    {
        //
        // GET: /WeChatPush/PaymentOperation/
        private readonly RedPacketLogic _redPacketLogic;

        public PaymentOperationController()
        {
            _redPacketLogic=new RedPacketLogic();
        }
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 根据搜索条件获取企业付款历史
        /// </summary>
        /// <param name="searchParas">搜索条件</param>
        /// <param name="para">页码信息</param>
        /// <returns></returns>
        public JsonResult GetPaymentInfos(Search_RedPacketHistory searchParas, GridParams para)
        {
            para.pagenum = para.pagenum + 1;//页0，+1
            var list = _redPacketLogic.GetPaymentInfos(searchParas, para);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
