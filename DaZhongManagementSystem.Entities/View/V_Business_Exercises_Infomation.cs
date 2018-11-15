using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.View
{
    public class V_Business_Exercises_Infomation
    {
        public string ExercisesName { get; set; }

        public string Description { get; set; }

        public string Remarks { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public int? Status { get; set; }

        public string TranslateStatus { get; set; }

        public int? InputType { get; set; }

        public string TranslateInputType { get; set; }

        public Guid Vguid { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedUser { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string ChangeUser { get; set; }
    }
}
