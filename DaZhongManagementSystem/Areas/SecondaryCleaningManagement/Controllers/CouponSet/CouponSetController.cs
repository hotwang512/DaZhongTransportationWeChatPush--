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
using System.Web.Http.Results;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.SecondaryCleaningManagement.Controllers.CouponSet
{
    public class CouponSetController : BaseController
    {
        public UserManageLogic _ul;
        public AuthorityManageLogic _al;
        public CouponSetController()
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
        public JsonResult GetCouponSetInfo(string rightsName,string status)
        {
            List<Business_EquityAllocation> couponSetList = new List<Business_EquityAllocation>();
            using (SqlSugarClient _db = SugarDao_MsSql.GetInstance())
            {
                couponSetList = _db.Queryable<Business_EquityAllocation>().ToList();
                if (rightsName != "" && rightsName != null)
                {
                    couponSetList = couponSetList.Where(x => x.RightsName.Contains(rightsName)).ToList();
                }
                if (status != "" && status != null)
                {
                    couponSetList = couponSetList.Where(x => x.Status == status).ToList();
                }
            }
            return Json(couponSetList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteCouponSetInfo(string[] vguidList)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = false;
            using (SqlSugarClient _db = SugarDao_MsSql.GetInstance())
            {
                foreach (var item in vguidList)
                {
                    Guid vguid = Guid.Parse(item);
                    models.isSuccess = _db.Delete<Business_EquityAllocation>(i => i.VGUID == vguid);
                    models.isSuccess = _db.Delete<Business_MyRights>(i => i.EquityVGUID == item);
                }
            }
            models.respnseInfo = models.isSuccess == true ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateCouponSetInfo(string[] vguidList)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = false;
            using (SqlSugarClient _db = SugarDao_MsSql.GetInstance())
            {
                foreach (var item in vguidList)
                {
                    Guid vguid = Guid.Parse(item);
                    models.isSuccess = _db.Update<Business_EquityAllocation>(new { Status = "已发布" }, i => i.VGUID == vguid);
                    models.isSuccess = _db.Update<Business_MyRights>(new { Status = "未使用" }, i => i.EquityVGUID == item);
                }
            }
            models.respnseInfo = models.isSuccess == true ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }
    }
}
