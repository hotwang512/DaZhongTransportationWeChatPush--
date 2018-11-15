using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Car_Maintain
    {
        public string CarNO { get; set; }
        public string JobNumber { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime MaintainDate { get; set; }
        public string MaintainTime { get; set; }
        public int? Type { get; set; }
        public bool IsVerify { get; set; }
        public string Address { get; set; }
        public int? Mileage { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ChangeUser { get; set; }
        public Guid? PersonnelVGUID { get; set; }
        public Guid? PushVGUID { get; set; }
        public Guid Vguid { get; set; }
    }
}
