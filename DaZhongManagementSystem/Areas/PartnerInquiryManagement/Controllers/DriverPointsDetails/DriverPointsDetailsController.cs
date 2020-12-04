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
            var fleetAll = PartnerHomePageController.getSqlInValue(cm.MotorcadeName);
            //var ownedCompany = "市北大众";//测试使用
            //var ownedFleet = "一车队";
            //var date = DateTime.Now.ToString("yyyy-MM-dd");
            List<DriverScore> driverScoreList = new List<DriverScore>();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                if (fleet == "0")
                {
                    driverScoreList = _dbMsSql.SqlQuery<DriverScore>(@"select DriverId,Name,convert(int,score) as Score from [DZ_DW].[dbo].[Visionet_DriverInfo_View] vdv
                                        left join tb_query_score qs on qs.id_no=vdv.IdCard
                                        where vdv.Organization=@OwnedCompany and vdv.MotorcadeName in (" + fleetAll + @")  and vdv.status='1' and qs.score is not null
                                        order by DriverId asc",
                                        new { OwnedCompany = ownedCompany }).ToList();
                }
                else
                {
                    driverScoreList = _dbMsSql.SqlQuery<DriverScore>(@"select DriverId,Name,convert(int,score) as Score from [DZ_DW].[dbo].[Visionet_DriverInfo_View] vdv
                                        left join tb_query_score qs on qs.id_no=vdv.IdCard
                                        where vdv.Organization=@OwnedCompany and vdv.MotorcadeName=@OwnedFleet and vdv.status='1' and qs.score is not null
                                        order by DriverId asc",
                                        new { OwnedCompany = ownedCompany, OwnedFleet = fleet }).ToList();
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
