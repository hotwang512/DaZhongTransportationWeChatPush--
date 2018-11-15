using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Common.WeChatPush
{
    public class WxPayException:Exception
    {
        public WxPayException(string msg)
            : base(msg)
        {

        }
    }
}
