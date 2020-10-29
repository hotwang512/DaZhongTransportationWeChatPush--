using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Business_VehicleRepairComplaints
    {
        public Guid VGUID { get; set; }

        public string ReflectCategory { get; set; }

        public string ReflectName { get; set; }

        public string ReflectJobNumber { get; set; }

        public string CarNumber { get; set; }

        public string Subsidiary { get; set; }

        public string ContactNumber { get; set; }

        public string ReflectCarModel { get; set; }

        public string ReflectContent { get; set; }

        public string ProblemPhoto { get; set; }

        public string WorkOrderNumber { get; set; }

        public string IncidentTime { get; set; }

        public DateTime ReflectDate { get; set; }

        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ChangeUser { get; set; }
    }
}
