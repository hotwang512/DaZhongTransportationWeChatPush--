using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SyntacticSugar;
using SqlSugar;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using DaZhongManagementSystem.Common.LogHelper;


namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers
{
    /// <summary>
    /// 接口推送
    /// </summary>
    public class PushAPIController : Controller
    {
        //
        // GET: /WeChatPush/PushAPI/
        /// <summary>
        /// 微信纯文本推送
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult Push_WebChat_Message(string title, string message, string idcard)
        {
            PushAPIresult PushAPIresult = new PushAPIresult();
            try
            {
                Business_WeChatPush_Information weChatMain = new Business_WeChatPush_Information();

                if (string.IsNullOrEmpty(title))//标题不为空                   
                {
                    PushAPIresult.Succeed = "false";
                    PushAPIresult.ErrorMessage = "必填字段存在空数据，检查标题";
                    //返回为空的错误信息
                    return Json(PushAPIresult, JsonRequestBehavior.AllowGet);
                }
                else if (string.IsNullOrEmpty(message))//推送内容不为空
                {
                    PushAPIresult.Succeed = "false";
                    PushAPIresult.ErrorMessage = "必填字段存在空数据，检查推送内容";
                    //返回为空的错误信息
                    return Json(PushAPIresult, JsonRequestBehavior.AllowGet);
                }
                else if (string.IsNullOrEmpty(idcard))//接收者不能为空
                {
                    PushAPIresult.Succeed = "false";
                    PushAPIresult.ErrorMessage = "必填字段存在空数据，检查推送者";
                    //返回为空的错误信息
                    return Json(PushAPIresult, JsonRequestBehavior.AllowGet);
                }
                else if (!findAPIConfig())
                {
                    PushAPIresult.Succeed = "false";
                    PushAPIresult.ErrorMessage = "API发送已被关闭";
                    //返回为空的错误信息
                    return Json(PushAPIresult, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var UserID = "";
                    UserID = findNonexistent(idcard);
                    if (string.IsNullOrEmpty(UserID))
                    {

                        //返回当前身份证不存在的错误信息
                        PushAPIresult.Succeed = "false";
                        PushAPIresult.ErrorMessage = "接收人员不存在";

                        return Json(PushAPIresult, JsonRequestBehavior.AllowGet);
                    }

                    weChatMain.Title = title;
                    weChatMain.PushType = 1;
                    weChatMain.Timed = false;
                    //weChatMain.TimedSendTime = TimedSendTime;
                    weChatMain.PushPeople = "SYSADMIN_API";
                    weChatMain.Important = false;
                    weChatMain.PeriodOfValidity = DateTime.Now.AddMonths(1);
                    weChatMain.MessageType = 1;
                    weChatMain.Message = message;
                    PushAPIresult = SavePushApi(weChatMain, UserID);
                   
                }

                //var Title = title;//标题  必填
                //var PushType = 1;//推送方式  必填
                //var Timed = false;//是否定时发送  必填
                //var TimedSendTime = timedsendtime;//发送时间  可空
                //var pushPeople = pushpeople;//推送接收者  必填
                //var Important = false;//是否永久  必填
                //var PeriodOfValidity = periodofvalidity;//有效日期  必填
                //var MessageType = 1;//微信推送类型  必填
                //var Message = message;//推送内容  必填

            }
            catch (Exception)
            {
                PushAPIresult.Succeed = "false";
                PushAPIresult.ErrorMessage = "保存时报错";
            }
            return Json(PushAPIresult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 当前人不存在与库
        /// </summary>
        /// <param name="IDcard"></param>
        /// <returns></returns>
        public string findNonexistent(string IDcard)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                var id = "";
                id = _dbMsSql.Queryable<Business_Personnel_Information>().Where(i => i.IDNumber == IDcard).Select(i => i.UserID).FirstOrDefault();
                return id;
            }
        }
        /// <summary>
        /// API权限是否开启
        /// </summary>
        /// <returns></returns>
        public bool findAPIConfig()
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                return _dbMsSql.Queryable<Master_Configuration>().Any(i => i.ID == 17 && i.ConfigValue == "1");
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="weChatMain"></param>
        /// <returns></returns>
        public PushAPIresult SavePushApi(Business_WeChatPush_Information weChatMain, string userid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                PushAPIresult PushAPIresult = new PushAPIresult();
                var result = false;
                try
                {

                    _dbMsSql.BeginTran();
                    weChatMain.VGUID = Guid.NewGuid();
                    weChatMain.CreatedDate = DateTime.Now;
                    weChatMain.CreatedUser = "SYSADMIN_API";
                    result = _dbMsSql.Insert<Business_WeChatPush_Information>(weChatMain, false) != DBNull.Value;//保存微信推送信息表
                    PushAPIresult.ErrorMessage = "ok";
                    PushAPIresult.Succeed = result.ToString();

                    Business_WeChatPushDetail_Information BusinessWeChatPushDetailInformation = new Business_WeChatPushDetail_Information();
                    BusinessWeChatPushDetailInformation.Type = "1";
                    BusinessWeChatPushDetailInformation.PushObject = userid;
                    BusinessWeChatPushDetailInformation.CreatedDate = DateTime.Now;
                    BusinessWeChatPushDetailInformation.CreatedUser = "SYSADMIN_API";
                    BusinessWeChatPushDetailInformation.Vguid = Guid.NewGuid();
                    BusinessWeChatPushDetailInformation.Business_WeChatPushVguid = weChatMain.VGUID;
                    _dbMsSql.Insert<Business_WeChatPushDetail_Information>(BusinessWeChatPushDetailInformation, false);//保存微信推送详细信息表


                    _dbMsSql.CommitTran();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.ToString() + ex.StackTrace);
                    PushAPIresult.ErrorMessage = ex.ToString() + "\r\n" + ex.Source + "\r\n" + ex.StackTrace;
                    PushAPIresult.Succeed = "false";
                    _dbMsSql.RollbackTran();
                }
                return PushAPIresult;
            }
        }


    }
}
