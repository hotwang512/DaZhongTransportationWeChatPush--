using DaZhongManagementSystem.Areas.PartnerInquiryManagement.Controllers.PartnerHomePage;
using DaZhongManagementSystem.Areas.PartnerInquiryManagement.Models;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using DaZhongTransitionLiquidation.Common.Pub;
using SqlSugar;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.PartnerInquiryManagement.Controllers.DriverManagement
{
    public class DriverManagementController : Controller
    {
        public ActionResult Index(string code)
        {
            return View();
        }
        public JsonResult getAnswerStatus(string fleet,string code)
        {
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            List<int> status = new List<int>();
            var ownedFleet = cm.OldMotorcadeName;//公司
            var ownedCompany = fleet;//车队
            var fleetAll = PartnerHomePageController.getSqlInValue(cm.MotorcadeName,code);
            //var ownedCompany = "第一服务中心";
            //var ownedFleet = "仁强";
            var date = DateTime.Now.ToString("yyyy-MM");
            List<AnswerStatus> answerStatusList = new List<AnswerStatus>();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
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
                    var ownedFleetAll = getOwnedFleetAll(_dbMsSql, cm.MotorcadeName);
                    answerStatusList = _dbMsSql.SqlQuery<AnswerStatus>(@"select * 
                         from(select p.Name, p.IDNumber, convert(varchar(7), ei.CreatedDate, 120) as sDate,
                              case when eai.TotalScore >= 60 then '合格' --已阅答题
                             when eai.TotalScore < 60 then '不合格' --已阅未答题
                             else '未答题'  --未阅未答题
                              end as Result
                          from[dbo].[Business_Personnel_Information] p
                          left join Business_ExercisesAnswer_Information eai on p.Vguid = eai.BusinessPersonnelVguid and convert(varchar, eai.CreatedDate, 23) >= @Date
                          left join Business_Exercises_Infomation ei on eai.BusinessExercisesVguid = ei.Vguid    and convert(varchar, ei.CreatedDate, 23) >= @Date
                          where p.ApprovalStatus = '2' and  OwnedFleet in (" + ownedFleetAll + @")
                          )a PIVOT(MAX(Result) FOR sDate IN([Status])) AS T ", new { Date = date });
                }
                else
                {
                    answerStatusList = _dbMsSql.SqlQuery<AnswerStatus>(@"select * 
                         from(select p.Name, p.IDNumber, convert(varchar(7), ei.CreatedDate, 120) as sDate,
                              case when eai.TotalScore >= 60 then '合格' --已阅答题
                             when eai.TotalScore < 60 then '不合格' --已阅未答题
                             else '未答题'  --未阅未答题
                              end as Result
                          from[dbo].[Business_Personnel_Information] p
                          left join Business_ExercisesAnswer_Information eai on p.Vguid = eai.BusinessPersonnelVguid and convert(varchar, eai.CreatedDate, 23) >= @Date
                          left join Business_Exercises_Infomation ei on eai.BusinessExercisesVguid = ei.Vguid    and convert(varchar, ei.CreatedDate, 23) >= @Date
                          where p.ApprovalStatus = '2' and p.OwnedCompany in (" + fleet + @") and OwnedFleet = @OwnedFleet
                          )a PIVOT(MAX(Result) FOR sDate IN([Status])) AS T ", new { Date = date, OwnedFleet = ownedFleet });
                }
                var result1 = answerStatusList.Where(x => x.Status == "合格").ToList().Count;
                var result2 = answerStatusList.Where(x => x.Status == "不合格").ToList().Count;
                var result3 = answerStatusList.Where(x => x.Status != "合格" && x.Status != "不合格").ToList().Count;
                status.Add(result1);
                status.Add(result2);
                status.Add(result3);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public static string getOwnedFleetAll(SqlSugarClient _dbMsSql, string motorcadeName)
        {
            var orgAll = "";
            var str = motorcadeName.Split(',');
            var OrgData = _dbMsSql.Queryable<Master_Organization>().ToList();
            foreach (var item in str)
            {
                var vguidList = OrgData.Where(x => x.Description == item).ToList().FirstOrDefault();
                if(vguidList != null)
                {
                    orgAll += "'" + vguidList.Vguid + "',";
                }
            }
            orgAll = orgAll.Substring(0, orgAll.Length - 1);
            return orgAll;
        }
        public JsonResult GetDriverScore(string fleet, string code)
        {
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var ownedCompany = cm.Organization;
            var fleetAll = PartnerHomePageController.getSqlInValue(cm.MotorcadeName,code);
            //var ownedCompany = "市北大众";
            //fleet = "一车队";
            List<int> scoreList = new List<int>();
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
                                        where vdv.Organization in (" + fleetAll + @")  and vdv.status='1' and qs.score is not null
                                        order by DriverId asc",
                                        new { OwnedCompany = ownedCompany }).ToList();
                }
                else
                {
                    driverScoreList = _dbMsSql.SqlQuery<DriverScore>(@"select DriverId,Name,convert(int,score) as Score from [DZ_DW].[dbo].[Visionet_DriverInfo_View] vdv
                                        left join tb_query_score qs on qs.id_no=vdv.IdCard
                                        where vdv.Organization=@OwnedCompany and vdv.MotorcadeName in (" + fleet + @")  and vdv.status='1' and qs.score is not null
                                        order by DriverId asc",
                                        new { OwnedCompany = ownedCompany }).ToList();
                }
                var result1 = driverScoreList.Where(x => x.Score == 12).ToList().Count;
                var result2 = driverScoreList.Where(x => x.Score > 8 && x.Score < 11).ToList().Count;
                var result3 = driverScoreList.Where(x => x.Score == 0).ToList().Count;
                scoreList.Add(result1);
                scoreList.Add(result2);
                scoreList.Add(result3);
            }
            return Json(scoreList, JsonRequestBehavior.AllowGet);
        }
    }
}
