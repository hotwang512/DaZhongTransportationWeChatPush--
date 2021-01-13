using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.SecondaryCleaningManagement.Models
{
    public class Business_EquityAllocation
    {
        public Guid VGUID { get; set; }
        public string RightsName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string ValidType { get; set; }
        public DateTime? StartValidity { get; set; }
        public DateTime? EndValidity { get; set; }
        public string Status { get; set; }
        public string Period { get; set; }
        public string PushObject { get; set; }
        public string PushPeople { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ChangeUser { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}