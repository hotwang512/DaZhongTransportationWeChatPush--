using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DaZhongManagementSystem.Infrastructure.ReportManagement
{
    public class PushMsgReportServer
    {
        public LogLogic _logLogic;
        public PushMsgReportServer()
        {
            _logLogic = new LogLogic();
        }

        /// <summary>
        /// 获取推送列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<U_WeChatPushRate> GetSysUserListBySearch(string pushMsgName)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                List<U_WeChatPushRate> weChatPushRateList = new List<U_WeChatPushRate>();
                string sql = string.Format(@"EXEC usp_WeChatPushRate @Title,@userVguid");
                DataTable dt = new DataTable();
                dt = _dbMsSql.GetDataTable(sql, new
                {
                    Title = pushMsgName,
                    userVguid=CurrentUser.GetCurrentUser().Vguid
                });
                weChatPushRateList = GetPushMsgRateList(dt);

                return weChatPushRateList;
            }
        }

        /// <summary>
        ///将datatable转成list
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<U_WeChatPushRate> GetPushMsgRateList(DataTable table)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                List<U_WeChatPushRate> list = new List<U_WeChatPushRate>();
                U_WeChatPushRate t = default(U_WeChatPushRate);
                PropertyInfo[] propertypes = null;
                string tempName = string.Empty;
                foreach (DataRow row in table.Rows)
                {
                    t = Activator.CreateInstance<U_WeChatPushRate>();
                    propertypes = t.GetType().GetProperties();
                    foreach (PropertyInfo pro in propertypes)
                    {
                        tempName = pro.Name;
                        if (table.Columns.Contains(tempName))
                        {
                            object value = row[tempName];
                            if (value != DBNull.Value)
                            {
                                pro.SetValue(t, value, null);
                            }
                        }
                    }
                    list.Add(t);
                }
                return list;
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="pushMsgName"></param>
        public void Export(string pushMsgName)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                string sql = string.Format(@"EXEC usp_WeChatPushRate @Title,@userVguid");
                DataTable dt = new DataTable();
                dt = _dbMsSql.GetDataTable(sql, new
                {
                    Title = pushMsgName,
                    userVguid = CurrentUser.GetCurrentUser().Vguid
                });
                dt.TableName = "table";
                string amountFileName = SyntacticSugar.ConfigSugar.GetAppString("PushTemplate");
                Common.ExportExcel.ExportExcels("PushTemplate.xlsx", amountFileName, dt);

                _logLogic.SaveLog(13, 23, Common.CurrentUser.GetCurrentUser().LoginName, "PushTemplate", Common.Tools.DataTableHelper.Dtb2Json(dt));
            }
        }
    }
}
