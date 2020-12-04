using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.PartnerInquiryManagement.Models
{
    public class Accident_cabInfo
    {
        public string driverName { get; set; }
        public string carNo { get; set; }
        public string occurrenceTime { get; set; }
        public string accidentNo { get; set; }
        public string accidentGradeName { get; set; }
        public string accidentTypeName { get; set; }
        public string accidentLocation { get; set; }
    }
}