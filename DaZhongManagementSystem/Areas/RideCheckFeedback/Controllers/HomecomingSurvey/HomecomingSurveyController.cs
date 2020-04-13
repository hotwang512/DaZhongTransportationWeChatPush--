using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.HomecomingSurvey.BusinessLogic;
using DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.RideCheckFeedback.BusinessLogic;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;

namespace DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.HomecomingSurvey
{
    public class HomecomingSurveyController : Controller
    {
        private HomecomingSurveyLogic _hsl;
        private RideCheckFeedbackLogic _logic;
        public HomecomingSurveyController()
        {
            _hsl = new HomecomingSurveyLogic();
            _logic = new RideCheckFeedbackLogic();
        }

        public ActionResult Index(string code)
        {
            U_WeChatUserID userInfo = new U_WeChatUserID();
            //string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
            //string userInfoStr = Common.WeChatPush.WeChatTools.GetUserInfoByCode(accessToken, code);
            //userInfo = Common.JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr);//用户ID
            userInfo.UserId = "18555557780";
            Business_Personnel_Information personInfoModel = _logic.GetUserInfo(userInfo.UserId);
            Business_HomecomingSurvey bhs = _hsl.GetHomecomingSurvey(userInfo.UserId, DateTime.Now.Year.ToString());
            if (bhs == null)
            {
                bhs = new Business_HomecomingSurvey();
                bhs.Name = personInfoModel != null ? personInfoModel.Name : "";
                bhs.Year = DateTime.Now.Year.ToString();
                bhs.CreatedUser = userInfo.UserId;
            }
            return View(bhs);
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