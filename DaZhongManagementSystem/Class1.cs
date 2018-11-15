using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceProcess;
using System.Diagnostics;

namespace DaZhongManagementSystem
{
    public class Class1
    {
        /// <summary>
        /// 安装服务
        /// </summary>
        public void InstallService()
        {
            string CurrentDirectory = System.Environment.CurrentDirectory;
            System.Environment.CurrentDirectory = CurrentDirectory + "\\Service";
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = "Install.bat";
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            System.Environment.CurrentDirectory = CurrentDirectory;
        }

        /// <summary>
        /// 卸载服务
        /// </summary>
        public void UinstallService()
        {
            string CurrentDirectory = System.Environment.CurrentDirectory;
            System.Environment.CurrentDirectory = CurrentDirectory + "\\Service";
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = "Uninstall.bat";
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            System.Environment.CurrentDirectory = CurrentDirectory;
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <returns></returns>
        public void StartService()
        {
            ServiceController serviceController = new ServiceController("DaZhongWindowsService");
            serviceController.Start();
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void StopService()
        {
            ServiceController serviceController = new ServiceController("DaZhongWindowsService");
            if (serviceController.CanStop)
            {
                serviceController.Stop();
            }
        }

        /// <summary>
        /// 检查状态
        /// </summary>
        /// <returns></returns>
        public string CheckStatus()
        {
            string status = "";
            ServiceController serviceController = new ServiceController("DaZhongWindowsService");
            status = serviceController.Status.ToString();
            return status;
        }
    }
}