using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.WeChatExercise.BusinessLogic;
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

namespace DaZhongManagementSystem.Areas.PartnerInquiryManagement.Controllers.DriverPointsDetails
{
    public class DriverPointsDetailsController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.MaxDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            return View();
        }
        public JsonResult GetPointsListBySearch(string fleet, string name, string code)
        {
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var ownedCompany = cm.Organization;
            var ownedFleet = cm.MotorcadeName;
            var fleetAll = PartnerHomePageController.getSqlInValue(cm.MotorcadeName,code);
            List<DriverScore> driverScoreList = new List<DriverScore>();
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
                if (cm.DepartmenManager == "12")
                {
                    driverScoreList = _dbMsSql.SqlQuery<DriverScore>(@"select DriverId,Name,convert(int,score) as Score from [DZ_DW].[dbo].[Visionet_DriverInfo_View] vdv
                                        left join tb_query_score qs on qs.id_no=vdv.IdCard
                                        where vdv.Organization in (" + fleetAll + @")  and vdv.status='1' and qs.score is not null and qs.score != 0
                                        order by DriverId asc").ToList();
                }
                else
                {
                    driverScoreList = _dbMsSql.SqlQuery<DriverScore>(@"select DriverId,Name,convert(int,score) as Score from [DZ_DW].[dbo].[Visionet_DriverInfo_View] vdv
                                        left join tb_query_score qs on qs.id_no=vdv.IdCard
                                        where vdv.Organization=@OwnedCompany and vdv.MotorcadeName in (" + fleet + @")  and vdv.status='1' and qs.score is not null and qs.score != 0
                                        order by DriverId asc",
                                        new { OwnedCompany = ownedCompany }).ToList();
                }
                if (name != "" && name != null)
                {
                    driverScoreList = driverScoreList.Where(x => x.Name.Contains(name)).ToList();
                }
            }
            return Json(driverScoreList, JsonRequestBehavior.AllowGet);
        }
    }
}
