using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.SystemManagement;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.Systemmanagement.Controllers.UserManagement.BussinessLogic
{
    public class UserManageLogic
    {
        public UserManageServer _us;
        public UserManageLogic()
        {
            _us = new UserManageServer();
        }

        /// <summary>
        /// 通过查询条件获取用户信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<V_User_Information> GetSysUserListBySearch(SearchSysUserList searchParam, GridParams para)
        {
            return _us.GetSysUserListBySearch(searchParam, para);
        }

        /// <summary>
        /// 通过Vguid查询用户信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public Sys_User GetUserInfoByVguid(string vguid)
        {
            return _us.GetUserInfoByVguid(vguid);
        }

        /// <summary>
        /// 绑定角色
        /// </summary>
        /// <returns></returns>
        public List<Sys_Role> GetSysRoleList()
        {
            return _us.GetSysRoleList();
        }

        /// <summary>
        /// 绑定公司列表
        /// </summary>
        /// <returns></returns>
        public List<Master_Organization> GetCompanyList()
        {
            return _us.GetCompanyList();
        }

        /// <summary>
        /// 绑定部门信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public List<Master_Organization> GetDepartmentList(string vguid)
        {
            return _us.GetDepartmentList(vguid);
        }

        /// <summary>
        /// 批量删除用户信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public bool DeleteUserInfo(string[] vguidList)
        {
            bool result = false;
            foreach (var item in vguidList)
            {
                result = _us.DeleteUserInfo(item);
            }
            return result;
        }

        /// <summary>
        /// 批量启用用户
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public bool EnableUser(string[] vguidList)
        {
            bool result = false;
            foreach (var item in vguidList)
            {
                result = _us.EnableUser(item);
            }
            return result;
        }

        /// <summary>
        /// 批量禁用用户
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public bool DisableUser(string[] vguidList)
        {
            bool result = false;
            foreach (var item in vguidList)
            {
                result = _us.DisableUser(item);
            }
            return result;
        }

        /// <summary>
        /// 检查登录名称是否重复
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public bool CheckLoginName(string loginName, string vguid, bool isEdit)
        {
            return _us.CheckLoginName(loginName, vguid, isEdit);
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        public bool SaveUserInfo(Sys_User userModel, bool isEdit)
        {
            bool result = false;
            Sys_User userInfoModel = new Sys_User();
            if (isEdit)//编辑
            {
                userInfoModel = _us.GetUserInfoByVguid(userModel.Vguid.ToString());
                userInfoModel.ChangeDate = DateTime.Now;
                userInfoModel.ChangeUser = Common.CurrentUser.GetCurrentUser().LoginName;
                userInfoModel.Company = userModel.Company;
                userInfoModel.Department = userModel.Department;
                userInfoModel.Email = userModel.Email;
                userInfoModel.Enable = userModel.Enable;
                userInfoModel.LoginName = userModel.LoginName;
                userInfoModel.MobileNnumber = userModel.MobileNnumber;
                userInfoModel.Remark = userModel.Remark;
                userInfoModel.Role = userModel.Role;
                userInfoModel.UserName = userModel.UserName;
                userInfoModel.WorkPhone = userModel.WorkPhone;
                result = _us.SaveUserInfo(userInfoModel, isEdit);
            }
            else//新增
            {
                userModel.Vguid = Guid.NewGuid();
                userModel.CreatedUser = Common.CurrentUser.GetCurrentUser().LoginName;
                userModel.CreatedDate = DateTime.Now;
                userModel.ChangeDate = DateTime.Now;
                userModel.Password = ConfigSugar.GetAppString("DefaultPassword");
                result = _us.SaveUserInfo(userModel, isEdit);
            }

            return result;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="vguid"></param>
        /// <param name="newPwd"></param>
        /// <param name="oldPwd"></param>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool ChangPassword(Guid vguid, string newPwd, string oldPwd, string name, out  string msg)
        {
            return _us.ChangePassWord(vguid, newPwd, oldPwd, name, out msg);
        }

        /// <summary>
        /// 获取系统中所有的角色
        /// </summary>
        /// <returns></returns>
        public List<Sys_Role> GetAllRoles()
        {
            return _us.GetAllRoles();
        }
    }
}