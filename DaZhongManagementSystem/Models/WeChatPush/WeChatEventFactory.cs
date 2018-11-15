using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using DaZhongManagementSystem.Entities.UserDefinedEntity;

namespace DaZhongManagementSystem.Models.WeChatPush
{
    public class WeChatEventFactory
    {

        private U_WeChatCallbackParameter _wcp;
        private Stream _inputStream;

        /// <summary>
        /// 构造微信回调工厂
        /// </summary>
        /// <param name="wcp">微信参数</param>
        /// <param name="inputStream">微信 Post 数据</param>
        public WeChatEventFactory(U_WeChatCallbackParameter wcp, Stream inputStream)
        {
            _wcp = wcp;
            _inputStream = inputStream;
        }

        /// <summary>
        /// 根据Post参数获取对应处理事件
        /// </summary>
        /// <param name="postParamater"></param>
        /// <returns></returns>
        public WeChatHandle GetWeChatHandle()
        {

            string postParamater = WeChatCallbackLogic.GetPostParameter(_wcp, _inputStream);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(new System.IO.MemoryStream(System.Text.Encoding.GetEncoding("GB2312").GetBytes(postParamater)));
            XmlNode MsgType = xmldoc.SelectSingleNode("/xml/MsgType");

            WeChatHandle weChatHandle = null;
            if (MsgType != null)
            {
                switch (MsgType.InnerText)
                {
                    case "event":
                        weChatHandle = new WeChatEventHandle(_wcp, xmldoc);//事件处理
                        break;
                    case "text":
                        weChatHandle = new WeChatTextHandle(_wcp, xmldoc);//事件处理
                        break;
                    default:
                        break;
                }
            }
            return weChatHandle;
        }

    }
}
