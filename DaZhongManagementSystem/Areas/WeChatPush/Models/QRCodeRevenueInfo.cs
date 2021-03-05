using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.WeChatPush.Models
{
    public class QRCodeRevenueInfo
    {
        public string Code { get; set; }
        public string message { get; set; }
        public QRCodeRevenue QRCodeRevenue { get; set; }
    }

    public class QRCodeRevenue
    {
        public string AccountPeriod { get; set; }
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public string OperationNo { get; set; }
        public string DebtAmount { get; set; }
        public string DueAmount { get; set; }
        public string PaidAmount { get; set; }
        public string PayDebtAmount { get; set; }
        public string BillNo { get; set; }
        public string BillDate { get; set; }
        public string BillQRCodeURL { get; set; }
        public string ResponseTimestamp { get; set; }
        public string Fee { get; set; }
        public string FeeMoney { get; set; }
        public string TotalAmount { get; set; }
    }
}