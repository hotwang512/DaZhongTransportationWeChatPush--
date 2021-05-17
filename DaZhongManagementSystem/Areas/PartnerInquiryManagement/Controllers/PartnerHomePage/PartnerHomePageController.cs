using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.OrganizationManagement.OrganizationManageLogic;
using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.WeChatExercise.BusinessLogic;
using DaZhongManagementSystem.Areas.PartnerInquiryManagement.Models;
using DaZhongManagementSystem.Common.LogHelper;
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
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.PartnerInquiryManagement.Controllers.PartnerHomePage
{
    public class PartnerHomePageController : Controller
    {
        public WeChatExerciseLogic _wl;
        public OrganizationManagementLogic _ol;
        public PartnerHomePageController()
        {
            _wl = new WeChatExerciseLogic();
            _ol = new OrganizationManagementLogic();
        }
        public ActionResult Index(string code)
        {
            string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
            U_WeChatUserID userInfo = new U_WeChatUserID();
            string userInfoStr = Common.WeChatPush.WeChatTools.GetUserInfoByCode(accessToken, code);
            userInfo = Common.JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr);//用户ID
            //userInfo.UserId = "13671595340";//合伙人
            //userInfo.UserId = "15921961501";//司机
            Business_Personnel_Information personInfoModel = GetUserInfo(userInfo.UserId);//获取人员表信息
            if (personInfoModel.DepartmenManager == 10 || personInfoModel.DepartmenManager == 11)
            {
                //合伙人;高级合伙人
                getPartnerInfo(personInfoModel);

            }
            else if (personInfoModel.DepartmenManager == 12)
            {
                //公司经理
                getManagerInfo(personInfoModel);
            }
            else if (personInfoModel.DepartmenManager == 1)
            {
                //司机;从Visionet_DriverInfo_View表中取出最新数据
                Personnel_Info Personnel = getPersonnelInfo(personInfoModel);
                if (Personnel != null)
                {
                    Master_Organization organizationDetail = new Master_Organization();
                    organizationDetail = _ol.GetOrganizationDetail(personInfoModel.OwnedFleet.ToString());
                    Personnel.OldMotorcadeName = personInfoModel.OwnedFleet;//公司
                    Personnel.OldOrganization = personInfoModel.OwnedCompany;//车队
                    var key = PubGet.GetUserKey + personInfoModel.Vguid;
                    var csche = CacheManager<Personnel_Info>.GetInstance().Get(key);
                    if (csche != null)
                    {
                        CacheManager<Personnel_Info>.GetInstance().Remove(key);
                    }
                    CacheManager<Personnel_Info>.GetInstance().Add(key, Personnel, 8 * 60 * 60 * 1000);
                    var data = GetTaxiInfo(personInfoModel.Vguid.ToString());
                    if (data == "" || data == null)
                    {
                        return View("/Areas/PartnerInquiryManagement/Views/DriverCheck/Index2.cshtml");
                    }
                    else
                    {
                        ViewBag.Code = personInfoModel.Vguid;
                        return View("/Areas/PartnerInquiryManagement/Views/DriverCheck/Index.cshtml");
                    }
                }
                else
                {
                    return View("/Areas/PartnerInquiryManagement/Views/DriverCheck/Index2.cshtml");
                }

            }
            else
            {
                //ViewBag.Validate = false;
                //ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
                return View("/Areas/PartnerInquiryManagement/Views/PartnerHomePage/Index2.cshtml");
            }
            return View();
        }
        public JsonResult GetTaxiSummaryInfo(string fleet, string code)
        {
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var orgName = cm.Organization;
            var fleetAll = getSqlInValue(cm.MotorcadeName, code);
            if (cm.DepartmenManager == "12")
            {
                fleetAll = getSqlInValue(cm.MotorcadeNameRemark, code);
            }
            var dataList = "";
            var dataList2 = "";
            //var orgName = "第一服务中心";
            //fleet = "仁强";
            JObject jObject = new JObject();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                #region 查询车辆信息
                var date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");//昨天
                //date = "2021-05-12";
                //dataList = _dbMsSql.SqlQueryJson(@"select * from t_taxi_summary where 日期=@Date",new { Date = date });
                var date1 = date.TryToDate();
                var date2 = date1.AddDays(-1).ToString("yyyy-MM-dd");//前天
                //date1 = date.TryToDate();
                //date2 = date1.AddDays(-1).ToString("yyyy-MM-dd");//前天
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
                    var count = _dbMsSql.SqlQuery<int>(@"select count(上线司机数) from  t_taxi_summary where 日期='" + date + "' and 公司 in (" + fleet + ")").FirstOrDefault();
                    if (count == 0)
                    {
                        //没有数据时,避免除0报错或者用nullif(0,0)函数
                        count = 1;
                    }
                    //最新数据
                    dataList = _dbMsSql.SqlQueryJson(@"select isnull(Sum(convert(decimal(18,2),上线司机数)),0) as 上线司机数,
                                            isnull(Sum(convert(decimal(18,2),上线车辆数)),0) as 上线车辆数, 
                                            isnull(Sum(convert(decimal(18,2),总差次)),0) as 总差次,
                                            convert(decimal(18,2),
			                                isnull(Sum(convert(decimal(18,2),车均营收)),0)/" + count + @") as 车均营收,
			                                convert(decimal(18,2),
			                                isnull(Sum(convert(decimal(18,2),车均差次)),0)/" + count + @") as 车均差次,
			                                convert(decimal(18,2),
			                                isnull(Sum(convert(decimal(18,2),车均在线时长)),0)/" + count + @") as 车均在线时长
                                            from t_taxi_summary where 日期='" + date + "' and 公司 in (" + fleet + ")");
                    //前一天数据
                    dataList2 = _dbMsSql.SqlQueryJson(@"select isnull(Sum(convert(decimal(18,2),上线司机数)),0) as 上线司机数,
                                            isnull(Sum(convert(decimal(18,2),上线车辆数)),0) as 上线车辆数, 
                                            isnull(Sum(convert(decimal(18,2),总差次)),0) as 总差次,
                                            convert(decimal(18,2),
			                                isnull(Sum(convert(decimal(18,2),车均营收)),0)/" + count + @") as 车均营收,
			                                convert(decimal(18,2),
			                                isnull(Sum(convert(decimal(18,2),车均差次)),0)/" + count + @") as 车均差次,
			                                convert(decimal(18,2),
			                                isnull(Sum(convert(decimal(18,2),车均在线时长)),0)/" + count + @") as 车均在线时长
                                            from t_taxi_summary where 日期='" + date2 + "' and 公司 in (" + fleet + ")");
                    LogHelper.WriteLog(string.Format("级别：{0},查询日期:{1},上线司机总数：{2},查询日期最新数据：{3}", cm.DepartmenManager, date, count, dataList));
                }
                else
                {
                    var count = _dbMsSql.SqlQuery<int>(@"select count(上线司机数) from  t_taxi_summary where 日期='" + date + "' and 车队 in (" + fleet + ") and 公司='" + orgName + "'").FirstOrDefault();
                    if (count == 0)
                    {
                        //没有数据时,避免除0报错
                        count = 1;
                    }
                    //最新数据
                    dataList = _dbMsSql.SqlQueryJson(@"select isnull(Sum(convert(decimal(18,2),上线司机数)),0) as 上线司机数,
                                            isnull(Sum(convert(decimal(18,2),上线车辆数)),0) as 上线车辆数, 
                                            isnull(Sum(convert(decimal(18,2),总差次)),0) as 总差次,
                                            convert(decimal(18,2),
			                                isnull(Sum(convert(decimal(18,2),车均营收)),0)/" + count + @") as 车均营收,
			                                convert(decimal(18,2),
			                                isnull(Sum(convert(decimal(18,2),车均差次)),0)/" + count + @") as 车均差次,
			                                convert(decimal(18,2),
			                                isnull(Sum(convert(decimal(18,2),车均在线时长)),0)/" + count + @") as 车均在线时长
                                            from t_taxi_summary where 日期='" + date + "' and 车队 in (" + fleet + ") and 公司='" + orgName + "'");
                    //前一天数据
                    dataList2 = _dbMsSql.SqlQueryJson(@"select isnull(Sum(convert(decimal(18,2),上线司机数)),0) as 上线司机数,
                                            isnull(Sum(convert(decimal(18,2),上线车辆数)),0) as 上线车辆数, 
                                            isnull(Sum(convert(decimal(18,2),总差次)),0) as 总差次,
                                            convert(decimal(18,2),
			                                isnull(Sum(convert(decimal(18,2),车均营收)),0)/" + count + @") as 车均营收,
			                                convert(decimal(18,2),
			                                isnull(Sum(convert(decimal(18,2),车均差次)),0)/" + count + @") as 车均差次,
			                                convert(decimal(18,2),
			                                isnull(Sum(convert(decimal(18,2),车均在线时长)),0)/" + count + @") as 车均在线时长
                                            from t_taxi_summary where 日期='" + date2 + "' and 车队 in (" + fleet + ") and 公司='" + orgName + "'");
                    LogHelper.WriteLog(string.Format("级别：{0},查询日期:{1},上线司机总数：{2},查询日期最新数据：{3}", cm.DepartmenManager, date, count, dataList));
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
                    List<string> strValue = new List<string>() { "上线司机数", "上线车辆数", "总差次", "车均营收", "车均差次", "车均在线时长" };
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
                #endregion
            }
            return Json(jObject.ObjToString(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVehicleMaintenanceInfo(string fleet, string code)
        {
            //保养
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var orgName = cm.Organization;
            var fleetAll = getSqlInValue(cm.MotorcadeName, code);
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
                if (cm.DepartmenManager == "12")
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
                              order by MotorcadeName asc,Date asc,Time asc,MaintenanceType asc", new { OrgName = orgName }).ToList();
                }
            }
            return Json(resultInfo, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCarViolationInfo(string fleet, string code)
        {
            //电子警察违章
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var orgName = cm.Organization;
            var fleetAll = getSqlInValue(cm.MotorcadeName, code);
            List<Electronic_police> resultInfo = new List<Electronic_police>();
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
                    resultInfo = _dbMsSql.SqlQuery<Electronic_police>(@"select  plate_no,peccancy_date,score,amercement,area,act from tb_electronic_police ep
                        left join [DZ_DW].[dbo].[Visionet_CabInfo_View] vcv on ep.plate_no=vcv.CabLicense
                        where vcv.Organization in (" + fleet + @") 
                        and ep.status_cd='0'
                        order by peccancy_date desc", new { OrgName = orgName }).ToList();
                }
                else
                {
                    resultInfo = _dbMsSql.SqlQuery<Electronic_police>(@"select  plate_no,peccancy_date,score,amercement,area,act from tb_electronic_police ep
                        left join [DZ_DW].[dbo].[Visionet_CabInfo_View] vcv on ep.plate_no=vcv.CabLicense
                        where vcv.Organization=@OrgName and vcv.Motorcade in (" + fleet + @") 
                        and ep.status_cd='0'
                        order by peccancy_date desc", new { OrgName = orgName }).ToList();
                }
            }
            return Json(resultInfo, JsonRequestBehavior.AllowGet);
        }
        public Personnel_Info getPersonnelInfo(Business_Personnel_Information personInfoModel)
        {
            Personnel_Info pi = new Personnel_Info();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                pi = _dbMsSql.SqlQuery<Personnel_Info>(@"select Name,IdCard,CabLicense,CabVMLicense,MotorcadeName,Organization from [DZ_DW].[dbo].[Visionet_DriverInfo_View] where IdCard=@IDNumber
                                        and status='1'"
                                        , new { IDNumber = personInfoModel.IDNumber }).ToList().FirstOrDefault();
            }
            return pi;
        }
        public void getPartnerInfo(Business_Personnel_Information personInfoModel)
        {
            Master_Organization organizationDetail = new Master_Organization();
            organizationDetail = _ol.GetOrganizationDetail(personInfoModel.OwnedFleet.ToString());
            var ownedCompany = personInfoModel.OwnedCompany;
            if (ownedCompany == "全部")
            {
                ownedCompany = getAllOwnedCompany(organizationDetail.Description, "11");
            }
            Personnel_Info Personnel = new Personnel_Info();
            Personnel.IdCard = personInfoModel.IDNumber;
            Personnel.OldMotorcadeName = personInfoModel.OwnedFleet;//公司
            Personnel.OldOrganization = ownedCompany;//车队
            Personnel.Organization = organizationDetail.Description;
            //Personnel.Organization = "第一服务中心";
            Personnel.MotorcadeName = ownedCompany;//车队
            var key = PubGet.GetUserKey + personInfoModel.Vguid;
            var csche = CacheManager<Personnel_Info>.GetInstance().Get(key);
            if (csche != null)
            {
                CacheManager<Personnel_Info>.GetInstance().Remove(key);
            }
            CacheManager<Personnel_Info>.GetInstance().Add(key, Personnel, 8 * 60 * 60 * 1000);
            var newfleet = Personnel.MotorcadeName;//现车队
            ViewBag.MotorcadeName = newfleet;
            //ViewBag.Date = getTaxiSummary();
            ViewBag.Validate = true;
            ViewBag.Code = personInfoModel.Vguid;
        }
        public void getManagerInfo(Business_Personnel_Information personInfoModel)
        {
            Master_Organization organizationDetail = new Master_Organization();
            organizationDetail = _ol.GetOrganizationDetail(personInfoModel.OwnedFleet.ToString());
            var ownedCompany = personInfoModel.OwnedCompany;//查公司
            var ownedCompanyRemark = getAllOwnedCompany(ownedCompany, "12");
            if (ownedCompany == "全部")
            {
                ownedCompany = getAllOwnedCompany("全部", "12");
                ownedCompanyRemark = getAllOwnedCompany("R", "12");
            }
            Personnel_Info Personnel = new Personnel_Info();
            Personnel.IdCard = personInfoModel.IDNumber;
            Personnel.OldMotorcadeName = personInfoModel.OwnedFleet;//公司
            Personnel.OldOrganization = ownedCompany;//当前分公司
            Personnel.Organization = organizationDetail.Description;
            //Personnel.Organization = "第一服务中心";
            Personnel.MotorcadeName = ownedCompany;//当前分公司
            Personnel.MotorcadeNameRemark = ownedCompanyRemark;//当前分公司备注
            Personnel.DepartmenManager = "12";
            var key = PubGet.GetUserKey + personInfoModel.Vguid;
            var csche = CacheManager<Personnel_Info>.GetInstance().Get(key);
            if (csche != null)
            {
                CacheManager<Personnel_Info>.GetInstance().Remove(key);
            }
            CacheManager<Personnel_Info>.GetInstance().Add(key, Personnel, 8 * 60 * 60 * 1000);
            var newfleet = Personnel.MotorcadeName;//现车队
            ViewBag.MotorcadeName = newfleet;
            //ViewBag.Date = getTaxiSummary();
            ViewBag.Validate = true;
            ViewBag.Code = personInfoModel.Vguid;
        }
        public static string getAllOwnedCompany(string description, string level)
        {
            var str = "";
            var fleetList = new List<string>();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                if (description != "" && level == "12" && description != "全部" && description != "R")
                {
                    //查所配置公司备注
                    var description2 = getSqlInValue(description, "");
                    fleetList = _dbMsSql.SqlQuery<string>(@"select distinct Remark from DZ_Organization where OrganizationName in (" + description2 + ") and status=0").ToList();
                }
                else if (description == "全部" && level == "12")
                {
                    //查全公司名称
                    fleetList = _dbMsSql.SqlQuery<string>(@"select distinct OrganizationName from DZ_Organization where status=0").ToList();
                }
                else if (description == "R" && level == "12")
                {
                    //查全公司备注
                    fleetList = _dbMsSql.SqlQuery<string>(@"select distinct Remark from DZ_Organization where status=0").ToList();
                }
                else if (description != "" && level == "11")
                {
                    //查全车队名称
                    fleetList = _dbMsSql.SqlQuery<string>(@"select CarTeamName from DZ_Organization where OrganizationName=@OrgName and status=0", new { OrgName = description }).ToList();
                }
                if (fleetList.Count > 0)
                {
                    foreach (var item in fleetList)
                    {
                        str += item + ",";
                    }
                    str = str.Substring(0, str.Length - 1);
                }
            }
            return str;
        }
        public static string getSqlInValue(string motorcadeName, string code)
        {
            //var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var value = "";
            var str = motorcadeName.Split(',');
            foreach (var item in str)
            {
                //循环公司或者车队,构造sql结构
                value += "'" + item + "',";
            }
            value = value.Substring(0, value.Length - 1);
            return value;
        }
        public Business_Personnel_Information GetUserInfo(string userID)
        {
            Business_Personnel_Information personModel = new Business_Personnel_Information();
            personModel = _wl.GetUserInfo(userID);
            return personModel;
        }
        public string GetTaxiInfo(string code)
        {
            var dataList = "";
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var cabVMLicense = cm.CabVMLicense;
            JObject jObject = new JObject();
            var value1_1 = "";
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                var date1 = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                //最新数据
                dataList = _dbMsSql.SqlQueryJson(@"select * from t_taximeter_data where 日期='" + date1 + " 00:00:00' and 车牌号='" + cabVMLicense + "'");
                if (dataList != null && dataList != "" && dataList.Count() > 2)
                {
                    Regex rgx = new Regex(@"(?i)(?<=\[)(.*)(?=\])");//中括号[]
                    string tmp1 = rgx.Match(dataList).Value;//中括号[]
                    JObject jo1 = (JObject)JsonConvert.DeserializeObject(tmp1);
                    jObject = jo1;
                    //计算上涨或者下跌率
                    List<string> strValue = new List<string>() { "营收", "差次", "线上营收", "线上差次" };
                    value1_1 = jo1[strValue[0]].TryToString();
                }
            }
            return value1_1;
        }
    }
}
