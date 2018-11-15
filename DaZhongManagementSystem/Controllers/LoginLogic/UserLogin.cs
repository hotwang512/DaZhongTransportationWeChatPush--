using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.UserLogin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Controllers.LoginLogic
{
    public class UserLogin
    {
        public UserLoginServer _us;
        public UserLogin()
        {
            _us = new UserLoginServer();
        }

        /// <summary>
        /// 判断登录的用户名和密码是否正确
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public string ProcessLogin(string userName, string pwd)
        {
            return _us.ProcessLogin(userName, pwd);
        }

        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public V_User_Information GeUserManagement(string userName)
        {
            return _us.GeUserManagement(userName);
        }
    }
}