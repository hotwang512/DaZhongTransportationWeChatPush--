using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.PartnerInquiryManagement.Models
{
    public class Personnel_Info
    {
        public string Name { get; set; }
        public string IdCard { get; set; }
        public string CabLicense { get; set; }
        public string CabVMLicense { get; set; }
        public string MotorcadeName { get; set; }
        public string Organization { get; set; }
        public Guid OldMotorcadeName { get; set; }
        public string OldOrganization { get; set; }
    }
}