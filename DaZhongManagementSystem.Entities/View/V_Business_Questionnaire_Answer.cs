using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.View
{
    public class V_Business_Questionnaire_Answer
    {
        public string QuestionnaireName { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
        public Guid? QuestionnaireVguid { get; set; }
        public string QuestionnaireAnswerStatus { get; set; }
        public string QuestionnaireAnswerStatusName { get; set; }
        public string ParticipateStatus { get; set; }
        public string ParticipateStatusName { get; set; }
        public Guid? QuestionnaireAnswerVguid { get; set; }
        public Guid? BusinessPersonnelVguid { get; set; }
        public string CreatedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ChangeUser { get; set; }
        public DateTime? ChangeDate { get; set; }
    }
}
