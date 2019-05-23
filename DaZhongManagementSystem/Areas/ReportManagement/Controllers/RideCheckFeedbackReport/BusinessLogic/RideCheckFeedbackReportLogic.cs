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

        public SelectionRatioReport GetPersionSelectionRatioReportData(string startDate, string endDate)
        {
            return _ss.GetPersionSelectionRatioReportData(startDate, endDate);
        }

    }
}