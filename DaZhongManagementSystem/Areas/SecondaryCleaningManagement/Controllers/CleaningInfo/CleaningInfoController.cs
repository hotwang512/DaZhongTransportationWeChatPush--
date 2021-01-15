using DaZhongManagementSystem.Areas.SecondaryCleaningManagement.Models;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.UserManagement.BussinessLogic;
using DaZhongManagementSystem.Controllers;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.SecondaryCleaningManagement.Controllers.CleaningInfo
{
    public class CleaningInfoController : BaseController
    {
        public UserManageLogic _ul;
        public AuthorityManageLogic _al;
        public CleaningInfoController()
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
        public JsonResult GetCleaningInfo(string cabOrgName, string manOrgName,string couponType,string cabLicense)
        {
            List<Business_SecondaryCleaning> cleaningList = new List<Business_SecondaryCleaning>();
            using (SqlSugarClient _db = SugarDao_MsSql.GetInstance())
            {
                cleaningList = _db.Queryable<Business_SecondaryCleaning>().OrderBy(x=>x.OperationDate,OrderByType.Desc).ToList();
                if (cabOrgName != "" && cabOrgName != null)
                {
                    cleaningList = cleaningList.Where(x => x.CabOrgName.Contains(cabOrgName)).ToList();
                }
                if (manOrgName != "" && manOrgName != null)
                {
                    cleaningList = cleaningList.Where(x => x.ManOrgName.Contains(manOrgName)).ToList();
                }
                if (couponType != "" && couponType != null)
                {
                    cleaningList = cleaningList.Where(x => x.CouponType == couponType).ToList();
                }
                if (cabLicense != "" && cabLicense != null)
                {
                    cleaningList = cleaningList.Where(x => x.CabLicense.Contains(cabLicense)).ToList();
                }
            }
            return Json(cleaningList, JsonRequestBehavior.AllowGet);
        }
    }
}
