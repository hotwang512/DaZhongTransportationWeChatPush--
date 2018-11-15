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

namespace DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.WeChatExercise.BusinessLogic
{
    public class WeChatExerciseLogic
    {
        public WeChatExerciseServer _ws;
        public WeChatExerciseLogic()
        {
            _ws = new WeChatExerciseServer();
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
        /// 获取用户答案
        /// </summary>
        /// <param name="answerDetailList"></param>
        /// <returns></returns>
        public bool SaveUserAnswer(U_ExercisesAnswerDetail answerDetailModel)
        {
            bool result = false;//保存结果标识

            //习题人员与答案主表
            Business_ExercisesAnswer_Information mainExerciseAnswerModel = new Business_ExercisesAnswer_Information();
            mainExerciseAnswerModel.BusinessExercisesVguid = answerDetailModel.BusinessExercisesVguid;//习题主信息Vguid
            mainExerciseAnswerModel.BusinessPersonnelVguid = answerDetailModel.BusinessPersonnelVguid;//做题人Vguid
            mainExerciseAnswerModel.Marking = 1;//未阅卷
            mainExerciseAnswerModel.Status = 1;//未完成
            //mainExerciseAnswerModel.CreatedDate = DateTime.Now;
            //mainExerciseAnswerModel.ChangeDate = DateTime.Now;

            //习题人员与答案附表
            Business_ExercisesAnswerDetail_Information detailAnswerModel = new Business_ExercisesAnswerDetail_Information();
            detailAnswerModel.Answer = answerDetailModel.Answer;
            detailAnswerModel.Score = CheckAnswer(answerDetailModel.BusinessExercisesDetailVguid.ToString(), answerDetailModel.Answer);//计算题目得分
            detailAnswerModel.BusinessExercisesDetailVguid = answerDetailModel.BusinessExercisesDetailVguid;//具体习题Vguid
            //detailAnswerModel.BusinessAnswerExercisesVguid = answerDetailModel.BusinessAnswerExercisesVguid;
            //detailAnswerModel.CreatedDate = DateTime.Now;
            //detailAnswerModel.ChangeDate = DateTime.Now;
            result = _ws.SaveUserAnswer(mainExerciseAnswerModel, detailAnswerModel);

            LogHelper.WriteLog(JsonHelper.ModelToJson<Business_ExercisesAnswerDetail_Information>(detailAnswerModel));


            return result;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool UpLoadImg(string url, string exerciseVguid, string personVguid)
        {
            return _ws.UpLoadImg(url, exerciseVguid, personVguid);
        }

        /// <summary>
        /// 查询习题主信息
        /// </summary>
        /// <param name="Vguid"></param>
        /// <param name="personVguid"></param>
        /// <returns></returns>
        public JsonResultEntity<V_Business_ExercisesAndAnswer_Infomation, V_Business_ExercisesDetailAndExercisesAnswerDetail_Information> GetExerciseDoneAllMsg(string Vguid, string personVguid)
        {
            return _ws.GetExerciseDoneAllMsg(Vguid, personVguid);
        }

        /// <summary>
        /// 判断答案是否正确并计算得分
        /// </summary>
        /// <param name="exerciseDetailVguid">题目的Vguid</param>
        /// <param name="answer">答案</param>
        /// <returns></returns>
        public int CheckAnswer(string exerciseDetailVguid, string answer)
        {
            int score = 0;
            Business_ExercisesDetail_Infomation exerciseDetailModel = _ws.GetExerciseDetailModel(exerciseDetailVguid);
            string[] arrLetter = new string[8] { "A", "B", "C", "D", "E", "F", "G", "H" };
            if (exerciseDetailModel.ExerciseType == 1)//单选题
            {
                //arrLetter[answer]
                if (exerciseDetailModel.Answer == arrLetter[int.Parse(answer)])
                {
                    score = exerciseDetailModel.Score;
                }
            }
            else if (exerciseDetailModel.ExerciseType == 2)//多选题
            {
                if (string.IsNullOrEmpty(answer))
                {
                    return score;
                }
                var arrAnswer = answer.Split(',');
                string explainAnswer = "";
                foreach (var item in arrAnswer)
                {
                    explainAnswer += arrLetter[int.Parse(item)] + ",";
                }
                explainAnswer = explainAnswer.Substring(0, explainAnswer.Length - 1);
                if (exerciseDetailModel.Answer == explainAnswer)
                {
                    score = exerciseDetailModel.Score;
                }
            }
            else if (exerciseDetailModel.ExerciseType == 3)//判断题
            {
                //if (answer == "0")
                //{
                //    answer = "正确";
                //}
                //else if (answer == "1")
                //{
                //    answer = "错误";
                //}
                if (exerciseDetailModel.Answer == answer)
                {
                    score = exerciseDetailModel.Score;
                }
            }
            return score;
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
        /// 提交全部习题（更新总分数，批改状态和习题完成状态）
        /// </summary>
        /// <param name="businessPersonnelVguid"></param>
        /// <param name="wechatMainVguid"></param>
        /// <returns></returns>
        public U_ExerciseResult SubmitAllExercise(string businessPersonnelVguid, string wechatMainVguid)
        {
            return _ws.SubmitAllExercise(businessPersonnelVguid, wechatMainVguid);
        }

        /// <summary>
        /// 给阅读消息历史并且没有答过题的人重新推送习题
        /// </summary>
        /// <param name="businessPersonnelVguid"></param>
        /// <param name="wechatMainVguid"></param>
        /// <returns></returns>
        public bool ReWechatPushExercise(string businessPersonnelVguid, string wechatMainVguid)
        {
            return _ws.ReWechatPushExercise(businessPersonnelVguid, wechatMainVguid);
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
        /// 获取习题的详细信息
        /// </summary>
        /// <param name="exerVguid">习题的vguid</param>
        /// <returns></returns>
        public Business_Exercises_Infomation GetExerciseInfo(string exerVguid)
        {
            return _ws.GetExerciseInfo(exerVguid);
        }
    }
}