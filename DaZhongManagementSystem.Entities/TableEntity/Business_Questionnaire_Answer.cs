using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Business_Questionnaire_Answer
    {
        public Guid QuestionnaireVguid { get; set; }
        public Guid QuestionnaireDetailVguid { get; set; }
        public Guid Business_PersonnelVguid { get; set; }
        public string Status { get; set; }
        public string ParticipateStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ChangeUser { get; set; }
        public Guid Vguid { get; set; }

    }
}
