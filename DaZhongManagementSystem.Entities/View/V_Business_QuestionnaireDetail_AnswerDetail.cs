using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.View
{
    public class V_Business_QuestionnaireDetail_AnswerDetail
    {
        public string Status { get; set; }
        public string StatusName { get; set; }
        public string ParticipateStatus { get; set; }
        public string ParticipateStatusName { get; set; }
        public string Answer { get; set; }
        public string QuestionnaireType { get; set; }
        public string QuestionnaireTypeName { get; set; }
        public string QuestionTitleID { get; set; }
        public string QuestionnaireName { get; set; }
        public string QuestionOption { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string ChangeUser { get; set; }
        public Guid? QuestionnaireAnswerDetailVguid { get; set; }
        public Guid? QuestionnaireAnswerVguid { get; set; }
        public Guid? QuestionnaireDetailVguid { get; set; }
        public Guid? BusinessQuestionDetailVguid { get; set; }
        public Guid? QuestionnaireVguid { get; set; }
        public Guid? BusinessPersonnelVguid { get; set; }
    }
}
