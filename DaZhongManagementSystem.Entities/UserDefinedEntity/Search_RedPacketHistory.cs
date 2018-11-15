using System;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class Search_RedPacketHistory
    {

        public string Name { get; set; }

        public string UserID { get; set; }

        public int? RedpacketStatus { get; set; }

        public DateTime? ReceiveDateFrom { get; set; }

        public DateTime? ReceiveDateTo { get; set; }
    }
}