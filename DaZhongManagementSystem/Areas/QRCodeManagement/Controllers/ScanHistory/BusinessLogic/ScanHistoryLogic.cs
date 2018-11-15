using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.QRCodeManagement;

namespace DaZhongManagementSystem.Areas.QRCodeManagement.Controllers.ScanHistory.BusinessLogic
{
    public class ScanHistoryLogic
    {
        private ScanHistoryServer _scanHistoryServer;
        public ScanHistoryLogic()
        {
            _scanHistoryServer = new ScanHistoryServer();
        }

        /// <summary>
        /// 分页获取扫描历史列表信息
        /// </summary>
        /// <param name="searchParam">搜索条件</param>
        /// <param name="para">分页信息</param>
        /// <returns></returns>
        public JsonResultModel<Business_ScanHistory_Information> GetScanHistoryListBySearch(ScanHistorySearch searchParam, GridParams para)
        {
            return _scanHistoryServer.GetScanHistoryListBySearch(searchParam, para);
        }

        /// <summary>
        /// 批量删除扫码历史
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public bool DeletedScanHistory(Guid[] vguidList)
        {
            bool result = false;
            foreach (var item in vguidList)
            {
                result = _scanHistoryServer.DeletedScanHistory(item);
            }
            return result;
        }

        /// <summary>
        /// 导出
        /// </summary>
        public void Export(string searchParams)
        {
            _scanHistoryServer.Export(searchParams);
        }
    }
}