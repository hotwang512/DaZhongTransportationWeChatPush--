using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Models.APIModel
{
    public class NotificationSMSModel
    {
        public string mobile { get; set; }

        public string sign_id { get; set; }

        public string temp_id { get; set; }

        public Dictionary<string, string> temp_para { get; set; } = new Dictionary<string, string>();
    }
}