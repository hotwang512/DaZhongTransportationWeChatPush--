using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class ResultModel
    {
        public bool isSuccess { get; set; }
        public string respnseInfo { get; set; }
        public string ResponseMessage { get; set; }

        public string ErrorDataTable { get; set; }
    }
}
