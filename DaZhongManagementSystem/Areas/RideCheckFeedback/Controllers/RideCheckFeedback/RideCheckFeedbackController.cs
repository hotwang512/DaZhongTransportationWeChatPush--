using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.RideCheckFeedback.BusinessLogic;
using DaZhongManagementSystem.Controllers;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using SyntacticSugar;

namespace DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.RideCheckFeedback
{
    public class RideCheckFeedbackController : Controller
    {
        RideCheckFeedbackLogic _logic;
        public RideCheckFeedbackController()
        {
            _logic = new RideCheckFeedbackLogic();
        }

        public ActionResult Index(string code)
        {
            bool isOpen = false;
            string isOpenType = "unuriver";
            int count = _logic.GetMonthCountConfig();
            string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
            U_WeChatUserID userInfo = new U_WeChatUserID();
            string userInfoStr = Common.WeChatPush.WeChatTools.GetUserInfoByCode(accessToken, code);
            userInfo = Common.JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr);//用户ID
            Business_Personnel_Information personInfoModel = GetUserInfo(userInfo.UserId);//获取人员表信息
            //Business_Personnel_Information personInfoModel = GetUserInfo("WangCunBiao");//获取人员表信息
            Business_RideCheckFeedback rideCheckFeedback = new Business_RideCheckFeedback();
            if (personInfoModel != null && personInfoModel.DepartmenManager != 1)
            {
                isOpenType = "";
                DateTime startDate = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date;
                int maxCount = _logic.GetRideCheckFeedbackCount(startDate, startDate.AddMonths(1), personInfoModel.Vguid.ToString());
                if (maxCount < count)
                {
                    rideCheckFeedback = _logic.GetUserNewRideCheckFeedback(personInfoModel.Vguid.ToString());
                    if (rideCheckFeedback == null)
                    {
                        rideCheckFeedback = _logic.AddBusiness_RideCheckFeedback(personInfoModel.Vguid.ToString());
                    }
                    isOpen = true;
                }
            }

            ViewBag.RideCheckFeedback = rideCheckFeedback;
            ViewBag.IsOpen = isOpen;
            ViewBag.IsOpenType = isOpenType;
            ViewBag.User = personInfoModel;
            return View();
        }

        public JsonResult Submit(string user, Guid vguid)
        {
            var val = _logic.Submit(user, vguid);
            return Json(new { Success = true, Data = val }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveRideCheckFeedbackItemInfor(string user, Guid vguid, int number, string answer1, string answer2, string answer3, string answer4, string answer5, string answer6, string answer7)
        {
            var val = _logic.SaveBusiness_RideCheckFeedbackItem(user, vguid, number, answer1, answer2, answer3, answer4, answer5, answer6, answer7);

            return Json(new { Success = true, Data = val }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取人员详细信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Business_Personnel_Information GetUserInfo(string userID)
        {
            Business_Personnel_Information personModel = new Business_Personnel_Information();
            personModel = _logic.GetUserInfo(userID);
            return personModel;
        }
        public string SaveBusiness_RideCheckFeedbackAttachment(string user, Guid vguid, string fileName, string filePath)
        {
            return _logic.SaveBusiness_RideCheckFeedbackAttachment(user, vguid, fileName, filePath);
        }
        public void DeleteBusiness_RideCheckFeedbackAttachment(string filePath)
        {
            _logic.DeleteBusiness_RideCheckFeedbackAttachment(filePath);
        }

        public JsonResult uploadFile(string user, Guid vguid, int number, string type)
        {
            UploadFile uf = new UploadFile();
            uf.SetFileType("*");
            string url = "/UpLoadFile/";//文件保存路径
            string saveFolder = Server.MapPath(url);
            uf.SetFileDirectory(saveFolder);
            HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];
            string fileName = file.FileName;
            var reponseMessage = uf.Save(file, "RideCheckFeedback");
            if (type == "invoice")
            {
                SaveRideCheckFeedbackItemInfor(user, vguid, 7, reponseMessage.WebFilePath, "", "", "", "", "", "");
            }
            else
            {
                _logic.SaveBusiness_RideCheckFeedbackAttachment(user, vguid, fileName, reponseMessage.WebFilePath);
            }
            return Json(new { Success = true, Data = new { FilePath = reponseMessage.WebFilePath, FileName = fileName } }, JsonRequestBehavior.AllowGet);
        }


    }
}