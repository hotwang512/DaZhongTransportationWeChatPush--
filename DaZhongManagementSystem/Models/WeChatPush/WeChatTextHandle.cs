using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DaZhongManagementSystem.Entities.UserDefinedEntity;

namespace DaZhongManagementSystem.Models.WeChatPush
{
    public class WeChatTextHandle : WeChatHandle
    {
        private U_WeChatCallbackParameter _wcp;
        /// <summary>
        /// 构造一个新的微信事件处理类型
        /// </summary>
        /// <param name="xmldoc"></param>
        public WeChatTextHandle(U_WeChatCallbackParameter wcp, XmlDocument xmldoc)
        {
            _wcp = wcp;
            //AnalysisParameter(xmldoc);
        }

        public override string ExecuteEventHandle()
        {
            return "";
        }
    }
}
