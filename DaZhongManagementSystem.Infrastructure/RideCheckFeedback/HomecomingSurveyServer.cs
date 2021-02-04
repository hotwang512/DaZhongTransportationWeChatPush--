using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Common.LogHelper;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;

namespace DaZhongManagementSystem.Infrastructure.RideCheckFeedback
{
    public class HomecomingSurveyServer
    {
        /// <summary>
        /// 日志
        /// </summary>
        public LogLogic _logLogic;
        public HomecomingSurveyServer()
        {
            _logLogic = new LogLogic();
        }

        public Business_HomecomingSurvey GetHomecomingSurvey(string user, string year)
        {
            Business_HomecomingSurvey hs = null;
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                hs = _dbMsSql.Queryable<Business_HomecomingSurvey>().Where(c => c.CreatedUser == user & c.Year == year).FirstOrDefault();
            }
            return hs;
        }

        public void AddHomecomingSurvey(Business_HomecomingSurvey hs)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                _dbMsSql.Insert(hs);
            }
        }

        public void UpdateHomecomingSurvey(Business_HomecomingSurvey hs)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                _dbMsSql.Update<Business_HomecomingSurvey>(
                    new
                    {
                        LicensePlate = hs.LicensePlate,
                        WhetherReturnHome = hs.WhetherReturnHome,
                        StartDate = hs.StartDate,
                        EndDate = hs.EndDate,
                        ChangeDate = hs.ChangeDate,
                        ChangeUser = hs.CreatedUser,
                        CheckDrivingG = hs.CheckDrivingG,
                        CheckDrivingB = hs.CheckDrivingB,
                        BackCarNo = hs.BackCarNo,
                        BackAdress = hs.BackAdress,
                        GoCarNo = hs.GoCarNo,
                        OrganizationName = hs.OrganizationName,
                        Fleet = hs.Fleet,
                        CheckDrivingGR = hs.CheckDrivingGR,
                        CheckDrivingBR = hs.CheckDrivingBR
                    },
                    c => c.Vguid == hs.Vguid);
            }
        }

        public List<ReturnHomeStatistics> ReturnHomeStatistics(string year, string dept)
        {
            List<ReturnHomeStatistics> rhsList = new List<ReturnHomeStatistics>();
            if (string.IsNullOrEmpty(dept))
            {
                dept = CurrentUser.GetCurrentUser().Department;
            }
            string sql = string.Format(@"usp_HomecomingSurvey_Total '{0}','{1}'", year, dept);
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                try
                {
                    rhsList = dbMsSql.SqlQuery<ReturnHomeStatistics>(sql);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.Message);
                }
            }
            return rhsList;
        }


        public DataTable ExportReturnHomeStatistics(string year, string dept)
        {
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty(dept))
            {
                dept = CurrentUser.GetCurrentUser().Department;
            }
            //查统计数据
            //string sql = string.Format(@"usp_HomecomingSurvey_Total '{0}','{1}'", year, dept);
            //查明细数据
            string sql = string.Format(@"usp_HomecomingSurvey_ExportTotal '{0}','{1}'", year, dept);
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                try
                {
                    dt = dbMsSql.GetDataTable(sql);
                    dt.TableName = "table";
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.Message);
                }
            }
            return dt;
        }


    }
}
