using DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.RideCheckFeedback.BusinessLogic;
using DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.VehicleRepairComplaints.BusinessLogic;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.VehicleRepairComplaints
{
    public class VehicleRepairComplaintsController : Controller
    {
        private Business_VehicleRepairComplaintsLogic _bvrc;
        private RideCheckFeedbackLogic _logic;
        public VehicleRepairComplaintsController()
        {
            _bvrc = new Business_VehicleRepairComplaintsLogic();
            _logic = new RideCheckFeedbackLogic();
        }
        public ActionResult Index(string code)
        {
            U_WeChatUserID userInfo = new U_WeChatUserID();
            string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
            //string userInfoStr = Common.WeChatPush.WeChatTools.GetUserInfoByCode(accessToken, code);
            //userInfo = Common.JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr);//用户ID
            userInfo.UserId = "13524338060";
            DateTime currentDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            Business_Personnel_Information personInfoModel = _logic.GetUserInfo(userInfo.UserId);
            //Business_VehicleRepairComplaints vrc = _bvrc.GetVehicleRepairComplaint(userInfo.UserId, currentDate);
            Business_VehicleRepairComplaints vrc = null;
            if (vrc == null)
            {
                vrc = new Business_VehicleRepairComplaints();
                vrc.ReflectJobNumber = personInfoModel.JobNumber;
                //vrc.CarNumber = personInfoModel.LicensePlate;
                vrc.ContactNumber = personInfoModel.PhoneNumber;
                vrc.ReflectName = personInfoModel != null ? personInfoModel.Name : "";
                vrc.ReflectDate = currentDate;
                vrc.CreateUser = userInfo.UserId;
            }
            return View(vrc);
        }

        public ActionResult SaveVehicleRepairComplaints(Business_VehicleRepairComplaints vrc)
        {
            string result = "0";
            try
            {
                _bvrc.UpdateBusiness_VehicleRepairComplaints(vrc);

            }
            catch (Exception ex)
            {
                Common.LogHelper.LogHelper.WriteLog(ex.ToString());
                result = "1";
            }
            return Content(result);
        }

        public JsonResult uploadFile()
        {
            UploadFile uf = new UploadFile();
            uf.SetFileType("*");
            string url = "/UpLoadFile/";//文件保存路径
            string saveFolder = Server.MapPath(url);
            uf.SetFileDirectory(saveFolder);
            HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];
            string fileName = file.FileName;
            var reponseMessage = uf.Save(file, "VehicleRepairComplaints");
            return Json(new { Success = true, Data = new { FilePath = reponseMessage.WebFilePath, FileName = fileName } }, JsonRequestBehavior.AllowGet);
        }

    }
}