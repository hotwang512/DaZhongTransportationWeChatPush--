using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.SystemManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic
{
    public class AuthorityManageLogic
    {
        public AuthorityManageServer _as;
        public AuthorityManageLogic()
        {
            _as = new AuthorityManageServer();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="roleTypeVguid"></param>
        /// <returns></returns>
        public bool DeleteRoleType(string[] roleTypeVguid)
        {
            bool result = false;
            foreach (var item in roleTypeVguid)
            {
                result = _as.DeleteRoleType(item);
            }

            return result;
        }

        /// <summary>
        /// 从Sys_Role_Fixed获取每个界面权限列表界面
        /// </summary>
        /// <param name="para"></param>
        /// <param name="roleVGUID"></param>
        /// <returns></returns>
        public JsonResultModel<Sys_Role_Fixed> GetModulePermissionsList(GridParams para, string roleVGUID)
        {
            JsonResultModel<Sys_Role_Fixed> jsonResultModel = _as.GetModulePermissionsList(para);
            List<Sys_Role_Module> sysRoleList = GetRoleTypePermissionsList(roleVGUID);
            foreach (var modulePermission in jsonResultModel.Rows)
            {
                if (modulePermission.Reads == 1) //1
                {
                    if (sysRoleList.Any(i => i.ModuleVGUID == modulePermission.ModuleVGUID && i.Reads == 1))
                    {
                        modulePermission.Reads = 2;
                    }
                }
                if (modulePermission.Adds == 1)
                {
                    if (sysRoleList.Any(i => i.ModuleVGUID == modulePermission.ModuleVGUID && i.Adds == 1))
                    {
                        modulePermission.Adds = 2;
                    }
                }
                if (modulePermission.Edit == 1)
                {
                    if (sysRoleList.Any(i => i.ModuleVGUID == modulePermission.ModuleVGUID && i.Edit == 1))
                    {
                        modulePermission.Edit = 2;
                    }
                }
                if (modulePermission.Deletes == 1)
                {
                    if (sysRoleList.Any(i => i.ModuleVGUID == modulePermission.ModuleVGUID && i.Deletes == 1))
                    {
                        modulePermission.Deletes = 2;
                    }
                }
                if (modulePermission.Submit == 1)
                {
                    if (sysRoleList.Any(i => i.ModuleVGUID == modulePermission.ModuleVGUID && i.Submit == 1))
                    {
                        modulePermission.Submit = 2;
                    }
                }
                if (modulePermission.Approved == 1)
                {
                    if (sysRoleList.Any(i => i.ModuleVGUID == modulePermission.ModuleVGUID && i.Approved == 1))
                    {
                        modulePermission.Approved = 2;
                    }
                }
                if (modulePermission.Import == 1)
                {
                    if (sysRoleList.Any(i => i.ModuleVGUID == modulePermission.ModuleVGUID && i.Import == 1))
                    {
                        modulePermission.Import = 2;
                    }
                }
                if (modulePermission.Export == 1)
                {
                    if (sysRoleList.Any(i => i.ModuleVGUID == modulePermission.ModuleVGUID && i.Export == 1))
                    {
                        modulePermission.Export = 2;
                    }
                }
            }

            return jsonResultModel;
        }

        /// <summary>
        /// 获取所有模块列表（只包括功能模块，不包括标题模块）
        /// </summary>
        /// <returns></returns>
        public List<Sys_Role_Fixed> GetAllModule()
        {
            return _as.GetAllModule();
        }

        /// <summary>
        /// 获取当前用户角色权限
        /// </summary>
        /// <returns></returns>
        public List<V_Sys_Role_Module> GetCurrentUserPermissionList()
        {
            return _as.GetCurrentUserPermissionList();
        }

        /// <summary>
        /// 判断角色名称是否存在
        /// </summary>
        /// <param name="roleTypeModel"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        public bool IsExist_RoleName(Sys_Role roleTypeModel, bool isEdit)
        {
            return _as.IsExist_RoleName(roleTypeModel, isEdit);
        }

        /// <summary>
        /// 获取角色所拥有的权限列表数据源
        /// </summary>
        /// <param name="roleVguid"></param>
        /// <returns></returns>
        public List<Sys_Role_Module> GetRoleTypePermissionsList(string roleVguid)
        {
            return _as.GetRoleTypePermissionsList(roleVguid);
        }

        /// <summary>
        /// 获取角色列表数据源（分页获取）
        /// </summary>
        /// <param name="para"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public JsonResultModel<Sys_Role> GetRoleTypePageList(GridParams para, string roleName)
        {
            return _as.GetRoleTypePageList(para, roleName);
        }

        /// <summary>
        /// 通过Vguid获取角色名称及描述
        /// </summary>
        /// <param name="roleTypeVguid"></param>
        /// <returns></returns>
        public Sys_Role GetRoleTypeInfo(string roleTypeVguid)
        {
            return _as.GetRoleTypeInfo(roleTypeVguid);
        }

        /// <summary>
        /// 保存模块权限
        /// </summary>
        /// <param name="roleTypeModel"></param>
        /// <param name="permissionList"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        public bool SaveRole(Sys_Role roleTypeModel, List<U_Module> permissionList, bool isEdit)
        {
            bool result = false;
            if (isEdit)//编辑
            {
                Sys_Role sysRoleModels = _as.GetRoleTypeInfo(roleTypeModel.Vguid.ToString());
                sysRoleModels.Role = roleTypeModel.Role;
                sysRoleModels.Description = roleTypeModel.Description;
                sysRoleModels.ChangeDate = DateTime.Now;
                sysRoleModels.ChangeUser = Common.CurrentUser.GetCurrentUser().LoginName;

                //获取所有模块信息
                List<Sys_Module> sysModuleList = _as.GetSysModuleList();
                List<Sys_Role_Module> sysRoleList = new List<Sys_Role_Module>();
                foreach (var item in sysModuleList)
                {
                    //按照模块区分list
                    var list = permissionList.FindAll(p => p.ModuleName == item.Vguid.ToString());
                    Sys_Role_Module sysRoleModel = new Sys_Role_Module();
                    //list为循环当前模块的权限集合
                    foreach (var i in list)
                    {
                        switch (i.RightType)
                        {
                            case (int)AuthorityEnum.Reads:
                                sysRoleModel.Reads = 1;
                                break;
                            case (int)AuthorityEnum.Adds:
                                sysRoleModel.Adds = 1;
                                break;
                            case (int)AuthorityEnum.Edit:
                                sysRoleModel.Edit = 1;
                                break;
                            case (int)AuthorityEnum.Deletes:
                                sysRoleModel.Deletes = 1;
                                break;
                            case (int)AuthorityEnum.Submit:
                                sysRoleModel.Submit = 1;
                                break;
                            case (int)AuthorityEnum.Approved:
                                sysRoleModel.Approved = 1;
                                break;
                            case (int)AuthorityEnum.Import:
                                sysRoleModel.Import = 1;
                                break;
                            case (int)AuthorityEnum.Export:
                                sysRoleModel.Export = 1;
                                break;
                        }
                    }
                    if (list.Count > 0)
                    {
                        sysRoleModel.Vguid = Guid.NewGuid();
                        sysRoleModel.RoleVGUID = roleTypeModel.Vguid;
                        sysRoleModel.ModuleVGUID = item.Vguid;
                        sysRoleModel.CreatedDate = DateTime.Now;
                        sysRoleModel.CreatedUser = Common.CurrentUser.GetCurrentUser().LoginName;
                        sysRoleModel.ChangeDate = DateTime.Now;

                        sysRoleList.Add(sysRoleModel);
                    }
                }
                result = _as.SaveRole(sysRoleModels, sysRoleList, isEdit);
            }
            else//新增
            {
                //角色主信息
                roleTypeModel.Vguid = Guid.NewGuid();
                roleTypeModel.CreatedDate = DateTime.Now;
                roleTypeModel.CreatedUser = Common.CurrentUser.GetCurrentUser().LoginName;
                roleTypeModel.ChangeDate = DateTime.Now;

                //获取所有模块信息
                List<Sys_Module> sysModuleList = _as.GetSysModuleList();
                List<Sys_Role_Module> sysRoleList = new List<Sys_Role_Module>();
                foreach (var item in sysModuleList)
                {
                    //按照模块区分list
                    var list = permissionList.FindAll(p => p.ModuleName == item.Vguid.ToString());
                    Sys_Role_Module sysRoleModel = new Sys_Role_Module();
                    //list为循环当前模块的权限集合
                    foreach (var i in list)
                    {
                        switch (i.RightType)
                        {
                            case (int)AuthorityEnum.Reads:
                                sysRoleModel.Reads = 1;
                                break;
                            case (int)AuthorityEnum.Adds:
                                sysRoleModel.Adds = 1;
                                break;
                            case (int)AuthorityEnum.Edit:
                                sysRoleModel.Edit = 1;
                                break;
                            case (int)AuthorityEnum.Deletes:
                                sysRoleModel.Deletes = 1;
                                break;
                            case (int)AuthorityEnum.Submit:
                                sysRoleModel.Submit = 1;
                                break;
                            case (int)AuthorityEnum.Approved:
                                sysRoleModel.Approved = 1;
                                break;
                            case (int)AuthorityEnum.Import:
                                sysRoleModel.Import = 1;
                                break;
                            case (int)AuthorityEnum.Export:
                                sysRoleModel.Export = 1;
                                break;
                        }
                    }
                    if (list.Count > 0)
                    {
                        sysRoleModel.Vguid = Guid.NewGuid();
                        sysRoleModel.RoleVGUID = roleTypeModel.Vguid;
                        sysRoleModel.ModuleVGUID = item.Vguid;
                        sysRoleModel.CreatedDate = DateTime.Now;
                        sysRoleModel.CreatedUser = Common.CurrentUser.GetCurrentUser().LoginName;
                        sysRoleModel.ChangeDate = DateTime.Now;

                        sysRoleList.Add(sysRoleModel);
                    }
                }
                result = _as.SaveRole(roleTypeModel, sysRoleList, isEdit);
            }
            return result;
        }

        /// <summary>
        /// 获取角色下每个模块的权限
        /// </summary>
        /// <param name="roleVguid"></param>
        /// <param name="moduleVguid"></param>
        /// <returns></returns>
        public Sys_Role_Module GetRoleModulePermission(string roleVguid, string moduleVguid)
        {
            return _as.GetRoleModulePermission(roleVguid, moduleVguid);
        }

    }
}