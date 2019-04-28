using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity
{
    class Business_RideCheckFeedback_Attachment
    {
        public Guid VGUID { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ChangeUser { get; set; }
        public Guid RideCheckFeedbackVGUID { get; set; }
        public string AttachmentName { get; set; }

        public string AttachmentPath { get; set; }
    }
}
