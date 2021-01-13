using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.SecondaryCleaningManagement.Models
{
    public class Business_CleaningCompany
    {
        public Guid Vguid { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public string TXLocation { get; set; }
        public string QRCode { get; set; }
        public decimal Radius { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ChangeUser { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}