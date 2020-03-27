using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Business_HomecomingSurvey
    {
        public string LicensePlate { get; set; }
        public string Name { get; set; }
        public string WhetherReturnHome { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Year { get; set; }
        public Guid Vguid { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ChangeUser { get; set; }
    }
}
