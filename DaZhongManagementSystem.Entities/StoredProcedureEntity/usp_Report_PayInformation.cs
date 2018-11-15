using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.StoredProcedureEntity
{
    public class usp_Report_PayInformation
    {
        public decimal PaymentAmount { get; set; }

        public decimal ActualAmount { get; set; }

        public decimal CompanyAccount { get; set; }

    }
}
