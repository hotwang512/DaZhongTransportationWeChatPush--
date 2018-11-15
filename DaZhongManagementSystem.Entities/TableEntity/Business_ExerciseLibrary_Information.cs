using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Business_ExerciseLibrary_Information
    {

        public string ExerciseName { get; set; }
        public int? ExerciseType { get; set; }
        public string Option { get; set; }
        public string Answer { get; set; }
        public int Score { get; set; }
        public int InputType { get; set; }
        public int TopicType { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public string CreatedUser { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string ChangeUser { get; set; }

        public Guid Vguid { get; set; }
        public Guid BusinessExercisesVguid { get; set; }
    }
}
