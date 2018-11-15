using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Models.WeChatPush.WeChatMenuButton
{
    public class ComplaintRecordsButton : WeChatMenuButtonHandle
    {
        private WeChatMenuButtonType _buttonMenuType;
        private WeChatEventHandle _eventHandle;

        public ComplaintRecordsButton(WeChatMenuButtonType menuButtonType, WeChatEventHandle eventHandle)
        {
            _buttonMenuType = menuButtonType;
            _eventHandle = eventHandle;
        }

        public override string ExecuteButtonHandle()
        {
            throw new NotImplementedException();
        }
    }
}
