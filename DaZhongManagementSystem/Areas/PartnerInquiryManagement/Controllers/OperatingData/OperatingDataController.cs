using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.WeChatExercise.BusinessLogic;
using DaZhongManagementSystem.Areas.PartnerInquiryManagement.Controllers.PartnerHomePage;
using DaZhongManagementSystem.Areas.PartnerInquiryManagement.Models;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using DaZhongTransitionLiquidation.Common.Pub;
using JQWidgetsSugar;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.PartnerInquiryManagement.Controllers.OperatingData
{
    public class OperatingDataController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.MaxDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            return View();
        }
        public JsonResult GetTaxiSummaryInfo(string fleet, string dateSearch, string code)
        {
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var fleetAll = PartnerHomePageController.getSqlInValue(cm.MotorcadeName, code);
            if (cm.DepartmenManager == "12")
            {
                fleetAll = PartnerHomePageController.getSqlInValue(cm.MotorcadeNameRemark, code);
            }
            var orgName = cm.Organization;
            var dataList = "";
            var dataList2 = "";
            JObject jObject = new JObject();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                var date1 = dateSearch.TryToDate();
                var date2 = date1.AddDays(-1).ToString("yyyy-MM-dd");
                if (fleet == "0")
                {
                    fleet = fleetAll;
                }
                else
                {
                    if (cm.DepartmenManager == "12")
                    {
                        fleet = _dbMsSql.SqlQuery<string>(@"select Remark from DZ_Organization where status=0 and OrganizationName=@OrganizationName", new { OrganizationName = fleet }).ToList().FirstOrDefault(); ;
                    }
                    fleet = "'" + fleet + "'";
                }
                if (cm.DepartmenManager == "12")
                {
                    //最新数据
                    dataList = _dbMsSql.SqlQueryJson(@"select isnull(Sum(convert(decimal(18,2),总营收)),0) as 总营收,
                                        isnull(Sum(convert(decimal(18,2),总差次)),0) as 总差次, 
                                        isnull(Sum(convert(decimal(18,2),营运车辆总数)),0) as 营运车辆总数,
                                        isnull(Sum(convert(decimal(18,2),总线上营收)),0) as 总线上营收, 
                                        isnull(Sum(convert(decimal(18,2),总线上差次)),0) as 总线上差次,
                                        convert(decimal(18,2),
                                        isnull(Sum(convert(decimal(18,2),车均营收)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均营收,
			                            convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均差次)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均差次,
                                        convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均线上营收)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均线上营收,
                                        convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均线上差次)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均线上差次, 
                                        convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均行驶里程)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均行驶里程, 
                                        convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均营运里程)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均营运里程, 
                                        convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均空驶里程)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均空驶里程
                                        from t_taxi_summary where 日期='" + dateSearch + "' and 公司 in (" + fleet + ") ");
                    //前一天数据
                    dataList2 = _dbMsSql.SqlQueryJson(@"select isnull(Sum(convert(decimal(18,2),总营收)),0) as 总营收,
                                        isnull(Sum(convert(decimal(18,2),总差次)),0) as 总差次, 
                                        isnull(Sum(convert(decimal(18,2),营运车辆总数)),0) as 营运车辆总数,
                                        isnull(Sum(convert(decimal(18,2),总线上营收)),0) as 总线上营收, 
                                        isnull(Sum(convert(decimal(18,2),总线上差次)),0) as 总线上差次,
                                        convert(decimal(18,2),
                                        isnull(Sum(convert(decimal(18,2),车均营收)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均营收,
			                            convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均差次)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均差次,
                                         convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均线上营收)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均线上营收,
                                        convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均线上差次)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均线上差次, 
                                        convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均行驶里程)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均行驶里程, 
                                        convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均营运里程)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均营运里程, 
                                        convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均空驶里程)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均空驶里程  
                                        from t_taxi_summary where 日期='" + date2 + "' and 公司 in (" + fleet + ") ");
                }
                else
                {
                    //最新数据
                    dataList = _dbMsSql.SqlQueryJson(@"select isnull(Sum(convert(decimal(18,2),总营收)),0) as 总营收,
                                        isnull(Sum(convert(decimal(18,2),总差次)),0) as 总差次, 
                                        isnull(Sum(convert(decimal(18,2),营运车辆总数)),0) as 营运车辆总数,
                                        isnull(Sum(convert(decimal(18,2),总线上营收)),0) as 总线上营收, 
                                        isnull(Sum(convert(decimal(18,2),总线上差次)),0) as 总线上差次,
                                        convert(decimal(18,2),
                                        isnull(Sum(convert(decimal(18,2),车均营收)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均营收,
			                            convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均差次)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均差次,
                                         convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均线上营收)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均线上营收,
                                        convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均线上差次)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均线上差次, 
                                        convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均行驶里程)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均行驶里程, 
                                        convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均营运里程)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均营运里程, 
                                        convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均空驶里程)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均空驶里程
                                        from t_taxi_summary where 日期='" + dateSearch + "' and 车队 in (" + fleet + ") and 公司='" + orgName + "'");
                    //前一天数据
                    dataList2 = _dbMsSql.SqlQueryJson(@"select isnull(Sum(convert(decimal(18,2),总营收)),0) as 总营收,
                                        isnull(Sum(convert(decimal(18,2),总差次)),0) as 总差次, 
                                        isnull(Sum(convert(decimal(18,2),营运车辆总数)),0) as 营运车辆总数,
                                        isnull(Sum(convert(decimal(18,2),总线上营收)),0) as 总线上营收, 
                                        isnull(Sum(convert(decimal(18,2),总线上差次)),0) as 总线上差次,
                                        convert(decimal(18,2),
                                        isnull(Sum(convert(decimal(18,2),车均营收)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均营收,
			                            convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均差次)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均差次,
                                         convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均线上营收)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均线上营收,
                                        convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均线上差次)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均线上差次, 
                                        convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均行驶里程)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均行驶里程, 
                                        convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均营运里程)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均营运里程, 
                                        convert(decimal(18,2),
			                            isnull(Sum(convert(decimal(18,2),车均空驶里程)),0)/
			                            isnull(Sum(convert(decimal(18,2),营运车辆总数)),0)) as 车均空驶里程  
                                        from t_taxi_summary where 日期='" + date2 + "' and 车队 in (" + fleet + ") and 公司='" + orgName + "'");
                }
                if (dataList.Count() > 2)
                {
                    Regex rgx = new Regex(@"(?i)(?<=\[)(.*)(?=\])");//中括号[]
                    string tmp1 = rgx.Match(dataList).Value;//中括号[]
                    string tmp2 = rgx.Match(dataList2).Value;//中括号[]
                    JObject jo1 = (JObject)JsonConvert.DeserializeObject(tmp1);
                    JObject jo2 = (JObject)JsonConvert.DeserializeObject(tmp2);
                    jObject = jo1;
                    //计算上涨或者下跌率
                    string rate = "";
                    List<string> strValue = new List<string>() { "总营收", "总差次", "营运车辆总数", "总线上营收", "总线上差次",
                    "车均营收", "车均差次", "车均线上营收", "车均线上差次", "车均行驶里程","车均营运里程","车均空驶里程"};
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
    }
}
