using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.View
{
    public class V_Business_WeChatPushMain_Information
    {
        public int PushType { get; set; }

        public string TranslatePushType { get; set; }

        public string Title { get; set; }

        public int MessageType { get; set; }

        public string TranslateMessageType { get; set; }

        public Boolean Timed { get; set; }

        public DateTime? TimedSendTime { get; set; }

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

        public string History { get; set; }

        public Guid ExercisesVGUID { get; set; }

        public Guid Department_VGUID { get; set; }

        public string Label { get; set; }

    }
}
