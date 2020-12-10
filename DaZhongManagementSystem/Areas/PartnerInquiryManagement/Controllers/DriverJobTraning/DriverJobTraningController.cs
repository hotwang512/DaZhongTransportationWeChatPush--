using DaZhongManagementSystem.Areas.PartnerInquiryManagement.Controllers.DriverManagement;
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

namespace DaZhongManagementSystem.Areas.PartnerInquiryManagement.Controllers.DriverJobTraning
{
    public class DriverJobTraningController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.MaxDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            return View();
        }
        public JsonResult GetOwnedCompanyList(string fleet, string name, string date, string code)
        {
            //userInfo.UserId = "13611794751";
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var ownedFleet = cm.OldMotorcadeName;
            var ownedCompany = cm.OldOrganization;
            var fleetAll = PartnerHomePageController.getSqlInValue(cm.MotorcadeName,code);
            if(date == "")
            {
                date = DateTime.Now.ToString("yyyy-MM");
            }
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
                    var ownedFleetAll = DriverManagementController.getOwnedFleetAll(_dbMsSql, cm.MotorcadeName);
                    answerStatusList = _dbMsSql.SqlQuery<AnswerStatus>(@"select * 
                         from(select p.Name, p.IDNumber, convert(varchar(7), ei.CreatedDate, 120) as sDate,
                              case when eai.TotalScore >= 60 then '合格' --已阅答题
                             when eai.TotalScore < 60 then '不合格' --已阅未答题
                             else '未答题'  --未阅未答题
                              end as Result
                          from[dbo].[Business_Personnel_Information] p
                          left join Business_ExercisesAnswer_Information eai on p.Vguid = eai.BusinessPersonnelVguid and convert(varchar, eai.CreatedDate, 23) >= @Date
                          left join Business_Exercises_Infomation ei on eai.BusinessExercisesVguid = ei.Vguid    and convert(varchar, ei.CreatedDate, 23) >= @Date
                          where p.ApprovalStatus = '2'  and OwnedFleetin (" + ownedFleetAll + @")
                          )a PIVOT(MAX(Result) FOR sDate IN([Status])) AS T ", new { Date = date, OwnedFleet = ownedFleet });
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
                if (name != "" && name != null)
                {
                    answerStatusList = answerStatusList.Where(x => x.Name.Contains(name)).ToList();
                }
            }
            return Json(answerStatusList, JsonRequestBehavior.AllowGet);
        }   
    }
}
