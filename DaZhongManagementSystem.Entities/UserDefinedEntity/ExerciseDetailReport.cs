﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class ExerciseDetailReport
    {
        public Guid Vguid { get; set; }

        public string ExercisesName { get; set; }

        public Guid BusinessExercisesDetailVguid { get; set; }

        public string ExerciseNameDetail { get; set; }

        public int True { get; set; }

        public int False { get; set; }

        public Decimal Correct { get; set; }
    }
}
