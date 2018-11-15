using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.View
{
    public class V_Business_ExercisesDetail_Infomation
    {
        public int ExerciseType { get; set; }

        public string TranslateExerciseType { get; set; }

        public string ExerciseName { get; set; }

        public int? ExericseTitleID { get; set; }

        public string ExerciseOption { get; set; }

        public string Answer { get; set; }

        public int Score { get; set; }

        public int InputType { get; set; }

        public string ExerciseTypeInputType { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedUser { get; set; }

        public DateTime ChangeDate { get; set; }

        public string ChangeUser { get; set; }

        public Guid Vguid { get; set; }

        public Guid ExercisesInformationVguid { get; set; }
    }
}
