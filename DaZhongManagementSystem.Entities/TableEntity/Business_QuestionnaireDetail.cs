using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Business_QuestionnaireDetail
    {
        public string QuestionnaireDetailName { get; set; }
        public string QuestionnaireDetailType { get; set; }
        public string Answer { get; set; }
        public string QuestionTitleID { get; set; }
        public string QuestionOption { get; set; }
        public Guid QuestionnaireVguid { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string ChangeUser { get; set; }
        public Guid Vguid { get; set; }

    }
}
