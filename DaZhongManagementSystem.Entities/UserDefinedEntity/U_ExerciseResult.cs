using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    /// <summary>
    /// 提交全部习题后实体
    /// </summary>
    public class U_ExerciseResult
    {
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool isComplete { get; set; }

        /// <summary>
        /// 总分数
        /// </summary>
        public int score { get; set; }
    }
}
