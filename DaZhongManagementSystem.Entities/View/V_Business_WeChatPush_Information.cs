﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.View
{
    public class V_Business_WeChatPush_Information
    {
        public int PushType { get; set; }

        public string TranslatePushType { get; set; }

        public string Title { get; set; }

        public int MessageType { get; set; }

        public string TranslateMessageType { get; set; }

        public Boolean Timed { get; set; }

        public DateTime TimedSendTime { get; set; }

        public Boolean Important { get; set; }

        public string Message { get; set; }

        public DateTime PeriodOfValidity { get; set; }

        public DateTime PushDate { get; set; }

        public string PushPeople { get; set; }

        public int Status { get; set; }

        public string TranslateStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedUser { get; set; }

        public DateTime ChangeDate { get; set; }

        public string ChangeUser { get; set; }

        public Guid VGUID { get; set; }

        public string Type { get; set; }

        public string TranslateType { get; set; }

        public string PushObject { get; set; }

        public Guid Business_WeChatPushVguid { get; set; }
    }
}
