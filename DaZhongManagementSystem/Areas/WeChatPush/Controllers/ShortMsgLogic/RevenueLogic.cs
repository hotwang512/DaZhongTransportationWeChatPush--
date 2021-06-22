using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.TableEntity.DriverRevenueTable;
using DaZhongManagementSystem.Entities.TableEntity.RevenueAPIModel;
using DaZhongManagementSystem.Infrastructure;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.ShortMsgLogic
{
    public class RevenueLogic
    {
        public ShortMsgLogic _sl;
        public RevenueServer _revenueServer;
        public RevenueLogic()
        {
            _sl = new ShortMsgLogic();
            _revenueServer = new RevenueServer();
        }

        /// <summary>
        /// 获取成功查询营收数据回复
        /// </summary>
        /// <returns></returns>
        public string GetRevenueQueryReply()
        {
            return _revenueServer.GetRevenueQueryReply();
        }

        /// <summary>
        /// 获取不是司机查询营收数据回复
        /// </summary>
        /// <returns></returns>
        public string GetRevenueQueryRefuse()
        {
            return _revenueServer.GetRevenueQueryRefuse();
        }

        /// <summary>
        /// 获取查询营收数据超过次数回复
        /// </summary>
        /// <returns></returns>
        public string GetRevenueQueryTimesRefuse()
        {
            return _revenueServer.GetRevenueQueryTimesRefuse();
        }

        /// <summary>
        /// 获取营收信息每月查询次数
        /// </summary>
        /// <returns></returns>
        public string GetRevenueSearchTimes()
        {
            return _revenueServer.GetRevenueSearchTimes();
        }


        /// <summary>
        /// 获取当前用户当月查询营收次数
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public int GetUserCurrentMonthQueryTimes(string userID)
        {
            return _revenueServer.GetUserCurrentMonthQueryTimes(userID);
        }

        #region 短信
        /// <summary>
        /// 保存要发送的营收信息
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public int SaveRevenueMsg(string userID)
        {
            bool result = false;
            int response = 0;
            //获取人员姓名和工号
            Business_Personnel_Information personModel = new Business_Personnel_Information();
            personModel = GetUserNameAndJobNum(userID);
            if (personModel.DepartmenManager == 1)
            {
                //获取司机ID和车辆ID
                Driver driverModel = new Driver();
                driverModel = GetDriverMsg(personModel);
                //获取营收信息
                API_PaymentMonthly paymentModel = new API_PaymentMonthly();
                paymentModel = GetRevenueAPI_PaymentMonthlyData(driverModel);
                if (paymentModel == null)
                {
                    response = 0;//数据异常请联系管理员
                }
                else if (paymentModel.DebtAmount == 0 && paymentModel.DueAmount == 0 && paymentModel.PaidAmount == 0)
                {
                    response = 0;//数据异常请联系管理员
                }
                result = _sl.SaveRevenueMsg(paymentModel, userID);
                if (result)
                {
                    response = 1;//营收数据已经通过短信发送到您的手机，请及时查收
                }
            }
            else
            {
                response = 3;//您不是司机，无法查询营收数据
            }

            return response;
        }
        /// <summary>
        /// 判断营收信息是否是0
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool isRevenueZero(string userID)
        {
            bool result = false;
            //获取人员姓名和工号
            Business_Personnel_Information personModel = new Business_Personnel_Information();
            personModel = GetUserNameAndJobNum(userID);
            if (personModel.DepartmenManager == 1)
            {
                //获取司机ID和车辆ID
                Driver driverModel = new Driver();
                driverModel = GetDriverMsg(personModel);
                //获取营收信息
                API_PaymentMonthly paymentModel = new API_PaymentMonthly();
                paymentModel = GetRevenueAPI_PaymentMonthlyData(driverModel);

                if (paymentModel == null)
                {
                    result = true;
                }
                else if (paymentModel.DebtAmount == 0 && paymentModel.DueAmount == 0 && paymentModel.PaidAmount == 0)
                {
                    result = true;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取人员姓名以及工号
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Business_Personnel_Information GetUserNameAndJobNum(string userID)
        {
            Business_Personnel_Information personModel = new Business_Personnel_Information();
            personModel = _sl.GetUserNameAndJobNum(userID);
            return personModel;
        }

        /// <summary>
        /// 获取司机ID和车辆ID
        /// </summary>
        /// <param name="personModel"></param>
        /// <returns></returns>
        public Driver GetDriverMsg(Business_Personnel_Information personModel)
        {
            Driver driverModel = new Driver();
            driverModel = _sl.GetDriverMsg(personModel);
            return driverModel;
        }

        /// <summary>
        /// 获取营收信息
        /// </summary>
        /// <param name="driverModel"></param>
        /// <returns></returns>
        public PaymentMonthly GetRevenueMsg(Driver driverModel)
        {
            PaymentMonthly paymentModel = new PaymentMonthly();
            paymentModel = _sl.GetRevenueMsg(driverModel);
            return paymentModel;
        }

        /// <summary>
        /// 获取营收信息
        /// </summary>
        /// <param name="driverModel"></param>
        /// <returns></returns>
        public API_PaymentMonthly GetRevenueAPI_PaymentMonthlyData(Driver driverModel)
        {
            API_PaymentMonthly payment = new API_PaymentMonthly();
            try
            {
                WebClient wc = new WebClient();
                //测试数据
                //driverModel.OrganizationID = 17904;
                //driverModel.Id = 55;
                //driverModel.EmployeeNo = "993594";
                var data = "{" +
                            "\"OrganizationId\":\"{OrganizationId}\",".Replace("{OrganizationId}", driverModel.OrganizationID.ToString()) +
                            "\"DriverID\":\"{DriverID}\",".Replace("{DriverID}", driverModel.Id.ToString()) +
                            "\"EmployeeNo\":\"{EmployeeNo}\"".Replace("{EmployeeNo}", driverModel.EmployeeNo.ToString()) +
                            "}";
                wc.Headers.Clear();
                wc.Headers.Add("Content-Type", "application/json;charset=utf-8");
                wc.Encoding = System.Text.Encoding.UTF8;
                var result = wc.UploadString(new Uri(ConfigSugar.GetAppString("RevenueSytemPaymentMonthlyPath")), data);
                result = result.Replace("[]", "{}");
                API_Result apiResult = result.JsonToModel<API_Result>();
                if (apiResult.success)
                {
                    payment = apiResult.data;
                }
                Common.LogHelper.LogHelper.WriteLog(string.Format("{0}-{1}:{2}", driverModel.Name, driverModel.Id, result));
            }
            catch (Exception ex)
            {
                Common.LogHelper.LogHelper.WriteLog(string.Format("{0}-{1}:{2}", driverModel.Name, driverModel.Id, ex.ToString()));
            }
            return payment;
        }
        #endregion


        #region 微信

        #endregion
    }
}