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
        /// <summary>
        /// 根据Post参数获取对应处理事件
        /// </summary>
        /// <param name="postParamater"></param>
        /// <returns></returns>
        public static WeChatHandle GetWeChatHandle(U_WeChatCallbackParameter wcp, Stream inputStream)
        {

            string postParamater = WeChatCallbackLogic.GetPostParameter(wcp, inputStream);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(new System.IO.MemoryStream(System.Text.Encoding.GetEncoding("GB2312").GetBytes(postParamater)));
            XmlNode MsgType = xmldoc.SelectSingleNode("/xml/MsgType");

            WeChatHandle weChatHandle = null;
            if (MsgType != null)
            {
                switch (MsgType.InnerText)
                {
                    case "event":
                        weChatHandle = new WeChatEventHandle(wcp, xmldoc);//事件处理
                        break;
                    case "text":
                        weChatHandle = new WeChatTextHandle(wcp, xmldoc);//事件处理
                        break;
                    default:
                        break;
                }
            }
            return weChatHandle;
        }

    }
}
