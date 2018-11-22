using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Common.LogHelper;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.BasicDataManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.WeChatQuestion.BusinessLogic
{
    public class WeChatQuestionLogic
    {
        public WeChatQuestionServer _ws;
        public WeChatQuestionLogic()
        {
            _ws = new WeChatQuestionServer();
        }

        /// <summary>
        /// 更新是否查看推送
        /// </summary>
        /// <param name="pushVguid"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool UpdateIsRead(string pushVguid, string userID)
        {
            return _ws.UpdateIsRead(pushVguid, userID);
        }

        /// <summary>
        /// 获取用户详细信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Business_Personnel_Information GetUserInfo(string userID)
        {
            return _ws.GetUserInfo(userID);
        }

        /// <summary>
        /// 获取答题次数
        /// </summary>
        /// <returns></returns>
        public string GetAnswerCount()
        {
            return _ws.GetAnswerCount();
        }

        /// <summary>
        /// 保存用户答案
        /// </summary>
        /// <param name="answerDetailList"></param>
        /// <returns></returns>
        public bool SaveUserAnswer(U_QuestionAnswerDetail answerDetailModel)
        {
            bool result = false;//保存结果标识

            //问卷人员与答案主表
            Business_Questionnaire_Answer mainExerciseAnswerModel = new Business_Questionnaire_Answer();
            mainExerciseAnswerModel.QuestionnaireVguid = answerDetailModel.BusinessQuestionnaireVguid;//问卷主信息Vguid
            mainExerciseAnswerModel.Business_PersonnelVguid = answerDetailModel.BusinessPersonnelVguid;//做题人Vguid
            mainExerciseAnswerModel.Status = "1";//未完成
            mainExerciseAnswerModel.ParticipateStatus = "2";//参与
            //mainExerciseAnswerModel.CreatedDate = DateTime.Now;
            //mainExerciseAnswerModel.ChangeDate = DateTime.Now;

            //问卷人员与答案附表
            Business_Questionnaire_AnswerDetail detailAnswerModel = new Business_Questionnaire_AnswerDetail();
            detailAnswerModel.Answer = answerDetailModel.Answer;
            detailAnswerModel.BusinessQuestionDetailVguid = answerDetailModel.BusinessQuestionnaireDetailVguid;//具体问卷Vguid
            //detailAnswerModel.BusinessAnswerExercisesVguid = answerDetailModel.BusinessAnswerExercisesVguid;
            //detailAnswerModel.CreatedDate = DateTime.Now;
            //detailAnswerModel.ChangeDate = DateTime.Now;
            result = _ws.SaveUserAnswer(mainExerciseAnswerModel, detailAnswerModel);

            LogHelper.WriteLog(JsonHelper.ModelToJson<Business_Questionnaire_AnswerDetail>(detailAnswerModel));


            return result;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool UpLoadImg(string url, string questionVguid, string personVguid)
        {
            return _ws.UpLoadImg(url, questionVguid, personVguid);
        }

        /// <summary>
        /// 查询问卷主信息
        /// </summary>
        /// <param name="Vguid"></param>
        /// <param name="personVguid"></param>
        /// <returns></returns>
        public JsonResultEntity<V_Business_Questionnaire_Answer, V_Business_QuestionnaireDetail_AnswerDetail> GetQuestionDoneAllMsg(string Vguid, string personVguid)
        {
            return _ws.GetQuestionDoneAllMsg(Vguid, personVguid);
        }


        /// <summary>
        /// 是否存在简答题
        /// </summary>
        /// <param name="businessExercisesVguid"></param>
        /// <returns></returns>
        public bool IsExistShortMsg(string businessExercisesVguid)
        {
            return _ws.IsExistShortMsg(businessExercisesVguid);
        }

        /// <summary>
        /// 提交全部问卷（更新总分数，批改状态和问卷完成状态）
        /// </summary>
        /// <param name="businessPersonnelVguid"></param>
        /// <param name="wechatMainVguid"></param>
        /// <returns></returns>
        public U_ExerciseResult SubmitAllQuestion(string businessPersonnelVguid, string wechatMainVguid)
        {
            return _ws.SubmitAllQuestion(businessPersonnelVguid, wechatMainVguid);
        }

        /// <summary>
        /// 给阅读消息历史并且没有答过题的人重新推送问卷
        /// </summary>
        /// <param name="businessPersonnelVguid"></param>
        /// <param name="wechatMainVguid"></param>
        /// <returns></returns>
        public bool ReWechatPushQuestion(string businessPersonnelVguid, string wechatMainVguid)
        {
            return _ws.ReWechatPushQuestion(businessPersonnelVguid, wechatMainVguid);
        }

        /// <summary>
        /// 当前人是否推送过
        /// </summary>
        /// <param name="businessPersonnelVguid"></param>
        /// <param name="wechatMainVguid"></param>
        /// <returns></returns>
        public bool IsPushed(string businessPersonnelVguid, string wechatMainVguid)
        {
            return _ws.IsPushed(businessPersonnelVguid, wechatMainVguid);
        }


        /// <summary>
        /// 获取问卷的详细信息
        /// </summary>
        /// <param name="questionVguid">问卷的vguid</param>
        /// <returns></returns>
        public Business_Questionnaire GetQuestionInfo(string questionVguid)
        {
            return _ws.GetQuestionInfo(questionVguid);
        }
    }
}