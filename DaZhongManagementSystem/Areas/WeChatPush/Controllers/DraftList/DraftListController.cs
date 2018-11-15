using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.DraftList.BusinessLogic;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using JQWidgetsSugar;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Common.Tools;
using DaZhongManagementSystem.Common.WeChatPush;
using DaZhongManagementSystem.Controllers;
using Newtonsoft.Json;
using DataTable = System.Data.DataTable;


namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.DraftList
{
    public class DraftListController : BaseController
    {
        //
        // GET: /WeChatPush/DraftList/
        private readonly DraftInfoLogic _dl;
        private readonly AuthorityManageLogic _al;

        public DraftListController()
        {
            _dl = new DraftInfoLogic();
            _al = new AuthorityManageLogic();
        }

        public ActionResult Test()
        {
            return View();
        }

        /// <summary>
        /// 推送列表界面
        /// </summary>
        /// <returns></returns>
        public ActionResult DraftList()
        {
            List<CS_Master_2> pushType = new List<CS_Master_2>();
            pushType = _dl.GetPushTypeList();
            ViewData["PushType"] = pushType;
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.DraftListModule);

            ViewBag.CurrentModulePermission = roleModuleModel;
            return View();
        }

        public ActionResult DraftDetail()
        {
            Business_WeChatPush_Information weChatMainModel = new Business_WeChatPush_Information();

            bool isEdit = Boolean.Parse(Request.QueryString["isEdit"]);
            string vguid = Request.QueryString["Vguid"];
            List<CS_Master_2> pushType = new List<CS_Master_2>();
            pushType = _dl.GetPushTypeList();
            ViewData["PushType"] = pushType;


            List<CS_Master_2> weChatPushType = new List<CS_Master_2>();
            weChatPushType = _dl.GetWeChatPushType();
            ViewData["WeChatPushType"] = weChatPushType;

            List<Business_Exercises_Infomation> exerciseList = new List<Business_Exercises_Infomation>();
            exerciseList = _dl.GetExerciseList();
            ViewData["ExerciseList"] = exerciseList;

            List<Business_KnowledgeBase_Information> knowledgeList = new List<Business_KnowledgeBase_Information>();
            knowledgeList = _dl.GetknowledgeList();
            ViewData["KnowledgeList"] = knowledgeList;

            List<Business_Questionnaire> questionList = new List<Business_Questionnaire>();
            questionList = _dl.GetQuestionList();
            ViewData["QuestionList"] = questionList;

            ViewData["RevenueTypeList"] = _dl.GetRevenueType();

            ViewData["LabelType"] = _dl.GetLabels();

            string pushObject = string.Empty;
            string pushName = string.Empty;
            if (isEdit) //编辑
            {
                weChatMainModel = _dl.GetWeChatMainByVguid(vguid);
                pushObject = _dl.GetPushObjectPhone(vguid);
                pushName = _dl.GetPushObjectStr(vguid);
            }
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(CurrentUser.GetCurrentUser().Role, ModuleVguid.DraftListModule);

            ViewData["currentUserDepartment"] = "";
            if (!string.IsNullOrEmpty(CurrentUser.GetCurrentUser().Department))
            {
                ViewData["currentUserDepartment"] = CurrentUser.GetCurrentUser().Department;
            }
            ViewBag.PushObject = pushObject;
            ViewBag.PushName = pushName;
            ViewBag.CurrentModulePermission = roleModuleModel;
            ViewBag.WeChatModel = weChatMainModel;
            ViewBag.isEdit = isEdit;
            ViewBag.listCountersignType = _dl.GetMasterDataType(MasterVGUID.CountersignType);
            ViewBag.listRedPacketType = _dl.GetMasterDataType(MasterVGUID.RedPacketType);
            return View();
        }
        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLabels()
        {
            var list = _dl.GetAllLabel();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 多图文推送情况下获取子表的数据
        /// </summary>
        /// <param name="vguid">主表的guid</param>
        /// <returns></returns>
        public JsonResult GetMoreGraphicList(string vguid)
        {
            var list = _dl.GetMoreGraphicList(vguid);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 通过查询条件获取推送信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResult GetWeChatPushListBySearch(SearchWeChatPushList searchParam, GridParams para)
        {
            if (para.sortdatafield == null)
            {
                para.sortdatafield = "CreatedDate";
                para.sortorder = "desc";
            }
            para.pagenum = para.pagenum + 1; //页0，+1

            //List<DeparTment_1> departmenteList = _dl.GetDepartmentList(searchParam, para);
            var model = _dl.GetWeChatPushListBySearch(searchParam, para);
            return new ConfigurableJsonResult { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// 推送接收者树形结构
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOrganizationTreeList()
        {
            var model = _dl.GetOrganizationTreeList();
            var result = new ConfigurableJsonResult { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }

        public JsonResult GetUserListBySearch(v_Business_PersonnelDepartment_Information searchParam)
        {
            var model = _dl.GetUserList(searchParam);
            return new ConfigurableJsonResult { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// 下载模板
        /// </summary>
        public void DownLoadTemplate()
        {
            _dl.DownLoadTemplate();
        }

        /// <summary>
        /// 下载导入推送模板
        /// </summary>
        //public void DownLoadPushTemplate()
        //{
        //    //  _dl.DownLoadPushTemplate();

        //}
        /// <summary>
        /// 下载导入推送模板
        /// </summary>
        public void DownFile()
        {
            string filePath = "/PushTemplate/PushTemplate.xlsx";
            string absoluFilePath = Server.MapPath(filePath);
            ResponseSugar.ResponseFile(absoluFilePath, "PushTemplate.xlsx");
        }
        /// <summary>
        /// 获取习题有效时间
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public string GetExerciseEffectiveTime(string vguid)
        {
            var model = _dl.GetExerciseEffectiveTime(vguid);
            string effectTime = model.EffectiveDate.ToString("yyyy-MM-dd HH:mm");

            return effectTime;
        }

        /// <summary>
        /// 上传封面图片（临时保存本地文件）
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult UploadImg(string id)
        {
            UploadImage ui = new UploadImage();
            ui.SetAllowSize = 1000; //允许上传图片的大小（M）
            ui.SetAllowFormat = ".jpeg,.jpg,.bmp,.gif,.png"; //允许图片上传格式
            string url = "/Areas/WeChatPush/Views/_img/"; //文件保存路径
            string saveFolder = Server.MapPath(url);
            //HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];
            string fileName = "CoverImgs";
            if (id != "0")
            {
                fileName = "CoverImgs" + id;
            }
            HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[fileName];
            var reponseMessage = ui.FileSaveAs(file, saveFolder); //保存图片
            if (!reponseMessage.IsError)
            {
                Path.Combine(url, reponseMessage.FileName);
            }
            return Json(reponseMessage, "text/html");
        }
        /// <summary>
        /// 上海工资条封面图片
        /// </summary>
        /// <returns></returns>
        public JsonResult UploadSalaryImg()
        {
            var models = new ActionResultModel<string>() { isSuccess = false };
            if (Request.Files.Count <= 0) return Json(models);
            if (Request.Files[0].ContentLength >= 1000 * 1024)
            {
                models.respnseInfo = "文件超过1000M!";
                return Json(models);
            }
            string[] imgType = { ".jpeg", ".jpg", ".bmp", ".gif", ".png" };
            if (!imgType.Contains(Path.GetExtension(Request.Files[0].FileName)))
            {
                models.respnseInfo = "文件类型不匹配！";
                return Json(models);
            }
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
            string saveFolder = Server.MapPath("/Areas/WeChatPush/Views/_img/" + fileName + Path.GetExtension(Request.Files[0].FileName));
            Request.Files[0].SaveAs(saveFolder);
            models.isSuccess = true;
            models.respnseInfo = "/Areas/WeChatPush/Views/_img/" + fileName + Path.GetExtension(Request.Files[0].FileName);
            return Json(models, "text/html");

        }
        /// <summary>
        /// 将上传附件转换为html代码
        /// </summary>
        /// <param name="id">上传的文件</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public string ConvertToHtml(string id)
        {
            string bodyHtml = string.Empty;
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    //保存上传文件
                    UploadFile uf = new UploadFile();
                    uf.SetMaxSizeM(1000); //上传文件大小
                    string url = "/UpLoadFile/"; //文件保存路径
                    string saveFolder = Server.MapPath(url);
                    uf.SetFileDirectory(saveFolder);
                    string fileName = "importFile";
                    if (id != "0")
                    {
                        fileName = "importFile" + id;
                    }
                    //  string filesName = System.Web.HttpContext.Current.Request.Params[fileName];
                    HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[fileName];
                    var reponseMessage = uf.Save(file, "FileImport"); //保存文件
                    string readPath = Server.MapPath("/UpLoadFile/FileImport/" + reponseMessage.FileName);
                    string savePath = Server.MapPath("/UpLoadFile/FileImport/");

                    ////读取word文件内容并保存至html文件中
                    string strSaveFileName = savePath + DateTime.Now.ToString("yyyyMMddHHmmss") + ".html";
                    Aspose.Words.Document awd = new Aspose.Words.Document(readPath);
                    awd.Save(strSaveFileName, Aspose.Words.SaveFormat.Html);
                    //读取html文件中body内容
                    using (StreamReader reader = new StreamReader(strSaveFileName, Encoding.UTF8))
                    {
                        bodyHtml += reader.ReadToEnd();
                    }
                    Regex reg = new Regex("(?is)<body[^>]*>(?<body>.*?)</body>");
                    bodyHtml = reg.Match(bodyHtml).Groups["body"].Value;
                    bodyHtml = bodyHtml.Replace("&#xa0;", "");

                    Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
                    //新建一个matches的MatchCollection对象 保存 匹配对象个数(img标签)  
                    MatchCollection matches = regImg.Matches(bodyHtml);
                    //遍历所有的img标签对象  
                    foreach (Match match in matches)
                    {
                        bodyHtml = bodyHtml.Replace(match.Groups["imgUrl"].Value, "/UpLoadFile/FileImport/" + match.Groups["imgUrl"].Value);
                        //bodyHtml = bodyHtml.Replace("&#xa0;", "");
                        //将img中的src替换掉
                        //finalHtml = bodyHtml.Replace(match.Groups["imgUrl"].Value, "/UpLoadFile/FileImport/" + match.Groups["imgUrl"].Value);
                    }

                    System.IO.File.Delete(readPath);
                    System.IO.File.Delete(strSaveFileName);
                }
                catch (Exception ex)
                {
                    Common.LogHelper.LogHelper.WriteLog("【推送草稿列表上传附件】" + ex.ToString() + "/n" + ex.StackTrace);
                }
            }
            return bodyHtml;
        }

        /// <summary>
        /// 批量删除推送信息
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public JsonResult DeletePushList(string[] vguidList)
        {
            var models = new ActionResultModel<string> { isSuccess = false };
            models.isSuccess = _dl.DeletePushMsg(vguidList);
            models.respnseInfo = models.isSuccess ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批量提交推送信息
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public JsonResult SubmitDraftList(string[] vguidList)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = false;
            models.respnseInfo = "0";
            models.isSuccess = _dl.SubmitDraftList(vguidList);
            models.respnseInfo = models.isSuccess ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存推送信息（主信息，详细信息）
        /// </summary>
        /// <param name="pushMsgModel">推送实体</param>
        /// <param name="txtMessage">推送内容</param>
        /// <param name="isEdit">编辑/新增</param>
        /// <param name="history">是否保存消息历史</param>
        /// <param name="saveType">保存还是提交</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult SavePushMsg(U_PushMsg pushMsgModel, string txtMessage, string isEdit, string history, string saveType)
        {
            var model = new ActionResultModel<string>();
            model.isSuccess = false;
            pushMsgModel.Message = txtMessage;
            bool edit = bool.Parse(isEdit);
            bool isHistory = bool.Parse(history);
            pushMsgModel.History = isHistory ? "1" : "0";
            model.isSuccess = _dl.SavePushMsg(pushMsgModel, edit, saveType);
            model.respnseInfo = model.isSuccess ? "1" : "0";
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 保存多图文推送消息
        /// </summary>
        /// <param name="wechatPushList">推送消息</param>
        /// <param name="wechatPushMoreGraphicList">推送消息</param>
        /// <param name="isEdit">是否编辑</param>
        /// <param name="saveType">保存还是提交</param>
        /// <param name="countersignType">协议类型</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult SaveImagePushMsg(string wechatPushList, string wechatPushMoreGraphicList, string isEdit, string saveType, string countersignType)
        {
            var model = new ActionResultModel<string>();
            model.isSuccess = false;
            bool edit = bool.Parse(isEdit);
            model.isSuccess = _dl.SaveImagePushMsg(wechatPushList, wechatPushMoreGraphicList, edit, saveType, countersignType);
            model.respnseInfo = model.isSuccess ? "1" : "0";
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 保存工资条推送消息
        /// </summary>
        /// <param name="pushMsgModel"></param>
        /// <returns></returns>
        public JsonResult SaveSalaryPush(U_PushMsg pushMsgModel)
        {
            var model = new ResultModel<string>();
            model.IsSuccess = false;
            var msg = string.Empty;
            model.IsSuccess = _dl.SaveSalaryPush(pushMsgModel, ref msg);
            model.ReturnMsg = model.IsSuccess ? "1" : "0";
            if (!string.IsNullOrEmpty(msg))
            {
                model.ReturnMsg = "2";
                model.ResponseInfo = msg;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存保养消息推送消息
        /// </summary>
        /// <param name="pushMsgModel"></param>
        /// <returns></returns>
        public JsonResult SaveMaintenancePush(U_PushMsg pushMsgModel)
        {
            var model = new ResultModel<string>();
            model.IsSuccess = false;
            var msg = string.Empty;
            model.IsSuccess = _dl.SaveMaintenancePush(pushMsgModel, ref msg);
            model.ReturnMsg = model.IsSuccess ? "1" : "0";
            if (!string.IsNullOrEmpty(msg))
            {
                model.ReturnMsg = "2";
                model.ResponseInfo = msg;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 上传Excel
        /// </summary>
        /// <param name="pushFile">Excel文件</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult UpLoadPushObject(string pushFile)
        {
            // var model = new ActionResultModel<string>();
            UploadFile uf = new UploadFile();
            uf.SetMaxSizeM(1000); //上传文件大小
            string url = "/UpLoadFile/"; //文件保存路径
            string saveFolder = Server.MapPath(url);
            uf.SetFileDirectory(saveFolder);
            string filesName = System.Web.HttpContext.Current.Request.Params["pushFile"];
            HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[filesName];
            var reponseMessage = uf.Save(file, "PushObjectImport"); //保存文件
            UploadHelper uploadHelper = new UploadHelper();
            string readPath = Server.MapPath("/UpLoadFile/PushObjectImport/" + reponseMessage.FileName);
            DataTable dt = uploadHelper.GetDataByExcel(readPath);
            System.IO.File.Delete(readPath);
            if (dt == null || dt.Rows.Count == 0 || dt.Rows.Count == 1)
            {
                // model.respnseInfo = "2";  //表格为空
                return Json("2");
            }
            else if (dt.Columns.Count != 2)
            {
                //  model.respnseInfo = "3";  //模板不对
                return Json("3");
            }
            else
            {
                try
                {
                    var result = _dl.SearchUserId(dt);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {

                    return Json(new { userId = "", exp = ex.Message });
                }


                //  model.respnseInfo = result.username;
                //  model.isSuccess = ;
                // model.respnseInfo = model.isSuccess ? "1" : "0";
            }

            //return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 上传Excel
        /// </summary>
        /// <param name="pushFile">Excel文件</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult UpLoadPush(string pushFile)
        {
            UploadFile uf = new UploadFile();
            uf.SetMaxSizeM(1000); //上传文件大小
            string url = "/UpLoadFile/"; //文件保存路径
            string saveFolder = Server.MapPath(url);
            uf.SetFileDirectory(saveFolder);
            string filesName = System.Web.HttpContext.Current.Request.Params["pushFile"];
            HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[filesName];
            var reponseMessage = uf.Save(file, "PushObjectImport"); //保存文件
            UploadHelper uploadHelper = new UploadHelper();
            string readPath = Server.MapPath("/UpLoadFile/PushObjectImport/" + reponseMessage.FileName);
            DataTable dt = uploadHelper.GetDataByExcel(readPath);
            System.IO.File.Delete(readPath);
            var models = new ReturnResultModel<string>();
            try
            {
                string msg = "";
                models.IsSuccess = _dl.SaveUploadPushMSg(dt, ref msg);
                models.ResponseInfo = models.IsSuccess ? "1" : "0";
                models.ReturnMsg = msg;
            }
            catch (Exception ex)
            {
                models.ResponseInfo = ex.Message;
            }

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 上传工资Excel
        /// </summary>
        /// <returns></returns>
        public JsonResult UpLoadSalary()
        {
            var models = new ActionResultModel<string> { isSuccess = false, respnseInfo = "" };
            if (Request.Files.Count <= 0) return Json(models);
            if (Request.Files[0].ContentLength / 1024.0 / 1024.0 >= 1000)
            {
                models.respnseInfo = "文件超过1000M!";
                return Json(models);
            }
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
            string saveFolder = Server.MapPath("/UpLoadFile/PushObjectImport/" + fileName + Path.GetExtension(Request.Files[0].FileName));
            Request.Files[0].SaveAs(saveFolder);
            UploadHelper uploadHelper = new UploadHelper();
            DataTable dt = uploadHelper.GetDataByExcel(saveFolder, true);
            System.IO.File.Delete(saveFolder);
            if (dt.Rows.Count == 0)
            {
                models.respnseInfo = "表格不能为空！";
                return Json(models);
            }
            try
            {
                models.isSuccess = _dl.SaveUpLoadSalary(dt);
                models.respnseInfo = "1";

            }
            catch (Exception ex)
            {
                models.respnseInfo = ex.Message;
            }
            return Json(models);
        }

        /// <summary>
        /// 上传保养消息
        /// </summary>
        /// <returns></returns>
        public JsonResult UpLoadMaintence()
        {
            var models = new ActionResultModel<string> { isSuccess = false, respnseInfo = "" };
            if (Request.Files.Count <= 0) return Json(models);
            if (Request.Files[0].ContentLength / 1024.0 / 1024.0 >= 1000)
            {
                models.respnseInfo = "文件超过1000M!";
                return Json(models);
            }
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
            string saveFolder = Server.MapPath("/UpLoadFile/PushObjectImport/" + fileName + Path.GetExtension(Request.Files[0].FileName));
            Request.Files[0].SaveAs(saveFolder);
            UploadHelper uploadHelper = new UploadHelper();
            DataTable dt = uploadHelper.GetDataByExcelString(saveFolder, true);
            System.IO.File.Delete(saveFolder);
            if (dt.Rows.Count == 0)
            {
                models.respnseInfo = "表格不能为空！";
                return Json(models);
            }
            try
            {
                models.isSuccess = _dl.SaveUpLoadMaintence(dt);
                models.respnseInfo = "1";

            }
            catch (Exception ex)
            {
                models.respnseInfo = ex.Message;
            }
            return Json(models);
        }
        
        /// <summary>
        /// 获取导入推送中不存在的人员信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResult GetNotExistPeople(GridParams para)
        {
            para.pagenum = para.pagenum + 1;
            var notExistPeople = _dl.GetNotExistPeople(para);
            return Json(notExistPeople, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        ///下载导入推送中不存在的人员信息
        /// </summary>
        /// <returns></returns>
        public void DownNotExistPeople(string fileName)
        {
            _dl.DownNotExistPeople(fileName);
        }

        /// <summary>
        /// 删除存储推送失败人员的临时表
        /// </summary>
        public void DropNotExistPersonTable()
        {
            _dl.DropNotExistPersonTable();
        }
    }
}
