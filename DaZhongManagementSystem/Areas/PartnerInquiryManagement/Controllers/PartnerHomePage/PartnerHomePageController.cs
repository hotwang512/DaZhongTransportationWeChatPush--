using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.OrganizationManagement.OrganizationManageLogic;
using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.WeChatExercise.BusinessLogic;
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
            userInfo.UserId = "13671595340";//合伙人
            //userInfo.UserId = "15921961501";//司机
            Business_Personnel_Information personInfoModel = GetUserInfo(userInfo.UserId);//获取人员表信息
            if (personInfoModel.DepartmenManager == 10 || personInfoModel.DepartmenManager == 11)
            {
                //合伙人;高级合伙人
                Master_Organization organizationDetail = new Master_Organization();
                organizationDetail = _ol.GetOrganizationDetail(personInfoModel.OwnedFleet.ToString());
                Personnel_Info Personnel = new Personnel_Info();
                Personnel.IdCard = personInfoModel.IDNumber;
                Personnel.OldMotorcadeName = personInfoModel.OwnedFleet;//公司
                Personnel.OldOrganization = personInfoModel.OwnedCompany;//车队
                Personnel.Organization = organizationDetail.Description;
                Personnel.Organization = "第一服务中心";
                Personnel.MotorcadeName = personInfoModel.OwnedCompany;//车队
                var key = PubGet.GetUserKey + personInfoModel.Vguid;
                var csche = CacheManager<Personnel_Info>.GetInstance().Get(key);
                if(csche != null)
                {
                    CacheManager<Personnel_Info>.GetInstance().Remove(key);
                }
                CacheManager<Personnel_Info>.GetInstance().Add(key, Personnel, 8 * 60 * 60 * 1000);
                var newfleet = Personnel.MotorcadeName;//现车队
                ViewBag.MotorcadeName = newfleet;
                ViewBag.Date = getTaxiSummary();
                ViewBag.Validate = true;
                ViewBag.Code = personInfoModel.Vguid;
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
                    string rate = "";
                    List<string> strValue = new List<string>() { "营收", "差次", "线上营收", "线上差次" };
                    value1_1 = jo1[strValue[0]].TryToString();
                }
            }
            return value1_1;
        }
        public string getTaxiSummary()
        {
            var date = "";
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                date = _dbMsSql.SqlQuery<string>(@"select top(1) 日期 from t_taxi_summary order by 日期 desc").ToList().FirstOrDefault();
            }
            return date;
        }
        public JsonResult GetTaxiSummaryInfo(string fleet, string code)
        {
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var orgName = cm.Organization;
            var fleetAll = getSqlInValue(cm.MotorcadeName);
            var dataList = "";
            var dataList2 = "";
            //var orgName = "第一服务中心";
            //fleet = "仁强";
            JObject jObject = new JObject();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                #region 查询车辆信息
                var date = _dbMsSql.SqlQuery<string>(@"select top(1) 日期 from t_taxi_summary order by 日期 desc").ToList().FirstOrDefault();
                dataList = _dbMsSql.SqlQueryJson(@"select * from t_taxi_summary where 日期=@Date",
                                new { Date = date });
                var date1 = date.TryToDate();
                var date2 = date1.AddDays(-1).ToString("yyyy-MM-dd");
                if (fleet != "0")
                {
                    //最新数据
                    dataList = _dbMsSql.SqlQueryJson(@"select * from t_taxi_summary where 日期='" + date + "' and 车队='" + fleet + "' and 公司='" + orgName + "'");
                    //前一天数据
                    dataList2 = _dbMsSql.SqlQueryJson(@"select * from t_taxi_summary where 日期='" + date2 + "' and 车队='" + fleet + "'and 公司='" + orgName + "'");
                }
                else
                {
                    //最新数据
                    dataList = _dbMsSql.SqlQueryJson(@"select isnull(Sum(convert(decimal(18,2),上线司机数)),0) as 上线司机数,
                                            isnull(Sum(convert(decimal(18,2),上线车辆数)),0) as 上线车辆数, 
                                            isnull(Sum(convert(decimal(18,2),总差次)),0) as 总差次,
                                            isnull(Sum(convert(decimal(18,2),车均营收)),0) as 车均营收, 
                                            isnull(Sum(convert(decimal(18,2),车均差次)),0) as 车均差次,
                                            isnull(Sum(convert(decimal(18,2),车均在线时长)),0) as 车均在线时长 
                                            from t_taxi_summary where 日期='" + date + "' and 车队 in (" + fleetAll + ") and 公司='" + orgName + "'");
                    //前一天数据
                    dataList2 = _dbMsSql.SqlQueryJson(@"select isnull(Sum(convert(decimal(18,2),上线司机数)),0) as 上线司机数,
                                            isnull(Sum(convert(decimal(18,2),上线车辆数)),0) as 上线车辆数, 
                                            isnull(Sum(convert(decimal(18,2),总差次)),0) as 总差次,
                                            isnull(Sum(convert(decimal(18,2),车均营收)),0) as 车均营收, 
                                            isnull(Sum(convert(decimal(18,2),车均差次)),0) as 车均差次,
                                            isnull(Sum(convert(decimal(18,2),车均在线时长)),0) as 车均在线时长 
                                            from t_taxi_summary where 日期='" + date2 + "' and 车队 in (" + fleetAll + ") and 公司='" + orgName + "'");
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
                        if (value1_1 > value2_1)
                        {
                            rate = "↑" + (decimal.Round((value1_1 - value2_1) / value2_1 * 100, 2)) + "%";

                        }
                        else if (value1_1 < value2_1)
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
            var fleetAll = getSqlInValue(cm.MotorcadeName);
            List<VehicleMaintenanceInfo> resultInfo = new List<VehicleMaintenanceInfo>();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                //orgName = "第一服务中心";
                //fleet = "仁强";
                if(fleet == "0")
                {
                    resultInfo = _dbMsSql.SqlQuery<VehicleMaintenanceInfo>(@"SELECT top(2) MotorcadeName,Name,CabLicense,MaintenanceType,Date,Time,Address,Yanche,vm.Status,MobilePhone
                              FROM VehicleMaintenanceInfo vm
                              left join [DZ_DW].[dbo].[Visionet_DriverInfo_View] vdv on vm.carNo=vdv.CabVMLicense
                              where vdv.Organization=@OrgName  and vdv.MotorcadeName in ("+ fleetAll + @")  
                              and vm.Status='0'
                              order by MotorcadeName asc,Date asc,Time asc,MaintenanceType asc", new { OrgName = orgName }).ToList();
                }
                else
                {
                    resultInfo = _dbMsSql.SqlQuery<VehicleMaintenanceInfo>(@"SELECT top(2) MotorcadeName,Name,CabLicense,MaintenanceType,Date,Time,Address,Yanche,vm.Status,MobilePhone
                              FROM VehicleMaintenanceInfo vm
                              left join [DZ_DW].[dbo].[Visionet_DriverInfo_View] vdv on vm.carNo=vdv.CabVMLicense
                              where vdv.Organization=@OrgName  and vdv.MotorcadeName=@Fleet  
                              and vm.Status='0'
                              order by MotorcadeName asc,Date asc,Time asc,MaintenanceType asc", new { OrgName = orgName, Fleet = fleet }).ToList();
                }
            }
            return Json(resultInfo, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCarViolationInfo(string fleet, string code)
        {
            //电子警察违章
            var cm = CacheManager<Personnel_Info>.GetInstance()[PubGet.GetUserKey + code];
            var orgName = cm.Organization;
            var fleetAll = getSqlInValue(cm.MotorcadeName);
            List<Electronic_police> resultInfo = new List<Electronic_police>();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                //var orgName = "第一服务中心";
                if (fleet == "0")
                {
                    resultInfo = _dbMsSql.SqlQuery<Electronic_police>(@"select top(2) plate_no,peccancy_date,score,amercement,area,act from tb_electronic_police ep
                        left join [DZ_DW].[dbo].[Visionet_CabInfo_View] vcv on ep.plate_no=vcv.CabLicense
                        where vcv.Organization=@OrgName and vcv.Motorcade in (" + fleetAll + @") 
                        and ep.status_cd='0'
                        order by peccancy_date desc", new { OrgName = orgName}).ToList();
                }
                else
                {
                    resultInfo = _dbMsSql.SqlQuery<Electronic_police>(@"select top(2) plate_no,peccancy_date,score,amercement,area,act from tb_electronic_police ep
                        left join [DZ_DW].[dbo].[Visionet_CabInfo_View] vcv on ep.plate_no=vcv.CabLicense
                        where vcv.Organization=@OrgName and vcv.Motorcade=@Fleet 
                        and ep.status_cd='0'
                        order by peccancy_date desc", new { OrgName = orgName, Fleet = fleet }).ToList();
                }  
            }
            return Json(resultInfo, JsonRequestBehavior.AllowGet);
        }
        public static string getSqlInValue(string motorcadeName)
        {
            var value = "";
            var str = motorcadeName.Split(',');
            foreach (var item in str)
            {
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
    }
}
