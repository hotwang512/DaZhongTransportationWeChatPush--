using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class JsonResultModel<T>
    {
        public List<T> Rows { get; set; }
        public int TotalRows { get; set; }
    }
}
