using System;
using System.Collections.Generic;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;

namespace DaZhongManagementSystem.Infrastructure.KnowledgeBaseManagement
{
    public class FormalServer
    {
        /// <summary>
        /// 日志
        /// </summary>
        public LogLogic _ll;

        public FormalServer()
        {
            _ll = new LogLogic();
        }


        /// <summary>
        /// 获取正式知识库的列表信息
        /// </summary>
        /// <param name="searchParam">搜索条件</param>
        /// <param name="para">分页信息</param>
        /// <returns></returns>
        public JsonResultModel<V_Business_KnowledgeBase_Information> GetKnowledgeListBySearch(Business_KnowledgeBase_Information searchParam, GridParams para)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                JsonResultModel<V_Business_KnowledgeBase_Information> jsonResult = new JsonResultModel<V_Business_KnowledgeBase_Information>();
                var query = db.Queryable<V_Business_KnowledgeBase_Information>().Where(i => i.Status == "2");
                if (!string.IsNullOrEmpty(searchParam.Title))
                {
                    query.Where(i => i.Title.Contains(searchParam.Title));
                }
                if (!string.IsNullOrEmpty(searchParam.Remark))
                {
                    query.Where(i => i.Remark.Contains(searchParam.Remark));
                }
                if (!string.IsNullOrEmpty(searchParam.Type))
                {
                    query.Where(i => i.Type.Contains(searchParam.Type));
                }
                if (searchParam.CreatedDate != DateTime.MinValue)
                {
                    query.Where(i => i.CreatedDate >= searchParam.CreatedDate);
                }
                query.OrderBy(para.sortdatafield + " " + para.sortorder);
                jsonResult.TotalRows = query.Count();
                jsonResult.Rows = query.ToPageList(para.pagenum, para.pagesize);
                //存入操作日志表
                string logData = JsonHelper.ModelToJson(jsonResult);
                _ll.SaveLog(3, 43, CurrentUser.GetCurrentUser().LoginName, "正式列表", logData);
                return jsonResult;
            }
        }

        /// <summary>
        /// 获取正式知识库的列表信息
        /// </summary> 
        /// <param name="pageIndex">当前页</param>
        /// <param name="personVguid">浏览人主键</param>
        /// <returns></returns>
        public List<V_Business_KnowledgeBase_Information> GetKnowledgeList(int pageIndex, Guid personVguid)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                Business_Personnel_Information personInfo = new Business_Personnel_Information();
                personInfo = db.Queryable<Business_Personnel_Information>().Where(i => i.Vguid == personVguid).SingleOrDefault();
                var query = db.Queryable<V_Business_KnowledgeBase_Information>().Where(i => i.Status == "2").Select("Title,CreatedDate,Vguid");
                query.OrderBy("CreatedDate desc");
                var list = query.ToPageList(pageIndex, 10);
                //存入操作日志表
                string logData = JsonHelper.ModelToJson(list);
                _ll.SaveLog(3, 43, personInfo.ID + personInfo.Name, "正式列表", logData);
                return list;
            }
        }

        /// <summary>
        /// 获取知识库的详细信息
        /// </summary>
        /// <param name="vguid">主键</param>
        /// <param name="personVguid">人员主键</param>
        /// <returns></returns>
        public V_Business_KnowledgeBase_Information GetKnowledgeDetail(string vguid, Guid personVguid)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                Guid guid = Guid.Parse(vguid);
                var personInfo = db.Queryable<Business_Personnel_Information>().Where(i => i.Vguid == personVguid).SingleOrDefault();
                var model = db.Queryable<V_Business_KnowledgeBase_Information>().Where(i => i.Vguid == guid).SingleOrDefault();
                //存入操作日志表
                string logData = JsonHelper.ModelToJson(model);
                _ll.SaveLog(3, 43, personInfo.ID + personInfo.Name, "正式列表", logData);
                return model;
            }
        }

    }
}
