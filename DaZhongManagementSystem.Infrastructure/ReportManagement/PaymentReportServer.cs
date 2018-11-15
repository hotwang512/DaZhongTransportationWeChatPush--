using System;
using System.Collections.Generic;
using System.Data;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.StoredProcedureEntity;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;

namespace DaZhongManagementSystem.Infrastructure.ReportManagement
{
    public class PaymentReportServer
    {
        private LogLogic _logLogic;


        public PaymentReportServer()
        {
            _logLogic = new LogLogic();
        }

        /// <summary>
        /// 获取支付报表数据
        /// </summary>
        /// <param name="searchParas"></param>
        /// <returns></returns>
        public List<usp_Report_PayInformation> GetPaymentCount(U_PaymentHistory_Search searchParas)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var mainDep = Guid.Empty;
                if (CurrentUser.GetCurrentUser().LoginName != "sysAdmin")
                {
                    Guid dep = Guid.Parse(CurrentUser.GetCurrentUser().Department);
                    mainDep = dep;
                    var listDep = db.SqlQuery<Guid>("SELECT * FROM dbo.TF_OrganizationFDetail('" + dep + "')");
                    //找到该部门以及其所有子部门
                    if (!string.IsNullOrEmpty(searchParas.Department))
                    {
                        Guid searchDep = Guid.Parse(searchParas.Department);
                        if (!listDep.Contains(searchDep))
                        {
                            searchParas.Department = null;
                        }
                    }
                }
                else
                {
                    mainDep = db.Queryable<Master_Organization>().Where(i => i.ParentVguid == null).Select(i => i.Vguid).SingleOrDefault();
                }
                switch (searchParas.PaymentStatus)
                {
                    case "支付成功":
                        searchParas.PaymentStatus = "1";
                        break;
                    case "支付失败":
                        searchParas.PaymentStatus = "2";
                        break;
                    case "待支付":
                        searchParas.PaymentStatus = "3";
                        break;
                    case "已退款":
                        searchParas.PaymentStatus = "4";
                        break;
                    default:
                        searchParas.PaymentStatus = "1";
                        break;
                }
                var list = db.SqlQuery<usp_Report_PayInformation>("exec usp_Report_PayInformation @name,@phoneNumber,@OwnedFleet,@starDate,@endDate,@Status", new
                {
                    name = searchParas.Name ?? "",
                    phoneNumber = searchParas.PhoneNumber ?? "",
                    OwnedFleet = string.IsNullOrEmpty(searchParas.Department) ? mainDep.ToString() : searchParas.Department,
                    starDate = searchParas.PayDateFrom == null ? "1900-01-01 00:00:00" : string.Format("{0:yyyy-MM-dd HH:mm:ss}", searchParas.PayDateFrom),
                    endDate = searchParas.PayDateTo == null ? "9999-12-31 23:59:59" : string.Format("{0:yyyy-MM-dd HH:mm:ss}", searchParas.PayDateTo),
                    Status = searchParas.PaymentStatus
                });

