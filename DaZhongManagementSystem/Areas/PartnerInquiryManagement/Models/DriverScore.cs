using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.PartnerInquiryManagement.Models
{
    public class DriverScore
    {
        public string Name { get; set; }
        public string DriverId { get; set; }
        public int? Score { get; set; }
    }
}