using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DaZhongManagementSystem.Common.WeChatPush
{
    public class ResultNotify:Notify
    {
        public override void ProcessNotify()
        {
            WxPayData notifyData = GetNotifyData();

            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                LogHelper.LogHelper.WriteLog("支付结果中微信订单号不存在: " + res.ToXml());
                HttpContext.Current.Response.Write(res.ToXml());
                HttpContext.Current.Response.End();
            }

            string transaction_id = notifyData.GetValue("transaction_id").ToString();

            //查询订单，判断订单真实性
            if (!QueryOrder(transaction_id))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                LogHelper.LogHelper.WriteLog("订单查询失败: " + res.ToXml());
                HttpContext.Current.Response.Write(res.ToXml());
                HttpContext.Current.Response.End();
            }
            //查询订单成功
            else
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                HttpContext.Current.Response.Write(res.ToXml());
                HttpContext.Current.Response.End();
            }
        }

        //查询订单
        private bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WeChatTools.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" && res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
