using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.HomecomingSurvey.BusinessLogic;
using DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.RideCheckFeedback.BusinessLogic;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Areas.PartnerInquiryManagement.Models;
using SqlSugar;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.OrganizationManagement.OrganizationManageLogic;

namespace DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.HomecomingSurvey
{
    public class HomecomingSurveyController : Controller
    {
        private HomecomingSurveyLogic _hsl;
        private RideCheckFeedbackLogic _logic;
        public OrganizationManagementLogic _ol;
        public HomecomingSurveyController()
        {
            _hsl = new HomecomingSurveyLogic();
            _logic = new RideCheckFeedbackLogic();
            _ol = new OrganizationManagementLogic();
        }

        public ActionResult Index(string code)
        {
            U_WeChatUserID userInfo = new U_WeChatUserID();
            string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
            string userInfoStr = Common.WeChatPush.WeChatTools.GetUserInfoByCode(accessToken, code);
            userInfo = Common.JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr);//用户ID
            //userInfo.UserId = "18936495119";
            Business_Personnel_Information personInfoModel = _logic.GetUserInfo(userInfo.UserId);
            Personnel_Info Personnel = getPersonnelInfo(personInfoModel);
            Master_Organization organizationDetail = new Master_Organization();
            organizationDetail = _ol.GetOrganizationDetail(personInfoModel.OwnedFleet.ToString());
            Business_HomecomingSurvey bhs = _hsl.GetHomecomingSurvey(userInfo.UserId, DateTime.Now.Year.ToString());
            if (bhs == null)
            {
                bhs = new Business_HomecomingSurvey();
                bhs.Name = personInfoModel != null ? personInfoModel.Name : "";
                bhs.Year = DateTime.Now.Year.ToString();
                bhs.CreatedUser = userInfo.UserId;
            }
            bhs.OrganizationName = organizationDetail.OrganizationName;
            if(Personnel != null)
            {
                bhs.Fleet = Personnel.MotorcadeName;
                bhs.LicensePlate = Personnel.CabLicense;
            }
            return View(bhs);
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
        public ActionResult SaveHomecomingSurvey(Business_HomecomingSurvey bhs)
        {
            string result = "0";
            try
            {
                if (bhs.Vguid == Guid.Empty)
                {
                    _hsl.AddHomecomingSurvey(bhs);
                }
                else
                {
                    _hsl.UpdateHomecomingSurvey(bhs);
                }

            }
            catch (Exception ex)
            {
                Common.LogHelper.LogHelper.WriteLog(ex.ToString());
                result = "1";
            }
            return Content(result);
        }

        public ActionResult ReturnHomeStatistics()
        {
            return View();
        }

        public ActionResult ReturnHomeStatisticsSource(string year, string dept)
        {
            JsonResultModel<ReturnHomeStatistics> result = new JsonResultModel<ReturnHomeStatistics>();
            result.Rows = _hsl.ReturnHomeStatistics(year, dept);
            result.TotalRows = result.Rows.Count();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void ReturnHomeStatisticsExport(string year, string dept)
        {
            _hsl.ReturnHomeStatisticsExport(year, dept);
        }
    }
}