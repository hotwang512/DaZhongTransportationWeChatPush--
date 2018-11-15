using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class v_Business_PersonnelDepartment_Information
    {
        public string UserID { get; set; }

        public string IDNumber { get; set; }

        public string name { get; set; }
        
        public string Sex { get; set; }

        public string JobNumber { get; set; }

        public string ServiceNumber { get; set; }
        
        public Guid OwnedFleet { get; set; }

        public string TranslationOwnedFleet { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public int ApprovalStatus { get; set; }

        public int ApprovalType { get; set; }
        
        public string  TranslationApprovalStatus { get; set; }

        public Guid vguid { get; set; }

        public string LabName { get; set; }
    }
}
