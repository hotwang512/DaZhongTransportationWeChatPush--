using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.SecondaryCleaningManagement.Models
{
    public class SecondaryCleaning
    {
        public Guid Vguid { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Personnel { get; set; }
        public DateTime OperationDate { get; set; }
        public string CompanyVguid { get; set; }
        public string CompanyName { get; set; }
        public string CouponType { get; set; }
        public string CabLicense { get; set; }
        public string CabOrgName { get; set; }
        public string ManOrgName { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ChangeUser { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}