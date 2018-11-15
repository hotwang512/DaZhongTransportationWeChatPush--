using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.UserManagement.BussinessLogic;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using JQWidgetsSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.Systemmanagement.Controllers.UserManagement
{
    public class UserManageController : BaseController
    {
        public UserManageLogic _ul;
        public AuthorityManageLogic _al;
        public UserManageController()
        {
            _ul = new UserManageLogic();
            _al = new AuthorityManageLogic();
        }
        //
        // GET: /Systemmanagement/UserManage/

        public ActionResult UserManagement()
        {
            var listAllRoles = _ul.GetAllRoles();
            ViewBag.listAllRoles = listAllRoles;
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.UserSystemModule);

            ViewBag.CurrentModulePermission = roleModuleModel;
            return View();
        }

        public ActionResult UserManagementDetail()
        {
            bool isEdit = bool.Parse(Request.QueryString["isEdit"]);

            //绑定角色
            List<Sys_Role> sysRoleList = new List<Sys_Role>();
            sysRoleList = _ul.GetSysRoleList();
            ViewData["SysRoleList"] = sysRoleList;

            //绑定公司
            //  List<Master_Organization> companyList = new List<Master_Organization>();
            //  companyList = _ul.GetCompanyList();
            // ViewData["CompanyList"] = companyList;

            List<Master_Organization> departmentList = new List<Master_Organization>();
            Sys_User sysRoleModel = new Sys_User();
            if (isEdit)
            {
                string vguid = Request.QueryString["Vguid"];
                sysRoleModel = _ul.GetUserInfoByVguid(vguid);
                // departmentList = GetDepartmentListByEdit(sysRoleModel.Company.ToString());
            }
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.UserSystemModule);

            ViewBag.DepartmentList = departmentList;
            ViewBag.CurrentModulePermission = roleModuleModel;
            ViewBag.UserInfo = sysRoleModel;
            ViewBag.isEdit = isEdit;
            return View();
        }

        /// <summary>
        /// 通过查询条件获取用户信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResult GetSysUserListBySearch(SearchSysUserList searchParam, GridParams para)
        {
            if (para.sortdatafield == null)
            {
                para.sortdatafield = "CreatedDate";
                para.sortorder = "desc";
            }
            para.pagenum = para.pagenum + 1;//页0，+1

            //List<DeparTment_1> departmenteList = _dl.GetDepartmentList(searchParam, para);
            var model = _ul.GetSysUserListBySearch(searchParam, para);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过公司Vguid获取部门列表
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public JsonResult GetDepartmentList(string vguid)
        {
            List<Master_Organization> departmentList = new List<Master_Organization>();
            if (!string.IsNullOrEmpty(vguid))
            {
                departmentList = _ul.GetDepartmentList(vguid);
            }
            else
            {
                departmentList = null;
            }
            return Json(departmentList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 编辑用户时绑定部门下拉框值
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public List<Master_Organization> GetDepartmentListByEdit(string vguid)
        {
            List<Master_Organization> departmentList = new List<Master_Organization>();
            departmentList = _ul.GetDepartmentList(vguid);
            return departmentList;
        }

        /// <summary>
        /// 批量删除用户信息
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public JsonResult DeleteUserInfo(string[] vguidList)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = false;

            models.isSuccess = _ul.DeleteUserInfo(vguidList);
            models.respnseInfo = models.isSuccess == true ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批量启用用户
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public JsonResult EnableUser(string[] vguidList)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = false;

            models.isSuccess = _ul.EnableUser(vguidList);
            models.respnseInfo = models.isSuccess == true ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批量禁用用户
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public JsonResult DisableUser(string[] vguidList)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = false;

            models.isSuccess = _ul.DisableUser(vguidList);
            models.respnseInfo = models.isSuccess == true ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        [ValidateInput(false)]//敏感值验证
        public JsonResult SaveUserInfo(Sys_User userModel, bool isEdit)
        {
            var model = new ActionResultModel<string>();
            model.isSuccess = false;
            bool isExistLoginName = _ul.CheckLoginName(userModel.LoginName, userModel.Vguid.ToString(), isEdit);
            if (isExistLoginName)
            {
                model.isSuccess = false;
                model.respnseInfo = "2";
            }
            else
            {
                model.isSuccess = _ul.SaveUserInfo(userModel, isEdit);
                model.respnseInfo = model.isSuccess == true ? "1" : "0";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="vguid"></param>
        /// <param name="newPwd"></param>
        /// <param name="oldPwd"></param>
        /// <returns></returns>
        public JsonResult ChangePassWord(Guid vguid, string newPwd, string oldPwd)
        {
            string msg;
            var name = DaZhongManagementSystem.Common.CurrentUser.GetCurrentUser().LoginName;
            bool isSuccess = _ul.ChangPassword(vguid, newPwd, oldPwd, name, out msg);
            if (msg != "")
            {
                return Json(new { bRet = "保存失败！", sMsg = msg });
            }
            else
            {
                if (isSuccess)
                {
                    return Json(new { bRet = "ok", sMsg = "" });
                }
                return Json(new { bRet = "保存失败！", sMsg = "" });
            }
        }

    }
}
