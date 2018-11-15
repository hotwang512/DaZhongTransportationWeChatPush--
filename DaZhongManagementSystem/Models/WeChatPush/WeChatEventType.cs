using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Models.WeChatPush
{
    /// <summary>
    /// 微信回调模式 按钮事件类型
    /// </summary>
    public enum WeChatEventType
    {
        none=0,
        click=1,
        view=2,
        scancode_push=3,
        scancode_waitmsg = 4,
        pic_sysphoto=5,
        pic_photo_or_album=6,
        pic_weixin=7,
        location_select=8,
        enter_agent=9
    }
}