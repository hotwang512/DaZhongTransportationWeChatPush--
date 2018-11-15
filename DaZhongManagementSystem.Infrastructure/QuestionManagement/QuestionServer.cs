using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;
using System;
using System.Collections.Generic;
using DaZhongManagementSystem.Common;
using System.Linq;
using System.Text;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using System.Data;

namespace DaZhongManagementSystem.Infrastructure.QuestionManagement
{
    public class QuestionServer
    {
        /// <summary>
        /// 日志
        /// </summary>
        public LogLogic _ll;
        public QuestionServer()
        {
            _ll = new LogLogic();
        }

        /// <summary>
        /// 分页查询问卷信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<V_Business_Questionnaire> GetQuestionListBySearch(Business_Questionnaire_Search searchParam, GridParams para)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                JsonResultModel<V_Business_Questionnaire> jsonResult = new JsonResultModel<V_Business_Questionnaire>();
                var query = _dbMsSql.Queryable<V_Business_Questionnaire>().Where(i => i.Status == "1");
                if (!string.IsNullOrEmpty(searchParam.QuestionName))
                {
                    query.Where(i => i.QuestionnaireName.Contains(searchParam.QuestionName));
                }
                if (!string.IsNullOrEmpty(searchParam.EffectiveDate))
                {
                    DateTime effectiveDate = DateTime.Parse(searchParam.EffectiveDate);
                    query.Where(i => i.EffectiveDate < effectiveDate);
                }
                query.OrderBy(para.sortdatafield + " " + para.sortorder);
                jsonResult.TotalRows = query.Count();
                jsonResult.Rows = query.ToPageList(para.pagenum, para.pagesize);

                //存入操作日志表
                string logData = JsonHelper.ModelToJson<JsonResultModel<V_Business_Questionnaire>>(jsonResult);
                _ll.SaveLog(3, 52, Common.CurrentUser.GetCurrentUser().LoginName, "问卷列表", logData);

                return jsonResult;
            }
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

