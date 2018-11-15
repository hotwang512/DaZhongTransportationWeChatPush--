using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Infrastructure.DailyLogManagement
{
    public class LogServer
    {
        /// <summary>
        /// 获取事件类型列表
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetEventTypeList()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                List<CS_Master_2> eventTypeList = new List<CS_Master_2>();
                Guid eventTypeVguid = Guid.Parse(Common.Tools.MasterVGUID.EventType);
                eventTypeList = _dbMsSql.Queryable<CS_Master_2>().Where(i => i.VGUID == eventTypeVguid && i.Visible == "1").ToList();

                return eventTypeList;
            }
        }

        /// <summary>
        /// 通过查询条件获取日志信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<Business_OperationLog> GetLogListBySearch(SearchLogList searchParam, GridParams para)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                JsonResultModel<Business_OperationLog> jsonResult = new JsonResultModel<Business_OperationLog>();
                var listUser = _dbMsSql.Queryable<Sys_User>().Select(i => i.LoginName).ToList();
                var query = _dbMsSql.Queryable<Business_OperationLog>().Where(i => listUser.Contains(i.CreatedUser));

                if (!string.IsNullOrEmpty(searchParam.EventType))
                {
                    query.Where(c => c.EventType != "查询" && c.EventType != "错误异常" && c.EventType.Contains(searchParam.EventType));//事件类型
                }
                else
                {
                    query.Where(c => c.EventType != "查询" && c.EventType != "错误异常");
                }
                if (!string.IsNullOrEmpty(searchParam.LogUser))
                {
                    query.Where(c => c.LogUser.Contains(searchParam.LogUser));//用户
                }
                if (searchParam.BeginDate.ToString() != "0001/1/1 0:00:00" && searchParam.EndDate.ToString() != "0001/1/1 0:00:00")
                {
                    query.Where(c => c.CreatedDate > searchParam.BeginDate && c.CreatedDate < searchParam.EndDate);
                }
                else
                {
                    if (searchParam.BeginDate.ToString() != "0001/1/1 0:00:00")
                    {
                        query.Where(c => c.CreatedDate > searchParam.BeginDate);
                    }
                    if (searchParam.EndDate.ToString() != "0001/1/1 0:00:00")
                    {
                        query.Where(c => c.CreatedDate < searchParam.EndDate);
                    }
                }
                query.OrderBy(para.sortdatafield + " " + para.sortorder);
                jsonResult.TotalRows = query.Count();
                jsonResult.Rows = query.ToPageList(para.pagenum, para.pagesize);

                //存入操作日志表
                string logData = JsonHelper.ModelToJson<JsonResultModel<Business_OperationLog>>(jsonResult);

                return jsonResult;
            }
        }

        /// <summary>
        /// 获取日志详细信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public Business_OperationLog GetDailyDetail(string vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                Business_OperationLog operationLog = new Business_OperationLog();
                Guid logVguid = Guid.Parse(vguid);
                operationLog = _dbMsSql.Queryable<Business_OperationLog>().Where(i => i.Vguid == logVguid).SingleOrDefault();
                return operationLog;
            }
        }

        /// <summary>
        /// 保存系统操作日志
        /// </summary>
        /// <param name="operationLogModel"></param>
        /// <returns></returns>
        public void SaveLog(Business_OperationLog operationLogModel)
        {
            using (SqlSugar.SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                try
                {
                    result = _dbMsSql.Insert<Business_OperationLog>(operationLogModel, false) != DBNull.Value;
                }
                catch (Exception ex)
                {
                    Common.LogHelper.LogHelper.WriteLog(ex.Message + "/n" + ex.ToString() + "/n" + ex.StackTrace);
                }
            }
        }
    }
}