                return list;
            }
        }

        /// <summary>
        /// 导出报表
        /// </summary>
        /// <param name="searchParas">搜索条件</param>
        public void Export(U_PaymentHistory_Search searchParas)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                #region 获取支付历史数据

                var query = db.Queryable<v_PaymentHistory_Information>();
                if (CurrentUser.GetCurrentUser().LoginName != "sysAdmin")
                {
                    Guid dep = Guid.Parse(CurrentUser.GetCurrentUser().Department);
                    var listDep = db.SqlQuery<Guid>("SELECT * FROM dbo.TF_OrganizationFDetail('" + dep + "')");
                    //找到该部门以及其所有子部门
                    query.In(i => i.OwnedFleet, listDep);
                }
                if (!string.IsNullOrEmpty(searchParas.Name))
                {
                    query.Where(i => i.Name.Contains(searchParas.Name));
                }
                if (!string.IsNullOrEmpty(searchParas.PhoneNumber))
                {
                    query.Where(i => i.PhoneNumber.Contains(searchParas.PhoneNumber));
                }
                if (!string.IsNullOrEmpty(searchParas.IDNumber))
                {
                    query.Where(i => i.IDNumber.Contains(searchParas.IDNumber));
                }
                if (!string.IsNullOrEmpty(searchParas.JobNumber))
                {
                    query.Where(i => i.JobNumber.Contains(searchParas.JobNumber));
                }
                if (!string.IsNullOrEmpty(searchParas.TransactionID))
                {
                    query.Where(i => i.TransactionID.Contains(searchParas.TransactionID));
                }
                if (!string.IsNullOrEmpty(searchParas.Department))
                {
                    Guid department = Guid.Parse(searchParas.Department);
                    var listDep = db.SqlQuery<Guid>("SELECT * FROM dbo.TF_OrganizationFDetail('" + department + "')");
                    //找到该部门以及其所有子部门
                    query.In(i => i.OwnedFleet, listDep);
                }
                if (searchParas.PayDateFrom != null)
                {
                    query.Where(i => i.PayDate >= searchParas.PayDateFrom);
                }
                if (searchParas.PayDateTo != null)
                {
                    query.Where(i => i.PayDate <= searchParas.PayDateTo);
                }
                query.OrderBy(i => i.PayDate, OrderByType.Desc);
                var dtPaymentHistory = query.ToDataTable();
                dtPaymentHistory.TableName = "PaymentHistoryInfo";

                #endregion

                #region 获取支付总和

                var mainDep = Guid.Empty;
                if (CurrentUser.GetCurrentUser().LoginName != "sysAdmin")
                {
                    Guid dep = Guid.Parse(CurrentUser.GetCurrentUser().Department);
                    mainDep = dep;
                    var listDep = db.SqlQuery<Guid>("SELECT * FROM dbo.TF_OrganizationFDetail('" + dep + "')");
                    //找到该部门以及其所有子部门
                    if (!string.IsNullOrEmpty(searchParas.Department))
                    {
                        Guid searchDep = Guid.Parse(searchParas.Department);
                        if (!listDep.Contains(searchDep))
                        {
                            searchParas.Department = null;
                        }
                    }
                }
                else
                {
                    mainDep = db.Queryable<Master_Organization>().Where(i => i.ParentVguid == null).Select(i => i.Vguid).SingleOrDefault();
                }
                switch (searchParas.PaymentStatus)
                {
                    case "支付成功":
                        searchParas.PaymentStatus = "1";
                        break;
                    case "支付失败":
                        searchParas.PaymentStatus = "2";
                        break;
                    case "待支付":
                        searchParas.PaymentStatus = "3";
                        break;
                    case "已退款":
                        searchParas.PaymentStatus = "4";
                        break;
                    default:
                        searchParas.PaymentStatus = "1";
                        break;
                }
                var dtPaymentCount = db.GetDataTable("exec usp_Report_PayInformation @name,@phoneNumber,@OwnedFleet,@starDate,@endDate,@Status",
                        new
                        {
                            name = searchParas.Name ?? "",
                            phoneNumber = searchParas.PhoneNumber ?? "",
                            OwnedFleet = string.IsNullOrEmpty(searchParas.Department) ? mainDep.ToString() : searchParas.Department,
                            starDate =
                            searchParas.PayDateFrom == null ? "1900-01-01 00:00:00" : string.Format("{0:yyyy-MM-dd HH:mm:ss}", searchParas.PayDateFrom),
                            endDate = searchParas.PayDateTo == null ? "9999-12-31 23:59:59" : string.Format("{0:yyyy-MM-dd HH:mm:ss}", searchParas.PayDateTo),
                            Status = searchParas.PaymentStatus
                        });
                dtPaymentCount.TableName = "PaymentCount";

                #endregion

                var ds = new DataSet();
                ds.Tables.Add(dtPaymentHistory);
                ds.Tables.Add(dtPaymentCount);
                ExportExcel.ExportExcels("PaymentReport.xlsx", "PaymentReport" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", ds);
                _logLogic.SaveLog(13, 48, CurrentUser.GetCurrentUser().LoginName, "支付报表", Common.Tools.DataTableHelper.Dtb2Json(dtPaymentHistory));
            }
        }

        /// <summary>
        /// 获取月度统计报表
        /// </summary>
        /// <param name="searchParas"></param>
        /// <returns></returns>
        public List<usp_Report_MonthPayInformation> GetMonthlyPayment(U_PaymentHistory_Search searchParas)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var mainDep = Guid.Empty;
                if (CurrentUser.GetCurrentUser().LoginName != "sysAdmin")
                {
                    Guid dep = Guid.Parse(CurrentUser.GetCurrentUser().Department);
                    mainDep = dep;
                    var listDep = db.SqlQuery<Guid>("SELECT * FROM dbo.TF_OrganizationFDetail('" + dep + "')");
                    //找到该部门以及其所有子部门
                    if (!string.IsNullOrEmpty(searchParas.Department))
                    {
                        Guid searchDep = Guid.Parse(searchParas.Department);
                        if (!listDep.Contains(searchDep))
                        {
                            searchParas.Department = null;
                        }
                    }
                }
                else
                {
                    mainDep = db.Queryable<Master_Organization>().Where(i => i.ParentVguid == null).Select(i => i.Vguid).SingleOrDefault();
                }
                switch (searchParas.PaymentStatus)
                {
                    case "支付成功":
                        searchParas.PaymentStatus = "1";
                        break;
                    case "支付失败":
                        searchParas.PaymentStatus = "2";
                        break;
                    case "待支付":
                        searchParas.PaymentStatus = "3";
                        break;
                    case "已退款":
                        searchParas.PaymentStatus = "4";
                        break;
                    default:
                        searchParas.PaymentStatus = "1";
                        break;
                }
                var list = db.SqlQuery<usp_Report_MonthPayInformation>("exec usp_Report_MonthPayInformation @name,@phoneNumber,@OwnedFleet,@starDate,@endDate,@Status", new
                {
                    name = searchParas.Name ?? "",
                    phoneNumber = searchParas.PhoneNumber ?? "",
                    OwnedFleet = string.IsNullOrEmpty(searchParas.Department) ? mainDep.ToString() : searchParas.Department,
                    starDate = searchParas.PayDateFrom == null ? "1900-01-01 00:00:00" : string.Format("{0:yyyy-MM-dd HH:mm:ss}", searchParas.PayDateFrom),
                    endDate = searchParas.PayDateTo == null ? "9999-12-31 23:59:59" : string.Format("{0:yyyy-MM-dd HH:mm:ss}", searchParas.PayDateTo),
                    Status = searchParas.PaymentStatus
                });
                return list;
            }


        }

    }
}
