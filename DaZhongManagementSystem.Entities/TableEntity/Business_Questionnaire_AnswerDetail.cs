using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Business_Questionnaire_AnswerDetail
    {
        /// <summary>
        /// 问卷答案主键
        /// </summary>
        public Guid QuestionnaireAnswerVguid { get; set; }
        /// <summary>
        /// 问卷详情主键
        /// </summary>
        public Guid BusinessQuestionDetailVguid { get; set; }
        public string Answer { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ChangeUser { get; set; }
        public Guid Vguid { get; set; }

    }
}
