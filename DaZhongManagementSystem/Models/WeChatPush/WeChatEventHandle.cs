using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using SyntacticSugar;
using DaZhongManagementSystem.Models.WeChatPush.WeChatMenuButton;

namespace DaZhongManagementSystem.Models.WeChatPush
{
    public class WeChatEventHandle : WeChatHandle
    {
        private string _toUserName;

        public string ToUserName
        {
            get { return _toUserName; }
            set { _toUserName = value; }
        }

        private string _fromUserName;

        public string FromUserName
        {
            get { return _fromUserName; }
            set { _fromUserName = value; }
        }

        private string _msgType;

        public string MsgType
        {
            get { return _msgType; }
            set { _msgType = value; }
        }

        private string _agentID;

        public string AgentID
        {
            get { return _agentID; }
            set { _agentID = value; }
        }

        private string _event;

        public string Event
        {
            get { return _event; }
            set { _event = value; }
        }

        private string _eventKey;

        public string EventKey
        {
            get { return _eventKey; }
            set { _eventKey = value; }
        }
        private U_WeChatCallbackParameter _wcp;

        public U_WeChatCallbackParameter U_WeChatCallbackParameter
        {
            get { return _wcp; }
            set { _wcp = value; }
        }

        private WeChatMenuButtonType _wmButtonType = WeChatMenuButtonType.None;
        /// <summary>
        /// 
        /// </summary>
        public WeChatMenuButtonType WeChatMenuButtonType
        {
            get { return _wmButtonType; }
        }


        private WeChatEventType _weChatEventType = WeChatEventType.none;

        /// <summary>
        /// 
        /// </summary>
        public WeChatEventType WeChatEventType
        {
            get { return _weChatEventType; }
        }
        /// <summary>
        /// 构造一个新的微信事件处理类型
        /// </summary>
        /// <param name="xmldoc"></param>
        public WeChatEventHandle(U_WeChatCallbackParameter wcp, XmlDocument xmldoc)
        {
            _wcp = wcp;
            AnalysisParameter(xmldoc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmldoc"></param>
        private void AnalysisParameter(XmlDocument xmldoc)
        {
            _msgType = xmldoc.SelectSingleNode("/xml/MsgType").InnerText;
            _event = xmldoc.SelectSingleNode("/xml/Event").InnerText;
            _toUserName = xmldoc.SelectSingleNode("/xml/ToUserName").InnerText;
            _fromUserName = xmldoc.SelectSingleNode("/xml/FromUserName").InnerText;
            _eventKey = xmldoc.SelectSingleNode("/xml/EventKey").InnerText;
            _agentID = xmldoc.SelectSingleNode("/xml/AgentID").InnerText;
            _weChatEventType = (WeChatEventType)Enum.Parse(typeof(WeChatEventType), _event);
            if (_weChatEventType == WeChatEventType.click && !string.IsNullOrEmpty(_eventKey))
            {
                _wmButtonType = (WeChatMenuButtonType)Enum.Parse(typeof(WeChatMenuButtonType), _eventKey);
            }
           
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <returns></returns>
        public override string ExecuteEventHandle()
        {
            string result = string.Empty;
            WeChatMenuButtonHandle buttonHandle = WeChatMenuButtonFactory.WeChatButtonHandle(_wmButtonType, this);
            if (buttonHandle != null)
            {
                result = buttonHandle.ExecuteButtonHandle();
            }
            return result;
        }
    }
}
