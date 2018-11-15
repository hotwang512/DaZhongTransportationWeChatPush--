using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.SystemManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.Systemmanagement.Controllers.ConfigManagement.BusinessLogic
{
    public class ConfigManagementLogic
    {
        public ConfigManageServer _configManageServer;
        public ConfigManagementLogic()
        {
            _configManageServer = new ConfigManageServer();
        }

        /// <summary>
        /// 获取配置列表数据
        /// </summary>
        /// <returns></returns>
        public List<Master_Configuration> GetConfigList()
        {
            return _configManageServer.GetConfigList();
        }

        /// <summary>
        /// 保存配置文件内容
        /// </summary>
        /// <param name="configList">可配置文件列表</param>
        /// <returns></returns>
        public bool SaveConfig(List<Master_Configuration> configList)
        {
            bool result = false;
            foreach (var item in configList)
            {
                result = _configManageServer.SaveConfig(item);
                if (!result)
                {
                    return result;
                }
            }
            return result;
        }
    }
}