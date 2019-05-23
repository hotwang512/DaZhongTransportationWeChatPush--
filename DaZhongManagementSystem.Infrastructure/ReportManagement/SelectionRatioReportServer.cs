using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;

namespace DaZhongManagementSystem.Infrastructure.ReportManagement
{
    public class SelectionRatioReportServer
    {
        private LogLogic _logLogic;


        public SelectionRatioReportServer()
        {
            _logLogic = new LogLogic();
        }

        public SelectionRatioReport GetSelectionRatioReportData(string startDate, string endDate)
        {
            SelectionRatioReport selectionRatioReport = new SelectionRatioReport();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                selectionRatioReport = _dbMsSql.SqlQuery<SelectionRatioReport>("exec usp_SelectionRatioReport @StartDate,@EndDate", new { StartDate = startDate, EndDate = endDate }).SingleOrDefault();
            }
            return selectionRatioReport;
        }

        public SelectionRatioReport GetPersionSelectionRatioReportData(string startDate, string endDate)
        {
            SelectionRatioReport selectionRatioReport = new SelectionRatioReport();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                selectionRatioReport = _dbMsSql.SqlQuery<SelectionRatioReport>("exec usp_PersionSelectionRatioReport @StartDate,@EndDate", new { StartDate = startDate, EndDate = endDate }).SingleOrDefault();
            }
            return selectionRatioReport;
        }
    }
}
