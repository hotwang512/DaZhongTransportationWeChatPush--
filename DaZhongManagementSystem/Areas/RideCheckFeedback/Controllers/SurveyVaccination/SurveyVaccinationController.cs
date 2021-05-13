using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.OrganizationManagement.OrganizationManageLogic;
using DaZhongManagementSystem.Areas.PartnerInquiryManagement.Models;
using DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.RideCheckFeedback.BusinessLogic;
using DaZhongManagementSystem.Common.LogHelper;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using JQWidgetsSugar;
using SqlSugar;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.SurveyVaccination
{
    public class SurveyVaccinationController : Controller
    {
        private RideCheckFeedbackLogic _logic;
        public OrganizationManagementLogic _ol;
        public SurveyVaccinationController()
        {
            _logic = new RideCheckFeedbackLogic();
            _ol = new OrganizationManagementLogic();
        }
        public ActionResult Index(string code)
        {
            U_WeChatUserID userInfo = new U_WeChatUserID();
            string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
            string userInfoStr = Common.WeChatPush.WeChatTools.GetUserInfoByCode(accessToken, code);
            userInfo = Common.JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr);//用户ID
            userInfo.UserId = "18936495119";
            Business_Personnel_Information personInfoModel = _logic.GetUserInfo(userInfo.UserId);
            Personnel_Info Personnel = getPersonnelInfo(personInfoModel);
            Master_Organization organizationDetail = new Master_Organization();
            organizationDetail = _ol.GetOrganizationDetail(personInfoModel.OwnedFleet.ToString());

            Business_SurveyVaccination bsv = GetSurveyVaccination(userInfo.UserId);
            if (bsv == null)
            {
                bsv = new Business_SurveyVaccination();
                bsv.Name = personInfoModel != null ? personInfoModel.Name : "";
                bsv.UserID = userInfo.UserId;
            }
            //if (Personnel != null)
            //{
            //    bhs.Fleet = Personnel.MotorcadeName;
            //    bhs.LicensePlate = Personnel.CabLicense;
            //}
            return View(bsv);
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
        public Business_SurveyVaccination GetSurveyVaccination(string userID)
        {
            Business_SurveyVaccination bsv = null;
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bsv = _dbMsSql.Queryable<Business_SurveyVaccination>().Where(c => c.UserID == userID).FirstOrDefault();
            }
            return bsv;
        }
        public JsonResult SaveSurveyVaccination(Business_SurveyVaccination bsv)
        {
            string result = "0";
            try
            {
                var newUrlName = "";
                var FileData = System.Web.HttpContext.Current.Request.Files;
                if (FileData.Count > 0)
                {
                    newUrlName = SaveImageFile(FileData, bsv.UserID,bsv.Name);
                }
                using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
                {
                    var isAny = _dbMsSql.Queryable<Business_SurveyVaccination>().Any(x => x.UserID == bsv.UserID);
                    if (!isAny)
                    {
                        bsv.VGUID = Guid.NewGuid();
                        bsv.CreatedTime = DateTime.Now;
                        bsv.CreatedUser = bsv.Name;
                        bsv.ChangeUser = bsv.Name;
                        bsv.ChangeTime = DateTime.Now;
                        bsv.Attachment = newUrlName;
                        _dbMsSql.Insert(bsv);
                    }
                    else
                    {
                        var index = 0;
                        if(bsv.Attachment != "" && bsv.Attachment != null)
                        {
                            index = Regex.Matches(bsv.Attachment, ";").Count;
                        }
                        if(index == 1 && newUrlName == "")
                        {
                            newUrlName = bsv.Attachment;
                        }
                        _dbMsSql.Update<Business_SurveyVaccination>(
                        new
                        {
                            IsInoculation = bsv.IsInoculation,
                            FirstDate = bsv.FirstDate,
                            FirstAddress = bsv.FirstAddress,
                            SecondDate = bsv.SecondDate,
                            SecondAddress = bsv.SecondAddress,
                            Attachment = newUrlName,
                            UserID = bsv.UserID,
                            Name = bsv.Name,
                            ChangeUser = bsv.Name,
                            ChangeTime = DateTime.Now
                    },
                        c => c.UserID == bsv.UserID);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.ToString());
                result = "1";
            }
            return Json(result);
        }

        private string SaveImageFile(HttpFileCollection fileData,string userID,string name)
        {
            try
            {
                //保存上传文件
                var urlName = "";
                string url = "/UpLoadFile/SurveyVaccination/";//文件保存路径
                HttpPostedFile file = fileData[0];
                //string newName = userID.Trim() + name.Trim() + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                string newName = name.Trim() + userID.Trim() + ".jpg";
                string path = Path.Combine(Server.MapPath(url), newName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                file.SaveAs(path);
                urlName = url + newName;
                return urlName;            
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(string.Format("上传图片数量:{0},失败result:{1}", fileData.Count, ex.ToString()));
                return "";
            }
        }
    }
}
