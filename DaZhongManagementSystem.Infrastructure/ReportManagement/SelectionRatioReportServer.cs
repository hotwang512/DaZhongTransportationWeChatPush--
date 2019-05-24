using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DaZhongManagementSystem.Common;
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

        public DataTable GetSelectionRatioReportDataTable(string startDate, string endDate)
        {
            DataTable selectionRatioReport = new DataTable();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                selectionRatioReport = _dbMsSql.GetDataTable("exec usp_SelectionRatioReport @StartDate,@EndDate", new { StartDate = startDate, EndDate = endDate });
                selectionRatioReport.TableName = "table";
            }
            return selectionRatioReport;
        }


        public List<PersionSelectionRatioReport> GetPersionSelectionRatioReportData(string startDate, string endDate)
        {
            List<PersionSelectionRatioReport> persionSelectionRatios = new List<PersionSelectionRatioReport>();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                persionSelectionRatios = _dbMsSql.SqlQuery<PersionSelectionRatioReport>("exec usp_PersionSelectionRatioReport @StartDate,@EndDate", new { StartDate = startDate, EndDate = endDate }).ToList();
            }
            return persionSelectionRatios;
        }
        public DataTable GetPersionSelectionRatioReportDataTable(string startDate, string endDate)
        {
            DataTable persionSelectionRatios = new DataTable();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                persionSelectionRatios = _dbMsSql.GetDataTable("exec usp_PersionSelectionRatioReport @StartDate,@EndDate", new { StartDate = startDate, EndDate = endDate });
                persionSelectionRatios.TableName = "table";
            }
            return persionSelectionRatios;
        }


        public void ExportSelectionRatioReport(string startDate = "", string endDate = "")
        {
            DataTable data = GetSelectionRatioReportDataTable(startDate, endDate);
            ExportExcel.ExportExcels("SelectionRatioReportTemplate.xlsx", "跳车单报表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", data);

        }

        public void ExportPersionSelectionRatioReport(string startDate = "", string endDate = "")
        {

            DataTable data = GetPersionSelectionRatioReportDataTable(startDate, endDate);
            ExportExcel.ExportExcels("PersionSelectionRatioReportTemplate.xlsx", "人员跳车单报表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", data);
        }
    }
}
