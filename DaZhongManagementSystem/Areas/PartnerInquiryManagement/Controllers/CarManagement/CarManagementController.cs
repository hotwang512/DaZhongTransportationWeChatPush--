using DaZhongManagementSystem.Areas.PartnerInquiryManagement.Controllers.PartnerHomePage;
using DaZhongManagementSystem.Areas.PartnerInquiryManagement.Models;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using DaZhongTransitionLiquidation.Common.Pub;
using SqlSugar;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.PartnerInquiryManagement.Controllers.CarManagement
{
    public class CarManagementController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.MaxDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            return View();
        }
        public JsonResult GetElectronicInfo(string fleet, string date, string carID, string code)
        {
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var ownedCompany = cm.Organization;
            var fleetAll = PartnerHomePageController.getSqlInValue(cm.MotorcadeName, code);
            //ownedCompany = "第一服务中心";
            var date1 = date.TryToDate();
            var date2 = date1.AddDays(1).ToString("yyyy-MM-dd");
            //date1 = "2020-10-29";
            //date2 = "2020-11-30";
            List<Electronic_police> electronicList = new List<Electronic_police>();
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
                electronicList = getElectronicList(_dbMsSql, date, fleet, ownedCompany, date1, date2, cm.DepartmenManager);
                if (carID != null && carID != "")
                {
                    electronicList = electronicList.Where(x => x.plate_no.Contains(carID)).ToList();
                }
            }
            return Json(electronicList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAccidentInfo(string fleet, string date, string carID, string code)
        {
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var ownedCompany = cm.Organization;
            var fleetAll = PartnerHomePageController.getSqlInValue(cm.MotorcadeName, code);
            //ownedCompany = "第一服务中心";
            var date1 = date.TryToDate();
            var date2 = date1.AddDays(1).ToString("yyyy-MM-dd");
            //var date1 = "2020-04-24";
            //var date2 = "2020-06-14";
            List<Accident_cabInfo> visionetList = new List<Accident_cabInfo>();
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
                visionetList = getVisionetList(_dbMsSql, date, fleet, ownedCompany, date1, date2, cm.DepartmenManager);
                if (carID != null && carID != "")
                {
                    visionetList = visionetList.Where(x => x.carNo.Contains(carID)).ToList();
                }
            }
            return Json(visionetList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOperationInfo(string fleet, string date, string carID, string code)
        {
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var ownedCompany = cm.Organization;
            var fleetAll = PartnerHomePageController.getSqlInValue(cm.MotorcadeName, code);
            //ownedCompany = "第一服务中心";
            List<Visionet_CabInfo> carList = new List<Visionet_CabInfo>();
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
                    carList = _dbMsSql.SqlQuery<Visionet_CabInfo>(@"select CabLicense,Motorcade from [DZ_DW].[dbo].Visionet_CabInfo_View where Organization in (" + fleet + @")  
                                        and OperationStatus=0 order by Motorcade asc").ToList();
                }
                else
                {
                    carList = _dbMsSql.SqlQuery<Visionet_CabInfo>(@"select CabLicense,Motorcade from [DZ_DW].[dbo].Visionet_CabInfo_View where Organization=@OwnedCompany and Motorcade in (" + fleet + @")  
                                        and OperationStatus=0 order by Motorcade asc",
                                        new { OwnedCompany = ownedCompany, OwnedFleet = fleet }).ToList();
                }
                if (carID != null && carID != "")
                {
                    carList = _dbMsSql.SqlQuery<Visionet_CabInfo>(@"select CabLicense,Motorcade from [DZ_DW].[dbo].Visionet_CabInfo_View where Organization=@OwnedCompany and Motorcade in (" + fleet + @") 
                                        and OperationStatus=0 and CabLicense like '%" + carID + "%' order by Motorcade asc",
                                       new { OwnedCompany = ownedCompany, OwnedFleet = fleet }).ToList();
                }
            }
            return Json(carList, JsonRequestBehavior.AllowGet);
        }
        private List<Electronic_police> getElectronicList(SqlSugarClient _dbMsSql, string date, string fleet, string ownedCompany, DateTime date1, string date2, string departmenManager)
        {
            var electronicList = new List<Electronic_police>();
            if (departmenManager == "12")
            {
                if (date == "")
                {
                    electronicList = _dbMsSql.SqlQuery<Electronic_police>(@"select plate_no,peccancy_date,score,amercement,area,act from tb_electronic_police ep
                                        left join [DZ_DW].[dbo].[Visionet_CabInfo_View] vcv on ep.plate_no=vcv.CabLicense
                                        where vcv.Organization in (" + fleet + @") 
                                        --and ep.peccancy_date between @Date1 and @Date2
                                        and ep.status_cd='0'
                                        order by peccancy_date desc").ToList();
                }
                else
                {
                    electronicList = _dbMsSql.SqlQuery<Electronic_police>(@"select plate_no,peccancy_date,score,amercement,area,act from tb_electronic_police ep
                                        left join [DZ_DW].[dbo].[Visionet_CabInfo_View] vcv on ep.plate_no=vcv.CabLicense
                                        where vcv.Organization in (" + fleet + @") 
                                        and ep.peccancy_date between @Date1 and @Date2
                                        and ep.status_cd='0'
                                        order by peccancy_date desc",
                                        new { Date1 = date1, Date2 = date2 }).ToList();
                }
            }
            else
            {
                if (date == "")
                {
                    electronicList = _dbMsSql.SqlQuery<Electronic_police>(@"select plate_no,peccancy_date,score,amercement,area,act from tb_electronic_police ep
                                        left join [DZ_DW].[dbo].[Visionet_CabInfo_View] vcv on ep.plate_no=vcv.CabLicense
                                        where vcv.Organization=@OwnedCompany and vcv.Motorcade in (" + fleet + @") 
                                        --and ep.peccancy_date between @Date1 and @Date2
                                        and ep.status_cd='0'
                                        order by peccancy_date desc",
                                        new { OwnedCompany = ownedCompany }).ToList();
                }
                else
                {
                    electronicList = _dbMsSql.SqlQuery<Electronic_police>(@"select plate_no,peccancy_date,score,amercement,area,act from tb_electronic_police ep
                                        left join [DZ_DW].[dbo].[Visionet_CabInfo_View] vcv on ep.plate_no=vcv.CabLicense
                                        where vcv.Organization=@OwnedCompany and vcv.Motorcade in (" + fleet + @") 
                                        and ep.peccancy_date between @Date1 and @Date2
                                        and ep.status_cd='0'
                                        order by peccancy_date desc",
                                        new { Date1 = date1, Date2 = date2, OwnedCompany = ownedCompany }).ToList();
                }
            }
            return electronicList;
        }
        private List<Accident_cabInfo> getVisionetList(SqlSugarClient _dbMsSql, string date, string fleet, string ownedCompany, DateTime date1, string date2, string departmenManager)
        {
            var visionetList = new List<Accident_cabInfo>();
            if (departmenManager == "12")
            {
                if (date == "")
                {
                    visionetList = _dbMsSql.SqlQuery<Accident_cabInfo>(@"SELECT driverName,carNo,occurrenceTime,accidentNo,accidentGradeName,accidentTypeName,accidentLocation
                                        FROM [DZzl_DW].[dbo].[AccidentCompleteInfo] ac
                                        left join [DZ_DW].[dbo].[Visionet_CabInfo_View] vcv on ac.carNo=vcv.CabLicense
                                        where vcv.Organization in (" + fleet + @")  
                                        --and ac.occurrenceTime between @Date1 and @Date2").ToList();
                }
                else
                {
                    visionetList = _dbMsSql.SqlQuery<Accident_cabInfo>(@"SELECT driverName,carNo,occurrenceTime,accidentNo,accidentGradeName,accidentTypeName,accidentLocation
                                        FROM [DZzl_DW].[dbo].[AccidentCompleteInfo] ac
                                        left join [DZ_DW].[dbo].[Visionet_CabInfo_View] vcv on ac.carNo=vcv.CabLicense
                                        where vcv.Organization in (" + fleet + @")   
                                        and ac.occurrenceTime between @Date1 and @Date2",
                                        new { Date1 = date1, Date2 = date2 }).ToList();
                }
            }
            else
            {
                if (date == "")
                {
                    visionetList = _dbMsSql.SqlQuery<Accident_cabInfo>(@"SELECT driverName,carNo,occurrenceTime,accidentNo,accidentGradeName,accidentTypeName,accidentLocation
                                        FROM [DZzl_DW].[dbo].[AccidentCompleteInfo] ac
                                        left join [DZ_DW].[dbo].[Visionet_CabInfo_View] vcv on ac.carNo=vcv.CabLicense
                                        where vcv.Organization=@OwnedCompany and vcv.Motorcade in (" + fleet + @")  
                                        --and ac.occurrenceTime between @Date1 and @Date2",
                                        new { Date1 = date1, Date2 = date2, OwnedCompany = ownedCompany }).ToList();
                }
                else
                {
                    visionetList = _dbMsSql.SqlQuery<Accident_cabInfo>(@"SELECT driverName,carNo,occurrenceTime,accidentNo,accidentGradeName,accidentTypeName,accidentLocation
                                        FROM [DZzl_DW].[dbo].[AccidentCompleteInfo] ac
                                        left join [DZ_DW].[dbo].[Visionet_CabInfo_View] vcv on ac.carNo=vcv.CabLicense
                                        where vcv.Organization=@OwnedCompany and vcv.Motorcade in (" + fleet + @")  
                                        and ac.occurrenceTime between @Date1 and @Date2",
                                        new { Date1 = date1, Date2 = date2, OwnedCompany = ownedCompany }).ToList();
                }
            }
            return visionetList;
        }
    }
}
