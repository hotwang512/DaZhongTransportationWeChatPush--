using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using JQWidgetsSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement
{
    public class AuthorityManagementController : BaseController
    {
        //
        // GET: /Systemmanagement/AuthorityManagement/
        public AuthorityManageLogic _al;
        public AuthorityManagementController()
        {
            _al = new AuthorityManageLogic();
        }

        public ActionResult AuthorityManagement()
        {
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.AuthorityModule);

            ViewBag.CurrentModulePermission = roleModuleModel;
            return View();
        }

        public ActionResult AuthorityDetail()
        {
            bool isEdit = bool.Parse(Request.QueryString["isEdit"]);
            string roleInfoVguid = Request.QueryString["roleTypeVguid"];

            Sys_Role sysRole = new Sys_Role();
            if (isEdit)
            {
                sysRole = _al.GetRoleTypeInfo(roleInfoVguid);
            }
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.AuthorityModule);

            ViewBag.CurrentModulePermission = roleModuleModel;
            ViewBag.RoleTypeInfo = sysRole;
            ViewBag.isEdit = isEdit;
            return View();
        }



        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="para"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public JsonResult GetRoleTypeList(GridParams para, string roleName)
        {
            if (para.sortdatafield == null)
            {
                para.sortdatafield = "CreatedDate";
                para.sortorder = "desc";
            }
            para.pagenum = para.pagenum + 1;

            var model = _al.GetRoleTypePageList(para, roleName);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 从Sys_Role_Fixed获取每个界面权限列表界面
        /// </summary>
        /// <param name="para"></param>
        /// <param name="roleVGUID"></param>
        /// <returns></returns>
        public JsonResult GetModulePermissionsList(GridParams para, string roleVGUID)
        {
            if (para.sortdatafield == null)
            {
                para.sortdatafield = "PageID";
                para.sortorder = "asc";
            }
            para.pagenum = para.pagenum + 1;

            var model = _al.GetModulePermissionsList(para, roleVGUID);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除选中的角色列表数据(批量删除)
        /// </summary>
        /// <param name="roleTypeVguid"></param>
        /// <returns></returns>
        public JsonResult DeleteRoleType(string[] roleTypeVguid)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = false;
            models.isSuccess = _al.DeleteRoleType(roleTypeVguid);
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存角色权限信息
        /// </summary>
        /// <param name="roleModel"></param>
        /// <param name="permissionList"></param>
        /// <returns></returns>
        public JsonResult SaveRole(Sys_Role roleModel, string permissionList, bool isEdit)
        {
            List<U_Module> rolePermissionList = new List<U_Module>();
            if (!string.IsNullOrEmpty(permissionList))
            {
                rolePermissionList = Common.JsonHelper.JsonToModel<List<U_Module>>(permissionList);
                rolePermissionList = rolePermissionList.Distinct().ToList();
            }
            var models = new ActionResultModel<string>();
            models.isSuccess = false;
            models.respnseInfo = "0";
            if (_al.IsExist_RoleName(roleModel, isEdit))
            {
                models.respnseInfo = "2";//角色名称已经存在
            }
            else
            {
                models.isSuccess = _al.SaveRole(roleModel, rolePermissionList, isEdit);
                if (models.isSuccess)
                {
                    models.respnseInfo = "1";//保存成功
                }
                else
                {
                    models.respnseInfo = "0";//保存失败
                }
            }
            return Json(models, JsonRequestBehavior.AllowGet);
        }
    }
}
