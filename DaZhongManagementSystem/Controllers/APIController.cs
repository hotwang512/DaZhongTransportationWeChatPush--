using DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.RideCheckFeedback.BusinessLogic;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Common.LogHelper;
using DaZhongManagementSystem.Models.APIModel;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.DraftList.BusinessLogic;
using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.UserInfo.BussinessLogic;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using DaZhongManagementSystem.Common.WeChatPush;
using DaZhongManagementSystem.Entities.TableEntity;

namespace DaZhongManagementSystem.Controllers
{
    public class APIController : Controller
    {
        public JsonResult GetRideCheckFailed(string numberPlate)
        {
            ExecutionResult result = new ExecutionResult();
            if (string.IsNullOrEmpty(numberPlate))
            {
                result.Message = "车牌号不能为空！";
            }
            else
            {
                try
                {
                    RideCheckFeedbackLogic logic = new RideCheckFeedbackLogic();
                    var rideCheckFaileds = logic.GetRideCheckFailed(numberPlate);
                    if (rideCheckFaileds.Count > 0)
                    {
                        List<string> list = new List<string>();
                        foreach (var item in rideCheckFaileds)
                        {
                            string str = string.Format("该车于时间：{0} 上车地点：{1} 下车地点：{2} 接受过跳车检查，存在以下不合格项：{3}",
                                item.跳车时间,
                                item.上车地点,
                                item.下车地点,
                                item.跳车检查结果.Replace("不合格:", ""));
                            list.Add(str);
                        }
                        result.Result = list;
                        result.Success = true;
                    }
                    else
                    {
                        result.Message = "该车辆不存在不合格项！";
                    }

                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 接口调用认证
        /// </summary>
        /// <param name="SECURITYKEY">认证码</param>
        /// <returns></returns>
        public bool API_Authentication(string SECURITYKEY)
        {
            var security = AesClass.AesDecrypt(SECURITYKEY, SyntacticSugar.ConfigSugar.GetAppString("API_EncryptionKey"));
            return security == SyntacticSugar.ConfigSugar.GetAppString("API_AuthenticationValue");
        }

        /// <summary>
        /// 根据微信code获取企业号人员信息
        /// </summary>
        /// <param name="SECURITYKEY">加密值</param>
        /// <param name="code">微信code</param>
        /// <returns></returns>
        public JsonResult UserInformation(string SECURITYKEY, string code)
        {
            ExecutionResult result = new ExecutionResult();
            try
            {
                if (API_Authentication(SECURITYKEY))
                {
                    string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
                    string userInfoStr = Common.WeChatPush.WeChatTools.GetUserInfoByCode(accessToken, code);
                    U_WeChatUserID userInfo = Common.JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr);//用户ID  _logic;
                    result.Result = new RideCheckFeedbackLogic().GetUserInfo(userInfo.UserId);
                    result.Success = true;
                }
                else
                {
                    result.Message = "SECURITYKEY 无效！";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                LogHelper.WriteLog(ex.Message);
            }
            return Json(result);
        }


        public JsonResult TextPush(string SECURITYKEY, string pushparam)
        {
            ExecutionResult result = new ExecutionResult();
            try
            {
                if (API_Authentication(SECURITYKEY))
                {
                    PushParamModel textPush = Extend.JsonToModel<PushParamModel>(pushparam);
                    U_PushMsg pushMsg = new U_PushMsg();
                    pushMsg.VGUID = Guid.NewGuid();
                    pushMsg.PushType = 1;
                    pushMsg.MessageType = 1;
                    pushMsg.Title = textPush.Title;
                    pushMsg.Message = textPush.Message;
                    pushMsg.PushPeople = textPush.founder;
                    pushMsg.CreatedUser = "微企推送";
                    pushMsg.CreatedDate = DateTime.Now;
                    UserInfoLogic userInfoLogic = new UserInfoLogic();
                    foreach (string item in textPush.PushPeople)
                    {
                        var user = userInfoLogic.GetPerson(item);
                        if (user != null && user.UserID != null && user.UserID != "")
                        {
                            pushMsg.PushObject += user.UserID + "|";
                        }
                        else
                        {
                            result.Message += item + "|";
                        }
                    }
                    if (!string.IsNullOrEmpty(result.Message))
                    {
                        result.Message = result.Message.Remove(result.Message.Length - 1, 1);
                        result.Message = "不存在身份证号码：" + result.Message;
                    }
                    DraftInfoLogic logic = new DraftInfoLogic();
                    Guid vguid = Guid.Empty;
                    result.Success = logic.APISavePushMsg(pushMsg);
                    result.Result = new { Uniquekey = pushMsg.VGUID };
                }
                else
                {
                    result.Message = "SECURITYKEY 无效！";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                LogHelper.WriteLog(ex.Message);
            }
            return Json(result);
        }

        public JsonResult GraphicPush(string SECURITYKEY, string pushparam)
        {
            ExecutionResult result = new ExecutionResult();
            try
            {
                if (API_Authentication(SECURITYKEY))
                {
                    List<PushParamModel> textPush = Extend.JsonToModel<List<PushParamModel>>(pushparam);
                    result = SaveGraphicPushData(textPush);
                    string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
                    string _sendUrl = "https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}";
                    string postUrl = string.Format(_sendUrl, accessToken);
                    //获取推送内容Json
                    string json = GetPushJson(textPush);
                    string pushResult = PostWebRequest(postUrl, json, Encoding.UTF8);
                }
                else
                {
                    result.Message = "SECURITYKEY 无效！";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                LogHelper.WriteLog(ex.Message);
            }
            return Json(result);
        }

        public ExecutionResult SaveGraphicPushData(List<PushParamModel> textPush)
        {
            ExecutionResult result = new ExecutionResult();
            U_PushMsg pushMsg = new U_PushMsg();
            pushMsg.VGUID = Guid.NewGuid();
            pushMsg.PushType = 1;
            pushMsg.MessageType = 3;
            pushMsg.Title = textPush[0].Title;
            pushMsg.Message = textPush[0].Message;
            pushMsg.PushPeople = textPush[0].founder;
            pushMsg.CreatedUser = "微企推送";
            pushMsg.CreatedDate = DateTime.Now;
            UserInfoLogic userInfoLogic = new UserInfoLogic();
            foreach (string item in textPush[0].PushPeople)
            {
                var user = userInfoLogic.GetPerson(item);
                if (user != null && user.UserID != null && user.UserID != "")
                {
                    textPush[0].PushPeoples += user.UserID + "|";
                    pushMsg.PushObject += user.UserID + "|";
                }
                else
                {
                    result.Message += item + "|";
                }
            }
            textPush[0].PushPeoples = textPush[0].PushPeoples.TrimEnd('|');

            List<Business_WeChatPush_MoreGraphic_Information> pushMoreGraphicList = new List<Business_WeChatPush_MoreGraphic_Information>();
            foreach (PushParamModel push in textPush)
            {
                Business_WeChatPush_MoreGraphic_Information pushMoreGraphic = new Business_WeChatPush_MoreGraphic_Information();
                pushMoreGraphic.Title = push.Title;
                pushMoreGraphic.CoverImg = pushMoreGraphic.Message = push.Message;
                pushMoreGraphic.CoverImg = push.Image;
                pushMoreGraphic.CreatedDate = DateTime.Now;
                pushMoreGraphic.CreatedUser = "微企推送";
                pushMoreGraphic.WeChatPushVguid = pushMsg.VGUID;
                pushMoreGraphic.VGUID = Guid.NewGuid();
                pushMoreGraphicList.Add(pushMoreGraphic);
            }

            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = result.Message.Remove(result.Message.Length - 1, 1);
                result.Message = "不存在身份证号码：" + result.Message;
            }
            DraftInfoLogic logic = new DraftInfoLogic();
            Guid vguid = Guid.Empty;
            result.Success = logic.APISaveImagePushMsg(pushMsg, pushMoreGraphicList);
            result.Result = new { Uniquekey = pushMsg.VGUID };
            return result;
        }

        protected string GetPushJson(List<PushParamModel> pushMsg)
        {
            string pushUrl = string.Empty;
            string url = string.Empty;

            string responeJsonStr = "";
            responeJsonStr = "{";
            responeJsonStr += "\"touser\": \"" + pushMsg[0].PushPeoples + "\",";
            responeJsonStr += "\"toparty\": \"\",";
            responeJsonStr += "\"totag\": \"\",";
            responeJsonStr += "\"msgtype\": \"news\",";
            responeJsonStr += "\"agentid\":\"10\",";
            responeJsonStr += "\"news\": {";
            responeJsonStr += "\"articles\":[";
            foreach (var content in pushMsg)
            {
                responeJsonStr += " {";
                responeJsonStr += "\"title\": \"" + content.Title + "\",";
                responeJsonStr += "\"description\": \"" + content.Message + "\",";
                responeJsonStr += "\"url\": \"" + content.Url + "\",";
                responeJsonStr += "\"picurl\": \"" + content.Image + "\"";
                responeJsonStr += "}";
                responeJsonStr += ",";
            }
            responeJsonStr += "]";
            responeJsonStr += "}";
            responeJsonStr += "}";
            return responeJsonStr;

        }

        /// <summary>
        /// Post数据接口
        /// </summary>
        /// <param name="postUrl">接口地址</param>
        /// <param name="jsonData">提交json数据</param>
        /// <param name="dataEncode">编码方式</param>
        /// <param name="isUseCert">是否使用证书</param>
        /// <returns></returns>
        protected string PostWebRequest(string postUrl, string jsonData, Encoding dataEncode, bool isUseCert = false)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(jsonData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                if (isUseCert)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory;
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
                LogHelper.WriteLog(ex.Message);
                return ex.Message;
            }
            return ret;
        }
    }
}