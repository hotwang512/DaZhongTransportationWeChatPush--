using System;
using System.Data;
using System.Runtime.InteropServices;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;

namespace DaZhongManagementSystem.Infrastructure.QRCodeManagement
{
    public class ScanHistoryServer
    {
        /// <summary>
        /// 日志
        /// </summary>
        public LogLogic _logLogic;
        public ScanHistoryServer()
        {
            _logLogic = new LogLogic();
        }
        /// <summary>
        /// 分页获取扫描历史列表信息
        /// </summary>
        /// <param name="searchParam">搜索条件</param>
        /// <param name="para">分页信息</param>
        /// <returns></returns>
        public JsonResultModel<Business_ScanHistory_Information> GetScanHistoryListBySearch(ScanHistorySearch searchParam, GridParams para)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                JsonResultModel<Business_ScanHistory_Information> jsonResult = new JsonResultModel<Business_ScanHistory_Information>();
                var query = db.Queryable<Business_ScanHistory_Information>();
                if (!string.IsNullOrEmpty(searchParam.MachineCode))
                {
                    query.Where(it => it.MachineCode.Contains(searchParam.MachineCode));
                }
                if (!string.IsNullOrEmpty(searchParam.CreatedDateFrom) && !string.IsNullOrEmpty(searchParam.CreatedDateTo))
                {
                    DateTime createdTimeStart = DateTime.Parse(searchParam.CreatedDateFrom);
                    DateTime createdTimeEnd = DateTime.Parse(searchParam.CreatedDateTo);
                    query.Where(i => i.CreatedDate > createdTimeStart && i.CreatedDate < createdTimeEnd);
                }
                else
                {
                    if (!string.IsNullOrEmpty(searchParam.CreatedDateFrom))
                    {
                        DateTime createdTimeStart = DateTime.Parse(searchParam.CreatedDateFrom);
                        query.Where(i => i.CreatedDate > createdTimeStart);
                    }
                    if (!string.IsNullOrEmpty(searchParam.CreatedDateTo))
                    {
                        DateTime createdTimeEnd = DateTime.Parse(searchParam.CreatedDateTo);
                        query.Where(i => i.CreatedDate < createdTimeEnd);
                    }
                }
                query.OrderBy(para.sortdatafield + " " + para.sortorder);
                jsonResult.TotalRows = query.Count();
                jsonResult.Rows = query.ToPageList(para.pagenum, para.pagesize);
                //存入操作日志表
                string logData = JsonHelper.ModelToJson(jsonResult);
                _logLogic.SaveLog(3, 44, CurrentUser.GetCurrentUser().LoginName, "扫码历史列表", logData);
                return jsonResult;
            }
        }

        /// <summary>
        /// 删除扫码历史
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public bool DeletedScanHistory(Guid vguid)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                try
                {
                    db.BeginTran();
                    var model = db.Queryable<Business_ScanHistory_Information>().Where(i => i.Vguid == vguid).SingleOrDefault();
                    string logData = JsonHelper.ModelToJson(model);
                    db.Delete<Business_ScanHistory_Information>(i => i.Vguid == vguid);     //删除
                    _logLogic.SaveLog(2, 44, Common.CurrentUser.GetCurrentUser().LoginName, "扫码历史", logData);
                    db.CommitTran();
                    return true;
                }
                catch (Exception exp)
                {
                    db.RollbackTran();
                    Common.LogHelper.LogHelper.WriteLog(exp.ToString());
                    _logLogic.SaveLog(5, 44, Common.CurrentUser.GetCurrentUser().LoginName, "", exp.ToString());
                    return false;
                }
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        public void Export(string searchParams)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {

                var model = JsonHelper.JsonToModel<ScanHistorySearch>(searchParams);
                var query = db.Queryable<Business_ScanHistory_Information>();
                if (!string.IsNullOrEmpty(model.MachineCode))
                {
                    query.Where(it => it.MachineCode.Contains(model.MachineCode));
                }
                if (!string.IsNullOrEmpty(model.CreatedDateFrom) && !string.IsNullOrEmpty(model.CreatedDateTo))
                {
                    DateTime createdTimeStart = DateTime.Parse(model.CreatedDateFrom);
                    DateTime createdTimeEnd = DateTime.Parse(model.CreatedDateTo);
                    query.Where(i => i.CreatedDate > createdTimeStart && i.CreatedDate < createdTimeEnd);
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.CreatedDateFrom))
                    {
                        DateTime createdTimeStart = DateTime.Parse(model.CreatedDateFrom);
                        query.Where(i => i.CreatedDate > createdTimeStart);
                    }
                    if (!string.IsNullOrEmpty(model.CreatedDateTo))
                    {
                        DateTime createdTimeEnd = DateTime.Parse(model.CreatedDateTo);
                        query.Where(i => i.CreatedDate < createdTimeEnd);
                    }
                }
                query.OrderBy(it => it.CreatedDate, OrderByType.Desc);
                var dt = query.ToDataTable();
                dt.TableName = "Business_ScanHistory_Information";
                string fileName = SyntacticSugar.ConfigSugar.GetAppString("ScanHistoryTemplate");
                Common.ExportExcel.ExportExcels("ScanHistory.xlsx", fileName, dt);
                _logLogic.SaveLog(13, 44, Common.CurrentUser.GetCurrentUser().LoginName, "ScanHistoryTemplate", Common.Tools.DataTableHelper.Dtb2Json(dt));
            }
        }
    }
}
