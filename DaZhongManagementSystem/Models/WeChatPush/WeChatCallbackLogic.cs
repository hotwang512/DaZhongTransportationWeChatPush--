using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using SyntacticSugar;
using DaZhongManagementSystem.Common.LogHelper;

namespace DaZhongManagementSystem.Models.WeChatPush
{
    /// <summary>
    /// 微信回调逻辑
    /// </summary>
    public class WeChatCallbackLogic
    {
        /// <summary>
        /// 微信回调消息解密
        /// </summary>
        /// <returns></returns>
        public static string DecryptMessage(U_WeChatCallbackParameter wcp, string postString)
        {
            WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(ConfigSugar.GetAppString("WeChatCallbackToken"), ConfigSugar.GetAppString("WeChatCallbackEncodingAESKey"), ConfigSugar.GetAppString("CorpID"));
            string result = "";
            int errorCode = wxcpt.DecryptMsg(wcp.msg_signature, wcp.timestamp, wcp.nonce, postString, ref result);
            if (errorCode != 0)
            {
                //错误记录日志
                LogHelper.WriteLog(errorCode.ToString());
            }
            return result;

        }
        /// <summary>
        /// 被动响应给微信的数据加密
        /// </summary>
        /// <param name="wcp"></param>
        /// <param name="postString"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string EncryptMessage(U_WeChatCallbackParameter wcp, string postString, string message)
        {
            WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(ConfigSugar.GetAppString("WeChatCallbackToken"), ConfigSugar.GetAppString("WeChatCallbackEncodingAESKey"), ConfigSugar.GetAppString("CorpID"));
            string result = postString;
            int errorCode = wxcpt.EncryptMsg(message, wcp.timestamp, wcp.nonce, ref result);
            if (errorCode != 0)
            {
                //错误记录日志
                LogHelper.WriteLog(errorCode.ToString());
            }
            return result;
        }

        public static string GetPostParameter(U_WeChatCallbackParameter wcp, Stream inputStream)
        {
            Byte[] postBytes = new Byte[inputStream.Length];
            inputStream.Read(postBytes, 0, (Int32)inputStream.Length);
            string postString = Encoding.UTF8.GetString(postBytes);
            string postParmater = WeChatCallbackLogic.DecryptMessage(wcp, postString);
            return postParmater;
        }

        /// <summary>
        /// 根据微信参数执行对应时间消息
        /// </summary>
        /// <param name="postParamter"></param>
        /// <returns></returns>
        public string ExecuteEventHandle(string postParameter)
        {
            return "";

        }

    }
}
