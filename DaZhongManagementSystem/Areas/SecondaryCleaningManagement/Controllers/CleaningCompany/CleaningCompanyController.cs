using DaZhongManagementSystem.Areas.SecondaryCleaningManagement.Models;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.UserManagement.BussinessLogic;
using DaZhongManagementSystem.Controllers;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using JQWidgetsSugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.SecondaryCleaningManagement.Controllers.CleaningCompany
{
    public class CleaningCompanyController : BaseController
    {
        public UserManageLogic _ul;
        public AuthorityManageLogic _al;
        public CleaningCompanyController()
        {
            _ul = new UserManageLogic();
            _al = new AuthorityManageLogic();
        }
        public ActionResult Index()
        {
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.UserSystemModule);
            ViewBag.CurrentModulePermission = roleModuleModel;
            return View();
        }
        public JsonResult GetCleaningCompanyInfo(string companyName, string contactPerson)
        {
            List<Business_CleaningCompany> cleaningCompanyList = new List<Business_CleaningCompany>();
            using (SqlSugarClient _db = SugarDao_MsSql.GetInstance())
            {
                cleaningCompanyList = _db.Queryable<Business_CleaningCompany>().ToList();
                if(companyName != "" || companyName != null)
                {
                    cleaningCompanyList = cleaningCompanyList.Where(x => x.CompanyName.Contains(companyName)).ToList();
                }
                if (contactPerson != "" || contactPerson != null)
                {
                    cleaningCompanyList = cleaningCompanyList.Where(x => x.ContactPerson.Contains(contactPerson)).ToList();
                }
            }
            return Json(cleaningCompanyList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteCleaning(string[] vguidList)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = false;
            using (SqlSugarClient _db = SugarDao_MsSql.GetInstance())
            {
                foreach (var item in vguidList)
                {
                    Guid vguid = Guid.Parse(item);
                    models.isSuccess = _db.Delete<Business_CleaningCompany>(i => i.Vguid == vguid);
                }
            }
            models.respnseInfo = models.isSuccess == true ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }
    }
}
