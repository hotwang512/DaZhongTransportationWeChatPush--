using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity.DriverRevenueTable
{
    public class PaymentMonthly
    {
        public int Id { get; set; }

        public int Version { get; set; }

        public DateTime CreateTime { get; set; }

        public string Creator { get; set; }

        public string CreatorIP { get; set; }

        public DateTime EditTime { get; set; }

        public string Editor { get; set; }

        public string EditorIP { get; set; }

        public int DriverId { get; set; }

        public Decimal InitialAmount { get; set; }

        public Decimal PlanAmount { get; set; }

        public Decimal DueAmount { get; set; }

        public Decimal DebtAmount { get; set; }

        public Decimal PaidAmount { get; set; }

        public DateTime AccountPeriod { get; set; }

        public Decimal OperationDayCount { get; set; }

        public int OrganizationID { get; set; }

        public Decimal RiskAmount { get; set; }

        public int CabID { get; set; }

        public int WorkTypeID { get; set; }

        public int MotorcadeID { get; set; }
    }
}
