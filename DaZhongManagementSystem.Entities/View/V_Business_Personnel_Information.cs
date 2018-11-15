using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.View
{
    public class V_Business_Personnel_Information
    {
        public string UserID { get; set; }

        public string Name { get; set; }

        public int DepartmenManager { get; set; }

        public string OrganizationName { get; set; }

        public Guid Vguid { get; set; }

        public Guid ParentVguid { get; set; }

    }
}
