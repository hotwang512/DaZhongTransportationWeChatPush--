using System;
using System.Collections.Generic;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.ShortMsgLogic;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Common.WeChatPush;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.WeChatRevenue;
using System.Net;
using SyntacticSugar;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.WeChatRevenue.BusinessLogic
{
    public class WeChatRevenueLogic
    {
        private WeChatRevenueServer _weChatRevenueServer;
        public ShortMsgLogic.ShortMsgLogic _sl;
        private RevenueLogic _rl;
        public WeChatRevenueLogic()
        {
            _rl = new RevenueLogic();
            _sl = new ShortMsgLogic.ShortMsgLogic();
            _weChatRevenueServer = new WeChatRevenueServer();
        }


        /// <summary>
        /// 根据人员信息获取营收信息
        /// </summary>
        /// <param name="personModel"></param>
        /// <returns></returns>
        public U_Revenue GetRevenueMsgByPersonInfo(Business_Personnel_Information personModel)
        {
            //获取司机ID和车辆ID
            var driverModel = _rl.GetDriverMsg(personModel);
            //获取营收信息
            var paymentModel = _rl.GetRevenueAPI_PaymentMonthlyData(driverModel);
            U_Revenue revenue = new U_Revenue();
            if (paymentModel == null)
            {
                revenue.HistoricalArrears = "0";
                revenue.TheBalance = "0";
                revenue.AmountDue = "0";
                revenue.CurrentPayment = "0";
                revenue.CurrentAccountBalance = "0";
            }
            else
            {
                //revenue.TheBalance = (paymentModel.DebtAmount * -1).ToString("F2");     //上期结余
                //revenue.HistoricalArrears = (paymentModel.DebtAmount * -1) >= 0 ? "0.00" : paymentModel.DebtAmount.ToString("F2"); //历史欠款
                //revenue.AmountDue = paymentModel.DueAmount.ToString("F2");  //应缴金额
                //revenue.CurrentPayment = paymentModel.PaidAmount.ToString("F2");  //本期缴款
                //revenue.CurrentAccountBalance = (paymentModel.PaidAmount - paymentModel.DueAmount + (paymentModel.DebtAmount * -1)).ToString("F2"); //本期结余

                revenue.HistoricalArrears = "0";
                revenue.TheBalance = paymentModel.DebtAmount.ToString("F2"); //历史欠款
                //revenue.TheBalance = paymentModel.DebtAmount.ToString("F2");
                revenue.AmountDue = paymentModel.DueAmount.ToString("F2");  //应缴金额
                revenue.CurrentPayment = paymentModel.PaidAmount.ToString("F2");  //本期缴款
                revenue.CurrentAccountBalance = (paymentModel.PayDebtAmount * -1).ToString("F2");  //本期欠款
                revenue.Fee = paymentModel.fee;
                revenue.FeeMoney = paymentModel.feeMoney.ToString("F2");
                revenue.TotalAmount = (paymentModel.totalAmount * -1).ToString("F2");
                revenue.Fee_CurrentAccountBalance = paymentModel.PayDebtAmount >= 0 ? paymentModel.PayDebtAmount.ToString("f2") : (paymentModel.PayDebtAmount * -1).ToString("F2");  //本期欠款
                revenue.Fee_TotalAmount = paymentModel.totalAmount >= 0 ? paymentModel.totalAmount.ToString("F2") : (paymentModel.totalAmount * -1).ToString("F2");
            }
            return revenue;
        }

        /// <summary>
        /// 新增支付历史
        /// </summary>
        /// <param name="paymentHistoryInfo">实体信息</param>
        /// <returns>返回成功与否</returns>
        public bool AddPaymentHistory(Business_PaymentHistory_Information paymentHistoryInfo)
        {
            return _weChatRevenueServer.AddPaymentHistory(paymentHistoryInfo);
        }
        /// <summary>
        /// 新增支付历史
        /// </summary>
        /// <param name="paymentHistoryInfo">实体信息</param>
        /// <returns>返回成功与否</returns>
        public bool UpdatePaymentHistory(Business_Personnel_Information personInfoModel, Business_PaymentHistory_Information paymentHistoryInfo, WxPayData notifyData)
        {
            paymentHistoryInfo.PaymentBrokers = "微信";
            paymentHistoryInfo.Beneficiary = "大众出租汽车分公司";
            paymentHistoryInfo.Description = WxPayConfig.BODY;
            paymentHistoryInfo.Remarks = notifyData.GetValue("out_trade_no").ToString();   //商户单号
            var paymentHistoryInfoOld = _weChatRevenueServer.GetPaymentHistory(paymentHistoryInfo.Remarks);
            if (notifyData.GetValue("return_code").ToString() == "SUCCESS" && notifyData.GetValue("result_code").ToString() == "SUCCESS")
            {
                paymentHistoryInfo.ReceiptAccount = notifyData.GetValue("mch_id").ToString();  //商户号
                paymentHistoryInfo.TransactionID = notifyData.GetValue("transaction_id").ToString();     //微信支付订单号

                paymentHistoryInfo.ActualAmount = decimal.Parse(notifyData.GetValue("total_fee").ToString()) / 100;  //实际付款
                string timeEnd = notifyData.GetValue("time_end").ToString();
                string year = timeEnd.Substring(0, 4);
                string month = timeEnd.Substring(4, 2);
                string day = timeEnd.Substring(6, 2);
                string hour = timeEnd.Substring(8, 2);
                string minute = timeEnd.Substring(10, 2);
                string second = timeEnd.Substring(12, 2);
                paymentHistoryInfo.PayDate = DateTime.Parse(year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second);  //付款时间
                paymentHistoryInfo.PaymentStatus = "1";  //支付成功

                SynchronousReceivableSystem(personInfoModel, paymentHistoryInfo, paymentHistoryInfoOld);

            }
            else
            {
                paymentHistoryInfo.TransactionID = notifyData.GetValue("out_trade_no").ToString();   //商户单号
                paymentHistoryInfo.PaymentAmount = 0;
                paymentHistoryInfo.PayDate = DateTime.Now;
                paymentHistoryInfo.PaymentStatus = "2";  //支付失败
                paymentHistoryInfo.ErrorCode = notifyData.GetValue("result_code").ToString();
                paymentHistoryInfo.ErrorDescription = notifyData.GetValue("return_msg").ToString();
            }

            return _weChatRevenueServer.UpdatePaymentHistory(personInfoModel, paymentHistoryInfo, paymentHistoryInfoOld);
        }

        public void SynchronousReceivableSystem(Business_Personnel_Information personModel, Business_PaymentHistory_Information paymentHistoryInfo, Business_PaymentHistory_Information paymentHistoryInfoOld)
        {
            //获取司机ID和车辆ID
            try
            {
                var driverModel = _rl.GetDriverMsg(personModel);

                WebClient wc = new WebClient();
                var data = "{" +
                            "\"BillLable\":\"WX\"," +
                            "\"DriverID\":\"{DriverID}\",".Replace("{DriverID}", driverModel.Id.ToString()) +
                            "\"JobNumber\":\"{JobNumber}\",".Replace("{JobNumber}", driverModel.EmployeeNo) +
                            "\"Name\":\"{Name}\",".Replace("{Name}", personModel.Name == null ? "" : personModel.Name) +
                            "\"OriginalId\":\"{OriginalId}\",".Replace("{OriginalId}", driverModel.OriginalId.ToString()) +
                            "\"OrganizationID\":\"{OrganizationID}\",".Replace("{OrganizationID}", driverModel.OrganizationID.ToString()) +
                            "\"PayDate\":\"{PayDate}\",".Replace("{PayDate}", paymentHistoryInfo.PayDate.ToString("yyyy-MM-dd HH:mm:ss")) +
                            "\"TransactionId\":\"{TransactionId}\",".Replace("{TransactionId}", paymentHistoryInfo.TransactionID) +
                            "\"PaymentBrokers\":\"WeiChat\"," +
                            "\"Channel_Id\":\"1465779302\"," +
                            "\"SubjectId\":\"1465779302_{0}\",".Replace("{0}", driverModel.OrganizationID.ToString()) +
                            "\"PaymentAmount\":{PaymentAmount}, ".Replace("{PaymentAmount}", paymentHistoryInfoOld.RevenueReceivable.ToString()) +
                            "\"CopeFee\":{CopeFee},".Replace("{CopeFee}", (paymentHistoryInfo.ActualAmount * paymentHistoryInfoOld.RevenueReceivable).ToString()) +
                            "\"ActualAmount\":{ActualAmount},".Replace("{ActualAmount}", paymentHistoryInfo.ActualAmount.ToString()) +
                            "\"ReceiptCategory\":11," +
                            "\"Remark\":\"WeiChat\"," +
                            "\"CompanyAccount\":0" +
                            "}";
                wc.Headers.Clear();
                wc.Headers.Add("Content-Type", "application/json;charset=utf-8");
                wc.Encoding = System.Text.Encoding.UTF8;
                var result = wc.UploadString(new Uri(ConfigSugar.GetAppString("RevenueSytemSyncPath")), data);
                Common.LogHelper.LogHelper.WriteLog(string.Format("{0}-{1}:{2}", personModel.Name, paymentHistoryInfo.TransactionID, result));
            }
            catch (Exception ex)
            {
                Common.LogHelper.LogHelper.WriteLog(string.Format("{0}-{1}:{2}", personModel.Name, paymentHistoryInfo.TransactionID, ex.ToString()));
            }
        }


        /// <summary>
        /// 获取所有的支付历史
        /// </summary>
        /// <param name="searchParas"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<v_PaymentHistory_Information> GetAllPaymentHistoryInfo(U_PaymentHistory_Search searchParas, GridParams para)
        {
            return _weChatRevenueServer.GetAllPaymentHistoryInfo(searchParas, para);
        }


        /// <summary>
        /// 删除付款历史
        /// </summary>
        /// <param name="vguidList">主键</param>
        /// <returns></returns>
        public bool DeletePaymentHistory(List<Guid> vguidList)
        {
            int flag = 0;
            foreach (var guid in vguidList)
            {
                bool result = _weChatRevenueServer.DeletePaymentHistory(guid);
                flag = result ? flag + 1 : 0;
            }
            return flag == vguidList.Count;
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="searchParas">导出条件</param>
        public void Export(string searchParas)
        {
            var model = JsonHelper.JsonToModel<U_PaymentHistory_Search>(searchParas);
            _weChatRevenueServer.Export(model);
        }

        /// <summary>
        /// 根据guid获取人员相关信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public Business_Personnel_Information GetPersonInfo(Guid vguid)
        {
            return _weChatRevenueServer.GetPersonInfo(vguid);
        }

        /// <summary>
        /// 获取司机的服务号、车牌号以及公司代码
        /// </summary>
        /// <param name="personInfo"></param>
        /// <returns></returns>
        public U_DriverInfo GetDriverInfo(Business_Personnel_Information personInfo)
        {
            return _weChatRevenueServer.GetDriverInfo(personInfo);
        }



        /// <summary>
        /// 查询当前人员是否已经支付过
        /// </summary>
        /// <param name="personVguid"></param>
        /// <param name="pushCongtentVguid"></param>
        /// <param name="revenueType"></param>
        /// <returns></returns>
        public bool HasPaymentHistory(Guid personVguid, Guid pushCongtentVguid, int revenueType)
        {
            return _weChatRevenueServer.HasPaymentHistory(personVguid, pushCongtentVguid, revenueType);
        }

        /// <summary>
        /// 退款成功后，修改付款状态
        /// </summary>
        /// <param name="transaction_id"></param>
        /// <returns></returns>
        public bool UpdateStatus(string transaction_id)
        {
            return _weChatRevenueServer.UpdateStatus(transaction_id);
        }

        /// <summary>
        /// 判断推送消息是否过期
        /// </summary>
        /// <param name="pushContentVguid"></param>
        /// <returns></returns>
        public bool IsValid(Guid pushContentVguid)
        {
            return _weChatRevenueServer.IsValid(pushContentVguid);
        }

        /// <summary>
        /// 将支付历史表中营收状态为未匹配的重新插入到营收表(ThirdPartyPublicPlatformPayment)中
        /// </summary>
        /// <returns></returns>
        public bool Insert2Revenue(List<Guid> vguidList)
        {
            int flag = 0;
            foreach (var guid in vguidList)
            {
                var isSuccess = _weChatRevenueServer.Insert2Revenue(guid);
                if (!isSuccess)
                {
                    flag++;
                }
            }
            return flag == 0;
        }




    }
}