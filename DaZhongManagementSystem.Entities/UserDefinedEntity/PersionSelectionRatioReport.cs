using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class PersionSelectionRatioReport
    {
        public Guid VGUID { get; set; }
        public string Name { get; set; }

        public string IDNumber { get; set; }

        public string Sex { get; set; }

        public string PhoneNumber { get; set; }

        public int Counts { get; set; }
    }
}
