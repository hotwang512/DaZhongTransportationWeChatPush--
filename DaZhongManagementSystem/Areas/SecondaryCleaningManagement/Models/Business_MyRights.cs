using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.SecondaryCleaningManagement.Models
{
    public class Business_MyRights
    {
        public Guid VGUID { get; set; }
        public string RightsName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime? StartValidity { get; set; }
        public DateTime? EndValidity { get; set; }
        public DateTime? UsageTime { get; set; }
        public int NumberTimes { get; set; }
        public string Status { get; set; }
        public string UserVGUID { get; set; }
        public string EquityVGUID { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ChangeUser { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}