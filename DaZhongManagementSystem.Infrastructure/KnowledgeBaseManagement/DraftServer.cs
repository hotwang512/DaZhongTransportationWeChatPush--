using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DaZhongManagementSystem.Infrastructure.KnowledgeBaseManagement
{
    public class DraftServer
    {
        /// <summary>
        /// 日志
        /// </summary>
        public LogLogic _ll;

        public DraftServer()
        {
            _ll = new LogLogic();
        }

        /// <summary>
        /// 获取草稿知识库的列表信息
        /// </summary>
        /// <param name="searchParam">搜索条件</param>
        /// <param name="para">分页信息</param>
        /// <returns></returns>
        public JsonResultModel<V_Business_KnowledgeBase_Information> GetKnowledgeListBySearch(Business_KnowledgeBase_Information searchParam, GridParams para)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                JsonResultModel<V_Business_KnowledgeBase_Information> jsonResult = new JsonResultModel<V_Business_KnowledgeBase_Information>();
                var query = db.Queryable<V_Business_KnowledgeBase_Information>().Where(i => i.Status == "1");
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
                _ll.SaveLog(3, 40, CurrentUser.GetCurrentUser().LoginName, "草稿列表", logData);

                return jsonResult;
            }
        }

        /// <summary>
        /// 提交草稿知识
        /// </summary>
        /// <param name="vguid">主键</param>
        /// <returns></returns>
        public bool SubmitKnowledgeBase(Guid vguid)
        {
            using (SqlSugarClient db = SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                try
                {
                    db.BeginTran();
                    result =
                        db.Update<Business_KnowledgeBase_Information>(
                            new
                            {
                                Status = 2,
                                ChangeDate = DateTime.Now,
                                ChangeUser = Common.CurrentUser.GetCurrentUser().LoginName
                            }, i => i.Vguid == vguid);
                    Business_KnowledgeBase_Information knowledgeInfo =
                        db.Queryable<Business_KnowledgeBase_Information>()
                            .Where(i => i.Vguid == vguid)
                            .SingleOrDefault();
                    string knowledgeJson = JsonHelper.ModelToJson(knowledgeInfo);
                    //存入操作日志表
                    _ll.SaveLog(8, 40, Common.CurrentUser.GetCurrentUser().LoginName, knowledgeInfo.Title, knowledgeJson);
                    db.CommitTran();
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                    Common.LogHelper.LogHelper.WriteLog(ex.ToString());
                    _ll.SaveLog(5, 40, Common.CurrentUser.GetCurrentUser().LoginName, "", ex.ToString());
                }
                return result;
            }
        }

        /// <summary>
        /// 删除草稿知识
        /// </summary>
        /// <param name="vguid">主键</param>
        /// <returns></returns>
        public bool DeleteKnowledgeBase(Guid vguid)
        {
            using (SqlSugarClient db = SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                try
                {
                    db.BeginTran();
                    Business_KnowledgeBase_Information knowledgeInfo =
                        db.Queryable<Business_KnowledgeBase_Information>()
                            .Where(i => i.Vguid == vguid)
                            .SingleOrDefault();
                    string knowledgeJson = JsonHelper.ModelToJson(knowledgeInfo);
                    result = db.Delete<Business_KnowledgeBase_Information>(it => it.Vguid == vguid);
                    //存入操作日志表
                    _ll.SaveLog(2, 40, Common.CurrentUser.GetCurrentUser().LoginName, knowledgeInfo.Title, knowledgeJson);
                    db.CommitTran();
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                    Common.LogHelper.LogHelper.WriteLog(ex.ToString());
                    _ll.SaveLog(5, 40, Common.CurrentUser.GetCurrentUser().LoginName, "", ex.ToString());
                }
                return result;
            }
        }

        /// <summary>
        /// 判断是否存在相同的知识
        /// </summary>
        /// <param name="knowledgeBaseInfo">知识信息实体</param>
        /// <param name="isEdit">是否编辑</param>
        /// <returns></returns>
        public bool IsExistKnowledge(Business_KnowledgeBase_Information knowledgeBaseInfo, bool isEdit)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                return isEdit
                    ? db.Queryable<Business_KnowledgeBase_Information>()
                        .Any(it => it.Title == knowledgeBaseInfo.Title && it.Vguid != knowledgeBaseInfo.Vguid)
                    : db.Queryable<Business_KnowledgeBase_Information>().Any(it => it.Title == knowledgeBaseInfo.Title);
            }
        }


        /// <summary>
        /// 保存新增或编辑的知识
        /// </summary>
        /// <param name="knowledgeBaseInfo">知识信息实体</param>
        /// <param name="isEdit">是否编辑</param>
        /// <param name="saveType">保存类型 保存(1)还是提交(2)</param>
        /// <returns></returns>
        public bool SaveKnowledge(Business_KnowledgeBase_Information knowledgeBaseInfo, bool isEdit, string saveType)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                try
                {
                    knowledgeBaseInfo.Type = knowledgeBaseInfo.Type == "on" ? "2" : knowledgeBaseInfo.Type;
                    if (isEdit) //编辑
                    {
                        knowledgeBaseInfo.Status = saveType == "1" ? "1" : "2";
                        knowledgeBaseInfo.ChangeUser = CurrentUser.GetCurrentUser().LoginName;
                        knowledgeBaseInfo.ChangeDate = DateTime.Now;
                        db.DisableUpdateColumns = new[] { "CreatedUser", "CreatedDate", "Vguid" };
                        string knowledgeJson = JsonHelper.ModelToJson(knowledgeBaseInfo);
                        _ll.SaveLog(saveType == "1" ? 1 : 8, 42, Common.CurrentUser.GetCurrentUser().LoginName, knowledgeBaseInfo.Title, knowledgeJson);
                        db.Update<Business_KnowledgeBase_Information>(knowledgeBaseInfo, c => c.Vguid == knowledgeBaseInfo.Vguid);

                    }
                    else //新增
                    {
                        knowledgeBaseInfo.Type = knowledgeBaseInfo.Type == "on" ? "2" : knowledgeBaseInfo.Type;
                        knowledgeBaseInfo.Status = saveType == "1" ? "1" : "2";
                        knowledgeBaseInfo.Type = "1";
                        knowledgeBaseInfo.CreatedDate = DateTime.Now;
                        knowledgeBaseInfo.CreatedUser = CurrentUser.GetCurrentUser().LoginName;
                        knowledgeBaseInfo.Vguid = Guid.NewGuid();
                        string knowledgeJson = JsonHelper.ModelToJson(knowledgeBaseInfo);
                        _ll.SaveLog(saveType == "1" ? 1 : 8, 41, Common.CurrentUser.GetCurrentUser().LoginName, knowledgeBaseInfo.Title, knowledgeJson);
                        db.Insert(knowledgeBaseInfo, false);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Common.LogHelper.LogHelper.WriteLog(ex.ToString());
                    _ll.SaveLog(5, isEdit ? 42 : 41, Common.CurrentUser.GetCurrentUser().LoginName, "", ex.ToString());
                    return false;
                }
            }
        }

        /// <summary>
        /// 通过vguid获取知识的详细信息实体
        /// </summary>
        /// <param name="vguid">主键</param>
        /// <returns></returns>
        public Business_KnowledgeBase_Information GetKnowledgeBaseInfoByVguid(Guid vguid)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                return
                    db.Queryable<Business_KnowledgeBase_Information>().Where(it => it.Vguid == vguid).SingleOrDefault();
            }
        }

        /// <summary>
        /// 将上传的excel插入数据库中
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool InsertExcelToDatabase(DataTable dt, ref string msg)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                try
                {
                    var list = DatatableToList(dt, ref msg);
                    string knowledgeJson = JsonHelper.ModelToJson(list);
                    db.SqlBulkCopy(list);
                    _ll.SaveLog(7, 40, Common.CurrentUser.GetCurrentUser().LoginName, "草稿知识", knowledgeJson);
                    return true;
                }
                catch (Exception ex)
                {
                    Common.LogHelper.LogHelper.WriteLog(ex.ToString());
                    _ll.SaveLog(5, 40, Common.CurrentUser.GetCurrentUser().LoginName, "", ex.ToString());
                    return false;
                }

            }
        }

        /// <summary>
        /// 将DataTable内容绑定到知识详细信息列表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="msg">返回其他信息</param>
        /// <returns></returns>
        public List<Business_KnowledgeBase_Information> DatatableToList(DataTable dt, ref string msg)
        {
            List<Business_KnowledgeBase_Information> listKnowledgeBaseInfo = new List<Business_KnowledgeBase_Information>();
            try
            {

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        if (!string.IsNullOrEmpty(dr["column1"].ToString()) && !string.IsNullOrEmpty(dr["column2"].ToString()))
                        {
                            DataRow[] rows = dt.Select(string.Format("column1='{0}'", dr["column1"]));
                            if (rows.Length > 1)
                            {
                                msg = "知识" + rows[0].ItemArray[0] + "存在重复！";
                                listKnowledgeBaseInfo.Clear();
                                break;
                            }
                            Business_KnowledgeBase_Information knowledgeInfo = new Business_KnowledgeBase_Information();
                            knowledgeInfo.Title = dr["column1"].ToString();
                            if (IsExistKnowledge(knowledgeInfo, false))
                            {
                                i = i + 1;
                                msg = "第" + i + "行知识已经存在！";
                                listKnowledgeBaseInfo.Clear();
                                break;
                            }
                            knowledgeInfo.Type = "2";
                            knowledgeInfo.Status = "1";
                            knowledgeInfo.Content = dr["column2"].ToString();
                            knowledgeInfo.CreatedUser = Common.CurrentUser.GetCurrentUser().LoginName;
                            knowledgeInfo.CreatedDate = DateTime.Now;
                            knowledgeInfo.Vguid = Guid.NewGuid();
                            listKnowledgeBaseInfo.Add(knowledgeInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listKnowledgeBaseInfo.Clear();
                Common.LogHelper.LogHelper.WriteLog(ex.ToString());
                _ll.SaveLog(5, 40, Common.CurrentUser.GetCurrentUser().LoginName, "", ex.ToString());
            }
            return listKnowledgeBaseInfo;
        }
    }
}
