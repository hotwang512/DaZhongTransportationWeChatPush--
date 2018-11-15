using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.View
{
    public class V_Sys_Role_Module
    {
        public int Reads { get; set; }

        public int Edit { get; set; }

        public int Deletes { get; set; }

        public int Adds { get; set; }

        public int Submit { get; set; }

        public int Approved { get; set; }

        public int Import { get; set; }

        public int Export { get; set; }

        public Guid Vguid { get; set; }

        public Guid ModuleVGUID { get; set; }

        public Guid RoleVGUID { get; set; }

        public int PageID { get; set; }

        public string ParentID { get; set; }

        public string PageName { get; set; }
    }
}
