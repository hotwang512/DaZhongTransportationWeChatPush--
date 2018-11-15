
using DaZhongManagementSystem.Areas.ExerciseManagement.Controllers.ExerciseManagement.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Common.Tools;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using JQWidgetsSugar;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.ExerciseLibraryManagement.Controllers.ExerciseLibraryManagement.BusinessLogic;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.ExerciseLibraryManagement.Controllers.ExerciseLibraryManagement
{
    public class ExerciseLibraryManagementController : BaseController
    {
        //
        // GET: /ExerciseLibraryManagement/ExerciseLibraryManagement/

        private ExerciseLibraryLogic _exerciseLibraryLogic;
        private ExerciseLogic _el;
        private AuthorityManageLogic _al;
        public ExerciseLibraryManagementController()
        {
            _exerciseLibraryLogic = new ExerciseLibraryLogic();
            _al = new AuthorityManageLogic();
            _el = new ExerciseLogic();

        }

        /// <summary>
        /// 习题列表界面
        /// </summary>
        /// <returns></returns>
        public ActionResult ExerciseLibraryList()
        {
            //绑定习题状态
            var exerciseStatus = new List<CS_Master_2>();
            exerciseStatus = _el.GetExerciseStatus();
            ViewData["ExerciseStatus"] = exerciseStatus;

            //绑定录入类型
            var inputType = new List<CS_Master_2>();
            inputType = _exerciseLibraryLogic.GetInputType();
            ViewData["InputType"] = inputType;

            var exerciseType = _el.GetExerciseType();
            ViewData["exerciseType"] = exerciseType;
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.ExerciseLibraryDraftModule);
            ViewBag.CurrentModulePermission = roleModuleModel;
            return View();
        }

        /// <summary>
        /// 分页查询习题信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResult GetExerciseListBySearch(Business_Exercises_Infomation_Search searchParam, GridParams para)
        {
            if (para.sortdatafield == null)
            {
                para.sortdatafield = "CreatedDate";
                para.sortorder = "desc";
            }
            para.pagenum = para.pagenum + 1;
            var model = _exerciseLibraryLogic.GetExerciseListBySearch(searchParam, para);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 下载习题模板
        /// </summary>
        public void DownLoadTemplate()
        {
            _exerciseLibraryLogic.DownLoadTemplate();
        }

        /// <summary>
        /// 上传习题Excel
        /// </summary>
        /// <param name="exerciseFile">习题Excel文件</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult UpLoadExercise(string exerciseFile)
        {
            var model = new ActionResultModel<string>();
            UploadFile uf = new UploadFile();
            uf.SetMaxSizeM(1000);//上传文件大小
            string url = "/UpLoadFile/";//文件保存路径
            string saveFolder = Server.MapPath(url);
            uf.SetFileDirectory(saveFolder);
            string filesName = System.Web.HttpContext.Current.Request.Params["exerciseFile"];
            HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[filesName];
            var reponseMessage = uf.Save(file, "ExerciseImport");//保存文件
            UploadHelper uploadHelper = new UploadHelper();
            string readPath = Server.MapPath("/UpLoadFile/ExerciseImport/" + reponseMessage.FileName);
            DataTable exerciseDt = uploadHelper.GetDataByExcel(readPath);
            model.isSuccess = _exerciseLibraryLogic.InsertExcelToDatabase(exerciseDt);
            System.IO.File.Delete(readPath);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 批量审核习题
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public JsonResult CheckedExercise(string[] vguidList)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = false;
            models.isSuccess = _exerciseLibraryLogic.CheckedExercise(vguidList);
            models.respnseInfo = models.isSuccess ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批量删除习题
        /// </summary>
        /// <param name="vguidList">勾选习题Vguid数组</param>
        /// <returns></returns>
        public JsonResult DeletedExercise(string[] vguidList)
        {
            var models = new ActionResultModel<string>();
            models.isSuccess = false;

            models.isSuccess = _exerciseLibraryLogic.DeletedExercise(vguidList);
            models.respnseInfo = models.isSuccess == true ? "1" : "0";

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过习题信息的Vguid获取习题详细信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public JsonResult GetExerciseDetailInfo(Guid vguid)
        {
            var model = _exerciseLibraryLogic.GetExerciseDetailInfo(vguid);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存习题信息
        /// </summary>
        /// <param name="exerciseData">习题信息</param>
        /// <param name="isEdit">是否编辑</param>
        /// <returns></returns>
        public JsonResult SaveExerciseMain(string exerciseData, bool isEdit)
        {
            var models = new ActionResultModel<String>();
            models.isSuccess = false;
            models.isSuccess = _exerciseLibraryLogic.SaveExercise(exerciseData, isEdit);
            models.respnseInfo = models.isSuccess == true ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }

    }
}
