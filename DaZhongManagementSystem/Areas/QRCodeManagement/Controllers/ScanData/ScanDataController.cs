using System;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.QRCodeManagement.Controllers.ScanData.BusinessLogic;

namespace DaZhongManagementSystem.Areas.QRCodeManagement.Controllers.ScanData
{
    public class ScanDataController : Controller
    {
        //
        // GET: /QRCodeManagement/ScanData/
       
        public ScanDataLogic _ScanData;
        public ScanDataController()
        {

            _ScanData = new ScanDataLogic();
        }
        public ActionResult Index()
        {
            ViewBag.vguid =Request["vguid"];
            Guid vguid = Guid.Parse(ViewBag.vguid);
            Save(vguid);
            return View();
        }
        /// <summary>
        /// 扫二维码跳转页面展示数据
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public JsonResult Save(Guid vguid)
        {
            var model= _ScanData.Save(vguid);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }
}
