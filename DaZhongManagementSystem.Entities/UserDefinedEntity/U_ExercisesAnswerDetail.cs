using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class U_ExercisesAnswerDetail
    {
        //提交每一道题实体
        public string Answer { get; set; }//答案     

        public Guid BusinessExercisesVguid { get; set; }//习题主表Vguid

        public Guid BusinessExercisesDetailVguid { get; set; }//习题详情Vguid

        public Guid BusinessPersonnelVguid { get; set; }//答题人Vguid
    }
}
