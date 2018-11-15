using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Infrastructure.UserLogin
{
    public class UserLoginServer
    {
        public LogLogic _ll;
        public UserLoginServer()
        {
            _ll = new LogLogic();
        }

        /// <summary>
        /// 判断登录的用户名和密码是否正确
        /// </summary>
        /// <param name="loginName">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public string ProcessLogin(string loginName, string pwd)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bool userResult = _dbMsSql.Queryable<Sys_User>().Any(i => i.LoginName == loginName);
                if (!userResult)
                {
                    return "用户名错误！";
                }
                else
                {
                    bool enableUser = _dbMsSql.Queryable<Sys_User>().Any(i => i.Enable == "1");
                    if (!enableUser)
                    {
                        return "用户已被禁用！";
                    }

                    bool pwdResult = _dbMsSql.Queryable<Sys_User>().Any(i => i.LoginName == loginName && i.Password == pwd);
                    if (!pwdResult)
                    {
                        return "用户密码错误！";
                    }
                }
            }
            //存入操作日志表
            //_ll.SaveLog(14, 0, "", loginName);
            return "登陆成功！";
        }

        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public V_User_Information GeUserManagement(string loginName)
        {
            using (SqlSugarClient sqlSugar = SugarDao_MsSql.GetInstance())
            {
                return  sqlSugar.Queryable<V_User_Information>().Where(i => i.LoginName == loginName).SingleOrDefault();
            }
        }
    }
}
