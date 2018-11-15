using System;
using System.Collections.Generic;
using Aspose.Pdf.Facades;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;
namespace DaZhongManagementSystem.Infrastructure.DraftManagement
{
    public class AgreementServer
    {
        private readonly LogLogic _ll;
        public AgreementServer()
        {
            _ll = new LogLogic();
        }
        /// <summary>
        /// 获取协议操作历史记录
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<v_Business_ProtocolOperations_Information> GetAgreementOpertaionList(Search_AgreementOperation searchParam, GridParams para)
        {
            var jsonResult = new JsonResultModel<v_Business_ProtocolOperations_Information>();
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var query = db.Queryable<v_Business_ProtocolOperations_Information>();
                if (!string.IsNullOrEmpty(searchParam.Title))
                {
                    query.Where(i => i.Title.Contains(searchParam.Title));
                }
                if (!string.IsNullOrEmpty(searchParam.Result))
                {
                    var listArr = searchParam.Result.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    query.In(i => i.Result, listArr);
                }
                if (searchParam.OperationTimeFrom != null && searchParam.OperationTimeTo == null)
                {
                    query.Where(i => i.OperationTime >= searchParam.OperationTimeFrom);
                }
                if (searchParam.OperationTimeTo != null && searchParam.OperationTimeFrom == null)
                {
                    query.Where(i => i.OperationTime <= searchParam.OperationTimeTo);
                }
                if (searchParam.OperationTimeTo != null && searchParam.OperationTimeFrom != null)
                {
                    query.Where(i => i.OperationTime >= searchParam.OperationTimeFrom && i.OperationTime <= searchParam.OperationTimeTo);
                }
                if (!string.IsNullOrEmpty(searchParam.Name))
                {
                    query.Where(i => i.Name.Contains(searchParam.Name));
                }
                if (!string.IsNullOrEmpty(searchParam.PhoneNumber))
                {
                    query.Where(i => i.PhoneNumber.Contains(searchParam.PhoneNumber));
                }
                query.OrderBy(i => i.CreatedDate, OrderByType.Desc);
                int pageCount = 0;
                jsonResult.Rows = query.Select("Title,Result,OperationTime,Name,PhoneNumber,Vguid").ToPageList(para.pagenum, para.pagesize, ref pageCount);
                jsonResult.TotalRows = pageCount;
                //存入操作日志表
                string logData = JsonConverter.Serialize(jsonResult);
                _ll.SaveLog(3, 49, CurrentUser.GetCurrentUser().LoginName, "协议操作历史列表", logData);
                return jsonResult;
            }
        }

        /// <summary>
        /// 获取协议的详细信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public v_Business_ProtocolOperations_Information GetAgreementDetailByVguid(Guid vguid)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var model = db.Queryable<v_Business_ProtocolOperations_Information>().Where(i => i.Vguid == vguid).Select("Title,Message").SingleOrDefault();
                //存入操作日志表
                string logData = JsonConverter.Serialize(model);
                _ll.SaveLog(3, 50, CurrentUser.GetCurrentUser().LoginName, "协议操作历史详细界面", logData);
                return model;
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="searchParam"></param>
        public void Export(Search_AgreementOperation searchParam)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var query = db.Queryable<v_Business_ProtocolOperations_Information>();
                if (!string.IsNullOrEmpty(searchParam.Title))
                {
                    query.Where(i => i.Title.Contains(searchParam.Title));
                }
                if (!string.IsNullOrEmpty(searchParam.Result))
                {
                    var listArr = searchParam.Result.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    query.In(i => i.Result, listArr);
                }
                if (searchParam.OperationTimeFrom != null && searchParam.OperationTimeTo == null)
                {
                    query.Where(i => i.OperationTime >= searchParam.OperationTimeFrom);
                }
                if (searchParam.OperationTimeTo != null && searchParam.OperationTimeFrom == null)
                {
                    query.Where(i => i.OperationTime <= searchParam.OperationTimeTo);
                }
                if (searchParam.OperationTimeTo != null && searchParam.OperationTimeFrom != null)
                {
                    query.Where(i => i.OperationTime >= searchParam.OperationTimeFrom && i.OperationTime <= searchParam.OperationTimeTo);
                }
                if (!string.IsNullOrEmpty(searchParam.Name))
                {
                    query.Where(i => i.Name.Contains(searchParam.Name));
                }
                if (!string.IsNullOrEmpty(searchParam.PhoneNumber))
                {
                    query.Where(i => i.PhoneNumber.Contains(searchParam.PhoneNumber));
                }
                query.OrderBy(i => i.CreatedDate, OrderByType.Desc);
                var dt = query.Select("Title,Result,OperationTime,Name,PhoneNumber,Vguid").ToDataTable();
                dt.TableName = "tb";
                //存入操作日志表
                string logData = JsonConverter.DataTableToJson(dt);
                _ll.SaveLog(13, 49, CurrentUser.GetCurrentUser().LoginName, "协议操作历史列表", logData);
                ExportExcel.ExportExcels("Agreement.xlsx", "协议操作历史.xls", dt);



            }
        }

        /// <summary>
        /// 获取协议类型
        /// </summary>
        /// <returns></returns>
        public List<Business_ProtocolOperations_Information> GetAgreementTypeList()
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                return db.Queryable<Business_ProtocolOperations_Information>().GroupBy(i => i.Result).Select("Result").ToList();

            }
        }


    }
}
