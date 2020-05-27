using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.OrganizationManagement.OrganizationManageLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Entities.TableEntity;
using JQWidgetsSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.OrganizationManagement
{
    public class OrganizationManagementController : BaseController
    {
        //
        // GET: /BasicDataManagement/OrganizationManagement/
        public OrganizationManagementLogic _ol;
        public AuthorityManageLogic _al;
        public OrganizationManagementController()
        {
            _ol = new OrganizationManagementLogic();
            _al = new AuthorityManageLogic();
        }

        public ActionResult OrganizationManagement()
        {
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.OrganizationModule);

            ViewBag.CurrentModulePermission = roleModuleModel;
            return View();
        }

        /// <summary>
        /// 获取组织结构属性结构数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOrganizationTreeList()
        {
            List<Master_Organization> organizationModel = _ol.GetOrganizationModel();
            //string jsonString = DaZhongManagementSystem.Common.Extend.ModelToJson(organizationModel);
            return Json(organizationModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取组织结构属性结构数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserOrganizationTreeList()
        {
            List<Master_Organization> organizationModel = _ol.GetUserOrganizationModel();
            //string jsonString = DaZhongManagementSystem.Common.Extend.ModelToJson(organizationModel);
            return Json(organizationModel, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 通过vguid获取部门详细信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public JsonResult GetOrganizationDetail(string vguid)
        {
            Master_Organization organizationDetail = new Master_Organization();
            organizationDetail = _ol.GetOrganizationDetail(vguid);
            return Json(organizationDetail, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存部门信息
        /// </summary>
        /// <param name="organizationModel"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        public JsonResult Save(Master_Organization organizationModel, string isEdit)
        {
            var models = new ActionResultModel<String>();
            models.isSuccess = false;

            models.isSuccess = _ol.CheckIsExist(organizationModel, isEdit);//判断同一部门下是否存在同名称部门
            if (!models.isSuccess)
            {
                models.isSuccess = _ol.Save(organizationModel, isEdit == "0" ? false : true);
                models.respnseInfo = models.isSuccess == true ? "1" : "0";
            }
            else
            {
                models.respnseInfo = "2";//同一部门下存在同名称部门
            }
            return Json(models, JsonRequestBehavior.AllowGet);
        }

    }
}
