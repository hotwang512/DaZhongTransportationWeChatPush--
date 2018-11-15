using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Master_Configuration
    {
        public int ID { get; set; }

        public string ConfigValue { get; set; }

        public string ConfigDescription { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModifyUser { get; set; }

        public DateTime? ModifyDate { get; set; }

        public Guid VGUID { get; set; }
    }
}
