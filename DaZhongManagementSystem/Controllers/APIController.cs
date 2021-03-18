using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.UserInfo.BussinessLogic;
using DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.RideCheckFeedback.BusinessLogic;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.DraftList.BusinessLogic;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Common.LogHelper;
using DaZhongManagementSystem.Common.WeChatPush;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using DaZhongManagementSystem.Models.APIModel;
using Newtonsoft.Json.Linq;
using SqlSugar;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

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
            ExecHistry("UserIDNumber", phoneNumber, JsonHelper.ModelToJson(result));
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
            string userInfoStr = "";
            try
            {
                if (API_Authentication(SECURITYKEY))
                {
                    string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
                    userInfoStr = Common.WeChatPush.WeChatTools.GetUserInfoByCode(accessToken, code);
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
            ExecHistry("UserInformation", code + ",WeChat-Code:" + userInfoStr, JsonHelper.ModelToJson(result));
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
                    if (textPush.PushPeople.Count == 0)
                    {
                        result.Message = "推送人员不能为空！";
                        ExecHistry("TextPush", pushparam, JsonHelper.ModelToJson(result));
                        return Json(result);
                    }
                    foreach (string item in textPush.PushPeople)
                    {
                        var user = userInfoLogic.GetPerson(item);
                        if (user != null && user.UserID != null && user.UserID != "")
                        {
                            pushMsg.PushPeople += user.UserID + "|";
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
                        ExecHistry("TextPush", pushparam, JsonHelper.ModelToJson(result));
                        return Json(result);
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
            ExecHistry("TextPush", pushparam, JsonHelper.ModelToJson(result));
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
                    if (result.Success)
                    {
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
            ExecHistry("GraphicPush", pushparam, JsonHelper.ModelToJson(result));
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
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = result.Message.Remove(result.Message.Length - 1, 1);
                result.Message = "不存在身份证号码：" + result.Message;
                return result;
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
            ExecHistry("WeChatRegistered", pushparam, JsonHelper.ModelToJson(result));
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
            ExecHistry("WeChatRegistered", pushparam, JsonHelper.ModelToJson(result));
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
            ExecHistry("WeChatRegistered", pushparam, JsonHelper.ModelToJson(result));
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

        public JsonResult WXTextPush(string SECURITYKEY, string pushparam)
        {
            ExecutionResult result = new ExecutionResult();
            try
            {
                if (API_Authentication(SECURITYKEY))
                {
                    PushParamModel textPush = Extend.JsonToModel<PushParamModel>(pushparam);
                    var textPushs = new List<PushParamModel> { textPush };
                    //result = SaveGraphicPushData(textPushs);
                    string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
                    string _sendUrl = "https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}";
                    string postUrl = string.Format(_sendUrl, accessToken);
                    //获取推送内容Json
                    string json = GetTextPushJson(textPush);
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
            ExecHistry("WeChatTextPush", pushparam, JsonHelper.ModelToJson(result));
            return Json(result);
        }

        private string GetTextPushJson(PushParamModel pushMsg)
        {
            var agentid = SyntacticSugar.ConfigSugar.GetAppString("WeChatAgentID", "1");
            string responeJsonStr = "";
            responeJsonStr = "{";
            responeJsonStr += "\"touser\": \"" + pushMsg.PushPeoples + "\",";
            responeJsonStr += "\"toparty\": \"\",";
            responeJsonStr += "\"totag\": \"\",";
            responeJsonStr += "\"msgtype\": \"text\",";
            responeJsonStr += "\"agentid\":\"" + agentid + "\",";
            responeJsonStr += "\"text\": {\"content\" : \"" + pushMsg.Message + "\"},";
            responeJsonStr += "\"safe\": \"0\",";
            responeJsonStr += "\"enable_id_trans\": \"0\",";
            responeJsonStr += "\"enable_duplicate_check\": \"0\",";
            responeJsonStr += "\"duplicate_check_interval\": \"1800\"";
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
            ExecHistry("WeChatRegistered", pushparam, JsonHelper.ModelToJson(result));
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

        /// <summary>
        /// 二维码缴费结束线上结果返回
        /// </summary>
        public JsonResult QRCodePayResult(string billNo, string PayStatus, string PayTarget, string TotalAmount, string PayMessage)
        {
            ExecutionResult result = new ExecutionResult();
            try
            {
                //获取支付状态,更新支付历史表
                using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
                {
                    var data = _dbMsSql.Queryable<Business_PaymentHistory_Information>().Where(x => x.Remarks == billNo).FirstOrDefault();
                    if (data != null)
                    {
                        var status = "";
                        switch (PayStatus)
                        {
                            case "PAID": status = "1"; break;//支付成功
                            case "UNPAID": status = "3"; break;//待支付
                            case "REFUND": status = "6"; break;//已退款
                            case "CLOSED": status = "4"; break;//已关闭
                            case "UNKNOWN": status = "5"; break;//未知
                            default: break;
                        }
                        data.PaymentStatus = status;
                        data.PaymentType = PayTarget;
                        data.RevenueReceivable = TotalAmount.TryToDecimal();
                        data.ChangeDate = DateTime.Now;
                        _dbMsSql.Update<Business_PaymentHistory_Information>(data, i => i.Remarks == billNo);
                        result.Success = true;
                        result.Message = "支付状态更新成功";
                    }
                    else
                    {
                        result.Success = true;
                        result.Message = "该单号未找到支付数据";
                    }
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                LogHelper.WriteLog(ex.Message);
            }
            return Json(result);
        }

        /// <summary>
        /// 根据手机号,身份证号删除人员信息
        /// </summary>
        /// <param name="SECURITYKEY">加密值</param>
        /// <param name="phoneNumber">手机号码</param>
        /// /// <param name="IDNumber">身份证号</param>
        /// <returns></returns>
        public JsonResult DeleteUserInfo(string SECURITYKEY, string phoneNumber, string IDNumber)
        {
            ExecutionResult result = new ExecutionResult();
            try
            {
                if (API_Authentication(SECURITYKEY))
                {
                    result.Result = "";
                    //删除本地人员表中信息
                    UserInfoLogic userInfoLogic = new UserInfoLogic();
                    var isDelete = userInfoLogic.DeletePersonInfo(IDNumber);
                    //删除微信官方后台人员信息
                    string accessToken = WeChatTools.GetAccessoken(true);
                    string GetUserInfoByUserID = WeChatTools.GetUserInfoByUserID(accessToken, phoneNumber);
                    string GetUserInfoByUserID2 = WeChatTools.GetUserInfoByUserID(accessToken, IDNumber);
                    U_UserInfo userDetail = JsonHelper.JsonToModel<U_UserInfo>(GetUserInfoByUserID);//用户信息phoneNumber
                    U_UserInfo userDetail2 = JsonHelper.JsonToModel<U_UserInfo>(GetUserInfoByUserID2);//用户信息IDNumber
                    string respText1 = "";
                    string respText2 = "";
                    string respText3 = "";
                    string userid = "";
                    if (userDetail.userid != null)
                    {
                        //手机号是userid
                        respText1 = WeChatTools.DeleteUser(accessToken, phoneNumber);
                    }
                    else
                    {
                        respText1 = "手机号是userid,未查找到人员数据";
                    }
                    if (userDetail2.userid != null)
                    {
                        //身份证是userid
                        respText2 = WeChatTools.DeleteUser(accessToken, IDNumber);
                    }
                    else
                    {
                        respText2 = "身份证是userid,未查找到人员数据";
                    }
                    //根据手机号查询userid
                    var useridJ = WeChatTools.GetUserId(accessToken, phoneNumber);
                    JObject useridJson = JObject.Parse(useridJ);
                    try
                    {
                        userid = useridJson["userid"].ToString();
                        respText3 = WeChatTools.DeleteUser(accessToken, userid);
                    }
                    catch (Exception)
                    {
                        respText3 = "通过手机号,未查找到人员数据";
                    }
                    result.Success = isDelete;
                    result.Result = respText1 + ";" + respText2 + ";" + respText3;
                    ExecHistry("根据身份证删除本地用户,WeChatTools.DeleteUser", IDNumber, isDelete.ToString());
                    ExecHistry("根据手机号删除微信用户,WeChatTools.DeleteUser", phoneNumber, respText1);
                    ExecHistry("根据身份证删除微信用户,WeChatTools.DeleteUser", IDNumber, respText2);
                    ExecHistry("根据手机号查询userid删除微信用户,WeChatTools.DeleteUser", userid, respText3);
                    ExecHistry("根据手机号查询userid,WeChatTools.GetUserId", phoneNumber, userid);
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

            return Json(result, JsonRequestBehavior.AllowGet);
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
