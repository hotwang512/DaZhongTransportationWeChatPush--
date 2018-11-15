using DaZhongManagementSystem.Entities.TableEntity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Infrastructure
{
    public class RevenueServer
    {
        /// <summary>
        /// 获取成功查询营收数据回复
        /// </summary>
        /// <returns></returns>
        public string GetRevenueQueryReply()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                string revenueQueryReply = string.Empty;
                revenueQueryReply = _dbMsSql.Queryable<Master_Configuration>().Where(i => i.ID == 5).SingleOrDefault().ConfigValue;

                return revenueQueryReply;
            }
        }

        /// <summary>
        /// 获取不是司机查询营收数据回复
        /// </summary>
        /// <returns></returns>
        public string GetRevenueQueryRefuse()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                string revenueQueryRefuse = string.Empty;
                revenueQueryRefuse = _dbMsSql.Queryable<Master_Configuration>().Where(i => i.ID == 3).SingleOrDefault().ConfigValue;

                return revenueQueryRefuse;
            }
        }

        /// <summary>
        /// 获取不是司机查询营收数据回复
        /// </summary>
        /// <returns></returns>
        public string GetRevenueQueryTimesRefuse()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                string revenueQueryRefuse = string.Empty;
                revenueQueryRefuse = _dbMsSql.Queryable<Master_Configuration>().Where(i => i.ID == 4).SingleOrDefault().ConfigValue;

                return revenueQueryRefuse;
            }
        }

        /// <summary>
        /// 获取营收信息每月查询次数
        /// </summary>
        /// <returns></returns>
        public string GetRevenueSearchTimes()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                string revenueSearchTimes = string.Empty;
                revenueSearchTimes = _dbMsSql.Queryable<Master_Configuration>().Where(i => i.ID == 6).SingleOrDefault().ConfigValue;

                return revenueSearchTimes;
            }
        }

        /// <summary>
        /// 获取当前用户当月查询营收次数
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public int GetUserCurrentMonthQueryTimes(string userID)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                int queryTimes = 0;
                DateTime now = DateTime.Now;
                DateTime currentMonth = new DateTime(now.Year, now.Month, 1);
                queryTimes = _dbMsSql.Queryable<Business_WeChatPushDetail_Information>().Where(i => i.Type.Contains("营收") && i.PushObject == userID && i.CreatedDate > currentMonth && i.CreatedDate < now).ToList().Count;

                return queryTimes;
            }
        }
    }
}
