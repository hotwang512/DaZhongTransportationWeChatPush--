using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.TableEntity.DriverRevenueTable;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DaZhongManagementSystem.Infrastructure.SystemManagement;
using DaZhongManagementSystem.Entities.TableEntity.RevenueAPIModel;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.ShortMsgLogic
{
    public class ShortMsgLogic
    {
        public ShortMsgServer _ss;
        private ConfigManageServer _cs;
        public ShortMsgLogic()
        {
            _ss = new ShortMsgServer();
            _cs = new ConfigManageServer();
        }

        /// <summary>
        /// 获取人员姓名以及工号
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Business_Personnel_Information GetUserNameAndJobNum(string userID)
        {
            return _ss.GetUserNameAndJobNum(userID);
        }

        /// <summary>
        ///  获取司机ID和车辆ID
        /// </summary>
        /// <param name="personModel"></param>
        /// <returns></returns>
        public Driver GetDriverMsg(Business_Personnel_Information personModel)
        {
            return _ss.GetDriverMsg(personModel);
        }

        /// <summary>
        /// 获取营收信息
        /// </summary>
        /// <param name="driverModel"></param>
        /// <returns></returns>
        public PaymentMonthly GetRevenueMsg(Driver driverModel)
        {
            return _ss.GetRevenueMsg(driverModel);
        }

        /// <summary>
        /// 保存要发送的营收信息
        /// </summary>
        /// <param name="paymentModel"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool SaveRevenueMsg(API_PaymentMonthly paymentModel, string userID)
        {
            bool isZero = false;
            bool result = false;
            U_Revenue revenue = new U_Revenue();
            var revenueQueryExceptionReply = _cs.GetConfigList()[12].ConfigValue;
            if (paymentModel == null)
            {
                revenue.HistoricalArrears = "0";
                revenue.TheBalance = "0";
                revenue.AmountDue = "0";
                revenue.CurrentPayment = "0";
                revenue.CurrentAccountBalance = "0";
                isZero = true;
            }
            else if (paymentModel.DebtAmount == 0 && paymentModel.DueAmount == 0 && paymentModel.PaidAmount == 0)
            {
                isZero = true;
            }
            else
            {
                //if (paymentModel.DebtAmount > 0)//正值（欠款）
                //{

                revenue.TheBalance = (paymentModel.DebtAmount * -1).ToString("F2");
                revenue.HistoricalArrears = (paymentModel.DebtAmount * -1) >= 0 ? "0" : paymentModel.DebtAmount.ToString("F2");
                //}
                //else//负值(结余）
                //{
                //    revenue.HistoricalArrears = (-paymentModel.DebtAmount).ToString("##.##");
                //    revenue.TheBalance = paymentModel.DebtAmount.ToString("##.##");
                //}
                revenue.AmountDue = paymentModel.DueAmount.ToString("F2");
                revenue.CurrentPayment = paymentModel.PaidAmount.ToString("F2");
                revenue.CurrentAccountBalance = (paymentModel.PaidAmount - paymentModel.DueAmount + (paymentModel.DebtAmount * -1)).ToString("F2");
            }
            
            string isSmsPush = _ss.GetIsSmsPush();
            string isWeChatPush = _ss.GetIsWeChatPush();
            string isRevenuePush = _ss.GetIsRevenuePush();

            string weChatRevenueTemp = ConfigSugar.GetAppString("WeChatRevenueTemp");//获取微信发送营收数据模板

            //{"HistoricalArrears":"0","TheBalance":"0","AmountDue":"0","CurrentPayment":"0","CurrentAccountBalance":"0"}
            //只通过短信发送营收信息
            if (isSmsPush == "1")
            {
                U_RevenueMonth revenueMonth = new U_RevenueMonth();
                revenueMonth.AmountDue = revenue.AmountDue;
                revenueMonth.CurrentAccountBalance = revenue.CurrentAccountBalance;
                revenueMonth.CurrentPayment = revenue.CurrentPayment;
                revenueMonth.HistoricalArrears = revenue.HistoricalArrears;
                revenueMonth.TheBalance = revenue.TheBalance;
                revenueMonth.Month = DateTime.Now.Month.ToString();
                Business_WeChatPush_Information weChatPush = new Business_WeChatPush_Information();
                weChatPush.PushType = 2;
                weChatPush.Title = "营收短信";
                weChatPush.MessageType = 8;
                weChatPush.Timed = false;
                weChatPush.TimedSendTime = null;
                weChatPush.Important = false;
                weChatPush.Message = Common.JsonHelper.ModelToJson<U_RevenueMonth>(revenueMonth);
                weChatPush.PeriodOfValidity = null;
                weChatPush.PushDate = DateTime.Now;
                weChatPush.Status = 3;
                weChatPush.CreatedDate = DateTime.Now;
                weChatPush.ChangeDate = DateTime.Now;
                weChatPush.VGUID = Guid.NewGuid();

                //推送接收者
                Business_WeChatPushDetail_Information weChatPushDetail = new Business_WeChatPushDetail_Information();
                weChatPushDetail.PushObject = userID;
                weChatPushDetail.Vguid = Guid.NewGuid();
                weChatPushDetail.CreatedDate = DateTime.Now;
                weChatPushDetail.ChangeDate = DateTime.Now;
                weChatPushDetail.Type = "短信营收";
                weChatPushDetail.Business_WeChatPushVguid = weChatPush.VGUID;
                if (!isZero)
                {
                    result = _ss.SaveRevenueMsg(weChatPush, weChatPushDetail);
                }
            }
            if (isWeChatPush == "1")//只通过微信发送营收信息
            {
                Business_WeChatPush_Information weChatPush = new Business_WeChatPush_Information();
                weChatPush.PushType = 1;
                weChatPush.Title = "营收微信";
                weChatPush.MessageType = 1;
                weChatPush.Timed = false;
                weChatPush.TimedSendTime = null;
                weChatPush.Important = false;
                weChatPush.Message = isZero ? revenueQueryExceptionReply : string.Format(weChatRevenueTemp, revenue.HistoricalArrears, revenue.TheBalance, revenue.AmountDue, revenue.CurrentPayment, revenue.CurrentAccountBalance, DateTime.Now.Month);
                weChatPush.PeriodOfValidity = null;
                weChatPush.PushDate = null;
                weChatPush.Status = 3;
                weChatPush.CreatedDate = DateTime.Now;
                weChatPush.ChangeDate = DateTime.Now;
                weChatPush.VGUID = Guid.NewGuid();

                //推送接收者
                Business_WeChatPushDetail_Information weChatPushDetail = new Business_WeChatPushDetail_Information();
                weChatPushDetail.PushObject = userID;
                weChatPushDetail.Vguid = Guid.NewGuid();
                weChatPushDetail.CreatedDate = DateTime.Now;
                weChatPushDetail.ChangeDate = DateTime.Now;
                weChatPushDetail.Type = "微信营收";
                weChatPushDetail.Business_WeChatPushVguid = weChatPush.VGUID;
                result = _ss.SaveRevenueMsg(weChatPush, weChatPushDetail);
            }
            if (isRevenuePush == "1")//只通过营收发送营收信息
            {
                Business_WeChatPush_Information weChatPush = new Business_WeChatPush_Information();
                weChatPush.PushType = 1;
                weChatPush.Title = "营收信息";
                weChatPush.MessageType = 11;
                weChatPush.Timed = false;
                weChatPush.TimedSendTime = null;
                weChatPush.RevenueType = 2;
                DateTime now = DateTime.Now;//当前时间
                DateTime d1 = new DateTime(now.Year, now.Month, 1);//当前月的第一天
                DateTime d2 = d1.AddMonths(1).AddDays(-1);//当前月的最后一天
                weChatPush.PeriodOfValidity = d2;
                weChatPush.CoverDescption = "个人营收账单明细";

                weChatPush.CoverImg = "/_img/微信图片.jpg";

                weChatPush.PushDate = DateTime.Now;
                weChatPush.Status = 3;

                weChatPush.CreatedDate = DateTime.Now;
                weChatPush.ChangeDate = DateTime.Now;
                weChatPush.VGUID = Guid.NewGuid();

                //推送接收者
                Business_WeChatPushDetail_Information weChatPushDetail = new Business_WeChatPushDetail_Information();
                weChatPushDetail.PushObject = userID;
                weChatPushDetail.Vguid = Guid.NewGuid();
                weChatPushDetail.CreatedDate = DateTime.Now;
                weChatPushDetail.ChangeDate = DateTime.Now;
                weChatPushDetail.Type = "营收发送";
                weChatPushDetail.Business_WeChatPushVguid = weChatPush.VGUID;
                result = _ss.SaveRevenueMsg(weChatPush, weChatPushDetail);
            }

            //else if (isWeChatPush == "1" && isSmsPush == "1")
            //{
            //    Business_WeChatPush_Information weChatPush = new Business_WeChatPush_Information();
            //    weChatPush.PushType = 1;
            //    weChatPush.Title = "营收微信";
            //    weChatPush.MessageType = 1;
            //    weChatPush.Timed = false;
            //    weChatPush.TimedSendTime = null;
            //    weChatPush.Important = false;
            //    weChatPush.Message = isZero ? revenueQueryExceptionReply : string.Format(weChatRevenueTemp, revenue.HistoricalArrears, revenue.TheBalance, revenue.AmountDue, revenue.CurrentPayment, revenue.CurrentAccountBalance, DateTime.Now.Month);
            //    weChatPush.PeriodOfValidity = null;
            //    weChatPush.PushDate = null;
            //    weChatPush.Status = 3;
            //    weChatPush.CreatedDate = DateTime.Now;
            //    weChatPush.ChangeDate = DateTime.Now;
            //    weChatPush.VGUID = Guid.NewGuid();

            //    //推送接收者
            //    Business_WeChatPushDetail_Information weChatPushDetail = new Business_WeChatPushDetail_Information();
            //    weChatPushDetail.PushObject = userID;
            //    weChatPushDetail.Vguid = Guid.NewGuid();
            //    weChatPushDetail.CreatedDate = DateTime.Now;
            //    weChatPushDetail.ChangeDate = DateTime.Now;
            //    weChatPushDetail.Type = "微信营收";
            //    weChatPushDetail.Business_WeChatPushVguid = weChatPush.VGUID;
            //    result = _ss.SaveRevenueMsg(weChatPush, weChatPushDetail);
            //    if (result)
            //    {
            //        U_RevenueMonth revenueMonth = new U_RevenueMonth();
            //        revenueMonth.AmountDue = revenue.AmountDue;
            //        revenueMonth.CurrentAccountBalance = revenue.CurrentAccountBalance;
            //        revenueMonth.CurrentPayment = revenue.CurrentPayment;
            //        revenueMonth.HistoricalArrears = revenue.HistoricalArrears;
            //        revenueMonth.TheBalance = revenue.TheBalance;
            //        revenueMonth.Month = DateTime.Now.Month.ToString();
            //        Business_WeChatPush_Information weChatPush1 = new Business_WeChatPush_Information();
            //        weChatPush1.PushType = 2;
            //        weChatPush1.Title = "营收短信";
            //        weChatPush1.MessageType = 8;
            //        weChatPush1.Timed = false;
            //        weChatPush1.TimedSendTime = null;
            //        weChatPush1.Important = false;
            //        weChatPush1.Message = Common.JsonHelper.ModelToJson(revenueMonth);
            //        weChatPush1.PeriodOfValidity = null;
            //        weChatPush1.PushDate = DateTime.Now;
            //        weChatPush1.Status = 3;
            //        weChatPush1.CreatedDate = DateTime.Now;
            //        weChatPush1.ChangeDate = DateTime.Now;
            //        weChatPush1.VGUID = Guid.NewGuid();

            //        //推送接收者
            //        Business_WeChatPushDetail_Information weChatPushDetail1 = new Business_WeChatPushDetail_Information();
            //        weChatPushDetail1.PushObject = userID;
            //        weChatPushDetail1.Vguid = Guid.NewGuid();
            //        weChatPushDetail1.CreatedDate = DateTime.Now;
            //        weChatPushDetail1.ChangeDate = DateTime.Now;
            //        weChatPushDetail1.Type = "短信营收";
            //        weChatPushDetail1.Business_WeChatPushVguid = weChatPush1.VGUID;
            //        if (!isZero)
            //        {
            //            result = _ss.SaveRevenueMsg(weChatPush1, weChatPushDetail1);
            //        }

            //    }
            //}
            if (isWeChatPush == "0" && isSmsPush == "0" && isRevenuePush == "0")
            {
                var queryReply = _cs.GetConfigList()[15].ConfigValue;
                Business_WeChatPush_Information weChatPush = new Business_WeChatPush_Information();
                weChatPush.PushType = 1;
                weChatPush.Title = "营收微信";
                weChatPush.MessageType = 1;
                weChatPush.Timed = false;
                weChatPush.TimedSendTime = null;
                weChatPush.Important = false;
                weChatPush.Message = queryReply;
                weChatPush.PeriodOfValidity = null;
                weChatPush.PushDate = null;
                weChatPush.Status = 3;
                weChatPush.CreatedDate = DateTime.Now;
                weChatPush.ChangeDate = DateTime.Now;
                weChatPush.VGUID = Guid.NewGuid();

                //推送接收者
                Business_WeChatPushDetail_Information weChatPushDetail = new Business_WeChatPushDetail_Information();
                weChatPushDetail.PushObject = userID;
                weChatPushDetail.Vguid = Guid.NewGuid();
                weChatPushDetail.CreatedDate = DateTime.Now;
                weChatPushDetail.ChangeDate = DateTime.Now;
                weChatPushDetail.Type = "微信营收";
                weChatPushDetail.Business_WeChatPushVguid = weChatPush.VGUID;
                result = _ss.SaveRevenueMsg(weChatPush, weChatPushDetail);
            }
            return result;
        }
    }
}