using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.QRCodeManagement.Controllers.ScanData
{
    public class WeChatScanDataController : Controller
    {
        //
        // GET: /QRCodeManagement/WeChatScanData/

        public ActionResult ScanData(string vguid)
        {

            return View();
        }

    }
}
