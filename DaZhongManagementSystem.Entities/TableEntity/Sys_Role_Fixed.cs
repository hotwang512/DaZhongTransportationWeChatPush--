using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Sys_Role_Fixed
    {
        public string ParentID { get; set; }

        public string PageName { get; set; }

        public int PageID { get; set; }

        public int Reads { get; set; }

        public int Adds { get; set; }

        public int Edit { get; set; }

        public int Deletes { get; set; }

        public int Submit { get; set; }

        public int Approved { get; set; }

        public int Import { get; set; }

        public int Export { get; set; }

        public int Creator { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedUser { get; set; }

        public DateTime ChangeDate { get; set; }

        public string ChangeUser { get; set; }

        public Guid Vguid { get; set; }

        public Guid ModuleVGUID { get; set; }
    }
}
