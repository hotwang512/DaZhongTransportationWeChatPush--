using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Business_RideCheckFeedback
    {
        public Guid VGUID { get; set; } = Guid.NewGuid();
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ChangeUser { get; set; }

        public string Status { get; set; }

        public string FeedbackCode { get; set; }

        public List<Business_RideCheckFeedback_Item> RideCheckFeedback_Items { get; set; } = new List<Business_RideCheckFeedback_Item>();

        public List<Business_RideCheckFeedback_Attachment> RideCheckFeedback_Attachments { get; set; } = new List<Business_RideCheckFeedback_Attachment>();
    }
}
