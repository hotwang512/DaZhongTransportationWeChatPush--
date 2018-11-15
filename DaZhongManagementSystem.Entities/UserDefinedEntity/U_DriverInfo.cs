using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class U_DriverInfo
    {
        /// <summary>
        /// 服务号
        /// </summary>
        public string OperationNo { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string License { get; set; }

        /// <summary>
        /// 公司代码
        /// </summary>
        public string UnitCode { get; set; }
    }
}
