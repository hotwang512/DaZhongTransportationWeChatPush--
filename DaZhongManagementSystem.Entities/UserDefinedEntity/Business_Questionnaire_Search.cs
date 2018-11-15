using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class Business_Questionnaire_Search
    {
        public string QuestionName { get; set; }
        public string EffectiveDate { get; set; }

        public string Status { get; set; }
        public string QuestionType { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedTimeStart { get; set; }

        public string CreatedTimeEnd { get; set; }
    }
}
