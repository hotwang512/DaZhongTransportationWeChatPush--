using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.ConfigManagement.BusinessLogic;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using JQWidgetsSugar;
using System.Collections.Generic;
using System.Web.Mvc;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.Systemmanagement.Controllers.ConfigManagement
{
    public class ConfigManagementController : BaseController
    {
        //
        // GET: /Systemmanagement/ConfigManagement/
        public ConfigManagementLogic _configManagementLogic;
        public AuthorityManageLogic _authorityManageLogic;
        public ConfigManagementController()
        {
            _configManagementLogic = new ConfigManagementLogic();
            _authorityManageLogic = new AuthorityManageLogic();
        }

        public ActionResult ConfigManagement()
        {
            List<Master_Configuration> configList = new List<Master_Configuration>();
            configList = _configManagementLogic.GetConfigList();
            Sys_Role_Module roleModuleModel = _authorityManageLogic.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.ConfigModule);

            ViewBag.CurrentModulePermission = roleModuleModel;
            ViewBag.configList = configList;
            return View();
        }
        /// <summary>
        /// 获取配置文件的值
        /// </summary>
        /// <returns></returns>
        public JsonResult GetConfigList()
        {
            var list = _configManagementLogic.GetConfigList();
            return Json(list,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存可配置信息
        /// </summary>
        /// <param name="configData">配置文件数组</param>
        /// <returns></returns>
        public JsonResult SaveConfig(string configData)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = false;
            List<Master_Configuration> configList = new List<Master_Configuration>();
            configList = JsonHelper.JsonToModel<List<Master_Configuration>>(configData);

            models.isSuccess = _configManagementLogic.SaveConfig(configList);
            models.respnseInfo = models.isSuccess == true ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }




    }
}
