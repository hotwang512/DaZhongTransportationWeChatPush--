using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class ReturnHomeStatistics
    {
        public Guid VGUID { get { return Guid.NewGuid(); } }
        public string OrganizationName { get; set; }

        public string NoReturnHome { get; set; }

        public string ReturnHome { get; set; }
    }
}
