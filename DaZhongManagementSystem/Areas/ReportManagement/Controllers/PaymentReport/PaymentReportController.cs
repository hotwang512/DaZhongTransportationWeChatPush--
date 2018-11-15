using System.Web.Mvc;
using DaZhongManagementSystem.Areas.ReportManagement.Controllers.PaymentReport.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Controllers;
using DaZhongManagementSystem.Entities.UserDefinedEntity;

namespace DaZhongManagementSystem.Areas.ReportManagement.Controllers.PaymentReport
{
    public class PaymentReportController : BaseController
    {
        //
        // GET: /ReportManagement/PaymentReport/
        private AuthorityManageLogic _al;
        private PaymentReportLogic _paymentReportLogic;
        public PaymentReportController()
        {
            _al = new AuthorityManageLogic();
            _paymentReportLogic = new PaymentReportLogic();
        }

        public ActionResult PaymentReport()
        {
            var roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.PaymentRecordsModule);
            ViewBag.CurrentModulePermission = roleModuleModel;
            return View();
        }
        /// <summary>
        /// 获取支付报表数据
        /// </summary>
        /// <param name="searchParas"></param>
        /// <returns></returns>
        public JsonResult GetPaymentCount(U_PaymentHistory_Search searchParas)
        {
            var list = _paymentReportLogic.GetPaymentCount(searchParas);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 导出报表
        /// </summary>
        /// <param name="searchParas">搜索条件</param>
        public void Export(string searchParas)
        {
            _paymentReportLogic.Export(searchParas);
        }

        /// <summary>
        /// 获取月度统计报表
        /// </summary>
        /// <param name="searchParas"></param>
        /// <returns></returns>
        public JsonResult GetMonthlyPayment(U_PaymentHistory_Search searchParas)
        {
            var list = _paymentReportLogic.GetMonthlyPayment(searchParas);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
