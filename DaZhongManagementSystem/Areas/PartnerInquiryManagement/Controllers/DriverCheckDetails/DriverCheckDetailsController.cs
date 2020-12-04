using DaZhongManagementSystem.Areas.PartnerInquiryManagement.Models;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using DaZhongTransitionLiquidation.Common.Pub;
using SqlSugar;
using SyntacticSugar;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.PartnerInquiryManagement.Controllers.DriverCheckDetails
{
    public class DriverCheckDetailsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetElectronicInfo(string code)
        {
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var orgName = cm.Organization;
            var fleet = cm.MotorcadeName;
            var plate_no = cm.CabLicense;
            List<Electronic_police> resultInfo = new List<Electronic_police>();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                resultInfo = _dbMsSql.SqlQuery<Electronic_police>(@"select plate_no,peccancy_date,score,amercement,area,act from tb_electronic_police ep
                        left join [DZ_DW].[dbo].[Visionet_CabInfo_View] vcv on ep.plate_no=vcv.CabLicense
                        where vcv.Organization=@OrgName and vcv.Motorcade=@Fleet and ep.plate_no=@Plate_no and ep.status_cd='0'
                        order by peccancy_date desc", new { OrgName = orgName, Fleet = fleet, Plate_no = plate_no }).ToList();
            }
            return Json(resultInfo, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAccidentInfo(string code)
        {
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var orgName = cm.Organization;
            var fleet = cm.MotorcadeName;
            var plate_no = cm.CabLicense;
            List<Accident_cabInfo> visionetList = new List<Accident_cabInfo>();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                visionetList = _dbMsSql.SqlQuery<Accident_cabInfo>(@"SELECT driverName,carNo,occurrenceTime,accidentNo,accidentGradeName,accidentTypeName,accidentLocation
                                        FROM [DZzl_DW].[dbo].[AccidentCompleteInfo] ac
                                        left join [DZ_DW].[dbo].[Visionet_CabInfo_View] vcv on ac.carNo=vcv.CabLicense
                                        where vcv.Organization=@OwnedCompany and vcv.Motorcade=@ownedFleet 
                                        and ac.carNo=@Plate_no",
                                        new { OwnedCompany = orgName, OwnedFleet = fleet, Plate_no = plate_no }).ToList();
            }
            return Json(visionetList, JsonRequestBehavior.AllowGet);
        }
    }
}
