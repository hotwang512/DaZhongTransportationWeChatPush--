using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.OrganizationManagement.OrganizationManageLogic;
using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.WeChatExercise.BusinessLogic;
using DaZhongManagementSystem.Areas.PartnerInquiryManagement.Models;
using DaZhongManagementSystem.Areas.SecondaryCleaningManagement.Models;
using DaZhongManagementSystem.Common.LogHelper;
using DaZhongManagementSystem.Common.WeChatPush;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using DaZhongTransitionLiquidation.Common.Pub;
using JQWidgetsSugar;
using SqlSugar;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.SecondaryCleaningManagement.Controllers.CleaningTypePage
{
    public class CleaningTypePageController : Controller
    {
        public WeChatExerciseLogic _wl;
        public OrganizationManagementLogic _ol;
        public CleaningTypePageController()
        {
            _wl = new WeChatExerciseLogic();
            _ol = new OrganizationManagementLogic();
        }
        public ActionResult Index(string code)
        {
            string accessToken = WeChatTools.GetAccessoken();
            U_WeChatUserID userInfo = new U_WeChatUserID();
            string userInfoStr = WeChatTools.GetUserInfoByCode(accessToken, code);
            userInfo = Common.JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr);//用户ID
            //userInfo.UserId = "13671595340";//合伙人
            userInfo.UserId = "18936495119";//司机
            Business_Personnel_Information personInfoModel = GetUserInfo(userInfo.UserId);//获取人员表信息
            var key = PubGet.GetUserKey + personInfoModel.Vguid;
            var csche = CacheManager<Business_Personnel_Information>.GetInstance().Get(key);
            if (csche != null)
            {
                CacheManager<Business_Personnel_Information>.GetInstance().Remove(key);
            }
            CacheManager<Business_Personnel_Information>.GetInstance().Add(key, personInfoModel, 8 * 60 * 60 * 1000);
            ViewBag.UserVGUID = personInfoModel.Vguid;
            ViewBag.UserID = userInfo.UserId;
            ViewBag.PhoneNumber = personInfoModel.PhoneNumber;
            ViewBag.Key = "lZagKrU56xPBvyNRZjym7jrdJPwOT1Z0W+HpZaTrvUobpwSQEAue7j0iWs/b0cu2";
            return View();
        }
        public Business_Personnel_Information GetUserInfo(string userID)
        {
            Business_Personnel_Information personModel = new Business_Personnel_Information();
            personModel = _wl.GetUserInfo(userID);
            return personModel;
        }
        public JsonResult GetCompanyLocation(string vguid)
        {
            Business_CleaningCompany cleaning = new Business_CleaningCompany();
            using (SqlSugarClient _db = SugarDao_MsSql.GetInstance())
            {
                cleaning = _db.SqlQuery<Business_CleaningCompany>(@"select * from Business_CleaningCompany where Vguid=@VGUID",
                                new { VGUID = vguid }).ToList().FirstOrDefault();
                //cleaning = _db.Queryable<Business_CleaningCompany>().Where(x => x.Vguid == vguid).FirstOrDefault();
            }
            return Json(cleaning, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetIsClearing(string code)
        {
            var model = new ActionResultModel<string>();
            bool isCleaning = false;
            var cabLicense = "";
            var cm = CacheManager<Business_Personnel_Information>.GetInstance()[PubGet.GetUserKey + code];
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                //根据人员获取车辆信息
                var manData = _dbMsSql.SqlQuery<Personnel_Info>(@"select Organization,CabLicense from [DZ_DW].[dbo].[Visionet_DriverInfo_View] 
                                where IdCard=@ID and status='1'", new { ID = cm.IDNumber }).FirstOrDefault();
                if (manData != null)
                {
                    cabLicense = manData.CabLicense;
                }
            }
            using (SqlSugarClient _db = SugarDao_MsSql.GetInstance())
            {
                //一辆车一月一次免费二级清洗
                var newDate = DateTime.Now;
                var data = _db.Queryable<Business_SecondaryCleaning>().Where(x => x.CabLicense == cabLicense && x.Type == "1").ToList();
                foreach (var item in data)
                {
                    var year = item.OperationDate.Year;
                    var month = item.OperationDate.Month;
                    if (year == newDate.Year && month == newDate.Month)
                    {
                        isCleaning = true;
                        LogHelper.WriteLog(string.Format("当前车牌号：{0},当前年月:{1},当前Code：{2},是否清洗：{3},身份证：{4}", cabLicense, year + month, code, isCleaning, cm.IDNumber));
                    } 
                }
            }
            model.isSuccess = isCleaning;
            model.respnseInfo = cabLicense;
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsPointInCircle(string companyVGUID, double lat, double lon)
        {
            bool isIn = false;
            using (SqlSugarClient _db = SugarDao_MsSql.GetInstance())
            {
                var data = _db.SqlQuery<Business_CleaningCompany>(@"select * from Business_CleaningCompany where Vguid=@VGUID",
                                new { VGUID = companyVGUID }).ToList().FirstOrDefault();
                string[] sArray = data.TXLocation.Split(new char[1] { ',' });
                var circleLat = double.Parse(sArray[0]);
                var circleLon = double.Parse(sArray[1]);
                var radius = double.Parse(data.Radius.TryToString());
                double R = 6378137.0;//椭球的长半轴
                double dLat = (circleLat - lat) * Math.PI / 180;
                double dLng = (circleLon - lon) * Math.PI / 180;
                double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat * Math.PI / 180) * Math.Cos(circleLat * Math.PI / 180) * Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                double d = R * c;
                double dis = Math.Round(d);
                if (dis <= radius)
                {  //点在圆内
                    isIn = true;
                }
                else
                {
                    isIn = false;
                }
            }
            return Json(isIn, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveCleaningInfo(string companyVGUID, string location, string type, string description, string code)
        {
            var model = new ActionResultModel<string>();
            var cm = CacheManager<Business_Personnel_Information>.GetInstance()[PubGet.GetUserKey + code];
            var cabLicense = "";
            var cabOrgName = "";
            Master_Organization organizationDetail = new Master_Organization();
            organizationDetail = _ol.GetOrganizationDetail(cm.OwnedFleet.ToString());
            var manOrgName = organizationDetail.OrganizationName;
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                var manData = _dbMsSql.SqlQuery<Personnel_Info>(@"select Organization,CabLicense from [DZ_DW].[dbo].[Visionet_DriverInfo_View] 
                                where IdCard=@ID and status='1'", new { ID = cm.IDNumber }).FirstOrDefault();
                if(manData != null)
                {
                    cabLicense = manData.CabLicense;
                    cabOrgName = manData.Organization;
                }
            }
            using (SqlSugarClient _db = SugarDao_MsSql.GetInstance())
            {
                Business_SecondaryCleaning cleaning = new Business_SecondaryCleaning();
                cleaning.Vguid = Guid.NewGuid();
                cleaning.Type = type;
                cleaning.Description = description;
                cleaning.Location = location;
                cleaning.Personnel = cm.UserID;
                cleaning.OperationDate = DateTime.Now;
                cleaning.CompanyVguid = companyVGUID;
                cleaning.CouponType = description;
                cleaning.CabLicense = cabLicense;
                cleaning.CabOrgName = cabOrgName;
                cleaning.ManOrgName = manOrgName;
                cleaning.CreatedUser = cm.Name;
                cleaning.CreatedDate = DateTime.Now;
                cleaning.ChangeDate = DateTime.Now;
                cleaning.ChangeUser = cm.Name;
                model.isSuccess = _db.Insert(cleaning, false) != DBNull.Value;
                model.respnseInfo = model.isSuccess == true ? cleaning.CreatedDate.TryToString() : "0";
                //同时修改权益表中优惠券状态
                UpdateMyRights(_db, description, cm.Vguid);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private void UpdateMyRights(SqlSugarClient _db, string description,Guid VGUID)
        {
            //根据描述和日期找对应优惠券
            var date = DateTime.Now;
            var guid = VGUID.ToString();
            _db.Update<Business_MyRights>(new { Status = "已使用" }, i => i.Type.Contains(description) && i.StartValidity<= date && i.EndValidity>= date 
                                                                    && i.UserVGUID == guid);
        }

        public JsonResult GetWXAPPIDInfo(string url)
        {
            string nonceStr = WxPayApi.GenerateNonceStr();  //随机字符串，不长于32位
            string timeStamp = WxPayApi.GenerateTimeStamp();
            string accessToken = WeChatTools.GetAccessoken(true);
            string jsapi_ticket1 = WeChatTools.PostWebRequest("https://qyapi.weixin.qq.com/cgi-bin/ticket/get?access_token="+ accessToken + "&type=agent_config", "", Encoding.UTF8);
            string jsapi_ticket2 = WeChatTools.PostWebRequest("https://qyapi.weixin.qq.com/cgi-bin/get_jsapi_ticket?access_token=" + accessToken, "", Encoding.UTF8);
            Dictionary<string, string> tickResult = jsapi_ticket1.JsonToModel<Dictionary<string,string>>();
            Dictionary<string, string> tickResult2 = jsapi_ticket2.JsonToModel<Dictionary<string, string>>();
            string param1 = "jsapi_ticket="+ tickResult["ticket"] + "&noncestr="+ nonceStr + "&timestamp="+ timeStamp + "&url="+ url + "";
            string param2 = "jsapi_ticket=" + tickResult2["ticket"] + "&noncestr=" + nonceStr + "&timestamp=" + timeStamp + "&url=" + url + "";
            string newjsapi_ticket1 = SHA1_Encrypt(param1);
            string newjsapi_ticket2 = SHA1_Encrypt(param2);
            //wx.agentConfig
            WxPayData wxPayData = new WxPayData();
            WxPayData wxPayData2 = new WxPayData();
            wxPayData.SetValue("appid", WxPayConfig.APPID);        //appid
            wxPayData.SetValue("nonce_str", nonceStr);             //随机字符串
            wxPayData.SetValue("timestamp", timeStamp);           //生成签名的时间戳
            wxPayData.SetValue("sign", newjsapi_ticket1.ToLower());          //签名1
            //wx.config
            wxPayData.SetValue("sign2", newjsapi_ticket2.ToLower());          //签名2
            wxPayData.SetValue("url", url);
            return Json(wxPayData.GetValues(), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 对字符串进行SHA1加密
        /// </summary>
        /// <param name="strIN">需要加密的字符串</param>
        /// <returns>密文</returns>
        public static string SHA1_Encrypt(string Source_String)
        {
            byte[] StrRes = Encoding.Default.GetBytes(Source_String);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString().ToUpper();
        }
    }
}
