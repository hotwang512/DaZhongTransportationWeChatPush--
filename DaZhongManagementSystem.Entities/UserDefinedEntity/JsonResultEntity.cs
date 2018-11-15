using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class JsonResultEntity<T1,T2>
    {
        public List<T1> MainRow { get; set; }

        public List<T2> DetailRow { get; set; }
        
    }
}
