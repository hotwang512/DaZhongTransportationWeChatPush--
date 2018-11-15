using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Infrastructure.QuestionManagement
{
    public class CheckedQuestionServer
    {
        /// <summary>
        /// 日志
        /// </summary>
        public LogLogic _ll;
        public CheckedQuestionServer()
        {
            _ll = new LogLogic();
        }

        /// <summary>
        /// 绑定问卷状态
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetQuestionStatus()
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(DaZhongManagementSystem.Common.Tools.MasterVGUID.QuestionStatus);
                return _dbMsSql.Queryable<CS_Master_2>().Where(i => i.VGUID == vguid).ToList();
            }
        }


        /// <summary>
        /// 绑定问卷类型
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetQuestionType()
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(DaZhongManagementSystem.Common.Tools.MasterVGUID.QuestionType);
                return _dbMsSql.Queryable<CS_Master_2>().Where(i => i.VGUID == vguid).ToList().OrderBy("MasterCode", OrderByType.Asc).ToList();
            }
        }

        /// <summary>
        /// 分页查询已审核问卷信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<V_Business_Questionnaire> GetQuestionListBySearch(Business_Questionnaire_Search searchParam, GridParams para)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                JsonResultModel<V_Business_Questionnaire> jsonResult = new JsonResultModel<V_Business_Questionnaire>();
                //只查询三个月内的数据
                // DateTime endDate = DateTime.Now.AddMonths(-3);

                var query = _dbMsSql.Queryable<V_Business_Questionnaire>().Where(i => i.Status == "2");
                if (!string.IsNullOrEmpty(searchParam.QuestionName))
                {
                    query.Where(i => i.QuestionnaireName.Contains(searchParam.QuestionName));
                }
                if (!string.IsNullOrEmpty(searchParam.EffectiveDate))
                {
                    DateTime effectiveDate = DateTime.Parse(searchParam.EffectiveDate);
                    query.Where(i => i.EffectiveDate < effectiveDate);
                }
                if (!string.IsNullOrEmpty(searchParam.CreatedTimeStart) && !string.IsNullOrEmpty(searchParam.CreatedTimeEnd))
                {
                    DateTime createdTimeStart = DateTime.Parse(searchParam.CreatedTimeStart);
                    DateTime createdTimeEnd = DateTime.Parse(searchParam.CreatedTimeEnd);
                    query.Where(i => i.CreatedDate > createdTimeStart && i.CreatedDate < createdTimeEnd); // && i.CreatedDate > endDate
                }
                else
                {
                    if (!string.IsNullOrEmpty(searchParam.CreatedTimeStart))
                    {
                        DateTime createdTimeStart = DateTime.Parse(searchParam.CreatedTimeStart);
                        query.Where(i => i.CreatedDate > createdTimeStart); //&& i.CreatedDate > endDate
                    }
                    if (!string.IsNullOrEmpty(searchParam.CreatedTimeEnd))
                    {
                        DateTime createdTimeEnd = DateTime.Parse(searchParam.CreatedTimeEnd);
                        query.Where(i => i.CreatedDate < createdTimeEnd); //&& i.CreatedDate > endDate
                    }
                }

                query.OrderBy(para.sortdatafield + " " + para.sortorder);
                jsonResult.TotalRows = query.Count();
                jsonResult.Rows = query.ToPageList(para.pagenum, para.pagesize);

                //存入操作日志表
                string logData = JsonHelper.ModelToJson<JsonResultModel<V_Business_Questionnaire>>(jsonResult);
                _ll.SaveLog(3, 56, Common.CurrentUser.GetCurrentUser().LoginName, "已审核问卷列表", logData);

                return jsonResult;
            }
        }

        /// <summary>
        /// 通过Vguid获取问卷信息（编辑）
        /// </summary>
        /// <param name="Vguid"></param>
        /// <returns></returns>
        public Business_Questionnaire GetQuestionByVguid(string Vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(Vguid);
                Business_Questionnaire questionInfoModel = _dbMsSql.Queryable<Business_Questionnaire>().Where(i => i.Vguid == vguid).SingleOrDefault();

                //存入操作日志表
                string logData = JsonHelper.ModelToJson<Business_Questionnaire>(questionInfoModel);
                _ll.SaveLog(3, 55, Common.CurrentUser.GetCurrentUser().LoginName, questionInfoModel.QuestionnaireName, logData);

                return questionInfoModel;
            }
        }

        /// <summary>
        /// 通过问卷主信息的Vguid获取问卷详细列表
        /// </summary>
        /// <param name="Vguid"></param>
        /// <returns></returns>
        public List<Business_QuestionnaireDetail> GetQuestionDetailListByMainVguid(string Vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                Guid mainVguid = Guid.Parse(Vguid);
                return _dbMsSql.Queryable<Business_QuestionnaireDetail>().Where(i => i.QuestionnaireVguid == mainVguid).OrderBy(c => c.QuestionTitleID, OrderByType.Asc).ToList();

            }
        }
    }
}
