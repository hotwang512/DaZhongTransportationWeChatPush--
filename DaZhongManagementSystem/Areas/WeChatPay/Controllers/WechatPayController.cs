using System.Web.Mvc;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Common.WeChatPush;
using DaZhongManagementSystem.Entities.UserDefinedEntity;

namespace DaZhongManagementSystem.Areas.WeChatPay.Controllers
{
    public class WechatPayController : Controller
    {
        //
        // GET: /WeChatPay/WechatPay/
      
        public ActionResult Index(string code)
        {
            #region 获取用户微信OpenId
            var accountToken = WeChatTools.GetAccessoken();
            ViewData["accountToken"] = accountToken;
            var userInfoStr = WeChatTools.GetUserInfoByCode(accountToken, code);
            var userInfo =JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr);//用户ID
            var openInfoStr = WeChatTools.ConvertToOpenidByUserId(accountToken, userInfo.UserId);
            var openInfo = JsonHelper.JsonToModel<U_OpenInfo>(openInfoStr);
            var openidExt = openInfo.openid;
            ViewData["openid"] = openidExt;
            #endregion
            return View();
        }

        /// <summary>
        /// 获取微信支付配置
        /// </summary>
        /// <returns></returns>
       // [HttpGet]
        //public JsonResult GetPayConfig(string accessToken)
        //{
        //    string timeStamp = WxPayApi.GenerateTimeStamp();
        //    string nonceStr = WxPayApi.GenerateNonceStr(); 
        //    string signature = new WxPayApi().CreateMd5Sign(accessToken, timeStamp, nonceStr,Request.Url.ToString());

        //    return Json(new { appId = AppId, timeStamp = timeStamp, nonceStr = nonceStr, signature = signature }, JsonRequestBehavior.AllowGet);
        //}


        /// <summary>
        /// 支付接口
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPaySign(string openid)
        {
            string nonceStr = WxPayApi.GenerateNonceStr();  //随机字符串，不长于32位
            string outTradeNo = WxPayApi.GenerateOutTradeNo();//订单号：32个字符内、不得重复
            string spbillCreateIp = WxPayConfig.GetIP(System.Web.HttpContext.Current);//用户端IP
            int total_fee = 1;//订单金额（单位：分），整数
            string trade_type = "JSAPI";//JSAPI,NATIVE,APP,WAP
            #region 调用统一支付接口获得prepay_id（预支付交易会话标识）
            WxPayData wxPayData = new WxPayData();
            wxPayData.SetValue("appid",WxPayConfig.APPID);              //appid
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

            return Json(new { data = wxPaySign.GetValues(), openid = openid }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 支付结果回调地址
        /// </summary>
        /// <returns></returns>

        public void PayNotifyUrl()
        {
            ResultNotify resultNotify = new ResultNotify();
            resultNotify.ProcessNotify();
        }

    }


}
