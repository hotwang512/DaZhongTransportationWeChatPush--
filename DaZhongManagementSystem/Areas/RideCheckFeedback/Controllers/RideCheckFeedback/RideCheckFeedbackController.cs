using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Controllers;
using SyntacticSugar;

namespace DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.RideCheckFeedback
{
    public class RideCheckFeedbackController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getRideCheckFeedback(string vguid)
        {

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult uploadFile()
        {
            UploadFile uf = new UploadFile();
            string url = "/UpLoadFile/";//文件保存路径
            string saveFolder = Server.MapPath(url);
            uf.SetFileDirectory(saveFolder);
            HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];

            string filesName = file.FileName;
            var reponseMessage = uf.Save(file, "RideCheckFeedback");
            //保存
            //string filePath = "/UpLoadFile/RideCheckFeedback/" + reponseMessage.FileName;
            /* string readPath = Server.MapPath(filePath)*/
            return Json(new { Success = true, Data = new { FilePath = reponseMessage.WebFilePath, FileName = filesName } }, JsonRequestBehavior.AllowGet);
        }
    }
}