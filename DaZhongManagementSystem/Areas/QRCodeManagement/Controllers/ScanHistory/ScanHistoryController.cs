using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.QRCodeManagement.Controllers.ScanHistory.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using JQWidgetsSugar;

namespace DaZhongManagementSystem.Areas.QRCodeManagement.Controllers.ScanHistory
{
    public class ScanHistoryController : Controller
    {
        //
        // GET: /QRCodeManagement/ScanHistory/


        private ScanHistoryLogic _scanHistoryLogic;
        private AuthorityManageLogic _al;
        public ScanHistoryController()
        {
            _al = new AuthorityManageLogic();
            _scanHistoryLogic = new ScanHistoryLogic();
        }

        public ActionResult ScanHistoryList()
        {
            var roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.ScanHistoryModule);
            ViewBag.CurrentModulePermission = roleModuleModel;
            return View();
        }

        /// <summary>
        /// 分页获取扫描历史列表信息
        /// </summary>
        /// <param name="searchParam">搜索条件</param>
        /// <param name="para">分页信息</param>
        /// <returns></returns>
        public JsonResult GetScanHistoryListBySearch(ScanHistorySearch searchParam, GridParams para)
        {
            if (para.sortdatafield == null)
            {
                para.sortdatafield = "CreatedDate";
                para.sortorder = "desc";
            }
            para.pagenum = para.pagenum + 1;
            var model = _scanHistoryLogic.GetScanHistoryListBySearch(searchParam, para);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除扫码历史
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public JsonResult DeletedScanHistory(Guid[] vguidList)
        {
            var model = new ActionResultModel<string>();
            model.isSuccess = _scanHistoryLogic.DeletedScanHistory(vguidList);
            model.respnseInfo = model.isSuccess ? "1" : "0";
            return Json(model);
        }

        /// <summary>
        /// 导出
        /// </summary>
        public void Export(string searchParams)
        {
            _scanHistoryLogic.Export(searchParams);
        }
    }
}
