using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class U_ExerciseMainCollect
    {
        public Guid Vguid { get; set; }

        public string ExercisesName { get; set; }

        public int CountPersion { get; set; }

        public int HighScore { get; set; }

        public int LowScore { get; set; }

        public int AvgScore { get; set; }
    }
}
