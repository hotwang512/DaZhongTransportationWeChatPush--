using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Business_RideCheckFeedback
    {
        public Guid VGUID { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ChangeUser { get; set; }

        public string Status { get; set; }

        public string FeedbackCode { get; set; }
    }
}
