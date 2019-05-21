using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aspose.Pdf;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.TableEntity;
using JQWidgetsSugar;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Controllers;
using SyntacticSugar;
using DaZhongManagementSystem.Common.Tools;
using Newtonsoft.Json;
using System.Data;
using System.Web;
using System.Web.SessionState;

namespace DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.UserInfo
{
    public class UserInfoController : BaseController
    {
        //
        // GET: /BasicDataManagement/UserInfo/

        public BussinessLogic.UserInfoLogic _ul;
        public AuthorityManageLogic _al;
        public UserInfoController()
        {
            _ul = new BussinessLogic.UserInfoLogic();
            _al = new AuthorityManageLogic();
        }

        public ActionResult UserInfo()
        {
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.PersonModule);

            GetDepartment();
            ViewBag.CurrentModulePermission = roleModuleModel;
            //绑定状态清单
            List<CS_Master_2> itemStatus = _ul.ApprovalStatusSelect();
            List<SelectListItem> StatusSelect = new List<SelectListItem>();
            StatusSelect.Add(new SelectListItem() { Value = "", Text = "不限" });
            foreach (var list in itemStatus)
            {
                StatusSelect.Add(new SelectListItem() { Value = list.DESC0, Text = list.DESC0 });
            }
            ViewData["StatusSelect"] = StatusSelect;
            ViewData["CurrentUser"] = CurrentUser.GetCurrentUser().LoginName;
            return View();
        }

        /// <summary>
        /// 获取用户详细信息（用于更改用户所在部门）
        /// </summary>
        /// <param name="personVguid"></param>
        /// <returns></returns>
        public JsonResult GetUserDepartment(string personVguid)
        {
            var model = _ul.GetUserDepartment(personVguid);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 部门树形结构
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOrganizationTreeList()
        {
            var model = _ul.GetOrganizationTreeList();
            //string json = DaZhongManagementSystem.Common.JsonHelper.ModelToJson<List<V_Business_Personnel_Information>>(model);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDepartment()
        {
            var model = _ul.GetDepartmentList();
            //string departmentList = DaZhongManagementSystem.Common.JsonHelper.ModelToJson<List<Master_Organization>>(model);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 身份审核
        /// </summary>
        /// <returns></returns>
        public JsonResult CheckIdentity(Business_Personnel_Information personModel)
        {
            var model = new ActionResultModel<string>();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分页查询用户列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResult GetUserListBySearch(Business_PersonDepartmrnt_Search searchParam, GridParams para)
        {
            if (para.sortdatafield == null)
            {
                para.sortdatafield = "UserID";
                para.sortorder = "desc";
            }
            para.pagenum = para.pagenum + 1;//页0，+1

            //List<DeparTment_1> departmenteList = _dl.GetDepartmentList(searchParam, para);
            var model = _ul.GetUserPageList(searchParam, para);
            var result = new ConfigurableJsonResult { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            // return Json(model, JsonRequestBehavior.AllowGet);
            return result;
        }

        /// <summary>
        /// 更新用户部门
        /// </summary>
        /// <param name="vguid">部门Vguid</param>
        /// <returns></returns>
        public JsonResult UpdateDepartment(string vguid, string personVguid, string labelStr)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = _ul.UpdateDepartment(vguid, personVguid, labelStr);
            models.respnseInfo = models.isSuccess == true ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批量删除用户信息
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public JsonResult DeleteUserInfo(string[] vguidList)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = false;

            models.isSuccess = _ul.DeleteUserInfo(vguidList);
            models.respnseInfo = models.isSuccess == true ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批量手动关注企业号
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public JsonResult UserFocusWeChat(string[] vguidList)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = false;
            string response = string.Empty;
            models.isSuccess = _ul.UserFocusWeChat(vguidList, out response);
            models.respnseInfo = response;
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取用户的标签信息
        /// </summary>
        /// <param name="personVguid">用户vguid</param>
        /// <returns></returns>
        public JsonResult GetPersonLabel(Guid personVguid)
        {
            var list = _ul.GetPersonLabel(personVguid);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 下载导入标签模板
        /// </summary>
        public void DownLoadTemplate()
        {
            _ul.DownLoadTemplate();
        }

        /// <summary>
        /// 上传标签文件
        /// </summary>
        /// <param name="labelFile"></param>
        /// <returns></returns>
        public JsonResult UpLoadLabel(string labelFile)
        {
            //var model = new ActionResultModel<string>();
            var model = new ResultModel();
            UploadFile uf = new UploadFile();
            uf.SetMaxSizeM(1000);//上传文件大小
            string url = "/UpLoadFile/";//文件保存路径
            string saveFolder = Server.MapPath(url);
            uf.SetFileDirectory(saveFolder);
            string filesName = System.Web.HttpContext.Current.Request.Params["labelFile"];
            HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[filesName];
            var reponseMessage = uf.Save(file, "ExerciseImport");//保存文件
            UploadHelper uploadHelper = new UploadHelper();
            string readPath = Server.MapPath("/UpLoadFile/ExerciseImport/" + reponseMessage.FileName);
            var dt = uploadHelper.GetDataByExcel(readPath);

            var CorrectDt = _ul.getCorrectDt(dt);//筛选正确的表格
            var ErrorDt = _ul.getErrorDt(dt);//筛选错误的表格
            string isNullorisnotExist = "";
            if (ErrorDt.Rows.Count > 0)
            {
                //导出错误的表格
                //ExportExcel.ExportExcels("错误报表.xlsx", "错误报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", ErrorDt);
                //dt.TableName = "table";
                //string amountFileName = SyntacticSugar.ConfigSugar.GetAppString("Errortabel");
                //Common.ExportExcel.ExportExcels("Errortabel.xlsx", amountFileName, ErrorDt);

                //_ul.Existup(ErrorDt);

                isNullorisnotExist = _ul.getNulldata(dt);



            }

            model.isSuccess = false;
            try
            {
                model.isSuccess = _ul.InsertExcelToDatabase(CorrectDt);
                if (ErrorDt.Rows.Count < 0)
                {
                    model.respnseInfo = model.isSuccess ? "1" : "0";
                }
                else
                {
                    var errorrow = ErrorDt.Rows.Count;
                    model.respnseInfo = "2";
                    model.ResponseMessage = "共有" + errorrow + "条错误行 <br/>" + isNullorisnotExist;
                    model.ErrorDataTable = JsonConvert.SerializeObject(ErrorDt);

                    Session["jsondatatable"] = model.ErrorDataTable;

                }

            }
            catch (Exception ex)
            {
                model.respnseInfo = ex.Message;
            }
            System.IO.File.Delete(readPath);
            return Json(model, JsonRequestBehavior.AllowGet);

        }


        public void getDatatalbe()
        {
            var datatable = Session["jsondatatable"].ToString();
            DataTable ErrorDataTable = JsonConvert.DeserializeObject<DataTable>(datatable);
            _ul.Existup(ErrorDataTable);
        }


    }
}
