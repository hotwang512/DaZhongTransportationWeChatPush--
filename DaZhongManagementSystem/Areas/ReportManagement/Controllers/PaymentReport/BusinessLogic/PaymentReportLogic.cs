using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.StoredProcedureEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.ReportManagement;

namespace DaZhongManagementSystem.Areas.ReportManagement.Controllers.PaymentReport.BusinessLogic
{
    public class PaymentReportLogic
    {
        private PaymentReportServer _paymentReportServer;

        public PaymentReportLogic()
        {
            _paymentReportServer = new PaymentReportServer();
        }

        /// <summary>
        /// 获取支付报表数据
        /// </summary>
        /// <param name="searchParas"></param>
        /// <returns></returns>
        public List<usp_Report_PayInformation> GetPaymentCount(U_PaymentHistory_Search searchParas)
        {
            return _paymentReportServer.GetPaymentCount(searchParas);
        }
        
        /// <summary>
        /// 获取月度统计报表
        /// </summary>
        /// <param name="searchParas"></param>
        /// <returns></returns>
        public List<usp_Report_MonthPayInformation> GetMonthlyPayment(U_PaymentHistory_Search searchParas)
        {
            return _paymentReportServer.GetMonthlyPayment(searchParas);
        }
        /// <summary>
        /// 导出报表
        /// </summary>
        /// <param name="searchParas">搜索条件</param>
        public void Export(string searchParas)
        {
            var model = JsonHelper.JsonToModel<U_PaymentHistory_Search>(searchParas);
            _paymentReportServer.Export(model);
        }
    }
}