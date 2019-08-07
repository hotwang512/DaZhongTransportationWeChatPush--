using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class ExecutionResult
    {
        public bool Success { get; set; } = false;

        public string Message { get; set; } = "";

        public object Result { get; set; }
    }
}
