using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Infrastructure.DailyLogManagement
{
    public class LogLogic
    {
        public LogServer _ls;
        public LogLogic()
        {
            _ls = new LogServer();
        }

        /// <summary>
        /// 保存系统操作日志
        /// </summary>
        /// <param name="operationLogModel"></param>
        /// <returns></returns>
        public void SaveLog(int eventType, int page, string user, string logmessage, string logData)
        {
            Business_OperationLog operationLog = new Business_OperationLog();
            operationLog.EventType = ((LogEnum)eventType).ToString();
            operationLog.Page = ((PageEnum)page).ToString();
            operationLog.LogMessage = user + "-在" + ((PageEnum)page).ToString() + "-执行" + ((LogEnum)eventType).ToString() + "-" + logmessage + "-" + (LogEnum)12;
            operationLog.LogData = logData;
            operationLog.CreatedDate = DateTime.Now;
            operationLog.CreatedUser = user;
            operationLog.ChangeDate = DateTime.Now;
            operationLog.Vguid = Guid.NewGuid();
            _ls.SaveLog(operationLog);
        }
    }
}
