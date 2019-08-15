using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Models.APIModel
{
    public class PushParamModel
    {
        public string Title { get; set; }
        public string Message { get; set; }

        public string Url { get; set; }

        public string Image { get; set; }

        public List<string> PushPeople { get; set; } = new List<string>();

        public string PushPeoples { get; set; }
        public string founder { get; set; }
    }
}