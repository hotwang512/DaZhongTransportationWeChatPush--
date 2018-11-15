using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.BasicDataManagement;
using DaZhongManagementSystem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DaZhongManagementSystem.Common.LogHelper;
using Aspose.Cells;
using System.Data;
using DaZhongManagementSystem.Common.Tools;
using DaZhongManagementSystem.Infrastructure.QuestionManagement;

namespace DaZhongManagementSystem.Areas.QuestionManagement.Controllers.QuestionManagement.BusinessLogic
{
    public class QuestionLogic
    {
        public QuestionServer _es;
        public QuestionLogic()
        {
            _es = new QuestionServer();
        }

        /// <summary>
        /// 分页查询问卷信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<V_Business_Questionnaire> GetQuestionListBySearch(Business_Questionnaire_Search searchParam, GridParams para)
        {
            return _es.GetQuestionListBySearch(searchParam, para);
        }

        /// <summary>
        /// 下载问卷模板
        /// </summary>
        public void DownLoadTemplate()
        {
            string exerciseFileName = SyntacticSugar.ConfigSugar.GetAppString("QuestionFileName");
            UploadHelper.ExportExcels("QuestionTemplate.xlsx", exerciseFileName);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="routList"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetRoutName(List<string[]> routList, int index)
        {
            if (routList == null)
                return "";
            if (routList.Count == 0)
                return "";
            return routList[routList.Count - 1][index];
        }

        /// <summary>
        /// 绑定问卷状态
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetQuestionStatus()
        {
            return _es.GetQuestionStatus();
        }

        /// <summary>
        /// 绑定问卷类型
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetQuestionType()
        {
            return _es.GetQuestionType();
        }

        /// <summary>
        /// 通过Vguid获取问卷信息（编辑）
        /// </summary>
        /// <param name="Vguid"></param>
        /// <returns></returns>
        public Business_Questionnaire GetQuestionByVguid(string Vguid)
        {
            return _es.GetQuestionByVguid(Vguid);
        }


        /// <summary>
        /// 批量审核问卷
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public bool CheckedQuestion(string[] vguidList)
        {
            bool result = false;
            foreach (var item in vguidList)
            {
                result = _es.CheckedQuestion(item);
            }
            return result;
        }

        /// <summary>
        /// 批量删除问卷
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public bool DeletedQuestion(string[] vguidList)
        {
            bool result = false;
            foreach (var item in vguidList)
            {
                result = _es.DeletedQuestion(item);
            }
            return result;
        }

        /// <summary>
        /// 通过问卷主信息的Vguid获取问卷详细列表
        /// </summary>
        /// <param name="Vguid"></param>
        /// <returns></returns>
        public List<Business_QuestionnaireDetail> GetQuestionDetailListByMainVguid(string Vguid)
        {
            return _es.GetQuestionDetailListByMainVguid(Vguid);
        }

        /// <summary>
        /// 检查用户是否做过本套题
        /// </summary>
        /// <param name="Vguid"></param>
        /// <param name="personVguid"></param>
        /// <returns></returns>
        public int CheckUserIsAnswered(string Vguid, string personVguid)
        {
            return _es.CheckUserIsAnswered(Vguid, personVguid);
        }

        /// <summary>
        /// 保存问卷信息(主信息、详细信息)
        /// </summary>
        /// <param name="questionMainModel"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        public bool SaveQuestionMain(Business_Questionnaire questionMainModel, bool isEdit, string questionData)
        {
            bool result = false;
            var questionModel = new Business_Questionnaire();//问卷主信息
            var questionList = JsonHelper.JsonToModel<List<Business_QuestionnaireDetail>>(questionData);//待保存的问卷详细信息
            var questionDetailList = new List<Business_QuestionnaireDetail>();
            if (isEdit)
            {
                questionModel = GetQuestionByVguid(questionMainModel.Vguid.ToString());
                questionModel.QuestionnaireName = questionMainModel.QuestionnaireName;
                questionModel.Description = questionMainModel.Description;
                questionModel.Remarks = questionMainModel.Remarks;
                questionModel.EffectiveDate = questionMainModel.EffectiveDate;
                questionModel.Status = "1";
                questionModel.ChangeUser = Common.CurrentUser.GetCurrentUser().LoginName;
                questionModel.ChangeDate = DateTime.Now;

                //循环遍历问卷详情
                foreach (var item in questionList)
                {
                    Business_QuestionnaireDetail questionDetail = new Business_QuestionnaireDetail();
                    questionDetail.QuestionnaireDetailType = item.QuestionnaireDetailType;
                    questionDetail.QuestionnaireDetailName = item.QuestionnaireDetailName;
                    questionDetail.QuestionOption = item.QuestionOption;
                    questionDetail.QuestionTitleID = item.QuestionTitleID;
                    questionDetail.Answer = item.Answer;
                    questionDetail.Vguid = Guid.NewGuid();
                    questionDetail.QuestionnaireVguid = questionMainModel.Vguid;//问卷主信息Vguid
                    questionDetail.CreatedUser = Common.CurrentUser.GetCurrentUser().LoginName;
                    questionDetail.CreatedDate = DateTime.Now;
                    questionDetail.ChangeDate = DateTime.Now;
                    questionDetailList.Add(questionDetail);
                }
                result = _es.SaveQuestionMain(questionModel, isEdit, questionDetailList);
            }
            else
            {
                questionModel.QuestionnaireName = questionMainModel.QuestionnaireName;
                questionModel.Description = questionMainModel.Description;
                questionModel.Remarks = questionMainModel.Remarks;
                questionModel.EffectiveDate = questionMainModel.EffectiveDate;
                questionModel.Status = "1";
                questionModel.CreatedDate = DateTime.Now;
                questionModel.CreatedUser = Common.CurrentUser.GetCurrentUser().LoginName;
                questionModel.ChangeDate = DateTime.Now;
                questionModel.Vguid = questionMainModel.Vguid;

                foreach (var item in questionList)
                {
                    Business_QuestionnaireDetail questionDetail = new Business_QuestionnaireDetail();
                    questionDetail.QuestionnaireDetailType = item.QuestionnaireDetailType;
                    questionDetail.QuestionnaireDetailName = item.QuestionnaireDetailName;
                    questionDetail.QuestionOption = item.QuestionOption;
                    questionDetail.QuestionTitleID = item.QuestionTitleID;
                    questionDetail.Answer = item.Answer;
                    questionDetail.Vguid = Guid.NewGuid();
                    questionDetail.QuestionnaireVguid = questionMainModel.Vguid;//问卷主信息Vguid
                    questionDetail.CreatedUser = Common.CurrentUser.GetCurrentUser().LoginName;
                    questionDetail.CreatedDate = DateTime.Now;
                    questionDetail.ChangeDate = DateTime.Now;
                    questionDetailList.Add(questionDetail);
                }
                result = _es.SaveQuestionMain(questionModel, isEdit, questionDetailList);
            }
            return result;
        }
    }
}