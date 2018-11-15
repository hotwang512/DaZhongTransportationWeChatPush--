using Aspose.Cells;
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
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using DaZhongManagementSystem.Areas.ExerciseLibraryManagement.Controllers.ExerciseLibraryManagement.BusinessLogic;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.ExerciseManagement.Controllers.ExerciseManagement
{
    public class ExerciseManagementController : BaseController
    {
        //
        // GET: /ExerciseManagement/ExerciseManagement/

        public ExerciseLogic _el;
        public AuthorityManageLogic _al;
        public ExerciseManagementController()
        {
            _el = new ExerciseLogic();
            _al = new AuthorityManageLogic();
        }

        /// <summary>
        /// 习题列表界面
        /// </summary>
        /// <returns></returns>
        public ActionResult ExerciseList()
        {
            //绑定习题状态
            var exerciseStatus = new List<CS_Master_2>();
            exerciseStatus = _el.GetExerciseStatus();
            ViewData["ExerciseStatus"] = exerciseStatus;

            //绑定录入类型
            var inputType = new List<CS_Master_2>();
            inputType = _el.GetInputType();
            ViewData["InputType"] = inputType;
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.ExerciseModule);

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
            var model = _el.GetExerciseListBySearch(searchParam, para);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 下载习题模板
        /// </summary>
        public void DownLoadTemplate()
        {
            _el.DownLoadTemplate();
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
            model.isSuccess = _el.InsertExcelToDatabase(exerciseDt);
            System.IO.File.Delete(readPath);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 习题详情界面（新增/编辑）
        /// </summary>
        /// <returns></returns>
        public ActionResult ExerciseDetail()
        {
            //绑定习题类型
            var exerciseType = new List<CS_Master_2>();
            exerciseType = _el.GetExerciseType();
            ViewData["ExerciseType"] = exerciseType;
            ExerciseLibraryLogic exerciseLibraryLogic=new ExerciseLibraryLogic();
            var exerciseInputType = exerciseLibraryLogic.GetInputType();
            ViewData["exerciseInputType"] = exerciseInputType;
            //绑定录入类型
            var inputType = new List<CS_Master_2>();
            inputType = _el.GetInputType();
            ViewData["InputType"] = inputType;
            bool isEdit = Boolean.Parse(Request.QueryString["isEdit"]);
            string vguid = Request.QueryString["Vguid"];
            Business_Exercises_Infomation exerciseMain = new Business_Exercises_Infomation();
            if (isEdit)//编辑
            {
                exerciseMain = _el.GetExerciseByVguid(vguid);

            }
            else//新增
            {
                exerciseMain.Vguid = Guid.NewGuid();
            }
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.ExerciseModule);

            ViewBag.CurrentModulePermission = roleModuleModel;
            ViewBag.isEdit = isEdit;
            ViewBag.exerciseMainModel = exerciseMain;
            return View();
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
            models.isSuccess = _el.CheckScore(vguidList);
            if (!models.isSuccess)
            {
                models.respnseInfo = "2";//存在习题总分不为100分的习题
            }
            else
            {
                models.isSuccess = _el.CheckedExercise(vguidList);
                models.respnseInfo = models.isSuccess == true ? "1" : "0";
            }
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

            models.isSuccess = _el.DeletedExercise(vguidList);
            models.respnseInfo = models.isSuccess == true ? "1" : "0";

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过习题主信息的Vguid获取习题详细列表
        /// </summary>
        /// <param name="Vguid"></param>
        /// <returns></returns>
        //[HttpPost]
        public JsonResult GetExerciseDetailListByMainVguid(string Vguid)
        {
            List<Business_ExercisesDetail_Infomation> exerciseDetailList = new List<Business_ExercisesDetail_Infomation>();
            exerciseDetailList = _el.GetExerciseDetailListByMainVguid(Vguid);
            return Json(exerciseDetailList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存习题信息(主信息、详细信息)
        /// </summary>
        /// <param name="exerciseMainModel"></param>
        /// <returns></returns>
        public JsonResult SaveExerciseMain(Business_Exercises_Infomation exerciseMainModel, bool isEdit, string exerciseData)
        {
            var models = new ActionResultModel<String>();
            models.isSuccess = false;
            models.isSuccess = _el.SaveExerciseMain(exerciseMainModel, isEdit, exerciseData);
            models.respnseInfo = models.isSuccess == true ? "1" : "0";
            return Json(models, JsonRequestBehavior.AllowGet);
        }

    }
}
