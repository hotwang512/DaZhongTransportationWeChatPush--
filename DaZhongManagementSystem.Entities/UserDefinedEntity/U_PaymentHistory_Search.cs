using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class U_PaymentHistory_Search
    {
        public string Name { get; set; }

        public string IDNumber { get; set; }

        public string JobNumber { get; set; }

        public string PhoneNumber { get; set; }

        public string TransactionID { get; set; }

        public string Department { get; set; }

        public DateTime? PayDateFrom { get; set; }

        public DateTime? PayDateTo { get; set; }

        public string PaymentStatus { get; set; }
    }
}
