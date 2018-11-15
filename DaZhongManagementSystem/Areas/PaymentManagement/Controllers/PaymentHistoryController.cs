
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.WeChatRevenue.BusinessLogic;
using DaZhongManagementSystem.Common.WeChatPush;
using DaZhongManagementSystem.Controllers;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using JQWidgetsSugar;

namespace DaZhongManagementSystem.Areas.PaymentManagement.Controllers
{
    public class PaymentHistoryController : BaseController
    {
        //
        // GET: /PaymentManagement/PaymentHistory/
        private AuthorityManageLogic _al;
        private WeChatRevenueLogic _weChatRevenueLogic;
        public PaymentHistoryController()
        {
            _weChatRevenueLogic = new WeChatRevenueLogic();
            _al = new AuthorityManageLogic();
        }

        public ActionResult PaymentHistory()
        {
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.PaymentHistoryModule);
            ViewBag.CurrentModulePermission = roleModuleModel;
            return View();
        }

        /// <summary>
        /// 获取所有的支付历史
        /// </summary>
        /// <param name="searchParas"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResult GetAllPaymentHistoryInfo(U_PaymentHistory_Search searchParas, GridParams para)
        {
            if (para.sortdatafield == null)
            {
                para.sortdatafield = "PayDate";
                para.sortorder = "desc";
            }
            para.pagenum = para.pagenum + 1; //页0，+1
            var list = _weChatRevenueLogic.GetAllPaymentHistoryInfo(searchParas, para);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除付款历史
        /// </summary>
        /// <param name="vguidList">主键</param>
        /// <returns></returns>
        public JsonResult DeletePaymentHistory(List<Guid> vguidList)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = _weChatRevenueLogic.DeletePaymentHistory(vguidList);
            models.respnseInfo = models.isSuccess ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="searchParas">搜索条件</param>
        public void Export(string searchParas)
        {
            _weChatRevenueLogic.Export(searchParas);
        }

        /// <summary>
        ///  申请退款
        /// </summary>
        /// <param name="transaction_id">微信单号</param>
        /// <param name="total_fee">退款金额</param>
        /// <param name="tradeNo">退款单号</param>
        /// <returns></returns>
        public JsonResult Refund(string transaction_id, string total_fee, string tradeNo)
        {
            var outTradeNo = string.Empty;
            var models = new ReturnResultModel<string>();
            WxPayData data = new WxPayData();
            var totalFee = (int)(decimal.Parse(total_fee) * 100);
            data.SetValue("transaction_id", transaction_id);
            data.SetValue("total_fee", totalFee);//订单总金额
            data.SetValue("refund_fee", totalFee);//退款金额
            if (string.IsNullOrEmpty(tradeNo))
            {
                outTradeNo = WxPayApi.GenerateOutTradeNo();
                data.SetValue("out_refund_no", outTradeNo);//随机生成商户退款单号
            }
            else
            {
                data.SetValue("out_refund_no", tradeNo);//随机生成商户退款单号
            }
            WxPayData result = WeChatTools.Refund(data);//提交退款申请给API，接收返回数据
            if (result.GetValue("return_code").ToString() == "SUCCESS" && result.GetValue("result_code").ToString() == "SUCCESS")
            {
                models.IsSuccess = true;
                models.ResponseInfo = "1";
                _weChatRevenueLogic.UpdateStatus(transaction_id);
            }
            else
            {
                models.IsSuccess = false;
                models.ResponseInfo = result.GetValue("return_msg").ToString();
                models.ReturnMsg = outTradeNo;
                
            }
            return Json(models);
        }

        /// <summary>
        /// 将支付历史表中营收状态为未匹配的重新插入到营收表(ThirdPartyPublicPlatformPayment)中
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public JsonResult Insert2Revenue(List<Guid> vguidList)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = _weChatRevenueLogic.Insert2Revenue(vguidList);
            models.respnseInfo = models.isSuccess ? "1" : "0";
            return Json(models);
        }

    }
}
