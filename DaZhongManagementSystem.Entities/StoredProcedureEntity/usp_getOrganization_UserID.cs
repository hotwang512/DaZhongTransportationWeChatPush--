using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.StoredProcedureEntity
{
    public class usp_getOrganization_UserID
    {

        public string UserID { get; set; }

        public string Name { get; set; }

        public string OrganizationName { get; set; }

        public string Vguid { get; set; }

        public string IDNumber { get; set; }

        public string OwnedFleet { get; set; }
    }
}
