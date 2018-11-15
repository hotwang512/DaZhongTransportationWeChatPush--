using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.StoredProcedureEntity
{
    public class usp_getOrganization
    {
        public string UserID { get; set; }

        public string Name { get; set; }

        public string OrganizationName { get; set; }

        public Guid Vguid { get; set; }

        public Guid ParentVguid { get; set; }
    }
}
