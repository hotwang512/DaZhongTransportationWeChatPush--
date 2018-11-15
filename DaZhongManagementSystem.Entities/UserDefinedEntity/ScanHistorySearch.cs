using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class ScanHistorySearch
    {
        public string MachineCode { get; set; }

        public string CreatedDateFrom { get; set; }

        public string CreatedDateTo { get; set; }
    }
}
