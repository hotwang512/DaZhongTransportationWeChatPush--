using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace DaZhongManagementSystem.Infrastructure.SystemManagement
{
    public class ConfigManageServer
    {
        public LogLogic _logLogic;
        public ConfigManageServer()
        {
            _logLogic = new LogLogic();
        }

        /// <summary>
        /// 获取配置列表数据
        /// </summary>
        /// <returns></returns>
        public List<Master_Configuration> GetConfigList()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                List<Master_Configuration> configList = new List<Master_Configuration>();
                configList = _dbMsSql.Queryable<Master_Configuration>().Where(i => i.CreateUser != "QRCode").OrderBy(i=>i.ID).ToList();

                return configList;
            }
        }

        /// <summary>
        /// 保存配置文件内容
        /// </summary>
        /// <param name="configModel">配置表实体</param>
        /// <returns></returns>
        public bool SaveConfig(Master_Configuration configModel)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                try
                {
                    result = _dbMsSql.Update<Master_Configuration>(new { ConfigValue = configModel.ConfigValue }, i => i.ID == configModel.ID);

                    //写入日志
                    Master_Configuration configuration = new Master_Configuration();
                    configuration = _dbMsSql.Queryable<Master_Configuration>().Where(i => i.ID == configModel.ID).SingleOrDefault();
                    string logData = Common.JsonHelper.ModelToJson(configuration);
                    _logLogic.SaveLog(4, 35, Common.CurrentUser.GetCurrentUser().LoginName, configuration.ConfigDescription, logData);
                }
                catch (Exception exp)
                {
                    Common.LogHelper.LogHelper.WriteLog(exp.Message + "/n" + exp.ToString() + "/n" + exp.StackTrace);
                }
                return result;
            }
        }
    }
}
