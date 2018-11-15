using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.ReportManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.ReportManagement.Controllers.PushMsgReport.BusinessLogic
{
    public class PushMsgReportLogic
    {
        public PushMsgReportServer _pushMsgReportServer;
        public PushMsgReportLogic()
        {

            _pushMsgReportServer = new PushMsgReportServer();
        }

        /// <summary>
        /// 获取推送列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<U_WeChatPushRate> GetPushMsgReportList(string pushMsgName)
        {
            return _pushMsgReportServer.GetSysUserListBySearch(pushMsgName);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="pushMsgName"></param>
        public void Export(string pushMsgName)
        {
            _pushMsgReportServer.Export(pushMsgName);
        }
    }
}