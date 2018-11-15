using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.QuestionManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.QuestionManagement.Controllers.CheckedQuestion.BusinessLogic
{
    public class CheckedQuestionLogic
    {
        public CheckedQuestionServer _cs;
        public CheckedQuestionLogic()
        {
            _cs = new CheckedQuestionServer();
        }

        /// <summary>
        /// 绑定问卷状态
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetQuestionStatus()
        {
            return _cs.GetQuestionStatus();
        }

        /// <summary>
        /// 绑定问卷类型
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetQuestionType()
        {
            return _cs.GetQuestionType();
        }

        /// <summary>
        /// 分页查询问卷信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<V_Business_Questionnaire> GetQuestionListBySearch(Business_Questionnaire_Search searchParam, GridParams para)
        {
            return _cs.GetQuestionListBySearch(searchParam, para);
        }

        /// <summary>
        /// 批量删除问卷
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        //public bool DeletedQuestion(string[] vguidList)
        //{
        //    bool result = false;
        //    foreach (var item in vguidList)
        //    {
        //        result = _cs.DeletedQuestion(item);
        //    }
        //    return result;
        //}

        /// <summary>
        /// 通过Vguid获取问卷信息（编辑）
        /// </summary>
        /// <param name="Vguid"></param>
        /// <returns></returns>
        public Business_Questionnaire GetQuestionByVguid(string Vguid)
        {
            return _cs.GetQuestionByVguid(Vguid);
        }

        /// <summary>
        /// 通过问卷主信息的Vguid获取问卷详细列表
        /// </summary>
        /// <param name="Vguid"></param>
        /// <returns></returns>
        public List<Business_QuestionnaireDetail> GetQuestionDetailListByMainVguid(string Vguid)
        {
            return _cs.GetQuestionDetailListByMainVguid(Vguid);
        }
    }
}