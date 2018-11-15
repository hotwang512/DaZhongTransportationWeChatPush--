using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.View
{
    public class V_Business_ExercisesAndAnswer_Infomation
    {
        public string ExercisesName { get; set; }

        public string Description { get; set; }

        public string Remarks { get; set; }

        public DateTime EffectiveDate { get; set; }

        public int Status { get; set; }

        public string TranslateStatus { get; set; }

        public int InputType { get; set; }

        public string TranslateInputType { get; set; }

        public Guid BusinessExercisesVGUID { get; set; }

        public int TotalScore { get; set; }

        public int Marking { get; set; }

        public string TranslateMarking { get; set; }

        public int ExercisesAnswerStatus { get; set; }

        public string TranslateExercisesAnswerStatus { get; set; }

        public string PicturePath { get; set; }

        public Guid BusinessExercisesAnswerVGUID { get; set; }

        public Guid BusinessPersonnelVguid { get; set; }

        /// <summary>
        /// 记录答题次数
        /// </summary>
        public int SolveNumber { get; set; }

        public string CreatedUser { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ChangeUser { get; set; }

        public DateTime ChangeDate { get; set; }
    }
}
