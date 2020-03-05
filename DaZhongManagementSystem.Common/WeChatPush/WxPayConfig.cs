
using System.Web;
using SyntacticSugar;

namespace DaZhongManagementSystem.Common.WeChatPush
{
    public class WxPayConfig
    {
        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        public static readonly string APPID = ConfigSugar.GetAppString("CorpID");

        public static readonly string APPSECRET = ConfigSugar.GetAppString("Secret");

        public static readonly string APPCONTACTSECRET = ConfigSugar.GetAppString("ContactSecret");

        public static readonly string MCHID = ConfigSugar.GetAppString("MCHID");    //商户id号

        public static readonly string KEY = ConfigSugar.GetAppString("APIKey");
        //支付描述
        public static readonly string BODY = ConfigSugar.GetAppString("PaymentDesc");
        //=======【支付结果通知url】===================================== 
        //支付结果通知回调url，用于商户接收支付结果
        public static readonly string NOTIFY_URL = ConfigSugar.GetAppString("OpenHttpAddress") + "/WeChatPush/WeChatRevenue/PayNotifyUrl";//支付结果回调地址，不能带参数（PayNotifyUrl回调里能接到订单号out_trade_no参数）

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public static readonly string SSLCERT_PATH = "cert/apiclient_cert.p12";
        public static readonly string SSLCERT_PASSWORD = ConfigSugar.GetAppString("MCHID");

        /// <summary>
        /// 客户端IP
        /// </summary>
        /// <param name="hc"></param>
        /// <returns></returns>
        public static string GetIP(HttpContext hc)
        {
            string ip = string.Empty;
            try
            {
                ip = hc.Request.ServerVariables["HTTP_VIA"] != null ? hc.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : hc.Request.ServerVariables["REMOTE_ADDR"];
                if (ip == string.Empty)
                {
                    ip = hc.Request.UserHostAddress;
                }
                return ip;
            }
            catch
            {
                return "8.8.8.8";
            }
        }
    }
}
