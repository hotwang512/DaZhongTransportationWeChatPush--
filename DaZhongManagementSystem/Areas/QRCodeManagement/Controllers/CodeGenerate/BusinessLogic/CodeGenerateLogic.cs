using System;
using System.Collections.Generic;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.QRCodeManagement;

namespace DaZhongManagementSystem.Areas.QRCodeManagement.Controllers.CodeGenerate.BusinessLogic
{
    public class CodeGenerateLogic
    {
        private readonly CodeGenerateServer _codeGenerateServer;
        public CodeGenerateLogic()
        {
            _codeGenerateServer = new CodeGenerateServer();
        }

        /// <summary>
        /// 获取二维码的配置(系统)
        /// </summary>
        /// <returns></returns>
        public List<Master_Configuration> GetSysConfigurations()
        {
            return _codeGenerateServer.GetSysConfigurations();
        }

        /// <summary>
        /// 获取二维码的配置（参数）
        /// </summary>
        /// <returns></returns>
        public List<Master_Configuration> GetConfigurations(string sysConfigId)
        {
            return _codeGenerateServer.GetConfigurations(sysConfigId);
        }

        /// <summary>
        /// 保存生成二维码配置信息
        /// </summary>
        /// <param name="configStr">参数配置</param>
        ///  <param name="sysConfigStr">系统配置</param>
        public bool SaveQRCodeConfig(string configStr, string sysConfigStr)
        {
            var configurations = JsonHelper.JsonToModel<List<Master_Configuration>>(configStr);
            var sysConfigurations = JsonHelper.JsonToModel<List<Master_Configuration>>(sysConfigStr);
            configurations.AddRange(sysConfigurations);
            foreach (var configuration in configurations)
            {
                configuration.CreateUser = "QRCode";
                configuration.CreateDate = DateTime.Now;
                configuration.VGUID = Guid.NewGuid();
            }
            return _codeGenerateServer.SaveQRCodeConfig(configurations);
        }
        /// <summary>
        /// 删除二维码的配置信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteQRCodeConfig(List<int> ids)
        {
            return _codeGenerateServer.DeleteQRCodeConfig(ids);
        }

        /// <summary>
        /// 根据具体人员替换二维码生成配置中具体的参数
        /// </summary>
        /// <param name="personnelInfo"></param>
        /// <returns></returns>
        public string GetPersonConfiguration(Business_Personnel_Information personnelInfo)
        {
            return _codeGenerateServer.GetSysConfiguration(personnelInfo);
        }
    }
}