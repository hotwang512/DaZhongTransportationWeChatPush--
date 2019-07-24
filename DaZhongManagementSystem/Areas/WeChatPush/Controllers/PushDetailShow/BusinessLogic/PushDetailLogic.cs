using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.DraftManagement;
using System;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.PushDetailShow.BusinessLogic
{
    public class PushDetailLogic
    {
        private readonly PushDetailServer _ps;

        public PushDetailLogic()
        {
            _ps = new PushDetailServer();

        }

        /// <summary>
        /// 通过vguid获取推送信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public Business_WeChatPush_Information GetPushDetail(string vguid)
        {
            return _ps.GetPushDetail(vguid);
        }

        /// <summary>
        /// 更新用户是否阅读推送状态
        /// </summary>
        /// <param name="userID">用户微信UserID</param>
        /// <param name="pushVguid">推送内容Vguid</param>
        /// <returns></returns>
        public bool UpdateIsRead(string userID, string pushVguid)
        {
            bool result = false;
            Guid guid = Guid.Empty;
            if (Guid.TryParse(pushVguid, out guid))
            {
                result = _ps.UpdateIsRead(userID, guid);
            }
            return result;
        }

        /// <summary>
        /// 获取倒计时内容
        /// </summary>
        /// <returns></returns>
        public string GetCountDown()
        {
            return _ps.GetCountDown();
        }

        /// <summary>
        /// 新增协议操作信息
        /// </summary>
        /// <param name="agreementInfo">协议操作信息</param>
        /// <returns></returns>
        public bool CreateAgreementOperationInfo(Business_ProtocolOperations_Information agreementInfo)
        {
            return _ps.CreateAgreementOperationInfo(agreementInfo);
        }

        /// <summary>
        /// 用户是否已经操作过协议
        /// </summary>
        /// <param name="agreementInfo"></param>
        /// <returns></returns>
        public bool IsExistAgreementOperationInfo(Business_ProtocolOperations_Information agreementInfo)
        {
            return _ps.IsExistAgreementOperationInfo(agreementInfo);
        }
    }
}