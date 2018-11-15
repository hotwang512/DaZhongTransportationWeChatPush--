using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Infrastructure.SystemManagement
{
    public class AuthorityManageServer
    {
        public LogLogic _logLogic;
        public AuthorityManageServer()
        {
            _logLogic = new LogLogic();
        }

        /// <summary>
        /// 从Sys_Role_Fixed获取每个界面权限列表界面
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<Sys_Role_Fixed> GetModulePermissionsList(GridParams para)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                JsonResultModel<Sys_Role_Fixed> jsonResult = new JsonResultModel<Sys_Role_Fixed>();
                var query = _dbMsSql.Queryable<Sys_Role_Fixed>();

                //query.OrderBy(para.sortdatafield + " " + para.sortorder);
                jsonResult.TotalRows = query.Count();
                jsonResult.Rows = query.ToList();
                return jsonResult;
            }
        }

        /// <summary>
        /// 获取所有模块列表（只包括功能模块，不包括标题模块）
        /// </summary>
        /// <returns></returns>
        public List<Sys_Role_Fixed> GetAllModule()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                List<Sys_Role_Fixed> moduleList = new List<Sys_Role_Fixed>();
                moduleList = _dbMsSql.Queryable<Sys_Role_Fixed>().ToList();
                return moduleList;
            }
        }

        /// <summary>
        /// 获取当前用户角色权限
        /// </summary>
        /// <returns></returns>
        public List<V_Sys_Role_Module> GetCurrentUserPermissionList()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                List<V_Sys_Role_Module> currentUserPermissionList = new List<V_Sys_Role_Module>();
                Guid vguid = Guid.Parse(Common.CurrentUser.GetCurrentUser().Role);
                currentUserPermissionList = _dbMsSql.Queryable<V_Sys_Role_Module>().Where(i => i.RoleVGUID == vguid).ToList();
                return currentUserPermissionList;
            }
        }

        /// <summary>
        /// 删除某一条角色数据（连同角色有关的权限全部删除掉）
        /// </summary>
        /// <param name="roleTypeVguid"></param>
        /// <returns></returns>
        public bool DeleteRoleType(string roleTypeVguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                Guid vguid = Guid.Parse(roleTypeVguid);

                Sys_Role sysRole = _dbMsSql.Queryable<Sys_Role>().Where(i => i.Vguid == vguid).SingleOrDefault();
                string sysRoleJson = JsonHelper.ModelToJson<Sys_Role>(sysRole);

                //保存日志
                _logLogic.SaveLog(2, 30, Common.CurrentUser.GetCurrentUser().LoginName, "角色", sysRoleJson);
                try
                {
                    result = _dbMsSql.Delete<Sys_Role>(i => i.Vguid == vguid);
                    _dbMsSql.Delete<Sys_Role_Module>(i => i.RoleVGUID == vguid);
                }
                catch (Exception exp)
                {
                    Common.LogHelper.LogHelper.WriteLog(exp.ToString());
                }
                return result;
            }
        }

        /// <summary>
        /// 判断角色名称是否存在
        /// </summary>
        /// <param name="roleTypeModel"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        public bool IsExist_RoleName(Sys_Role roleTypeModel, bool isEdit)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                if (isEdit)//编辑
                {
                    result = _dbMsSql.Queryable<Sys_Role>().Any(i => i.Role == roleTypeModel.Role && i.Vguid != roleTypeModel.Vguid);
                }
                else//新增
                {
                    result = _dbMsSql.Queryable<Sys_Role>().Any(i => i.Role == roleTypeModel.Role);
                }
                return result;
            }
        }

        /// <summary>
        /// 获取角色所拥有权限
        /// </summary>
        /// <param name="roleVguid"></param>
        /// <returns></returns>
        public List<Sys_Role_Module> GetRoleTypePermissionsList(string roleVguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(roleVguid);
                return _dbMsSql.Queryable<Sys_Role_Module>().Where(i => i.RoleVGUID == vguid).ToList();
            }
        }

        /// <summary>
        /// 分页获取角色列表数据源
        /// </summary>
        /// <param name="para"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public JsonResultModel<Sys_Role> GetRoleTypePageList(GridParams para, string roleName)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                JsonResultModel<Sys_Role> jsonResult = new JsonResultModel<Sys_Role>();
                var query = _dbMsSql.Queryable<Sys_Role>();
                Guid vguid = Guid.Parse(Common.Tools.MasterVGUID.SysAdminRole);
                query.Where(i => i.Vguid != vguid);
                if (!string.IsNullOrEmpty(roleName))
                {
                    query.Where(i => i.Role.Contains(roleName));
                }
                query.OrderBy(para.sortdatafield + " " + para.sortorder);
                jsonResult.TotalRows = query.Count();
                jsonResult.Rows = query.ToPageList(para.pagenum, para.pagesize);

                string logData = JsonHelper.ModelToJson(jsonResult);
                _logLogic.SaveLog(3, 30, Common.CurrentUser.GetCurrentUser().LoginName, "角色列表", logData);
                return jsonResult;
            }
        }

        /// <summary>
        /// 通过Vguid获取角色名称及描述
        /// </summary>
        /// <param name="roleTypeVguid"></param>
        /// <returns></returns>
        public Sys_Role GetRoleTypeInfo(string roleTypeVguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(roleTypeVguid);
                return _dbMsSql.Queryable<Sys_Role>().Where(i => i.Vguid == vguid).SingleOrDefault();
            }
        }

        /// <summary>
        /// 获取模块以及所有页面信息列表
        /// </summary>
        /// <returns></returns>
        public List<Sys_Module> GetSysModuleList()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                List<Sys_Module> sysModuleList = _dbMsSql.Queryable<Sys_Module>().OrderBy(i => i.CreatedDate, OrderByType.Desc).ToList();
                return sysModuleList;
            }
        }

        /// <summary>
        /// 保存角色信息
        /// </summary>
        /// <param name="roleTypeModel"></param>
        /// <param name="permissionList"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        public bool SaveRole(Sys_Role roleTypeModel, List<Sys_Role_Module> permissionList, bool isEdit)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                try
                {
                    _dbMsSql.BeginTran();
                    if (isEdit)//编辑
                    {
                        var model = new
                        {
                            ChangeDate = roleTypeModel.ChangeDate,
                            ChangeUser = roleTypeModel.ChangeUser,
                            Role = roleTypeModel.Role,
                            Description = roleTypeModel.Description
                        };
                        string logData = JsonHelper.ModelToJson<Sys_Role>(roleTypeModel);
                        _logLogic.SaveLog(4, 32, Common.CurrentUser.GetCurrentUser().LoginName, roleTypeModel.Role, logData);
                        result = _dbMsSql.Update<Sys_Role>(model, i => i.Vguid == roleTypeModel.Vguid);
                    }
                    else//新增
                    {
                        string logData = JsonHelper.ModelToJson<Sys_Role>(roleTypeModel);
                        _logLogic.SaveLog(1, 31, Common.CurrentUser.GetCurrentUser().LoginName, roleTypeModel.Role, logData);
                        result = _dbMsSql.Insert<Sys_Role>(roleTypeModel, false) != DBNull.Value;
                    }
                    _dbMsSql.Delete<Sys_Role_Module>(i => i.RoleVGUID == roleTypeModel.Vguid);
                    _dbMsSql.InsertRange(permissionList, false);
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
        /// 获取角色下每个模块的权限
        /// </summary>
        /// <param name="roleVguid">角色Vguid</param>
        /// <param name="moduleVguid">模块Vguid</param>
        /// <returns></returns>
        public Sys_Role_Module GetRoleModulePermission(string roleVguid, string moduleVguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                Guid roleVGUID = Guid.Parse(roleVguid);
                Guid moduleVGUID = Guid.Parse(moduleVguid);
                Sys_Role_Module roleModuleModel = new Sys_Role_Module();
                if (roleVguid == Common.Tools.MasterVGUID.SysAdminRole)
                {
                    roleModuleModel.Reads = 1;
                    roleModuleModel.Edit = 1;
                    roleModuleModel.Deletes = 1;
                    roleModuleModel.Adds = 1;
                    roleModuleModel.Submit = 1;
                    roleModuleModel.Approved = 1;
                    roleModuleModel.Import = 1;
                    roleModuleModel.Export = 1;
                }
                else
                {
                    roleModuleModel = _dbMsSql.Queryable<Sys_Role_Module>().Where(i => i.RoleVGUID == roleVGUID && i.ModuleVGUID == moduleVGUID).SingleOrDefault();
                    if (roleModuleModel==null)
                    {
                        return new Sys_Role_Module() { Reads = 0, Edit = 0, Deletes = 0, Adds = 0, Submit = 0, Approved = 0, Import = 0, Export =0};
                    }
                }
                return roleModuleModel;
            }
        }
    }
}
