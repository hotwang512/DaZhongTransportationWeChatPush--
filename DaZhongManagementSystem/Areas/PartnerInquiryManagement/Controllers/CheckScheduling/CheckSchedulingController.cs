using DaZhongManagementSystem.Areas.PartnerInquiryManagement.Controllers.PartnerHomePage;
using DaZhongManagementSystem.Areas.PartnerInquiryManagement.Models;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using DaZhongTransitionLiquidation.Common.Pub;
using SqlSugar;
using SyntacticSugar;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.PartnerInquiryManagement.Controllers.CheckScheduling
{
    public class CheckSchedulingController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Key = "lZagKrU56xPBvyNRZjym7jrdJPwOT1Z0W+HpZaTrvUobpwSQEAue7j0iWs/b0cu2";
            return View();
        }
        public JsonResult GetVehicleMaintenanceInfo(string fleet,string type,string status,string idCard, string code)
        {
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var orgName = cm.Organization;
            var fleetAll = PartnerHomePageController.getSqlInValue(cm.MotorcadeName,code);
            List<VehicleMaintenanceInfo> resultInfo = new List<VehicleMaintenanceInfo>();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                if (fleet == "0")
                {
                    fleet = fleetAll;
                }
                else
                {
                    fleet = "'" + fleet + "'";
                }
                if(cm.DepartmenManager == "12")
                {
                    resultInfo = _dbMsSql.SqlQuery<VehicleMaintenanceInfo>(@"SELECT  MotorcadeName,Name,CabLicense,MaintenanceType,Date,Time,Address,Yanche,vm.Status,MobilePhone
                              FROM VehicleMaintenanceInfo vm
                              left join [DZ_DW].[dbo].[Visionet_DriverInfo_View] vdv on vm.carNo=vdv.CabVMLicense
                              where vdv.Organization in (" + fleet + @") 
                              and vm.Status='0' 
                              order by MotorcadeName asc,Date asc,Time asc,MaintenanceType asc").ToList();
                }
                else
                {
                    resultInfo = _dbMsSql.SqlQuery<VehicleMaintenanceInfo>(@"SELECT  MotorcadeName,Name,CabLicense,MaintenanceType,Date,Time,Address,Yanche,vm.Status,MobilePhone
                              FROM VehicleMaintenanceInfo vm
                              left join [DZ_DW].[dbo].[Visionet_DriverInfo_View] vdv on vm.carNo=vdv.CabVMLicense
                              where vdv.Organization=@OrgName  and vdv.MotorcadeName in (" + fleet + @") 
                              and vm.Status='0' 
                              order by MotorcadeName asc,Date asc,Time asc,MaintenanceType asc", new { OrgName = orgName}).ToList();
                }
               
                if (type != "0" && type != "")
                {
                    if (type == "4")//验车
                    {
                        resultInfo = resultInfo.Where(x => x.Yanche == "1").ToList();
                    }
                    else
                    {
                        resultInfo = resultInfo.Where(x => x.MaintenanceType.Contains(type)).ToList();
                    }                      
                }
                if (status != "")
                {
                    resultInfo = resultInfo.Where(x => x.Status.Contains(status)).ToList();
                }
                if (idCard != "0" && idCard != "" && idCard != null)
                {
                    resultInfo = resultInfo.Where(x => x.CabLicense.Contains(idCard)).ToList();
                }
            }
            return Json(resultInfo, JsonRequestBehavior.AllowGet);
        }
    }
}
