using System;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.WeChatExercise.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.ConfigManagement.BusinessLogic;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.PushDetailShow.BusinessLogic;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.WeChatRevenue.BusinessLogic;
using DaZhongManagementSystem.Common.WeChatPush;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using JQWidgetsSugar;
using DaZhongManagementSystem.Common.LogHelper;
using SqlSugar;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using DaZhongManagementSystem.Areas.WeChatPush.Models;
using System.Linq;
using System.Net;
using SyntacticSugar;
using DaZhongManagementSystem.Infrastructure.WeChatRevenue;
using DaZhongTransitionLiquidation.Common.Pub;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.ShortMsgLogic;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.WeChatRevenue
{
    public class WeChatRevenueController : Controller
    {
        //
        // GET: /WeChatPush/WeChatRevenue/

        private PushDetailLogic _pl;
        private WeChatRevenueLogic _weChatRevenueLogic;
        private WeChatExerciseLogic _wl;
        private ConfigManagementLogic _configManagementLogic;
        private RevenueLogic _rl;
        public WeChatRevenueController()
        {
            _rl = new RevenueLogic();
            _wl = new WeChatExerciseLogic();
            _pl = new PushDetailLogic();
            _weChatRevenueLogic = new WeChatRevenueLogic();
            _configManagementLogic = new ConfigManagementLogic();
        }
        /// <summary>
        /// 营收支付手机界面
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult RevenuePay(string code)
        {
            #region 获取人员表信息

            string accessToken = WeChatTools.GetAccessoken();
            string userInfoStr = WeChatTools.GetUserInfoByCode(accessToken, code);
            var userInfo = Common.JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr); //用户ID
            //userInfo.UserId = "18936495119";
            var personInfoModel = _wl.GetUserInfo(userInfo.UserId); //获取人员表信息 
            ViewData["vguid"] = personInfoModel.Vguid;

            var driverInfo = _rl.GetDriverMsg(personInfoModel);
            //var driverInfo = getDriverInfo(personInfoModel);
            //ViewData["driverId"] = "21033";
            //ViewData["organizationId"] = "55";
            ViewData["driverId"] = driverInfo.Id;
            ViewData["organizationId"] = driverInfo.OrganizationID;

            //Business_Personnel_Information personInfoModel = new Business_Personnel_Information();
            //personInfoModel.Vguid = Guid.Parse("B0167926-C8AF-4AAE-9B18-573EEEDFE740");
            //ViewData["vguid"] = personInfoModel.Vguid;
            #endregion

            #region 查询车牌号是否为空
            ViewData["payException"] = "0";
            //var driverInfo = _weChatRevenueLogic.GetDriverInfo(personInfoModel);
            //if (driverInfo == null)
            //{
            //    ViewData["payException"] = "1";
            //}
            #endregion

            #region 获取openid

            var openInfoStr = WeChatTools.ConvertToOpenidByUserId(accessToken, userInfo.UserId);
            var openInfo = Common.JsonHelper.JsonToModel<U_OpenInfo>(openInfoStr);
            var openidExt = openInfo.openid;
            ViewData["openid"] = openidExt;

            #endregion

            //var configList = _configManagementLogic.GetConfigList();
            //var fee = configList[13].ConfigValue;
            //ViewData["driverPay"] = fee;
            //var driverPayfee = double.Parse(fee.Trim('%')) / 100;            //获取司机支付的手续费

            string pushContentVguid = Request.QueryString["Vguid"]; //推送的主键
            ViewData["pushContentVguid"] = pushContentVguid;
            var pushContentModel = _pl.GetPushDetail(pushContentVguid);
            bool isValidTime = false; //未过有效期

            if (pushContentModel != null)
            {
                //判断是否已经支付过
                bool isExist = _weChatRevenueLogic.HasPaymentHistory(personInfoModel.Vguid, pushContentModel.VGUID, 2);
                ViewData["isExist"] = isExist ? "1" : "0";
                #region 判断是否过了有效期

                if (pushContentModel.PeriodOfValidity != null)
                {
                    if (DateTime.Now > pushContentModel.PeriodOfValidity)
                    {
                        isValidTime = true; //已过有效期
                    }
                }

                #endregion

                #region 判断是否是司机

                //去查询营收
                //if (personInfoModel.DepartmenManager == 1) //说明是司机
                //{
                //    ViewData["IsDriver"] = "1";
                //}

                #endregion

                #region 查询营收金额

                if (pushContentModel.RevenueType == 2) //营收金额
                {
                    //去查询营收
                    var revenue = _weChatRevenueLogic.GetRevenueMsgByPersonInfo(personInfoModel);
                    //if (revenue.CurrentAccountBalance == "0" || revenue.CurrentAccountBalance == "0.00")
                    //{
                    //    revenue.CurrentAccountBalance = "-10000.123984";
                    //}
                    //decimal caBalance = Convert.ToDecimal(revenue.CurrentAccountBalance);
                    //var currentAccountBalance = FormatData(caBalance);
                    //revenue.CurrentAccountBalance = FormatData(caBalance).ToString();
                    ViewBag.Revenue = revenue;

                    //if (caBalance < 0)
                    //{
                    //    caBalance = caBalance * -1;
                    //}

                    //ViewBag.CurrentAccountBalance = caBalance.ToString("F2");
                    //var handlingFee = Convert.ToDecimal(revenue.FeeMoney);

                    //handlingFee = FormatData(handlingFee);
                    //if (handlingFee < 0)
                    //{
                    //    handlingFee = handlingFee * -1;
                    //}
                    //ViewBag.HandlingFee = handlingFee;

                    //ViewBag.TotalAmount = currentAccountBalance + handlingFee;
                }

                #endregion
            }
            ViewBag.isValidTime = isValidTime;
            ViewData["PushContentModel"] = pushContentModel;
            return View();
        }

        public DriverInfo getDriverInfo(Business_Personnel_Information personInfoModel)
        {
            DriverInfo pi = new DriverInfo();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                pi = _dbMsSql.SqlQuery<DriverInfo>(@"select DriverId,OrganizationId from [DZ_DW].[dbo].[Visionet_DriverInfo_View] where IdCard=@IDNumber
                                        and status='1'"
                                        , new { IDNumber = personInfoModel.IDNumber }).ToList().FirstOrDefault();
            }
            return pi;
        }
        private decimal FormatData(decimal amount)
        {
            bool negativeNumber = false;
            decimal result = amount * 100;
            var temp = result;
            if (temp < 0)
            {
                temp = temp * -1;
                negativeNumber = true;
            }
            if (temp > 1 && (temp - (int)temp) > 0)
            {
                temp++;
            }
            var foramtResult = (int)temp / 100M;

            if (negativeNumber)
            {
                foramtResult = foramtResult * -1;
            }
            return foramtResult;
        }

        /// <summary>
        /// 固定金额支付手机界面
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult FixedPay(string code)
        {
            string accessToken = WeChatTools.GetAccessoken();
            string userInfoStr = WeChatTools.GetUserInfoByCode(accessToken, code);
            var userInfo = Common.JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr); //用户ID
            var personInfoModel = _wl.GetUserInfo(userInfo.UserId); //获取人员表信息 
            ViewData["vguid"] = personInfoModel.Vguid;

            //Business_Personnel_Information personInfoModel = new Business_Personnel_Information();
            //personInfoModel.Vguid = Guid.Parse("B0167926-C8AF-4AAE-9B18-573EEEDFE740");
            //ViewData["vguid"] = personInfoModel.Vguid;

            #region 获取openid

            var openInfoStr = WeChatTools.ConvertToOpenidByUserId(accessToken, userInfo.UserId);
            var openInfo = Common.JsonHelper.JsonToModel<U_OpenInfo>(openInfoStr);
            var openidExt = openInfo.openid;
            ViewData["openid"] = openidExt;

            #endregion

            #region 查询车牌号是否为空
            ViewData["payException"] = "0";
            //var driverInfo = _weChatRevenueLogic.GetDriverInfo(personInfoModel);
            //if (driverInfo == null)
            //{
            //    ViewData["payException"] = "1";
            //}
            #endregion


            var configList = _configManagementLogic.GetConfigList();
            var fee = configList[13].ConfigValue;
            ViewData["driverPay"] = fee;
            var driverPayfee = double.Parse(fee.Trim('%')) / 100;            //获取司机支付的手续费

            string pushContentVguid = Request.QueryString["Vguid"]; //推送的主键
            ViewData["pushContentVguid"] = pushContentVguid;
            var pushContentModel = _pl.GetPushDetail(pushContentVguid);

            bool isValidTime = false; //未过有效期
            if (pushContentModel != null)
            {
                //判断是否已经支付过
                bool isExist = _weChatRevenueLogic.HasPaymentHistory(personInfoModel.Vguid, pushContentModel.VGUID, 1);
                ViewData["isExist"] = isExist ? "1" : "0";
                #region 判断是否过了有效期

                if (pushContentModel.PeriodOfValidity != null)
                {
                    if (DateTime.Now > pushContentModel.PeriodOfValidity)
                    {
                        isValidTime = true; //已过有效期
                    }
                }

                #endregion
            }
            decimal caBalance = Convert.ToDecimal(pushContentModel.Message);

            decimal currentAccountBalance = FormatData(caBalance);

            pushContentModel.Message = currentAccountBalance.ToString();

            var handlingFee = currentAccountBalance * (decimal)driverPayfee;
            handlingFee = FormatData(handlingFee);
            ViewBag.HandlingFee = handlingFee;


            ViewBag.TotalAmount = currentAccountBalance + handlingFee;

            ViewBag.isValidTime = isValidTime;
            ViewData["PushContentModel"] = pushContentModel;
            return View();
        }

        public ActionResult AliPay()
        {
            return View();
        }

        /// <summary>
        /// 判断推送消息是否过期
        /// </summary>
        /// <param name="pushContentVguid"></param>
        /// <returns></returns>
        public JsonResult IsValid(Guid pushContentVguid,string billNo)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = _weChatRevenueLogic.IsValid(pushContentVguid, billNo);
            models.respnseInfo = models.isSuccess ? "1" : "0";
            return Json(models);
        }

        /// <summary>
        /// 支付接口
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="totalFee">应付款钱数</param>
        ///// <param name="fee">手续费</param>
        /// <param name="personVguid">人员主键</param>
        /// <param name="pushContentVguid">推送主键</param>
        /// <param name="revenueType">营收类型</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPaySign(string openid, string revenueFee, string totalFee, Guid personVguid, Guid pushContentVguid, int revenueType)
        {

            var revenueReceivable = decimal.Parse(revenueFee);            //获取司机支付应付金额
            var totalFeeDouble = double.Parse(totalFee);
            //double totalFeeInt = tatol + driverPayfee;   //包含手续费的总钱数
            var totalFeeInt = (int)Math.Round(totalFeeDouble * 100);
            string nonceStr = WxPayApi.GenerateNonceStr();  //随机字符串，不长于32位
            string outTradeNo = WxPayApi.GenerateOutTradeNo();//订单号：32个字符内、不得重复
            string spbillCreateIp = WxPayConfig.GetIP(System.Web.HttpContext.Current);//用户端IP
            double total_fee = totalFeeInt;//订单金额（单位：分），整数
            string trade_type = "JSAPI";//JSAPI,NATIVE,APP,WAP
            #region 调用统一支付接口获得prepay_id（预支付交易会话标识）
            WxPayData wxPayData = new WxPayData();
            wxPayData.SetValue("appid", WxPayConfig.APPID);              //appid
            wxPayData.SetValue("body", WxPayConfig.BODY);               //支付描述
            wxPayData.SetValue("mch_id", WxPayConfig.MCHID);            //商户id
            wxPayData.SetValue("nonce_str", nonceStr);                 //随机字符串
            wxPayData.SetValue("notify_url", WxPayConfig.NOTIFY_URL);  //回调地址
            wxPayData.SetValue("openid", openid);                      //openid
            wxPayData.SetValue("out_trade_no", outTradeNo);            //订单号
            wxPayData.SetValue("spbill_create_ip", spbillCreateIp);    //客户端ip地址
            wxPayData.SetValue("total_fee", total_fee.ToString());     //订单金额（单位：分），整数
            wxPayData.SetValue("trade_type", trade_type);              //交易类型
            wxPayData.SetValue("sign", wxPayData.MakeSign());          //签名
            string data = wxPayData.ToXml();
            string prepayId = WeChatTools.UnifiedOrder(data);
            #endregion
            #region 支付参数
            string timeStamp = WxPayApi.GenerateTimeStamp();
            nonceStr = WxPayApi.GenerateNonceStr();
            WxPayData wxPaySign = new WxPayData();
            wxPaySign.SetValue("appId", WxPayConfig.APPID);
            wxPaySign.SetValue("timeStamp", timeStamp);
            wxPaySign.SetValue("nonceStr", nonceStr);
            wxPaySign.SetValue("package", string.Format("prepay_id={0}", prepayId));
            wxPaySign.SetValue("signType", "MD5");
            string paysign = wxPaySign.MakeSign();
            wxPaySign.SetValue("paySign", paysign);
            #endregion

            var paymentHistoryInfo = new Business_PaymentHistory_Information();
            paymentHistoryInfo.RevenueReceivable = revenueReceivable;
            paymentHistoryInfo.PaymentPersonnel = personVguid;
            paymentHistoryInfo.PaymentAmount = decimal.Parse(totalFee);
            paymentHistoryInfo.VGUID = Guid.NewGuid();
            paymentHistoryInfo.RevenueType = revenueType;
            paymentHistoryInfo.WeChatPush_VGUID = pushContentVguid;
            paymentHistoryInfo.Remarks = outTradeNo;  //商户订单号
            paymentHistoryInfo.CreateDate = DateTime.Now;
            paymentHistoryInfo.CreateUser = "sysadmin_Revenue";
            paymentHistoryInfo.PayDate = DateTime.Now;
            paymentHistoryInfo.PaymentStatus = "3";
            bool addsuccess = _weChatRevenueLogic.AddPaymentHistory(paymentHistoryInfo);

            return Json(new { success = addsuccess, data = wxPaySign.GetValues() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPaySignWX(string driverId, string organizationId)
        {
            var models = new ActionResultModel<string>();
            var modelData = new QRCodeRevenueInfo();
            var url = ConfigSugar.GetAppString("QRCodeRevenue");
            //Developer,Product 开发,正式
            var data = "{" +
                            "\"OperatorDeviceName\":\"{OperatorDeviceName}\",".Replace("{OperatorDeviceName}", "WXQYH") +
                            "\"OrganizationId\":\"{OrganizationId}\",".Replace("{OrganizationId}", organizationId) +
                            "\"DriverID\":\"{DriverID}\",".Replace("{DriverID}", driverId) +
                            "\"RunEnvironment\":\"{RunEnvironment}\"".Replace("{RunEnvironment}", "Product") +
                            "}";
            try
            {
                WebClient wc = new WebClient();
                wc.Headers.Clear();
                wc.Headers.Add("Content-Type", "application/json;charset=utf-8");
                wc.Encoding = System.Text.Encoding.UTF8;
                var resultData = wc.UploadString(new Uri(url), data);
                modelData = resultData.JsonToModel<QRCodeRevenueInfo>();
                if (modelData.Code == "0")
                {
                    //接口调用成功,获取支付界面url
                    models.isSuccess = true;
                    models.respnseInfo = modelData.data.BillQRCodeURL +","+ modelData.data.BillNo;
                    var key = PubGet.GetUserKey + driverId;
                    CacheManager<QRCodeRevenue>.GetInstance().Add(key, modelData.data, 8 * 60 * 60 * 1000);
                }
                else
                {
                    //接口调用失败,支付二维码失效
                    WeChatRevenueServer.sendQRCodeMessage(modelData.data.BillNo);
                    models.isSuccess = false;
                    models.respnseInfo = modelData.message;
                }
                LogHelper.WriteLog(string.Format("Data:{0},result:{1}", data,  resultData));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(string.Format("Data:{0},result:{1},error:{2}", data, modelData.message, ex.ToString()));
            }
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SavePaymentHistory(string driverId,string revenueFee, Guid personVguid, Guid pushContentVguid, int revenueType)
        {
            var cm = CacheManager<QRCodeRevenue>.GetInstance()[PubGet.GetUserKey + driverId];
            var paymentHistoryInfo = new Business_PaymentHistory_Information();
            paymentHistoryInfo.RevenueReceivable = decimal.Parse(revenueFee); ;
            paymentHistoryInfo.PaymentPersonnel = personVguid;
            paymentHistoryInfo.PaymentAmount = decimal.Parse(cm.PayDebtAmount);
            paymentHistoryInfo.VGUID = Guid.NewGuid();
            paymentHistoryInfo.RevenueType = revenueType;
            paymentHistoryInfo.WeChatPush_VGUID = pushContentVguid;
            paymentHistoryInfo.Remarks = cm.BillNo;  //商户订单号
            paymentHistoryInfo.CreateDate = DateTime.Now;
            paymentHistoryInfo.CreateUser = "sysadmin_Revenue";
            paymentHistoryInfo.PayDate = cm.BillDate.TryToDate();
            paymentHistoryInfo.PaymentStatus = "3";//待支付或未支付
            bool addsuccess = _weChatRevenueLogic.AddPaymentHistory(paymentHistoryInfo);

            return Json(new { success = addsuccess }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult gettest()
        {
            return Json(new { success = true, data = "ssss" }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 支付结果回调地址
        /// </summary>
        /// <returns></returns>
        public string PayNotifyUrl()
        {
            WxPayData resultInfo = new WxPayData();
            try
            {

                ResultNotify resultNotify = new ResultNotify();
                WxPayData notifyData = resultNotify.GetNotifyData();  //获取微信返回的数据
                string accessToken = WeChatTools.GetAccessoken();
                var userInfoStr = WeChatTools.ConvertToUserIdByOpenid(accessToken, notifyData.GetValue("openid").ToString());
                var userInfo = Common.JsonHelper.JsonToModel<U_WechatUserInfo>(userInfoStr);
                var userid = userInfo.userid;
                var personInfoModel = _wl.GetUserInfo(userid); //获取人员表信息   
                var paymentHistoryInfo = new Business_PaymentHistory_Information();
                paymentHistoryInfo.PaymentPersonnel = personInfoModel.Vguid; //付款人vguid
                _weChatRevenueLogic.UpdatePaymentHistory(personInfoModel, paymentHistoryInfo, notifyData);

                if (notifyData.GetValue("return_code").ToString() == "SUCCESS" && notifyData.GetValue("result_code").ToString() == "SUCCESS")
                {
                    resultInfo.SetValue("return_code", "SUCCESS");
                    resultInfo.SetValue("return_msg", "OK");
                }
                else
                {
                    resultInfo.SetValue("return_code", notifyData.GetValue("return_code").ToString());
                    resultInfo.SetValue("return_msg", notifyData.GetValue("return_msg").ToString());
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.ToString());
                resultInfo.SetValue("return_code", "FAIL");
                resultInfo.SetValue("return_msg", "交易失败");
            }
            return resultInfo.ToXml();
        }

    }


}
