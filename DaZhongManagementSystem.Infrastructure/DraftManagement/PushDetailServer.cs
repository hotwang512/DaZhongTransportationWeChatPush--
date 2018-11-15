using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;
using System;
using System.Linq;
using Aspose.Words.Fields;
using DaZhongManagementSystem.Common.LogHelper;

namespace DaZhongManagementSystem.Infrastructure.DraftManagement
{
    public class PushDetailServer
    {
        public LogLogic _logLogic;
        public PushDetailServer()
        {
            _logLogic = new LogLogic();
        }

        /// <summary>
        /// 通过vguid获取推送信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public Business_WeChatPush_Information GetPushDetail(string vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Business_WeChatPush_Information pushDetailModel = new Business_WeChatPush_Information();
                Guid Vguid = Guid.Parse(vguid);
                pushDetailModel = _dbMsSql.Queryable<Business_WeChatPush_Information>().Where(i => i.VGUID == Vguid).SingleOrDefault();
                if (pushDetailModel == null)
                {
                    var moreGraphicModel = _dbMsSql.Queryable<Business_WeChatPush_MoreGraphic_Information>().Where(i => i.VGUID == Vguid).SingleOrDefault();
                    pushDetailModel = _dbMsSql.Queryable<Business_WeChatPush_Information>().Where(i => i.VGUID == moreGraphicModel.WeChatPushVguid).SingleOrDefault();
                    pushDetailModel.Title = moreGraphicModel.Title;
                    pushDetailModel.Message = moreGraphicModel.Message;
                    pushDetailModel.CoverImg = moreGraphicModel.CoverImg;
                    pushDetailModel.CoverDescption = moreGraphicModel.CoverDescption;
                }
                return pushDetailModel;
            }
        }

        /// <summary>
        /// 更新用户是否阅读推送状态
        /// </summary>
        /// <param name="userID">用户微信UserID</param>
        /// <param name="pushVguid">推送内容Vguid</param>
        /// <returns></returns>
        public bool UpdateIsRead(string userID, string pushVguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                try
                {
                    _dbMsSql.BeginTran();
                    Guid vguid = Guid.Parse(pushVguid);
                    result = _dbMsSql.Update<Business_WeChatPushDetail_Information>(new { ISRead = "1" }, i => i.PushObject == userID && i.Business_WeChatPushVguid == vguid);
                    _dbMsSql.CommitTran();
                }
                catch (Exception exp)
                {
                    _dbMsSql.RollbackTran();
                    LogHelper.WriteLog(exp.Message + "/n" + exp.ToString() + "/n" + exp.StackTrace);
                }
                return result;
            }
        }

        /// <summary>
        /// 获取倒计时内容
        /// </summary>
        /// <returns></returns>
        public string GetCountDown()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                string countDown = string.Empty;
                countDown = _dbMsSql.Queryable<Master_Configuration>().Where(i => i.ID == 7).SingleOrDefault().ConfigValue;
                return countDown;
            }
        }

        /// <summary>
        /// 新增协议操作信息
        /// </summary>
        /// <param name="agreementInfo">协议操作信息</param>
        /// <returns></returns>
        public bool CreateAgreementOperationInfo(Business_ProtocolOperations_Information agreementInfo)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                try
                {
                    db.BeginTran();
                    var personInfo = db.Queryable<Business_Personnel_Information>().Where(i => i.Vguid == agreementInfo.PersonnelVGUID).SingleOrDefault();
                    agreementInfo.VGUID = Guid.NewGuid();
                    agreementInfo.OperationTime = DateTime.Now;
                    agreementInfo.CreatedDate = DateTime.Now;
                    agreementInfo.CreatedUser = personInfo.Name;
                    string logData = JsonConverter.Serialize(agreementInfo);
                    _logLogic.SaveLog(1, 34, personInfo.Name, "协议操作", logData);
                    db.Insert(agreementInfo, false);
                    db.CommitTran();
                    return true;
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                    LogHelper.WriteLog(ex.ToString());
                    return false;
                }

            }
        }

        /// <summary>
        /// 用户是否已经操作过协议
        /// </summary>
        /// <param name="agreementInfo"></param>
        /// <returns></returns>
        public bool IsExistAgreementOperationInfo(Business_ProtocolOperations_Information agreementInfo)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                return db.Queryable<Business_ProtocolOperations_Information>().Any(i => i.WeChatPushVGUID == agreementInfo.WeChatPushVGUID && i.PersonnelVGUID == agreementInfo.PersonnelVGUID);
            }
        }
    }
}
