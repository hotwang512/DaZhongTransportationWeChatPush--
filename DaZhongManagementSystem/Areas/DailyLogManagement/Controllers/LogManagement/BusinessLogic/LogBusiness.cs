using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.DailyLogManagement.Controllers.LogManagement.BusinessLogic
{
    public class LogBusiness
    {
        public LogServer _ls;
        public LogBusiness()
        {
            _ls = new LogServer();
        }

        /// <summary>
        /// 获取事件类型列表
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetEventTypeList()
        {
            return _ls.GetEventTypeList();
        }

        /// <summary>
        /// 获取日志详细信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public Business_OperationLog GetDailyDetail(string vguid)
        {
            return _ls.GetDailyDetail(vguid);
        }

        /// <summary>
        /// 通过查询条件获取日志信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<Business_OperationLog> GetLogListBySearch(SearchLogList searchParam, GridParams para)
        {
            return _ls.GetLogListBySearch(searchParam, para);
        }
    }
}