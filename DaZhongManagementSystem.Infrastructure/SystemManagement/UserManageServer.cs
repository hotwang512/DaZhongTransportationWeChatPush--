using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Infrastructure.SystemManagement
{
    public class UserManageServer
    {
        public LogLogic _logLogic;
        public UserManageServer()
        {
            _logLogic = new LogLogic();
        }

        /// <summary>
        /// 通过查询条件获取推送信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<V_User_Information> GetSysUserListBySearch(SearchSysUserList searchParam, GridParams para)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                JsonResultModel<V_User_Information> jsonResult = new JsonResultModel<V_User_Information>();
                Guid vguid = Guid.Parse(Common.Tools.MasterVGUID.SysAdmin);
                var query = _dbMsSql.Queryable<V_User_Information>().Where(i => i.Vguid != vguid);

                if (!string.IsNullOrEmpty(searchParam.LoginName))
                {
                    query.Where(i => i.LoginName.Contains(searchParam.LoginName));
                }
                if (!string.IsNullOrEmpty(searchParam.Role))
                {
                    query.Where(i => i.Role == searchParam.Role);
                }
                if (!string.IsNullOrEmpty(searchParam.Department))
                {
                    query.Where(i => i.TranslationDepartment.Contains(searchParam.Department));
                }

                query.OrderBy(para.sortdatafield + " " + para.sortorder);
                jsonResult.TotalRows = query.Count();
                jsonResult.Rows = query.ToPageList(para.pagenum, para.pagesize);

                //存入操作日志表
                string logData = JsonHelper.ModelToJson(jsonResult);
                _logLogic.SaveLog(3, 26, Common.CurrentUser.GetCurrentUser().LoginName, "用户管理列表", logData);

                return jsonResult;
            }
        }

        /// <summary>
        /// 通过vguid查询用户信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public Sys_User GetUserInfoByVguid(string vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                Guid Vguid = Guid.Parse(vguid);
                Sys_User sysRoleModel = new Sys_User();
                sysRoleModel = _dbMsSql.Queryable<Sys_User>().Where(i => i.Vguid == Vguid).SingleOrDefault();

                //写入日志
                string logData = JsonHelper.ModelToJson<Sys_User>(sysRoleModel);
                _logLogic.SaveLog(3, 29, Common.CurrentUser.GetCurrentUser().LoginName, sysRoleModel.LoginName, logData);

                return sysRoleModel;
            }
        }

        /// <summary>
        /// 绑定用户角色
        /// </summary>
        /// <returns></returns>
        public List<Sys_Role> GetSysRoleList()
        {
            using (SqlSugar.SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(Common.Tools.MasterVGUID.SysAdminRole);
                List<Sys_Role> sysRoleList = _dbMsSql.Queryable<Sys_Role>().Where(i => i.Vguid != vguid).ToList();
                return sysRoleList;
            }
        }

        /// <summary>
        /// 绑定公司信息
        /// </summary>
        /// <returns></returns>
        public List<Master_Organization> GetCompanyList()
        {
            using (SqlSugar.SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(Common.Tools.MasterVGUID.ParentCompany);
                List<Master_Organization> companyList = _dbMsSql.Queryable<Master_Organization>().Where(i => i.ParentVguid == vguid).ToList();
                return companyList;
            }
        }

        /// <summary>
        /// 绑定部门信息
        /// </summary>
        /// <returns></returns>
        public List<Master_Organization> GetDepartmentList(string vguid)
        {
            using (SqlSugar.SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                Guid VGUID = Guid.Parse(vguid);
                List<Master_Organization> departmentList = _dbMsSql.Queryable<Master_Organization>().Where(i => i.ParentVguid == VGUID).ToList();
                return departmentList;
            }
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public bool DeleteUserInfo(string vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                Guid Vguid = Guid.Parse(vguid);
                try
                {
                    _dbMsSql.BeginTran();

                    Sys_User userInfo = _dbMsSql.Queryable<Sys_User>().Where(i => i.Vguid == Vguid).SingleOrDefault();
                    string weChatJson = JsonHelper.ModelToJson<Sys_User>(userInfo);
                    result = _dbMsSql.Delete<Sys_User>(i => i.Vguid == Vguid);

                    //存入操作日志表
                    _logLogic.SaveLog(2, 26, Common.CurrentUser.GetCurrentUser().LoginName, userInfo.LoginName, weChatJson);

                    _dbMsSql.CommitTran();
                }
                catch (Exception ex)
                {
                    _dbMsSql.RollbackTran();
                    Common.LogHelper.LogHelper.WriteLog(ex.Message + "/n" + ex.ToString() + "/n" + ex.StackTrace);
                }
                finally
                {

                }
                return result;
            }
        }

        /// <summary>
        /// 批量启用用户
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public bool EnableUser(string vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                Guid Vguid = Guid.Parse(vguid);
                try
                {
                    _dbMsSql.BeginTran();
                    result = _dbMsSql.Update<Sys_User>(new { Enable = "1" }, i => i.Vguid == Vguid);

                    Sys_User userInfo = _dbMsSql.Queryable<Sys_User>().Where(i => i.Vguid == Vguid).SingleOrDefault();
                    string weChatJson = JsonHelper.ModelToJson<Sys_User>(userInfo);
                    ////存入操作日志表
                    _logLogic.SaveLog(15, 26, Common.CurrentUser.GetCurrentUser().LoginName, userInfo.LoginName, weChatJson);

                    _dbMsSql.CommitTran();
                }
                catch (Exception ex)
                {
                    _dbMsSql.RollbackTran();
                    Common.LogHelper.LogHelper.WriteLog(ex.Message);
                }
                finally
                {

                }
                return result;
            }
        }

        /// <summary>
        /// 批量禁用用户
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public bool DisableUser(string vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                Guid Vguid = Guid.Parse(vguid);
                try
                {
                    _dbMsSql.BeginTran();
                    result = _dbMsSql.Update<Sys_User>(new { Enable = "0" }, i => i.Vguid == Vguid);

                    Sys_User userInfo = _dbMsSql.Queryable<Sys_User>().Where(i => i.Vguid == Vguid).SingleOrDefault();
                    string weChatJson = JsonHelper.ModelToJson<Sys_User>(userInfo);
                    ////存入操作日志表
                    _logLogic.SaveLog(16, 26, Common.CurrentUser.GetCurrentUser().LoginName, userInfo.LoginName, weChatJson);

                    _dbMsSql.CommitTran();
                }
                catch (Exception ex)
                {
                    _dbMsSql.RollbackTran();
                    Common.LogHelper.LogHelper.WriteLog(ex.Message);
                }
                finally
                {

                }
                return result;
            }
        }

        /// <summary>
        /// 检查登录名称是否重复
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public bool CheckLoginName(string loginName, string vguid, bool isEdit)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                Guid Vguid = Guid.Parse(vguid);
                if (isEdit)
                {
                    result = _dbMsSql.Queryable<Sys_User>().Any(i => i.LoginName == loginName && i.Vguid != Vguid);
                }
                else
                {
                    result = _dbMsSql.Queryable<Sys_User>().Any(i => i.LoginName == loginName);
                }
                return result;
            }
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        public bool SaveUserInfo(Sys_User userModel, bool isEdit)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                try
                {
                    //_dbMsSql.Queryable<Master_Organization>().Where(it=>it.)
                    _dbMsSql.BeginTran();
                    if (isEdit)//编辑
                    {
                        var model = new
                        {
                            LoginName = userModel.LoginName,
                            Email = userModel.Email,
                            MobileNnumber = userModel.MobileNnumber,
                            WorkPhone = userModel.WorkPhone,
                            Role = userModel.Role,
                            Company = userModel.Company,
                            Department = userModel.Department,
                            Enable = userModel.Enable,
                            Remark = userModel.Remark,
                            ChangeUser = userModel.ChangeUser,
                            ChangeDate = userModel.ChangeDate
                        };
                        result = _dbMsSql.Update<Sys_User>(model, i => i.Vguid == userModel.Vguid);
                        string userJson = JsonHelper.ModelToJson<Sys_User>(userModel);
                        _logLogic.SaveLog(4, 28, Common.CurrentUser.GetCurrentUser().LoginName, userModel.LoginName, userJson);
                    }
                    else//新增
                    {
                        result = _dbMsSql.Insert<Sys_User>(userModel, false) != DBNull.Value;
                        //result = _dbMsSql.Insert<Sys_User>(userModel) != DBNull.Value;
                        string userJson = JsonHelper.ModelToJson<Sys_User>(userModel);
                        _logLogic.SaveLog(1, 27, Common.CurrentUser.GetCurrentUser().LoginName, userModel.LoginName, userJson);
                    }
                    _dbMsSql.CommitTran();
                }
                catch (Exception ex)
                {
                    _dbMsSql.RollbackTran();
                    Common.LogHelper.LogHelper.WriteLog(ex.Message + "/n" + ex.ToString() + "/n" + ex.StackTrace);
                }
                finally
                {

                }
                return result;
            }
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
        public bool ChangePassWord(Guid vguid, string newPwd, string oldPwd, string name, out string msg)
        {
            msg = "";
            bool result = false;
            using (SqlSugarClient sqlSugar = SugarDao_MsSql.GetInstance())
            {
                var list = sqlSugar.Queryable<Sys_User>().Where(i => i.Vguid == vguid).ToList();
                if (list[0].Password != oldPwd)
                {
                    msg = "旧密码不正确！";
                    return false;
                }
                result = sqlSugar.Update<Sys_User>(new { Password = newPwd, ChangeDate = DateTime.Now, ChangeUser = name }, i => i.Vguid == vguid);
                _logLogic.SaveLog(4, 0, Common.CurrentUser.GetCurrentUser().LoginName, "更新密码", oldPwd + "->" + newPwd);
                return result;

            }
        }
        /// <summary>
        /// 获取系统中所有的角色
        /// </summary>
        /// <returns></returns>
        public List<Sys_Role> GetAllRoles()
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                return db.Queryable<Sys_Role>().OrderBy(i => i.CreatedDate, OrderByType.Desc).ToList();
            }
        }
    }
}
