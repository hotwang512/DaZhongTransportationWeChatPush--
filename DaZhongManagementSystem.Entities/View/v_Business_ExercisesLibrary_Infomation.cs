using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;

namespace DaZhongManagementSystem.Entities.View
{
    public class v_Business_ExercisesLibrary_Infomation
    {

        public string ExerciseName { get; set; }

        public string Option { get; set; }
        public string Answer { get; set; }
        public int Score { get; set; }
        //public string Description { get; set; }

        //public string Remarks { get; set; }

        //public DateTime? EffectiveDate { get; set; }

        public int ExerciseType { get; set; }
        public string TranslateExerciseType { get; set; }
        public int? Status { get; set; }

        public string TranslateStatusExerciseType { get; set; }

        public int? InputType { get; set; }

        public string TranslateInputType { get; set; }
        public Guid Vguid { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedUser { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string ChangeUser { get; set; }
    }
}
