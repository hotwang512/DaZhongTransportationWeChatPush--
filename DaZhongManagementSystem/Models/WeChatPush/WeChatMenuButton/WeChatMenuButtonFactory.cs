using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Models.WeChatPush.WeChatMenuButton
{
    /// <summary>
    /// 微信菜单栏点击按钮
    /// </summary>
    public class WeChatMenuButtonFactory
    {
        public static WeChatMenuButtonHandle WeChatButtonHandle(WeChatMenuButtonType menuButtonType, WeChatEventHandle eventHandle)
        {
            WeChatMenuButtonHandle buttonHandle = null;
            switch (menuButtonType)
            {
                case WeChatMenuButtonType.SearchRevenue:
                    buttonHandle = new SearchRevenueButton(menuButtonType, eventHandle);
                    break;
                case WeChatMenuButtonType.ComplaintRecords:
                    buttonHandle = new ComplaintRecordsButton(menuButtonType, eventHandle);
                    break;
                default:
                    buttonHandle = null;
                    break;
            }
            return buttonHandle;
        }
    }
}
