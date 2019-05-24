using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.ReportManagement;

namespace DaZhongManagementSystem.Areas.ReportManagement.Controllers.RideCheckFeedbackReport.BusinessLogic
{
    public class RideCheckFeedbackReportLogic
    {
        public SelectionRatioReportServer _ss;
        public RideCheckFeedbackReportLogic()
        {
            _ss = new SelectionRatioReportServer();
        }

        public SelectionRatioReport GetSelectionRatioReportData(string startDate, string endDate)
        {
            return _ss.GetSelectionRatioReportData(startDate, endDate);
        }

        public List<PersionSelectionRatioReport> GetPersionSelectionRatioReportData(string startDate, string endDate)
        {
            return _ss.GetPersionSelectionRatioReportData(startDate, endDate);
        }

        public void ExportSelectionRatioReport(string startDate = "", string endDate = "")
        {
            _ss.ExportSelectionRatioReport(startDate, endDate);

        }

        public void ExportPersionSelectionRatioReport(string startDate = "", string endDate = "")
        {
            _ss.ExportPersionSelectionRatioReport(startDate, endDate);

        }

    }
}