using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Models.APIModel
{
    public class U_WechatUsersResult
    {
        public string errcode { get; set; }

        public string errmsg { get; set; }

        public List<U_WechatUser> userlist { get; set; } = new List<U_WechatUser>();

    }
}