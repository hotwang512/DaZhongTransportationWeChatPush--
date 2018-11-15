using DaZhongManagementSystem.Areas.WeChatPush.Controllers.ShortMsgLogic;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DaZhongManagementSystem.Infrastructure;
using DaZhongManagementSystem.Infrastructure.SystemManagement;

namespace DaZhongManagementSystem.Models.WeChatPush.WeChatMenuButton
{
    /// <summary>
    /// 查询营收信息按钮
    /// </summary>
    public class SearchRevenueButton : WeChatMenuButtonHandle
    {
        public RevenueLogic _rl;
        private WeChatMenuButtonType _buttonMenuType;
        private WeChatEventHandle _eventHandle;
        private ConfigManageServer _cs;
        private ShortMsgServer _ss;
        public SearchRevenueButton(WeChatMenuButtonType menuButtonType, WeChatEventHandle eventHandle)
        {
            _buttonMenuType = menuButtonType;
            _eventHandle = eventHandle;
            _rl = new RevenueLogic();
            _cs = new ConfigManageServer();
            _ss = new ShortMsgServer();
        }

        public override string ExecuteButtonHandle()
        {
            string response = string.Empty;
            string revenueQueryReply = _rl.GetRevenueQueryReply();//获取营收数据查询成功回复
            string revenueQueryRefuse = _rl.GetRevenueQueryRefuse();//获取您不是司机，无法查询营收数据
            string revenueQueryTimeUp = _rl.GetRevenueQueryTimesRefuse();//获取查询营收数据超过次数回复
            int revenueSearchTimes = int.Parse(_rl.GetRevenueSearchTimes());//获取每月可查收营收数据次数
            int queryTimes = _rl.GetUserCurrentMonthQueryTimes(_eventHandle.FromUserName);//获取当前用户当月查询营收次数
            var revenueQueryExceptionReply = _cs.GetConfigList()[12].ConfigValue;   //营收异常回复
            var queryReply = _cs.GetConfigList()[15].ConfigValue;               //微信和短信都为勾选时回复

            string isSmsPush = _ss.GetIsSmsPush();
            string isWeChatPush = _ss.GetIsWeChatPush();
            string isRevenuePush = _ss.GetIsRevenuePush();
            if (isSmsPush == "0" && isWeChatPush == "0" && isRevenuePush == "0")
            {
                response = queryReply;
            }
            else if (queryTimes < revenueSearchTimes)
            {
                int result = _rl.SaveRevenueMsg(_eventHandle.FromUserName);
                if (result == 0)
                {
                    response = revenueQueryExceptionReply;//营收异常回复

                }
                if (result == 1)
                {
                    response = revenueQueryReply;//营收数据已经通过短信发送到您的手机，请及时查收

                }
                else if (result == 3)
                {
                    response = revenueQueryRefuse;//您不是司机，无法查询营收数据
                }
                else
                {
                    response = "查询失败！";
                }
            }
            else
            {
                response = revenueQueryTimeUp;//获取查询营收数据超过次数回复
            }
            string sReplyMsg = string.Format(WeChatReplyType.Message_Text, _eventHandle.FromUserName, _eventHandle.ToUserName, DateTime.Now.Ticks, response);
            string sEncryptMsg = Cryptography.AES_encrypt(sReplyMsg, ConfigSugar.GetAppString("WeChatCallbackEncodingAESKey"), ConfigSugar.GetAppString("CorpID"));
            response = WeChatCallbackLogic.EncryptMessage(_eventHandle.U_WeChatCallbackParameter, sEncryptMsg, sReplyMsg);
            return response;
        }
    }
}
