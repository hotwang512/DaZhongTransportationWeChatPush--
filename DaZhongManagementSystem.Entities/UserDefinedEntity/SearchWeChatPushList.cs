using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class SearchWeChatPushList
    {
        public int? PushType { get; set; }

        public string Title { get; set; }

        public Boolean? Important { get; set; }

        public DateTime? PeriodOfValidity { get; set; }

        public DateTime? PushDate { get; set; }
    }
}
