using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Business_HomecomingSurvey
    {
        public string OrganizationName { get; set; }
        public string LicensePlate { get; set; }
        public string Name { get; set; }
        public string Fleet { get; set; }
        public string WhetherReturnHome { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Year { get; set; }
        public Guid Vguid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string ChangeUser { get; set; }

        public string CheckDrivingG { get; set; }
        public string CheckDrivingGR { get; set; }
        public string CheckDrivingB { get; set; }
        public string CheckDrivingBR { get; set; }
        public string BackCarNo { get; set; }
        public string BackAdress { get; set; }
        public string GoCarNo { get; set; }
    }
}
