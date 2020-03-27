using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                _dbMsSql.Update<Business_HomecomingSurvey>(new { LicensePlate = hs.LicensePlate, WhetherReturnHome = hs.WhetherReturnHome, StartDate = hs.StartDate, EndDate = hs.EndDate }, c => c.Vguid == hs.Vguid);
                _dbMsSql.Update(hs);
            }
        }

        public List<ReturnHomeStatistics> ReturnHomeStatistics()
        {
            List<ReturnHomeStatistics> rhsList = new List<ReturnHomeStatistics>();
            string sql = @"select OrganizationName,COUNT(NoReturnHome),COUNT(ReturnHome) from(
                          select o.OrganizationName,p.OwnedFleet, 
                          case h.WhetherReturnHome when '1' then '1' end as 'NoReturnHome', 
                          case h.WhetherReturnHome when '0' then '0' end as 'ReturnHome'
                          from[dbo].[Business_HomecomingSurvey] h
                          left join Business_Personnel_Information p on h.CreatedUser = p.UserID
                          left join[Master_Organization] o on p.OwnedFleet = o.Vguid
                          )a
                          group by OwnedFleet, OrganizationName order by OrganizationName";
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



    }
}
