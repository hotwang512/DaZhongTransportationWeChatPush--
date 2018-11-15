using DaZhongManagementSystem.Entities.TableEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class U_ExerciseAllMsg
    {
        /// <summary>
        /// 习题主信息
        /// </summary>
        public Business_Exercises_Infomation ExerciseMain { get; set; }

        /// <summary>
        /// 习题详细信息
        /// </summary>
        public List<Business_ExercisesDetail_Infomation> ExerciseDetail { get; set; }
    }
}
