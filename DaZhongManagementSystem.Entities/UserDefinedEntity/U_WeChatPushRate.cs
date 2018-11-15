using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class U_WeChatPushRate
    {
        public string Title { get; set; }

        public Guid VGUID { get; set; }

        public int SumQTY { get; set; }

        public decimal NoRead { get; set; }

        public decimal Reads { get; set; }
    }
}
