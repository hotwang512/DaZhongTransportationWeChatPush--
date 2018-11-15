using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    /// <summary>
    /// 微信回调参数
    /// </summary>
    public class U_WeChatCallbackParameter
    {
        /// <summary>
        /// 微信加密签名，msg_signature结合了企业填写的token、请求中的timestamp、nonce参数、加密的消息体
        /// </summary>
        public string msg_signature { set; get; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { get; set; }

        /// <summary>
        /// 随机数
        /// </summary>
        public string nonce { get; set; }
        /// <summary>
        /// 加密的随机字符串，以msg_encrypt格式提供。需要解密并返回echostr明文，解密后有random、msg_len、msg、$CorpID四个字段，其中msg即为echostr明文
        /// </summary>
        public string echostr { get; set; }
        /// <summary>
        /// 加密的随机字符串，以msg_encrypt格式提供。需要解密并返回echostr明文，解密后有random、msg_len、msg、$CorpID四个字段，其中msg即为echostr明文
        /// </summary>
        public string encrypt { get; set; }


    }
}
