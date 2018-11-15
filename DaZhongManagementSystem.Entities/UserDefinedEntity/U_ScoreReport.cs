using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class U_ScoreReport
    {
        public List<U_ExerciseMainCollect> exerciseMain { get; set; }

        public List<U_ExerciseDetailCollect> exerciseDetail { get; set; }
    }
}
