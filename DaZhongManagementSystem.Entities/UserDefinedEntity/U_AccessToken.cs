using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
   public class U_AccessToken
    {

        /// <summary>
        /// 公司id
        /// </summary>
        public string UserKey { get; set; }

        /// <summary>
        /// 公司秘钥
        /// </summary>
        public string UserPassword { get; set; }
    }
}
