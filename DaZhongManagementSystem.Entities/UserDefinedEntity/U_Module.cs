using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    /// <summary>
    /// 模块  权限
    /// </summary>
    public class U_Module
    {
        /// <summary>
        /// 模块
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public int RightType { get; set; }
    }
}
