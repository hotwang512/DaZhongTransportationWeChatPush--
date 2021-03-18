using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using DaZhongManagementSystem.Entities.UserDefinedEntity;

namespace DaZhongManagementSystem.Common.WeChatPush
{
    public class WeChatTools
    {
        /// <summary>
        /// 获取公司AccessToken
        /// </summary>
        /// <returns></returns>
        public static string GetAccessoken(bool isContact = false)
        {
            //获取AccessTokenUrl
            string tokenUrl = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}";
            string respText = "";
            string accessToken = "";
            string AppSecret = WxPayConfig.APPSECRET;
            if (isContact)
            {
                AppSecret = WxPayConfig.APPCONTACTSECRET;
            }
            //获取josn数据
            string url = string.Format(tokenUrl, WxPayConfig.APPID, AppSecret);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream resStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(resStream, Encoding.Default);
                respText = reader.ReadToEnd();
                resStream.Close();
                reader.Close();
            }
            try
            {
                JavaScriptSerializer Jss = new JavaScriptSerializer();
                Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);

                //通过键access_token获取值
                accessToken = respDic["access_token"].ToString();
                return accessToken;
            }
            catch (Exception ex)
            {
                LogHelper.LogHelper.WriteLog(ex.Message);
                return "";
            }

        }

        public static string GetUserInfo(string accessToken)
        {
            string userInfoUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/list?access_token={0}&department_id=1&fetch_child=1";
            string url = string.Format(userInfoUrl, accessToken);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string respText = "";
            using (Stream resStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(resStream, Encoding.UTF8);
                respText = reader.ReadToEnd();
                resStream.Close();
                reader.Close();
            }
            return respText;
        }


        /// <summary>
        /// 通过企业Code获取用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetUserInfoByCode(string accessToken, string code)
        {
            //通过企业Code获取用户UserID Url
            string userUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}";//ConfigSugar.GetAppString("UserInfoUrl");
            string url = string.Format(userUrl, accessToken, code);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string respText = "";
            using (Stream resStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(resStream, Encoding.UTF8);
                respText = reader.ReadToEnd();
                resStream.Close();
                reader.Close();
            }
            return respText;
        }

        /// <summary>
        /// 通过UserID获取人员信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string GetUserInfoByUserID(string accessToken, string userID)
        {
            string userInfoUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/get?access_token={0}&userid={1}";
            string url = string.Format(userInfoUrl, accessToken, userID);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string respText = "";
            using (Stream resStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(resStream, Encoding.UTF8);
                respText = reader.ReadToEnd();
                resStream.Close();
                reader.Close();
            }
            return respText;
        }

        /// <summary>
        /// 关注成功
        /// </summary>
        /// <returns></returns>
        public static string GetAuthSucee(string accessToken, string userID)
        {
            string successUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/authsucc?access_token={0}&userid={1}";
            string url = string.Format(successUrl, accessToken, userID);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string respText = "";
            using (Stream resStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(resStream, Encoding.UTF8);
                respText = reader.ReadToEnd();
                resStream.Close();
                reader.Close();
            }
            return respText;
        }

        /// <summary>
        /// 创建微信通讯录中的用户
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string CreateUser(string accessToken, U_WeChatRegistered user)
        {
            string createUserUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/create?access_token={0}";
            string url = string.Format(createUserUrl, accessToken);
            string jsonData = "{\"userid\":\"" + user.userid + "\",\"name\":\"" + user.name + "\",\"department\":[2],\"position\":\"司机\",\"mobile\":\"" + user.mobile + "\",\"gender\":\"" + user.gender + "\"}";
            string respText = PostWebRequest(url, jsonData, Encoding.UTF8);
            return respText;
        }
        /// <summary>
        /// 查询微信通讯录中的用户ID
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetUserId(string accessToken, string mobile)
        {
            string createUserUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserid?access_token={0}";
            string url = string.Format(createUserUrl, accessToken);
            string jsonData = "{\"mobile\":\"" + mobile + "\"}";
            string respText = PostWebRequest(url, jsonData, Encoding.UTF8);
            return respText;
        }

        /// <summary>
        /// 删除微信通讯录中的用户
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string DeleteUser(string accessToken, string userID)
        {
            string deleteUserUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/delete?access_token={0}&userid={1}";
            string url = string.Format(deleteUserUrl, accessToken, userID);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string respText = "";
            using (Stream resStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(resStream, Encoding.UTF8);
                respText = reader.ReadToEnd();
                resStream.Close();
                reader.Close();
            }
            return respText;
        }

        /// <summary>
        /// 禁用微信通讯录人员
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string DisableWeChatData(string accessToken, string userID)
        {
            string respText = "";
            string deleteUserUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/update?access_token={0}";

            string responeJsonStr = "";
            responeJsonStr = "{";
            responeJsonStr += "\"userid\": \"" + userID + "\",";
            responeJsonStr += "\"enable\": 0,";
            responeJsonStr += "}";
            string url = string.Format(deleteUserUrl, accessToken);
            respText = PostWebRequest(url, responeJsonStr, Encoding.UTF8);


            return respText;
        }

        /// <summary>
        /// 更新通讯录人员手机号码
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userID"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static string WeChatMobileChange(string accessToken, string userID, string mobile)
        {
            string respText = "";
            string updateUserUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/update?access_token={0}";

            string responeJsonStr = "";
            responeJsonStr = "{";
            responeJsonStr += "\"userid\": \"" + userID + "\",";
            responeJsonStr += "\"mobile\": \"" + mobile + "\"";
            responeJsonStr += "}";
            string url = string.Format(updateUserUrl, accessToken);
            respText = PostWebRequest(url, responeJsonStr, Encoding.UTF8);


            return respText;
        }
        /// <summary>
        /// 启用微信通讯录人员
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string EnableWeChatData(string accessToken, string userID)
        {
            string respText = "";
            string deleteUserUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/update?access_token={0}";

            string responeJsonStr = "";
            responeJsonStr = "{";
            responeJsonStr += "\"userid\": \"" + userID + "\",";
            responeJsonStr += "\"enable\": 1,";
            responeJsonStr += "}";
            string url = string.Format(deleteUserUrl, accessToken);
            respText = PostWebRequest(url, responeJsonStr, Encoding.UTF8);


            return respText;
        }
        /// <summary>
        /// 微信通讯录人员添加标记
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string TagWeChatData(string accessToken, string userID)
        {
            string respText = "";
            string tagUserUrl = "https://qyapi.weixin.qq.com/cgi-bin/tag/addtagusers?access_token={0}";

            string responeJsonStr = "";
            responeJsonStr = "{\"tagid\":1, \"userlist\":[\"" + userID + "\"]}";
            string url = string.Format(tagUserUrl, accessToken);
            respText = PostWebRequest(url, responeJsonStr, Encoding.UTF8);


            return respText;
        }
        /// <summary>
        /// 根据userid获取openid
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string ConvertToOpenidByUserId(string accessToken, string userId)
        {
            string respText = "";
            string convertToOpenidUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/convert_to_openid?access_token={0}";
            string responeJsonStr = "";
            responeJsonStr = "{";
            responeJsonStr += "\"userid\": \"" + userId + "\"";
            responeJsonStr += "}";
            string url = string.Format(convertToOpenidUrl, accessToken);
            respText = PostWebRequest(url, responeJsonStr, Encoding.UTF8);
            return respText;
        }


        /// <summary>
        /// 根据openid获取userid
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static string ConvertToUserIdByOpenid(string accessToken, string openid)
        {
            string respText = "";
            string convertToOpenidUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/convert_to_userid?access_token={0}";
            string responeJsonStr = "";
            responeJsonStr = "{";
            responeJsonStr += "\"openid\": \"" + openid + "\"";
            responeJsonStr += "}";
            string url = string.Format(convertToOpenidUrl, accessToken);
            respText = PostWebRequest(url, responeJsonStr, Encoding.UTF8);
            return respText;
        }

        /// <summary>
        /// 统一下单
        /// post方式提交
        /// </summary>
        /// <param name="xml">XML格式的参数</param>
        /// <returns>返回prepay_id</returns>
        public static string UnifiedOrder(string xml)
        {
            var urlFormat = "https://api.mch.weixin.qq.com/pay/unifiedorder";  //统一下单 
            string prepayId = "";
            string requestData = PostWebRequest(urlFormat, xml, Encoding.UTF8);
            var res = System.Xml.Linq.XDocument.Parse(requestData);
            if (res.Element("xml").Element("return_code").Value.ToUpper() == "SUCCESS")
            {
                prepayId = res.Element("xml").Element("prepay_id").Value;
            }
            return prepayId;
        }

        /// <summary>
        /// 获取jsapi_ticket
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static string GetJSApi_Ticket(string accessToken)
        {
            var jsapiUrl = "https://qyapi.weixin.qq.com/cgi-bin/get_jsapi_ticket?access_token={0}";
            string url = string.Format(jsapiUrl, accessToken);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string respText = "";
            using (Stream resStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(resStream, Encoding.UTF8);
                respText = reader.ReadToEnd();
                resStream.Close();
                reader.Close();
            }
            return respText;
        }

        /// <summary>
        ///  查询订单
        /// </summary>
        /// <param name="inputObj">提交给查询订单API的参数</param>
        /// <returns>成功时返回订单查询结果，其他抛异常</returns>
        public static WxPayData OrderQuery(WxPayData inputObj)
        {
            string url = "https://api.mch.weixin.qq.com/pay/orderquery";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new WxPayException("订单查询接口中，out_trade_no、transaction_id至少填一个！");
            }

            inputObj.SetValue("appid", WxPayConfig.APPID);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名

            string xml = inputObj.ToXml();

            string response = PostWebRequest(url, xml, Encoding.UTF8); //调用HTTP通信接口提交数据
            //将xml格式的数据转化为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);

            return result;
        }

        /// <summary>
        /// 申请退款（此方法需要安装操作证书）
        /// </summary>
        /// <param name="inputObj">提交给申请退款API的参数</param>
        /// <returns> 成功时返回接口调用结果，其他抛异常</returns>
        public static WxPayData Refund(WxPayData inputObj)
        {
            string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new WxPayException("退款申请接口中，out_trade_no、transaction_id至少填一个！");
            }
            else if (!inputObj.IsSet("out_refund_no"))
            {
                throw new WxPayException("退款申请接口中，缺少必填参数out_refund_no！");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                throw new WxPayException("退款申请接口中，缺少必填参数total_fee！");
            }
            else if (!inputObj.IsSet("refund_fee"))
            {
                throw new WxPayException("退款申请接口中，缺少必填参数refund_fee！");
            }
            inputObj.SetValue("appid", WxPayConfig.APPID);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名

            string xml = inputObj.ToXml();
            string response = PostWebRequest(url, xml, Encoding.UTF8, true);//调用HTTP通信接口提交数据到API

            //将xml格式的结果转换为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);
            return result;
        }
        /// <summary>
        /// 发红包（此方法需要证书）
        /// </summary>
        /// <param name="inputObj">提交给发红包API的参数</param>
        /// <returns>成功时返回接口调用结果，其他抛异常</returns>
        public static WxPayData SendRedPack(WxPayData inputObj)
        {
            string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";
            inputObj.SetValue("scene_id", "PRODUCT_4");           //场景(金额大于200元时必填)
            inputObj.SetValue("remark", "猜越多得越多，快来抢！");  //备注
            inputObj.SetValue("act_name", "猜灯谜抢红包活动");      //活动名称
            inputObj.SetValue("client_ip", WxPayConfig.GetIP(HttpContext.Current));  //ip地址
            inputObj.SetValue("wishing", "祝您元宵节快乐！");       //红包祝福语
            inputObj.SetValue("send_name", "大众交通集团");           //商户名称
            inputObj.SetValue("mch_billno", WxPayApi.GenerateOutTradeNo()); //商户订单号
            inputObj.SetValue("wxappid", WxPayConfig.APPID);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();
            string response = PostWebRequest(url, xml, Encoding.UTF8, true);//调用HTTP通信接口提交数据到API
            //将xml格式的结果转换为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);
            return result;
        }

        /// <summary>
        /// 查询红包领取记录（此方法需要证书）
        /// </summary>
        /// <param name="inputObj">提交给发红包API的参数</param>
        /// <returns>成功时返回接口调用结果，其他抛异常</returns>
        public static WxPayData GethbInfo(WxPayData inputObj)
        {
            string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/gethbinfo";
            inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());//随机字符串
            //  inputObj.SetValue("mch_billno", "147134870220170817175518586");           //商户发放红包的商户订单号 
            inputObj.SetValue("appid", WxPayConfig.APPID);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            inputObj.SetValue("bill_type", "MCHT");//通过商户订单号获取红包信息。 
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();
            string response = PostWebRequest(url, xml, Encoding.UTF8, true);//调用HTTP通信接口提交数据到API
            //将xml格式的结果转换为对象以返回
            WxPayData result = new WxPayData();
            result.XmlToEntity(response);
            return result;
        }

        /// <summary>
        /// 查询企业红包领取记录（此方法需要证书）
        /// </summary>
        /// <param name="inputObj">提交给发红包API的参数</param>
        /// <returns>成功时返回接口调用结果，其他抛异常</returns>
        public static WxPayData QueryWorkWxRedPacket(WxPayData inputObj)
        {
            string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/queryworkwxredpack";
            inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());//随机字符串
            //  inputObj.SetValue("mch_billno", "147134870220170817175518586");           //商户发放红包的商户订单号 
            inputObj.SetValue("appid", WxPayConfig.APPID);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();
            string response = PostWebRequest(url, xml, Encoding.UTF8, true);//调用HTTP通信接口提交数据到API
            //将xml格式的结果转换为对象以返回
            WxPayData result = new WxPayData();
            result.XmlToEntity(response);
            return result;
        }

        /// <summary>
        /// Post数据接口
        /// </summary>
        /// <param name="postUrl">接口地址</param>
        /// <param name="jsonData">提交json数据</param>
        /// <param name="dataEncode">编码方式</param>
        /// <param name="isUseCert">是否使用证书</param>
        /// <returns></returns>
        public static string PostWebRequest(string postUrl, string jsonData, Encoding dataEncode, bool isUseCert = false)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(jsonData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.ContentLength = byteArray.Length;
                //是否使用证书
                if (isUseCert)
                {
                    string path = HttpContext.Current.Request.PhysicalApplicationPath;
                    X509Certificate2 cert = new X509Certificate2(path + WxPayConfig.SSLCERT_PATH, WxPayConfig.SSLCERT_PASSWORD);
                    webReq.ClientCertificates.Add(cert);
                }
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return ret;
        }




    }
}
