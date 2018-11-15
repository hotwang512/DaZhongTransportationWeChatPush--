using DaZhongManagementSystem.Areas.DailyLogManagement.Controllers.LogManagement.BusinessLogic;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.DailyLogManagement.Controllers.LogManagement
{
    public class LogManagementController : BaseController
    {
        //
        // GET: /DailyLogManagement/LogManagement/
        public LogBusiness _lb;
        public LogManagementController()
        {
            _lb = new LogBusiness();
        }

        public ActionResult LogManagement()
        {
            List<CS_Master_2> eventTypeList = new List<CS_Master_2>();
            eventTypeList = _lb.GetEventTypeList();

            ViewBag.eventTypeList = eventTypeList;
            return View();
        }

        /// <summary>
        /// 获取日志详细信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public JsonResult GetDailyDetail(string vguid)
        {
            Business_OperationLog operationLog = new Business_OperationLog();
            operationLog = _lb.GetDailyDetail(vguid);
           // return Json(operationLog, JsonRequestBehavior.AllowGet);
            var result = new ConfigurableJsonResult() { Data = operationLog, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }

        /// <summary>
        /// 通过查询条件获取日志信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResult GetLogListBySearch(SearchLogList searchParam, GridParams para)
        {
            if (para.sortdatafield == null)
            {
                para.sortdatafield = "CreatedDate";
                para.sortorder = "desc";
            }
            para.pagenum = para.pagenum + 1;//页0，+1

            var model = _lb.GetLogListBySearch(searchParam, para);
            // return Json(model, JsonRequestBehavior.AllowGet);
            var result = new ConfigurableJsonResult() { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }
    }
}
