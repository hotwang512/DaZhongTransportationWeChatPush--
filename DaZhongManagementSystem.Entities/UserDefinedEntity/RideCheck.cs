using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class RideCheck
    {
        public Guid VGUID { get; set; }
        public string 跳车人姓名 { get; set; }

        public string 提交时间 { get; set; }
        public string 跳车时间 { get; set; }
        public string 车号 { get; set; }
        public string 所属公司 { get; set; }
        public string 所属车队 { get; set; }
        public string 上车地点 { get; set; }
        public string 下车地点 { get; set; }
        public string 服务卡号 { get; set; }
        public string 跳车检查结果 { get; set; }
        public string 备注信息 { get; set; }

    }
}
