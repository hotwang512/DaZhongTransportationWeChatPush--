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
using System.Threading.Tasks;
using System.Net.Http;
using SyntacticSugar;

namespace DaZhongManagementSystem.Controllers
{
    public class APIController : Controller
    {

        private void ExecHistry(string apiName, string pushparam, string result)
        {
            LogHelper.WriteLog(string.Format("接口名称:{0}  时间:{1}  参数:{2}  结果:{3}", apiName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), pushparam, result));
        }


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
            ExecHistry("GetRideCheckFailed", numberPlate, result.Success.ToString());
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
        /// 根据手机号码获取身份证号码
        /// </summary>
        /// <param name="SECURITYKEY">加密值</param>
        /// <param name="phoneNumber">手机号码</param>
        /// <returns></returns>
        public JsonResult UserIDNumber(string SECURITYKEY, string phoneNumber)
        {
            ExecutionResult result = new ExecutionResult();
            try
            {
                if (API_Authentication(SECURITYKEY))
                {
                    result.Result = "";
                    UserInfoLogic logic = new UserInfoLogic();
                    var user = logic.GetPersonByPhoneNumber(phoneNumber);
                    if (user != null)
                    {
                        result.Result = user.IDNumber;
                    }
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
            ExecHistry("UserIDNumber", phoneNumber, result.Success.ToString());
            return Json(result, JsonRequestBehavior.AllowGet);
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
            ExecHistry("UserInformation", code, result.Success.ToString());
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
                    pushMsg.PeriodOfValidity = DateTime.Now.AddMonths(1);
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
            ExecHistry("TextPush", pushparam, result.Success.ToString());
            return Json(result);
        }

        public JsonResult GraphicPush(string SECURITYKEY, string pushparam, bool OAuth2 = false)
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
                    string json = GetPushJson(OAuth2, textPush);
                    string pushResult = WeChatTools.PostWebRequest(postUrl, json, Encoding.UTF8);
                    var wechatResult = Extend.JsonToModel<U_WechatResult>(pushResult);
                    if (wechatResult.errcode == "0")
                    {
                        result.Success = true;
                    }
                    result.Message = pushResult;
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
            ExecHistry("GraphicPush", pushparam, result.Success.ToString());
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
            pushMsg.PeriodOfValidity = DateTime.Now.AddMonths(1);
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


        public JsonResult WeChatRegistrationVerification(string SECURITYKEY, string pushparam)
        {
            ExecutionResult result = new ExecutionResult();
            try
            {
                if (API_Authentication(SECURITYKEY))
                {

                    U_WeChatRegistered user = Extend.JsonToModel<U_WeChatRegistered>(pushparam);
                    string accessToken = WeChatTools.GetAccessoken(true);
                    string pushResult = WeChatTools.GetUserInfo(accessToken);
                    U_WechatUsersResult usersResult = Extend.JsonToModel<U_WechatUsersResult>(pushResult);

                    var fuser = usersResult.userlist.Find(c => c.userid == c.mobile && c.mobile == user.mobile);
                    if (fuser != null)
                    {
                        result.Success = true;
                        result.Message = string.Format("存在微信USERID为{0},手机号码为{1}的用户！", fuser.userid, fuser.mobile);
                        result.Result = 1;//USERID为手机号，手机号一致
                        return Json(result);
                    }
                    fuser = usersResult.userlist.Find(c => c.userid == user.idcard && c.mobile == user.mobile);
                    if (fuser != null)
                    {
                        result.Success = true;
                        result.Message = string.Format("存在微信USERID为{0},手机号码为{1}的用户！", fuser.userid, fuser.mobile);
                        result.Result = 2;//USERID为身份证号，手机号一致
                        return Json(result);
                    }
                    fuser = usersResult.userlist.Find(c => c.userid == user.mobile);
                    if (fuser != null)
                    {
                        result.Success = true;
                        result.Message = string.Format("存在微信USERID为{0},手机号码为{1}的用户！", fuser.userid, fuser.mobile);
                        result.Result = 3;//USERID为手机号，手机号不一致
                        return Json(result);
                    }
                    fuser = usersResult.userlist.Find(c => c.userid == user.idcard);
                    if (fuser != null)
                    {
                        result.Success = true;
                        result.Message = string.Format("存在微信USERID为{0},手机号码为{1}的用户！", fuser.userid, fuser.mobile);
                        result.Result = 4;//USERID为身份证号，手机号不一致
                        return Json(result);
                    }
                    fuser = usersResult.userlist.Find(c => c.mobile == user.mobile);
                    if (fuser != null)
                    {
                        result.Success = true;
                        result.Message = string.Format("存在微信USERID为{0},手机号码为{1}的用户！", fuser.userid, fuser.mobile);
                        result.Result = 5;//手机号一致，USERID不一致
                        return Json(result);
                    }
                    result.Success = true;
                    result.Message = "微信不存在该用户!";
                    result.Result = 0;//
                    return Json(result);
                }
            }

            catch (Exception ex)
            {
                result.Message = ex.Message;
                LogHelper.WriteLog(ex.Message);
            }
            return Json(result);
        }
        public JsonResult WeChatRegistered(string SECURITYKEY, string pushparam)
        {
            ExecutionResult result = new ExecutionResult();
            try
            {
                if (API_Authentication(SECURITYKEY))
                {
                    //if (!APICount())
                    //{
                    //    result.Message = "删除微信官方通讯录次数已经用完！";
                    //}
                    //else
                    //{

                    string accessToken = WeChatTools.GetAccessoken(true);
                    U_WeChatRegistered user = Extend.JsonToModel<U_WeChatRegistered>(pushparam);

                    string pushResult = WeChatTools.CreateUser(accessToken, user);
                    var wechatResult = Extend.JsonToModel<U_WechatResult>(pushResult);
                    if (wechatResult.errcode == "0")
                    {
                        UserInfoLogic logic = new UserInfoLogic();
                        logic.InsertTrainers(user);
                        result.Success = true;
                    }
                    result.Message = pushResult;


                    //string accessToken = WeChatTools.GetAccessoken(true);
                    //U_WeChatRegistered user = Extend.JsonToModel<U_WeChatRegistered>(pushparam);
                    //var wResult = WeChatTools.GetUserInfoByUserID(accessToken, user.idcard);
                    //var wechatResult = Extend.JsonToModel<U_WechatResult>(wResult);
                    //if (wechatResult.errcode == "0")
                    //{
                    //    WeChatTools.DeleteUser(accessToken, user.idcard);
                    //}
                    //wResult = WeChatTools.GetUserId(accessToken, user.mobile);
                    //Dictionary<string, string> dicReuslt = Extend.JsonToModel<Dictionary<string, string>>(wResult);
                    //if (dicReuslt["errcode"] == "0")
                    //{
                    //    string userid = dicReuslt["userid"];
                    //    WeChatTools.DeleteUser(accessToken, userid);
                    //}

                    //string pushResult = WeChatTools.CreateUser(accessToken, user);
                    //wechatResult = Extend.JsonToModel<U_WechatResult>(pushResult);
                    //if (wechatResult.errcode == "0")
                    //{
                    //    UserInfoLogic logic = new UserInfoLogic();
                    //    logic.InsertTrainers(user);
                    //    result.Success = true;
                    //}
                    //result.Message = pushResult;
                    //}
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
            ExecHistry("WeChatRegistered", pushparam, result.Success.ToString());
            return Json(result);
        }


        private bool APICount()
        {
            int apiCount = ConfigSugar.GetAppInt("APICount");
            string path = this.Server.MapPath("APICount");
            string fileName = "apicount.ini";
            string filePath = Path.Combine(path, fileName);
            string fileContent = System.IO.File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(fileContent))
            {
                fileContent = DateTime.Now.ToString("yyyy-MM-dd") + "|0";
            }
            string[] fileStrs = fileContent.Split('|');
            int count = 0;
            if (DateTime.Now.ToString("yyyy-MM-dd") == fileStrs[0])
            {
                count = Convert.ToInt32(fileStrs[1]);
            }
            if (count > apiCount)
            {
                return false;
            }
            else
            {
                count++;
                fileContent = DateTime.Now.ToString("yyyy-MM-dd") + "|" + count.ToString();
                System.IO.File.WriteAllText(filePath, fileContent);
                return true;
            }
        }


        public JsonResult WeChatMobileChange(string SECURITYKEY, string pushparam)
        {

            ExecutionResult result = new ExecutionResult();
            try
            {
                if (API_Authentication(SECURITYKEY))
                {
                    string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken(true);
                    U_WeChatRegistered user = Extend.JsonToModel<U_WeChatRegistered>(pushparam);
                    UserInfoLogic userInfoLogic = new UserInfoLogic();
                    var muser = userInfoLogic.GetPerson(user.idcard);
                    if (muser != null)
                    {
                        userInfoLogic.UpdatePhoneNumber(muser.UserID, user.mobile);
                        //userInfoLogic.DeleteUserInfo(new string[] { muser.Vguid.ToString() });
                        string pushResult = WeChatTools.DeleteUser(accessToken, muser.UserID);
                        var wechatResult = Extend.JsonToModel<U_WechatResult>(pushResult);
                        if (wechatResult.errcode != "0")
                        {
                            result.Message = pushResult;
                        }
                        else
                        {
                            UserInfoLogic logic = new UserInfoLogic();
                            var allTrainers = logic.GetTrainers(user);
                            if (allTrainers != null)
                            {
                                user.userid = allTrainers.IDCard;
                                user.name = allTrainers.Name;
                                user.gender = allTrainers.Gender.ToString();
                                //string pushResult = WeChatTools.WeChatMobileChange(accessToken, muser.UserID, user.mobile);

                                pushResult = WeChatTools.CreateUser(accessToken, user);
                                wechatResult = Extend.JsonToModel<U_WechatResult>(pushResult);
                                if (wechatResult.errcode == "0")
                                {
                                    logic.UpdateTrainers(user);
                                    result.Success = true;
                                }
                                result.Message = pushResult;
                            }
                            else
                            {
                                result.Message = string.Format("该身份证号码:{0} 用户在培训注册表中不存在！", user.idcard);
                            }
                        }
                    }
                    else
                    {
                        result.Message = string.Format("该身份证号码:{0} 用户不存在", user.idcard);
                    }
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
            ExecHistry("WeChatRegistered", pushparam, result.Success.ToString());
            return Json(result);
        }


        protected string GetPushJson(bool OAuth2, List<PushParamModel> pushMsg)
        {
            var agentid = SyntacticSugar.ConfigSugar.GetAppString("WeChatAgentID", "1");
            string _oAuthUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base#wechat_redirect";
            string responeJsonStr = "";
            responeJsonStr = "{";
            responeJsonStr += "\"touser\": \"" + pushMsg[0].PushPeoples + "\",";
            responeJsonStr += "\"toparty\": \"\",";
            responeJsonStr += "\"totag\": \"\",";
            responeJsonStr += "\"msgtype\": \"news\",";
            responeJsonStr += "\"agentid\":\"" + agentid + "\",";
            responeJsonStr += "\"news\": {";
            responeJsonStr += "\"articles\":[";
            foreach (var content in pushMsg)
            {
                string pushUrl = string.Empty;
                if (OAuth2)
                {
                    pushUrl = string.Format(_oAuthUrl, WxPayConfig.APPID, content.Url);
                }
                else
                {
                    pushUrl = content.Url;
                }

                responeJsonStr += " {";
                responeJsonStr += "\"title\": \"" + content.Title + "\",";
                responeJsonStr += "\"description\": \"" + content.Message + "\",";
                responeJsonStr += "\"url\": \"" + pushUrl + "\",";
                responeJsonStr += "\"picurl\": \"" + content.Image + "\"";
                responeJsonStr += "}";
                responeJsonStr += ",";
            }
            responeJsonStr += "]";
            responeJsonStr += "}";
            responeJsonStr += "}";
            return responeJsonStr;

        }


        public JsonResult TextCardPush(string SECURITYKEY, string pushparam, bool OAuth2 = false)
        {
            ExecutionResult result = new ExecutionResult();
            try
            {
                if (API_Authentication(SECURITYKEY))
                {
                    PushParamModel textPush = Extend.JsonToModel<PushParamModel>(pushparam);
                    var textPushs = new List<PushParamModel> { textPush };
                    result = SaveGraphicPushData(textPushs);
                    string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
                    string _sendUrl = "https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}";
                    string postUrl = string.Format(_sendUrl, accessToken);
                    //获取推送内容Json
                    string json = GetTextCardPushJson(OAuth2, textPush);
                    string pushResult = WeChatTools.PostWebRequest(postUrl, json, Encoding.UTF8);
                    var wechatResult = Extend.JsonToModel<U_WechatResult>(pushResult);
                    if (wechatResult.errcode == "0")
                    {
                        result.Success = true;
                    }
                    result.Message = pushResult;
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
            ExecHistry("WeChatRegistered", pushparam, result.Success.ToString());
            return Json(result);
        }


        protected string GetTextCardPushJson(bool OAuth2, PushParamModel pushMsg)
        {
            var agentid = SyntacticSugar.ConfigSugar.GetAppString("WeChatAgentID", "1");
            string _oAuthUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base#wechat_redirect";
            string responeJsonStr = "";
            responeJsonStr = "{";
            responeJsonStr += "\"touser\": \"" + pushMsg.PushPeoples + "\",";
            responeJsonStr += "\"toparty\": \"\",";
            responeJsonStr += "\"totag\": \"\",";
            responeJsonStr += "\"msgtype\": \"textcard\",";
            responeJsonStr += "\"agentid\":\"" + agentid + "\",";
            responeJsonStr += "\"textcard\": {";

            string pushUrl = string.Empty;
            if (OAuth2)
            {
                pushUrl = string.Format(_oAuthUrl, WxPayConfig.APPID, pushMsg.Url);
            }
            else
            {
                pushUrl = pushMsg.Url;
            }
            responeJsonStr += "\"title\": \"" + pushMsg.Title + "\",";
            responeJsonStr += "\"description\": \"" + pushMsg.Message + "\",";
            responeJsonStr += "\"url\": \"" + pushUrl + "\",";
            //responeJsonStr += "\"picurl\": \"" + pushMsg.Image + "\"";
            responeJsonStr += "\"btntxt\": \"更多\"";
            responeJsonStr += "}";
            responeJsonStr += "}";
            return responeJsonStr;
        }


        public JsonResult NotificationSMS(string SECURITYKEY, string pushparam)
        {

            ExecutionResult result = new ExecutionResult();
            try
            {
                if (API_Authentication(SECURITYKEY))
                {
                    result = SaveNotificationSMS(pushparam);
                    if (result.Success)
                    {
                        HttpClient httpClient = new HttpClient();
                        //将服务凭证转换为Base64编码格式  
                        byte[] auth = Encoding.UTF8.GetBytes(string.Format("{0}:{1}", ConfigSugar.GetAppString("SMSAppKey"), ConfigSugar.GetAppString("MasterSecret")));
                        String auth64 = Convert.ToBase64String(auth);
                        //创建并指定服务凭证，认证方案为Basic  
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", auth64);
                        string json = pushparam;
                        System.Net.Http.StringContent sc = new System.Net.Http.StringContent(json);
                        sc.Headers.Remove("Content-Type");
                        sc.Headers.Add("Content-Type", "application/json");
                        Task<HttpResponseMessage> taskHrm = httpClient.PostAsync("https://api.sms.jpush.cn/v1/messages", sc);
                        //Task.WaitAll(taskHrm);
                        Task<string> taskStr = taskHrm.Result.Content.ReadAsStringAsync();
                        result.Message = taskStr.Result;
                        //Task.WaitAll(taskStr);
                        if (result.Message.Contains("msg_id"))
                        {
                            result.Success = true;
                        }
                        else
                        {
                            result.Success = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                LogHelper.WriteLog(ex.Message);
            }
            ExecHistry("WeChatRegistered", pushparam, result.Success.ToString());
            return Json(result);

        }


        public ExecutionResult SaveNotificationSMS(string pushparam)
        {
            ExecutionResult result = new ExecutionResult();
            U_PushMsg pushMsg = new U_PushMsg();
            pushMsg.VGUID = Guid.NewGuid();
            pushMsg.PushType = 2;
            pushMsg.MessageType = 3;

            pushMsg.Title = "短信发送";
            pushMsg.Message = pushparam;
            pushMsg.PushDate = DateTime.Now;
            pushMsg.PushPeople = "接口短信";
            pushMsg.PeriodOfValidity = DateTime.Now.AddMonths(1);
            pushMsg.CreatedUser = "微企推送";
            pushMsg.CreatedDate = DateTime.Now;
            List<Business_WeChatPush_MoreGraphic_Information> pushMoreGraphicList = new List<Business_WeChatPush_MoreGraphic_Information>();
            DraftInfoLogic logic = new DraftInfoLogic();
            Guid vguid = Guid.Empty;
            result.Success = logic.APISaveImagePushMsg(pushMsg, pushMoreGraphicList);
            result.Result = new { Uniquekey = pushMsg.VGUID };
            return result;
        }



        //public JsonResult WeChatRegistered(string SECURITYKEY, string pushparam)
        //{
        //    ExecutionResult result = new ExecutionResult();
        //    try
        //    {
        //        if (API_Authentication(SECURITYKEY))
        //        {
        //            WeChatRegisteredModel textPush = Extend.JsonToModel<WeChatRegisteredModel>(pushparam);
        //            string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
        //            string _sendUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/create?access_token={0}";
        //            string postUrl = string.Format(_sendUrl, accessToken);
        //            //获取推送内容Json
        //            string json = GetWeChatRegisteredPushJson(textPush);
        //            string pushResult = WeChatTools.PostWebRequest(postUrl, json, Encoding.UTF8);
        //            var wechatResult = Extend.JsonToModel<U_WechatResult>(pushResult);
        //            if (wechatResult.errcode == "0")
        //            {
        //                result.Success = true;
        //            }
        //            result.Message = pushResult;
        //        }
        //        else
        //        {
        //            result.Message = "SECURITYKEY 无效！";
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        result.Message = ex.Message;
        //        LogHelper.WriteLog(ex.Message);
        //    }
        //    return Json(result);
        //}

        //protected string GetWeChatRegisteredPushJson(WeChatRegisteredModel pushMsg)
        //{
        //    var agentid = SyntacticSugar.ConfigSugar.GetAppString("WeChatAgentID", "1");
        //    string responeJsonStr = "";
        //    responeJsonStr = "{";
        //    responeJsonStr += "\"userid\": \"" + pushMsg.userid + "\",";
        //    responeJsonStr += "\"name\":\"" + pushMsg.name + "\",";
        //    responeJsonStr += "\"mobile\": \"" + pushMsg.mobile + "\",";
        //    responeJsonStr += "\"gender\": \"" + pushMsg.gender + "\"";
        //    responeJsonStr += "}";
        //    return responeJsonStr;
        //}


        //public JsonResult WeChatMobileChange(string SECURITYKEY, string pushparam)
        //{
        //    ExecutionResult result = new ExecutionResult();
        //    try
        //    {
        //        if (API_Authentication(SECURITYKEY))
        //        {
        //            WeChatRegisteredModel textPush = Extend.JsonToModel<WeChatRegisteredModel>(pushparam);
        //            string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
        //            string _sendUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/update?access_token={0}";
        //            string postUrl = string.Format(_sendUrl, accessToken);
        //            //获取推送内容Json
        //            string json = GetWeChatMobileChangePushJson(textPush);
        //            string pushResult = WeChatTools.PostWebRequest(postUrl, json, Encoding.UTF8);
        //            var wechatResult = Extend.JsonToModel<U_WechatResult>(pushResult);
        //            if (wechatResult.errcode == "0")
        //            {
        //                result.Success = true;
        //            }
        //            result.Message = pushResult;
        //        }
        //        else
        //        {
        //            result.Message = "SECURITYKEY 无效！";
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        result.Message = ex.Message;
        //        LogHelper.WriteLog(ex.Message);
        //    }
        //    return Json(result);
        //}

        //protected string GetWeChatMobileChangePushJson(WeChatRegisteredModel pushMsg)
        //{
        //    var agentid = SyntacticSugar.ConfigSugar.GetAppString("WeChatAgentID", "1");
        //    string responeJsonStr = "";
        //    responeJsonStr = "{";
        //    responeJsonStr += "\"userid\": \"" + pushMsg.userid + "\",";
        //    responeJsonStr += "\"mobile\": \"" + pushMsg.mobile + "\"";
        //    responeJsonStr += "}";
        //    return responeJsonStr;
        //}
    }
}