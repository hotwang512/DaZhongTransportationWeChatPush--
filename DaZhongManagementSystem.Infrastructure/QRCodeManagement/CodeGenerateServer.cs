using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Common.LogHelper;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;
using SyntacticSugar;

namespace DaZhongManagementSystem.Infrastructure.QRCodeManagement
{
    public class CodeGenerateServer
    {
        private LogLogic _logLogic;
        public CodeGenerateServer()
        {
            _logLogic = new LogLogic();
        }
        /// <summary>
        /// 获取二维码的配置(系统)
        /// </summary>
        /// <returns></returns>
        public List<Master_Configuration> GetSysConfigurations()
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var configurations = db.Queryable<Master_Configuration>().Where(i => i.CreateUser == "QRCode" && i.ModifyUser == null).OrderBy(i => i.ID).ToList();
                var logData = JsonHelper.ModelToJson(configurations);
                _logLogic.SaveLog(3, 35, CurrentUser.GetCurrentUser().LoginName, "二维码的配置", logData);
                return configurations;
            }
        }

        /// <summary>
        /// 获取二维码的配置（参数）
        /// </summary>
        /// <returns></returns>
        public List<Master_Configuration> GetConfigurations(string sysConfigId)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var configurations = db.Queryable<Master_Configuration>().Where(i => i.CreateUser == "QRCode" && i.ModifyUser == sysConfigId).OrderBy(i => i.ID).ToList();
                var logData = JsonHelper.ModelToJson(configurations);
                _logLogic.SaveLog(3, 35, CurrentUser.GetCurrentUser().LoginName, "二维码的配置", logData);
                return configurations;
            }
        }
        /// <summary>
        /// 保存生成二维码的配置信息
        /// </summary>
        /// <param name="configurations"></param>
        /// <returns></returns>
        public bool SaveQRCodeConfig(List<Master_Configuration> configurations)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                try
                {
                    foreach (var configuration in configurations)
                    {
                        var configModel = db.Queryable<Master_Configuration>().Where(i => i.ID == configuration.ID).SingleOrDefault();
                        if (configModel != null)
                        {
                            configModel.ConfigDescription = configuration.ConfigDescription;
                            configModel.ConfigValue = configuration.ConfigValue;
                            db.Update(configModel);
                        }
                        else
                        {
                            db.Insert(configuration, false);
                        }
                    }
                    var logData = JsonHelper.ModelToJson(configurations);
                    _logLogic.SaveLog(1, 35, CurrentUser.GetCurrentUser().LoginName, "二维码的配置", logData);
                    return true;
                }
                catch (Exception ex)
                {
                    _logLogic.SaveLog(5, 35, CurrentUser.GetCurrentUser().LoginName, "二维码的配置", ex.Message);
                    LogHelper.WriteLog("保存生成二维码的配置信息:" + ex.ToString());
                    return false;
                }
            }
        }

        /// <summary>
        /// 删除二维码的配置信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteQRCodeConfig(List<int> ids)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                try
                {
                    var sysConfigs = db.Queryable<Master_Configuration>().In(i => i.ID, ids);

                    var configs = db.Queryable<Master_Configuration>().In(i => i.ModifyUser, sysConfigs.Select(i => i.ID).ToList()).Select(i => i.ID).ToList();
                    var logData = JsonHelper.ModelToJson(sysConfigs.ToList());
                    _logLogic.SaveLog(2, 35, CurrentUser.GetCurrentUser().LoginName, "二维码的配置", logData);
                    db.Delete<Master_Configuration, int>(i => i.ID, ids);
                    db.Delete<Master_Configuration, int>(i => i.ID, configs);
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.ToString());
                    return false;
                }

            }
        }

        /// <summary>
        /// 根据配置的系统参数结合具体人员形成最后的url地址加参数的形式
        /// </summary>
        /// <param name="personnelInfo"></param>
        /// <returns></returns>
        public string GetSysConfiguration(Business_Personnel_Information personnelInfo)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var configurations = db.Queryable<Master_Configuration>().Where(i => i.CreateUser == "QRCode" && i.ModifyUser == null).OrderBy(i => i.ID).ToList();
                string symbol = ConfigSugar.GetAppString("Symbol") ?? "^";
                var configParas = configurations.Select(configuration => configuration.ConfigValue + symbol + configuration.ConfigDescription.Trim() + "?" + GetPersonConfiguration(personnelInfo, configuration.ID.ToString())).ToList();
                return string.Join("|", configParas);
            }

        }

        /// <summary>
        /// 根据具体人员替换二维码生成配置中具体的参数
        /// </summary>
        /// <param name="personnelInfo"></param>
        /// <returns></returns>
        public string GetPersonConfiguration(Business_Personnel_Information personnelInfo, string sysConfigId)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var configurations = db.Queryable<Master_Configuration>().Where(i => i.CreateUser == "QRCode" && i.ModifyUser == sysConfigId).OrderBy(i => i.ID).ToList();
                var configParas = new List<string>();
                foreach (var configuration in configurations)
                {
                    string configStr = string.Empty;
                    Match match = Regex.Match(configuration.ConfigDescription, "{DB:(.+)}");
                    if (match.Groups[1].Value == "")
                    {
                        configStr = string.Format("{0}={1}", configuration.ConfigValue, configuration.ConfigDescription);
                    }
                    else
                    {
                        var configParam = db.SqlQuery<string>(string.Format("select {0} from v_Business_PersonnelDepartmentDetail_Information where Vguid=@vguid", match.Groups[1].Value), new { vguid = personnelInfo.Vguid }).Single();
                        configStr = string.Format("{0}={1}", configuration.ConfigValue, configParam);
                    }
                    configParas.Add(configStr);
                }
                return string.Join("&", configParas);
            }
        }
    }
}