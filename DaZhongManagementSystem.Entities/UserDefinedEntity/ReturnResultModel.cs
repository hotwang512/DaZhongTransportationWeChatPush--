using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class ReturnResultModel<T>
    {

        public bool IsSuccess { get; set; }

        public T ReturnMsg { get; set; }

        public T ResponseInfo { get; set; }
    }


    public class ResultModel<T>
    {

        public bool IsSuccess { get; set; }

        public string ReturnMsg { get; set; }

        public T ResponseInfo { get; set; }
    }
}
