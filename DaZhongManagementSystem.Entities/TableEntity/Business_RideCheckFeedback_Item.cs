using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Business_RideCheckFeedback_Item
    {
        public Guid VGUID { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ChangeUser { get; set; }
        public Guid RideCheckFeedbackVGUID { get; set; }
        public int FeedbackNumber { get; set; }
        public string FeedbackName { get; set; }
        public string FeedbackAnswer1 { get; set; }
        public string FeedbackAnswer2 { get; set; }
        public string FeedbackAnswer3 { get; set; }
        public string FeedbackAnswer4 { get; set; }
        public string FeedbackAnswer5 { get; set; }
        public string FeedbackAnswer6 { get; set; }



    }
}
