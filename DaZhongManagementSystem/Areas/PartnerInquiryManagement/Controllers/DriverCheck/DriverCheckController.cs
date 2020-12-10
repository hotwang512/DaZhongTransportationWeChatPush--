using DaZhongManagementSystem.Areas.PartnerInquiryManagement.Models;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using DaZhongTransitionLiquidation.Common.Pub;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.PartnerInquiryManagement.Controllers.DriverCheck
{
    public class DriverCheckController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetTaxiSummaryInfo(string code)
        {
            var dataList = "";
            var dataList2 = "";
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var orgName = cm.Organization;
            var fleet = cm.MotorcadeName;
            var cabVMLicense = cm.CabVMLicense;
            //var orgName = "第一服务中心";
            //var fleet = "仁强";
            JObject jObject = new JObject();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                var date1 = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                var date2 = date1.TryToDate().AddDays(-1).ToString("yyyy-MM-dd");
                //最新数据
                dataList = _dbMsSql.SqlQueryJson(@"select * from t_taximeter_data where 日期='" + date1 + " 00:00:00' and 车牌号='" + cabVMLicense + "'");
                //前一天数据
                dataList2 = _dbMsSql.SqlQueryJson(@"select * from t_taximeter_data where 日期='" + date2 + " 00:00:00' and 车牌号='" + cabVMLicense + "'");
                if (dataList != null && dataList != "" && dataList.Count() > 2)
                {
                    Regex rgx = new Regex(@"(?i)(?<=\[)(.*)(?=\])");//中括号[]
                    string tmp1 = rgx.Match(dataList).Value;//中括号[]
                    string tmp2 = rgx.Match(dataList2).Value;//中括号[]
                    JObject jo1 = (JObject)JsonConvert.DeserializeObject(tmp1);
                    JObject jo2 = (JObject)JsonConvert.DeserializeObject(tmp2);
                    jObject = jo1;
                    //计算上涨或者下跌率
                    string rate = "";
                    List<string> strValue = new List<string>() { "营收", "差次", "线上营收", "线上差次" };
                    foreach (var item in strValue)
                    {
                        var value1_1 = jo1[item].TryToDecimal();
                        var value2_1 = jo2[item].TryToDecimal();
                        if (value1_1 > value2_1 && value2_1 != 0)
                        {
                            rate = "↑" + (decimal.Round((value1_1 - value2_1) / value2_1 * 100, 2)) + "%";

                        }
                        else if (value1_1 < value2_1 && value2_1 != 0)
                        {
                            rate = "↓" + (decimal.Round((value2_1 - value1_1) / value2_1 * 100, 2)) + "%";
                        }
                        else
                        {
                            rate = "0%";
                        }
                        jObject.Add(item + "率", rate);
                    }
                }
            }
            return Json(jObject.ObjToString(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCarViolationInfo(string code)
        {
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var orgName = cm.Organization;
            var fleet = cm.MotorcadeName;
            var plate_no = cm.CabLicense;
            List<Electronic_police> resultInfo = new List<Electronic_police>();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                resultInfo = _dbMsSql.SqlQuery<Electronic_police>(@"select  plate_no,peccancy_date,score,amercement,area,act from tb_electronic_police ep
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
                visionetList = _dbMsSql.SqlQuery<Accident_cabInfo>(@"SELECT  driverName,carNo,occurrenceTime,accidentNo,accidentGradeName,accidentTypeName,accidentLocation
                                        FROM [DZzl_DW].[dbo].[AccidentCompleteInfo] ac
                                        left join [DZ_DW].[dbo].[Visionet_CabInfo_View] vcv on ac.carNo=vcv.CabLicense
                                        where vcv.Organization=@OwnedCompany and vcv.Motorcade=@ownedFleet 
                                        and ac.carNo=@Plate_no  order by occurrenceTime desc",
                                        new { OwnedCompany = orgName, OwnedFleet = fleet, Plate_no = plate_no }).ToList();
            }
            return Json(visionetList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBaseData(string code)
        {
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var orgName = cm.Organization;
            var fleet = cm.MotorcadeName;
            var plate_no = cm.CabLicense;
            var idCard = cm.IdCard;
            var date = DateTime.Now.ToString("yyyy-MM");
            var score = "";
            List<string> BaseDataList = new List<string>();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                score = _dbMsSql.SqlQuery<string>(@"select * from dbo.tb_query_score where id_no=@IDCard",new { IDCard = plate_no }).ToList().FirstOrDefault();
                if(score == null || score == "")
                {
                    score = "0";
                }
            }
            AnswerStatus answerStatusList = new AnswerStatus();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                var answerList = _dbMsSql.SqlQuery<AnswerStatus>(@"select * 
                                 from(
                                  select p.Name,p.IDNumber,convert(varchar(7),ei.CreatedDate,120) as sDate,
                                      case when eai.TotalScore>=60 then '合格' --已阅答题
                                     when eai.TotalScore<60 then '不合格' --已阅未答题
                                     else '未答题' --未阅未答题
                                      end as Result
                                  from [dbo].[Business_Personnel_Information] p
                                  left join Business_ExercisesAnswer_Information eai on p.Vguid=eai.BusinessPersonnelVguid and convert(varchar,eai.CreatedDate,23)>=@Date
                                  left join Business_Exercises_Infomation ei on eai.BusinessExercisesVguid=ei.Vguid    and convert(varchar,ei.CreatedDate,23)>=@Date
                                  where p.ApprovalStatus='2'
                                  )a PIVOT( MAX(Result) FOR sDate IN([Status]) ) AS T ", new { Date = date}).ToList();
                answerStatusList = answerList.Where(x => x.IDNumber == idCard).ToList().FirstOrDefault();
            }
            BaseDataList.Add(score);
            BaseDataList.Add(answerStatusList.Status);
            BaseDataList.Add(DateTime.Now.Month.ToString());
            BaseDataList.Add(cm.Name);
            BaseDataList.Add(plate_no);
            BaseDataList.Add(fleet);
            return Json(BaseDataList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVehicleMaintenanceInfo(string code)
        {
            //保养
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var orgName = cm.Organization;
            var fleet = cm.MotorcadeName;
            var plate_no = cm.CabLicense;
            var idCard = cm.IdCard;
            List<VehicleMaintenanceInfo> resultInfo = new List<VehicleMaintenanceInfo>();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                orgName = "第一服务中心";
                fleet = "仁强";
                plate_no = "沪GV1017";
                idCard = "320825197806023613";
                resultInfo = _dbMsSql.SqlQuery<VehicleMaintenanceInfo>(@"SELECT  MotorcadeName,Name,CabLicense,MaintenanceType,Date,Time,Address,Yanche,vm.Status,MobilePhone
                              FROM VehicleMaintenanceInfo vm
                              left join [DZ_DW].[dbo].[Visionet_DriverInfo_View] vdv on vm.carNo=vdv.CabVMLicense
                              where vdv.Organization=@OrgName  and vdv.MotorcadeName=@Fleet  
                              and  vdv.CabLicense=@Plate_no and vdv.IdCard=@IdCard  order by Date desc,Time desc", 
                              new { OrgName = orgName, Fleet = fleet, Plate_no = plate_no, IdCard = idCard }).ToList();
            }
            return Json(resultInfo, JsonRequestBehavior.AllowGet);
        }
    }
}
