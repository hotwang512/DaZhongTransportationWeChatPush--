using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Models.WeChatPush.WeChatMenuButton
{
    public enum WeChatMenuButtonType
    {
        /// <summary>
        /// 默认
        /// </summary>
        None = 0,
        /// <summary>
        /// 营收查询按钮
        /// </summary>
        SearchRevenue = 1,
        /// <summary>
        /// 投诉记录按钮
        /// </summary>
        ComplaintRecords = 2
    }
}
