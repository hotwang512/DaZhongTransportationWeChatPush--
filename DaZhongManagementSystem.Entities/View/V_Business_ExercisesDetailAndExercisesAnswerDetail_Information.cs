using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.View
{
    public class V_Business_ExercisesDetailAndExercisesAnswerDetail_Information
    {
        public int TotalScore { get; set; }

        public int Marking { get; set; }

        public string TranslateMarking { get; set; }

        public int Status { get; set; }

        public string TranslateStatus { get; set; }

        public string ExercisesAnswerDetailAnswer { get; set; }

        public int ExercisesAnswerDetailScore { get; set; }

        public string ExercisesDetailAnswer { get; set; }

        public int ExercisesDetailScore { get; set; }

        public int ExerciseType { get; set; }

        public string TranslateExerciseType { get; set; }

        public int ExericseTitleID { get; set; }

        public string ExerciseName { get; set; }

        public string ExerciseOption { get; set; }

        public int InputType { get; set; }

        public string TranslateInputType { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedUser { get; set; }

        public DateTime ChangeDate { get; set; }

        public string ChangeUser { get; set; }

        public Guid ExercisesInformationVguid { get; set; }

        public Guid ExercisesAnswerDetailVGUID { get; set; }

        public Guid BusinessAnswerExercisesVguid { get; set; }

        public Guid ExercisesDetailVGUID { get; set; }

        public Guid BusinessExercisesDetailVguid { get; set; }

        public Guid BusinessPersonnelVguid { get; set; }
    }
}
