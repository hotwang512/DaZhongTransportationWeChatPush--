using DaZhongManagementSystem.Areas.SecondaryCleaningManagement.Models;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.UserManagement.BussinessLogic;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Controllers;
using DaZhongManagementSystem.Entities.TableEntity;
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
using System.Web.Http;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.SecondaryCleaningManagement.Controllers.CleaningCompanyDetail
{
    public class CleaningCompanyDetailController : BaseController
    {
        public UserManageLogic _ul;
        public AuthorityManageLogic _al;
        public CleaningCompanyDetailController()
        {
            _ul = new UserManageLogic();
            _al = new AuthorityManageLogic();
        }
        public ActionResult Index()
        {
            bool isEdit = bool.Parse(Request.QueryString["isEdit"]);
            Business_CleaningCompany cleaningCompany = new Business_CleaningCompany();
            if (isEdit)
            {
                string vguid = Request.QueryString["Vguid"];
                cleaningCompany = GetCleaningCompanyByVguid(vguid);
                // departmentList = GetDepartmentListByEdit(sysRoleModel.Company.ToString());
            }
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.UserSystemModule);

            ViewBag.CurrentModulePermission = roleModuleModel;
            ViewBag.UserInfo = cleaningCompany;
            ViewBag.isEdit = isEdit;
            return View();
        }
        public JsonResult CreateQRCode(Business_CleaningCompany cleaning)
        {
            string url = getWeChatQRCode(cleaning);
            return Json(url, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]//敏感值验证
        public JsonResult SaveCleaningCompany(Business_CleaningCompany cleaning, bool isEdit)
        {
            var model = new ActionResultModel<string>();
            model.isSuccess = false;
            using (SqlSugarClient _db = SugarDao_MsSql.GetInstance())
            {
                bool isExistCompanyName = _db.Queryable<Business_CleaningCompany>().Any(x=>x.CompanyName == cleaning.CompanyName);
                if (!isEdit && isExistCompanyName)
                {
                    model.isSuccess = false;
                    model.respnseInfo = "2";
                }
                else
                {
                    //保存时生成最新的二维码
                    if(cleaning.Vguid == Guid.Empty || cleaning.Vguid == null)
                    {
                        cleaning.Vguid = Guid.NewGuid();
                    }
                    cleaning.QRCode = getWeChatQRCode(cleaning);
                    if (isEdit)
                    {
                        cleaning.ChangeDate = DateTime.Now;
                        cleaning.ChangeUser = CurrentUser.GetCurrentUser().LoginName;
                        var data = new
                        {
                            CompanyName = cleaning.CompanyName,
                            Address = cleaning.Address,
                            Location = cleaning.Location,
                            TXLocation = cleaning.TXLocation,
                            ContactNumber = cleaning.ContactNumber,
                            ContactPerson = cleaning.ContactPerson,
                            QRCode = cleaning.QRCode,
                            Radius = cleaning.Radius,
                            ChangeUser = cleaning.ChangeUser,
                            ChangeDate = cleaning.ChangeDate
                        };
                        model.isSuccess = _db.Update<Business_CleaningCompany>(data, i => i.Vguid == cleaning.Vguid);
                    }
                    else
                    {
                        cleaning.CreatedUser = CurrentUser.GetCurrentUser().LoginName;
                        cleaning.CreatedDate = DateTime.Now;
                        cleaning.ChangeDate = DateTime.Now;
                        cleaning.ChangeUser = CurrentUser.GetCurrentUser().LoginName;
                        model.isSuccess = _db.Insert(cleaning, false) != DBNull.Value;
                    }
                    model.isSuccess = true;
                    model.respnseInfo = model.isSuccess == true ? cleaning.QRCode + "," + cleaning.Vguid : "0";
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public Business_CleaningCompany GetCleaningCompanyByVguid(string vguid)
        {
            Business_CleaningCompany cleaningCompany = new Business_CleaningCompany();
            using (SqlSugarClient _db = SugarDao_MsSql.GetInstance())
            {
                cleaningCompany = _db.SqlQuery<Business_CleaningCompany>(@"select * from Business_CleaningCompany where Vguid=@VGUID",
                                new { VGUID = vguid }).ToList().FirstOrDefault();
            }
            return cleaningCompany;
        }
        public string getWeChatQRCode(Business_CleaningCompany cleaning)
        {
            string file = cleaning.CompanyName + ".jpg";
            string forder = "UploadFile/WeChatQRCode";
            string filePath = Path.Combine(forder, file);
            string fileName = Server.MapPath(filePath);
            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }
            string guid = cleaning.Vguid.ToString();
            string url = ConfigSugar.GetAppString("CleaningAddress");
            string appid = ConfigSugar.GetAppString("CorpID");
            url = url + "?VGUID=" + guid;
            var configStr = "https://open.weixin.qq.com/connect/oauth2/authorize?appid="+ appid + "&redirect_uri="+ url + "&response_type=code&scope=snsapi_base&state=STATE#wechat_redirect";
            QRCodeHelper.GenerateQRCode(configStr, "/Areas/WeChatPush/Views/_img/logo1.png", forder, file);
            return filePath;
        }
    }
}