        /// <summary>
        /// 审核提交问卷
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public bool CheckedQuestion(string vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                Guid Vguid = Guid.Parse(vguid);
                try
                {
                    _dbMsSql.BeginTran();
                    result = _dbMsSql.Update<Business_Questionnaire>(new { Status = 2 }, i => i.Vguid == Vguid);
                    Business_Questionnaire questionInfo = _dbMsSql.Queryable<Business_Questionnaire>().Where(i => i.Vguid == Vguid).SingleOrDefault();
                    string exerciseJson = JsonHelper.ModelToJson(questionInfo);
                    //存入操作日志表
                    _ll.SaveLog(9, 52, CurrentUser.GetCurrentUser().LoginName, questionInfo.QuestionnaireName, exerciseJson);

                    _dbMsSql.CommitTran();
                }
                catch (Exception ex)
                {
                    _dbMsSql.RollbackTran();
                    Common.LogHelper.LogHelper.WriteLog(ex.ToString());
                    _ll.SaveLog(5, 52, CurrentUser.GetCurrentUser().LoginName, "", ex.ToString());
                }
                return result;
            }
        }

        /// <summary>
        /// 删除未提交问卷
        /// </summary>
        /// <param name="vguid">问卷Vguid</param>
        /// <returns></returns>
        public bool DeletedQuestion(string vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                Guid questionVguid = Guid.Parse(vguid);
                try
                {
                    _dbMsSql.BeginTran();
                    Business_Questionnaire exerciseModel = _dbMsSql.Queryable<Business_Questionnaire>().Where(i => i.Vguid == questionVguid).SingleOrDefault();
                    string logData = JsonHelper.ModelToJson(exerciseModel);
                    result = _dbMsSql.Delete<Business_Questionnaire>(i => i.Vguid == questionVguid);     //删除问卷主表
                    if (result)
                    {
                        List<Business_QuestionnaireDetail> exercisesDetail = _dbMsSql.Queryable<Business_QuestionnaireDetail>().Where(i => i.QuestionnaireVguid == questionVguid).ToList();
                        if (exercisesDetail.Count != 0)
                        {
                            result = _dbMsSql.Delete<Business_QuestionnaireDetail>(i => i.QuestionnaireVguid == questionVguid);       //删除问卷附表
                        }
                    }

                    _ll.SaveLog(2, 52, Common.CurrentUser.GetCurrentUser().LoginName, exerciseModel.QuestionnaireName, logData);
                    _dbMsSql.CommitTran();
                }
                catch (Exception exp)
                {
                    _dbMsSql.RollbackTran();
                    Common.LogHelper.LogHelper.WriteLog(exp.ToString());
                    _ll.SaveLog(5, 52, Common.CurrentUser.GetCurrentUser().LoginName, "", exp.ToString());
                }

                return result;
            }
        }

        /// <summary>
        /// 检查用户是否做过本套题
        /// </summary>
        /// <param name="Vguid"></param>
        /// <param name="personVguid"></param>
        /// <returns></returns>
        public int CheckUserIsAnswered(string Vguid, string personVguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(Vguid);
                Guid personVGUID = Guid.Parse(personVguid);
                List<Business_Questionnaire_Answer> questionAnswerList = new List<Business_Questionnaire_Answer>();
                questionAnswerList = _dbMsSql.Queryable<Business_Questionnaire_Answer>().Where(i => i.QuestionnaireVguid == vguid && i.Business_PersonnelVguid == personVGUID).ToList();
                return questionAnswerList.Count();
            }
        }

        /// <summary>
        /// 保存问卷信息(主信息、详细信息)
        /// </summary>
        /// <param name="questionMainModel"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        public bool SaveQuestionMain(Business_Questionnaire questionMainModel, bool isEdit, List<Business_QuestionnaireDetail> questionDetailList)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                try
                {
                    _dbMsSql.BeginTran();
                    if (isEdit)
                    {
                        //定义匿名类型model
                        var model = new
                        {
                            QuestionnaireName = questionMainModel.QuestionnaireName,
                            Description = questionMainModel.Description,
                            Remarks = questionMainModel.Remarks,
                            EffectiveDate = questionMainModel.EffectiveDate,
                            Status = questionMainModel.Status,
                            ChangeUser = questionMainModel.ChangeUser,
                            ChangeDate = questionMainModel.ChangeDate
                        };
                        result = _dbMsSql.Update<Business_Questionnaire>(model, i => i.Vguid == questionMainModel.Vguid);
                        //存入操作日志表
                        string logData = JsonHelper.ModelToJson(questionMainModel);
                        _ll.SaveLog(4, 54, CurrentUser.GetCurrentUser().LoginName, questionMainModel.QuestionnaireName + " " + "主信息", logData);
                    }
                    else
                    {
                        result = _dbMsSql.Insert(questionMainModel, false) != DBNull.Value;

                        //存入操作日志表
                        string logData = JsonHelper.ModelToJson(questionMainModel);
                        _ll.SaveLog(1, 53, CurrentUser.GetCurrentUser().LoginName, questionMainModel.QuestionnaireName + " " + "主信息", logData);
                    }
                    _dbMsSql.Delete<Business_QuestionnaireDetail>(i => i.QuestionnaireVguid == questionMainModel.Vguid);
                    foreach (var item in questionDetailList)
                    {
                        _dbMsSql.Insert(item, false);

                        //存入操作日志表
                        string logData = JsonHelper.ModelToJson(item);
                        _ll.SaveLog(4, 55, CurrentUser.GetCurrentUser().LoginName, questionMainModel.QuestionnaireName + " " + "问卷详细信息", logData);
                    }
                    _dbMsSql.CommitTran();
                }
                catch (Exception ex)
                {
                    _dbMsSql.RollbackTran();
                    Common.LogHelper.LogHelper.WriteLog(ex.Message + "/n" + ex + "/n" + ex.StackTrace);
                }
                return result;
            }
        }

        /// <summary>
        /// 通过问卷Vguid获取问卷详细信息
        /// </summary>
        /// <param name="questionDetailVguid"></param>
        /// <returns></returns>
        public Business_QuestionnaireDetail GetQuestionDetailModel(string questionDetailVguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(questionDetailVguid);
                Business_QuestionnaireDetail questionModel = _dbMsSql.Queryable<Business_QuestionnaireDetail>().Where(i => i.Vguid == vguid).SingleOrDefault();

                return questionModel;
            }
        }

    }
}
