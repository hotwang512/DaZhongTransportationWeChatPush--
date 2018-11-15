using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class U_MarkingPerson
    {
        public Guid Vguid { get; set; }

        public string Name { get; set; }

        public string IDNumber { get; set; }

        public Guid OwnedFleet { get; set; }

        public Guid PersonVguid { get; set; }

        public int Marking { get; set; }
    }
}
