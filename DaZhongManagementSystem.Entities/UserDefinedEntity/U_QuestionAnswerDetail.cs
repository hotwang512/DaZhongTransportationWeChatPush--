using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class U_QuestionAnswerDetail
    {
        //提交每一道题实体
        public string Answer { get; set; }//答案     

        public Guid BusinessQuestionnaireVguid { get; set; }//问卷主表Vguid

        public Guid BusinessQuestionnaireDetailVguid { get; set; }//问卷详情Vguid

        public Guid BusinessPersonnelVguid { get; set; }//答题人Vguid
    }
}
