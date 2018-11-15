using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.ExerciseLibraryManagement.Controllers.ExerciseLibraryManagement.BusinessLogic;
using DaZhongManagementSystem.Areas.KnowledgeBaseManagement.Controllers.Draft.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Common.Tools;
using DaZhongManagementSystem.Controllers;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using JQWidgetsSugar;
using SyntacticSugar;

namespace DaZhongManagementSystem.Areas.KnowledgeBaseManagement.Controllers.Draft
{
    public class DraftController : BaseController
    {
        //
        // GET: /KnowledgeBaseManagement/Draft/
        private ExerciseLibraryLogic _exerciseLibraryLogic;
        private AuthorityManageLogic _al;
        private DraftLogic _draftLogic;
        public DraftController()
        {
            _exerciseLibraryLogic = new ExerciseLibraryLogic();
            _al = new AuthorityManageLogic();
            _draftLogic = new DraftLogic();
        }

        public ActionResult DraftList()
        {
            //绑定录入类型
            var inputType = new List<CS_Master_2>();
            inputType = _exerciseLibraryLogic.GetInputType();
            ViewData["InputType"] = inputType;
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.KnowledgeDraftModule);
            ViewBag.CurrentModulePermission = roleModuleModel;
            return View();
        }
        public ActionResult DraftDetail()
        {
            bool isEdit = Boolean.Parse(Request.QueryString["isEdit"]);
            Guid vguid = Guid.Parse(Request.QueryString["Vguid"] ?? Guid.Empty.ToString());
            Business_KnowledgeBase_Information knowledgeInfo = new Business_KnowledgeBase_Information();
            if (isEdit)
            {
                knowledgeInfo = _draftLogic.GetKnowledgeBaseInfoByVguid(vguid);
            }
            else
            {
                knowledgeInfo.Vguid = Guid.NewGuid();
            }
            ViewBag.knowledgeInfo = knowledgeInfo;
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.KnowledgeDraftModule);
            ViewBag.CurrentModulePermission = roleModuleModel;
            ViewBag.isEdit = isEdit;
            return View();
        }
        /// <summary>
        /// 将上传附件转换为html代码
        /// </summary>
        /// <param name="importFile">上传的文件</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public string ConvertToHtml(string importFile)
        {
            string bodyHtml = string.Empty;
            if (!string.IsNullOrEmpty(importFile))
            {
                try
                {
                    //保存上传文件
                    UploadFile uf = new UploadFile();
                    uf.SetMaxSizeM(1000);//上传文件大小
                    string url = "/UpLoadFile/";//文件保存路径
                    string saveFolder = Server.MapPath(url);
                    uf.SetFileDirectory(saveFolder);
                    string filesName = System.Web.HttpContext.Current.Request.Params["importFile"];
                    HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[filesName];
                    var reponseMessage = uf.Save(file, "FileImport");//保存文件
                    string readPath = Server.MapPath("/UpLoadFile/FileImport/" + reponseMessage.FileName);
                    string savePath = Server.MapPath("/UpLoadFile/FileImport/");

                    //读取word文件内容并保存至html文件中
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
                    }

                    System.IO.File.Delete(readPath);
                    System.IO.File.Delete(strSaveFileName);
                }
                catch (Exception ex)
                {
                    Common.LogHelper.LogHelper.WriteLog("【知识库草稿列表上传附件】" + ex.ToString() + "/n" + ex.StackTrace);
                }
            }
            return bodyHtml;
        }


        /// <summary>
        /// 获取草稿知识库的列表信息
        /// </summary>
        /// <param name="searchParam">搜索条件</param>
        /// <param name="para">分页信息</param>
        /// <returns></returns>
        public JsonResult GetKnowledgeListBySearch(Business_KnowledgeBase_Information searchParam, GridParams para)
        {
            if (para.sortdatafield == null)
            {
                para.sortdatafield = "CreatedDate";
                para.sortorder = "desc";
            }
            para.pagenum = para.pagenum + 1;
            var model = _draftLogic.GetKnowledgeListBySearch(searchParam, para);
            var result = new ConfigurableJsonResult { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //  return Json(model, JsonRequestBehavior.AllowGet);
            return result;
        }
        /// <summary>
        /// 批量提交草稿知识
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public JsonResult SubmitKnowledgeBase(Guid[] vguidList)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = _draftLogic.SubmitKnowledgeBase(vguidList);
            models.respnseInfo = models.isSuccess ? "1" : "0";
            return Json(models);
        }

        /// <summary>
        /// 批量删除草稿知识
        /// </summary>
        /// <param name="vguidList">主键</param>
        /// <returns></returns>
        public JsonResult DeleteKnowledgeBase(Guid[] vguidList)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = _draftLogic.DeleteKnowledgeBase(vguidList);
            models.respnseInfo = models.isSuccess ? "1" : "0";
            return Json(models);
        }

        /// <summary>
        /// 下载习题模板
        /// </summary>
        public void DownLoadTemplate()
        {
            _draftLogic.DownLoadTemplate();
        }

        /// <summary>
        /// 上传Excel
        /// </summary>
        /// <param name="knowledgeFile">Excel文件</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult UpLoadKnowledge(string knowledgeFile)
        {
            var model = new ActionResultModel<string>();
            UploadFile uf = new UploadFile();
            uf.SetMaxSizeM(1000);//上传文件大小
            string url = "/UpLoadFile/";//文件保存路径
            string saveFolder = Server.MapPath(url);
            uf.SetFileDirectory(saveFolder);
            string filesName = System.Web.HttpContext.Current.Request.Params["knowledgeFile"];
            HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[filesName];
            var reponseMessage = uf.Save(file, "KnowledgeImport");//保存文件
            UploadHelper uploadHelper = new UploadHelper();
            string readPath = Server.MapPath("/UpLoadFile/KnowledgeImport/" + reponseMessage.FileName);
            DataTable dt = uploadHelper.GetDataByExcel(readPath);
            if (dt == null || dt.Rows.Count == 0 || dt.Rows.Count == 1)
            {
                model.respnseInfo = "2";  //表格为空
            }
            else if (dt.Columns.Count != 2)
            {
                model.respnseInfo = "3";  //模板不对

            }
            else
            {
                string msg = string.Empty;
                model.isSuccess = _draftLogic.InsertExcelToDatabase(dt, ref  msg);
                if (msg != string.Empty)
                {
                    model.respnseInfo = msg;
                }
                else
                {
                    model.respnseInfo = model.isSuccess ? "1" : "0";
                }
            }
            System.IO.File.Delete(readPath);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 保存新增或编辑的知识
        /// </summary>
        /// <param name="knowledgeBaseInfo">知识信息实体</param>
        /// <param name="isEdit">是否编辑</param>
        /// <param name="saveType">保存类型 保存(1)还是提交(2)</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult SaveKnowledge(Business_KnowledgeBase_Information knowledgeBaseInfo, bool isEdit, string saveType)
        {
            var models = new ActionResultModel<string>() { isSuccess = false, respnseInfo = "0" };
            if (_draftLogic.IsExistKnowledge(knowledgeBaseInfo, isEdit))
            {
                models.respnseInfo = "2";//存在相同的
            }
            else
            {
                models.isSuccess = _draftLogic.SaveKnowledge(knowledgeBaseInfo, isEdit, saveType);
                models.respnseInfo = models.isSuccess ? "1" : "0";
            }
            return Json(models);
        }



    }
}
