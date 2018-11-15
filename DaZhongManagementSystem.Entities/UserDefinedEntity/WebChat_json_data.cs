using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class WebChat_json_data
    {
        public string Title { get; set; }

        //public DateTime TimedSendTime { get; set; }

        public List<PushPeople> PushPeoplelist { get; set; }

        //public DateTime PeriodOfValidity { get; set; }

        public string Message { get; set; }

        public class PushPeople
        {
            public string Name { get; set; }
            public string IDcard { get; set; }
        }
           
    }

}
