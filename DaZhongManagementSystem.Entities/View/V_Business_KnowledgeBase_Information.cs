using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.View
{
    public class V_Business_KnowledgeBase_Information
    {
        public string Title { get; set; }

        public string Type { get; set; }

        public string TranslateType { get; set; }

        public string Content { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }

        public string TranslateStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedUser { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string ChangeUser { get; set; }

        public Guid Vguid { get; set; }
    }
}
