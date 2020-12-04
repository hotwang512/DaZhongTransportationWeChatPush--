using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.PartnerInquiryManagement.Models
{
    public class VehicleMaintenanceInfo
    {
        public string MotorcadeName { get; set; }
        public string Name { get; set; }
        public string CabLicense { get; set; }
        public string MaintenanceType { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Address { get; set; }
        public string Yanche { get; set; }
        public string Status { get; set; }
        public string MobilePhone { get; set; }
    }
}