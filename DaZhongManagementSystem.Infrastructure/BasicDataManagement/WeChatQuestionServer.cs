using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using DaZhongManagementSystem.Common.LogHelper;
using DaZhongManagementSystem.Infrastructure.SugarDao;

namespace DaZhongManagementSystem.Infrastructure.BasicDataManagement
{
    public class WeChatQuestionServer
    {
        private readonly LogLogic _ll;

        public WeChatQuestionServer()
        {
            _ll = new LogLogic();
        }

        /// <summary>
        /// 更新是否查看推送
        /// </summary>
        /// <param name="pushVguid"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool UpdateIsRead(string pushVguid, string userID)
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bool isRead = false;
                try
                {
                    Guid pushVGUID = Guid.Parse(pushVguid);
                    isRead = dbMsSql.Update<Business_WeChatPushDetail_Information>(new { ISRead = "1" }, i => i.Business_WeChatPushVguid == pushVGUID && i.PushObject == userID);
                }
                catch (Exception exp)
                {
                    LogHelper.WriteLog("更新是否查看推送状态：" + pushVguid + "/" + exp.ToString());
                }
                return isRead;
            }
        }

        /// <summary>
        /// 获取用户详细信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Business_Personnel_Information GetUserInfo(string userID)
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Business_Personnel_Information personModel = new Business_Personnel_Information();
                try
                {
                    personModel = dbMsSql.Queryable<Business_Personnel_Information>().Where(i => i.UserID == userID).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.Message);
                    _ll.SaveLog(5, 34, "", userID + personModel.Name, ex.Message);
                }
                return personModel;
            }
        }

        /// <summary>
        ///  获取答题次数
        /// </summary>
        /// <returns></returns>
        public string GetAnswerCount()
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                string answerCount = string.Empty;
                answerCount = dbMsSql.Queryable<Master_Configuration>().Where(i => i.ID == 8).SingleOrDefault().ConfigValue;
                return answerCount;
            }
        }

        /// <summary>
        /// 上传用户头像
        /// </summary>
        /// <param name="url"></param>
        /// <param name="questionVguid"></param>
        /// <param name="personVguid"></param>
        /// <returns></returns>
        public bool UpLoadImg(string url, string questionVguid, string personVguid)
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                try
                {
                    Guid mainQuestionVguid = Guid.Parse(questionVguid);
                    Guid personalVguid = Guid.Parse(personVguid);
                    Business_Questionnaire_Answer questionAnswerModel = new Business_Questionnaire_Answer();
                    questionAnswerModel = dbMsSql.Queryable<Business_Questionnaire_Answer>().Where(i => i.QuestionnaireVguid == mainQuestionVguid && i.Business_PersonnelVguid == personalVguid).SingleOrDefault();
                    if (questionAnswerModel != null)
                    {
                        //if (string.IsNullOrEmpty(questionAnswerModel.PicturePath))
                        //{
                        //    result = dbMsSql.Update<Business_ExercisesAnswer_Information>(new { PicturePath = url }, i => i.BusinessExercisesVguid == mainQuestionVguid && i.BusinessPersonnelVguid == personalVguid);
                        //}
                    }
                    else //如果Business_ExercisesAnswer_Information没有数据要新增一条
                    {
                        Business_Questionnaire_Answer mainQuestion = new Business_Questionnaire_Answer();
                        mainQuestion.Vguid = Guid.NewGuid();
                        mainQuestion.QuestionnaireVguid = mainQuestionVguid;
                        mainQuestion.Business_PersonnelVguid = personalVguid;
                        mainQuestion.ChangeDate = DateTime.Now;
                        mainQuestion.CreatedDate = DateTime.Now;
                        result = dbMsSql.Insert<Business_Questionnaire_Answer>(mainQuestion, false) != DBNull.Value;
                    }

                    //存入操作日志表
                    string logData = JsonHelper.ModelToJson<Business_Questionnaire_Answer>(questionAnswerModel);
                    _ll.SaveLog(13, 34, personVguid, "用户图片", logData);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.Message);
                    _ll.SaveLog(5, 34, personVguid, "用户图片", ex.Message);
                }
                return result;
            }
        }

        /// <summary>
        /// 通过问卷主信息Vguid和人员Vguid获取全套问卷信息
        /// </summary>
        /// <param name="Vguid"></param>
        /// <param name="personVguid"></param>
        /// <returns></returns>
        public JsonResultEntity<V_Business_Questionnaire_Answer, V_Business_QuestionnaireDetail_AnswerDetail> GetQuestionDoneAllMsg(string vguid, string personVguid)
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid Vguid = Guid.Parse(vguid);
                Guid personalVguid = Guid.Parse(personVguid);
                JsonResultEntity<V_Business_Questionnaire_Answer, V_Business_QuestionnaireDetail_AnswerDetail> exerciseAll = new JsonResultEntity<V_Business_Questionnaire_Answer, V_Business_QuestionnaireDetail_AnswerDetail>();

                string sql = string.Format("exec usp_UserQuestionInformation '{0}','{1}'", personalVguid, Vguid);

                var query1 = dbMsSql.Queryable<V_Business_Questionnaire_Answer>();
                var query2 = dbMsSql.SqlQuery<V_Business_QuestionnaireDetail_AnswerDetail>(sql).OrderBy(c => c.QuestionnaireType);

                List<V_Business_Questionnaire_Answer> mainRows = query1.Where(i => i.QuestionnaireVguid == Vguid && i.BusinessPersonnelVguid == personalVguid).ToList();
                //如果mainRows为0的话则代表此用户没有答过题
                if (mainRows.Count == 0)
                {
                    mainRows = dbMsSql.Queryable<V_Business_Questionnaire_Answer>().Where(i => i.QuestionnaireVguid == Vguid).ToList();
                    //mainRows[0].PicturePath = "";
                }
                exerciseAll.MainRow = mainRows;

                exerciseAll.DetailRow = query2.ToList();

                return exerciseAll;
            }
        }

        /// <summary>
        /// 通过问卷Vguid获取问卷详细信息
        /// </summary>
        /// <param name="questionDetailVguid"></param>
        /// <returns></returns>
        public Business_QuestionnaireDetail GetQuestionDetailModel(string questionDetailVguid)
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(questionDetailVguid);
                Business_QuestionnaireDetail questionModel = dbMsSql.Queryable<Business_QuestionnaireDetail>().Where(i => i.Vguid == vguid).SingleOrDefault();
                //存入操作日志表
                string logData = JsonHelper.ModelToJson(questionModel);
                _ll.SaveLog(3, 34, "", questionModel.QuestionnaireDetailName, logData);
                return questionModel;
            }
        }

        /// <summary>
        /// 保存用户答案
        /// </summary>
        /// <param name="mainQuestionAnswerModel"></param>
        /// <param name="detailAnswerModel"></param>
        /// <returns></returns>
        public bool SaveUserAnswer(Business_Questionnaire_Answer mainQuestionAnswerModel, Business_Questionnaire_AnswerDetail detailAnswerModel)
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                bool mainEdit = false; //问卷人员与答案主表中是否存在
                bool detailEdit = false; //问卷人员与答案附表中是否存在
                try
                {
                    dbMsSql.BeginTran();
                    mainEdit = dbMsSql.Queryable<Business_Questionnaire_Answer>().Any(i => i.Business_PersonnelVguid == mainQuestionAnswerModel.Business_PersonnelVguid && i.QuestionnaireVguid == mainQuestionAnswerModel.QuestionnaireVguid);

                    if (!mainEdit) //新增
                    {
                        mainQuestionAnswerModel.Vguid = Guid.NewGuid();
                        mainQuestionAnswerModel.CreatedDate = DateTime.Now;
                        mainQuestionAnswerModel.ChangeDate = DateTime.Now;
                        result = dbMsSql.Insert(mainQuestionAnswerModel, false) != DBNull.Value;

                        //存入操作日志表
                        string logData = JsonHelper.ModelToJson(mainQuestionAnswerModel);
                        _ll.SaveLog(1, 34, mainQuestionAnswerModel.Business_PersonnelVguid.ToString(), "用户" + mainQuestionAnswerModel.Business_PersonnelVguid + " " + mainQuestionAnswerModel.Business_PersonnelVguid + "套题的答题情况", logData);
                    }
                    else
                    {
                        mainQuestionAnswerModel = dbMsSql.Queryable<Business_Questionnaire_Answer>().Where(i => i.Business_PersonnelVguid == mainQuestionAnswerModel.Business_PersonnelVguid && i.QuestionnaireVguid == mainQuestionAnswerModel.QuestionnaireVguid).SingleOrDefault();
                        result = dbMsSql.Update<Business_Questionnaire_Answer>(new { ChangeDate = DateTime.Now }, i => i.Business_PersonnelVguid == mainQuestionAnswerModel.Business_PersonnelVguid && i.QuestionnaireVguid == mainQuestionAnswerModel.QuestionnaireVguid);
                        //存入操作日志表
                        string logData = JsonHelper.ModelToJson(mainQuestionAnswerModel);
                        _ll.SaveLog(4, 34, mainQuestionAnswerModel.Business_PersonnelVguid.ToString(), "用户" + mainQuestionAnswerModel.Business_PersonnelVguid + " " + mainQuestionAnswerModel.QuestionnaireVguid + "套题的答题情况", logData);
                    }

                    detailEdit = dbMsSql.Queryable<Business_Questionnaire_AnswerDetail>().Any(i => i.QuestionnaireAnswerVguid == mainQuestionAnswerModel.Vguid && i.BusinessQuestionDetailVguid == detailAnswerModel.BusinessQuestionDetailVguid);
                    if (result) //主表插入成功
                    {
                        Business_Questionnaire_AnswerDetail newDeatailAnswerModel = new Business_Questionnaire_AnswerDetail();
                        if (detailEdit) //编辑
                        {
                            newDeatailAnswerModel = dbMsSql.Queryable<Business_Questionnaire_AnswerDetail>().Where(i => i.QuestionnaireAnswerVguid == mainQuestionAnswerModel.Vguid && i.BusinessQuestionDetailVguid == detailAnswerModel.BusinessQuestionDetailVguid).SingleOrDefault();
                            var model = new
                            {
                                Answer = detailAnswerModel.Answer,
                                ChangeDate = DateTime.Now
                            };
                            result = dbMsSql.Update<Business_Questionnaire_AnswerDetail>(model, i => i.Vguid == newDeatailAnswerModel.Vguid);
                            //存入操作日志表
                            string logData = JsonHelper.ModelToJson(detailAnswerModel);
                            _ll.SaveLog(4, 34, "", detailAnswerModel.QuestionnaireAnswerVguid + "道题的答题情况", logData);
                        }
                        else //新增
                        {
                            detailAnswerModel.QuestionnaireAnswerVguid = mainQuestionAnswerModel.Vguid;
                            //关联”问卷人员与答案信息“主表
                            detailAnswerModel.Vguid = Guid.NewGuid();
                            detailAnswerModel.CreatedDate = DateTime.Now;
                            detailAnswerModel.ChangeDate = DateTime.Now;
                            result = dbMsSql.Insert(detailAnswerModel, false) != DBNull.Value;
                            //存入操作日志表
                            string logData = JsonHelper.ModelToJson(detailAnswerModel);
                            _ll.SaveLog(1, 34, "", detailAnswerModel.QuestionnaireAnswerVguid + "道题的答题情况", logData);
                        }
                    }
                    dbMsSql.CommitTran();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.ToString());
                    dbMsSql.RollbackTran();
                }
                return result;
            }
        }

        /// <summary>
        /// 是否存在简答题
        /// </summary>
        /// <param name="businessQuestionVguid"></param>
        /// <returns></returns>
        public bool IsExistShortMsg(string businessQuestionVguid)
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bool isExistShortMsg = false;
                Guid questionVguid = Guid.Parse(businessQuestionVguid);
                var questionDetailList = dbMsSql.Queryable<Business_QuestionnaireDetail>().Where(i => i.QuestionnaireVguid == questionVguid).ToList();
                isExistShortMsg = questionDetailList.Any(i => i.QuestionnaireDetailType == "4");
                return isExistShortMsg;
            }
        }

        /// <summary>
        /// 提交全部问卷（更新总分数，批改状态和问卷完成状态）
        /// </summary>
        /// <param name="businessPersonnelVguid"></param>
        /// <param name="businessQuestionVguid"></param>
        /// <returns></returns>
        public U_ExerciseResult SubmitAllQuestion(string businessPersonnelVguid, string businessQuestionVguid)
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                //bool isExistShortAnswer = IsExistShortMsg(businessQuestionVguid);
                U_ExerciseResult exerciseResult = new U_ExerciseResult();
                int isParticipateStatus = 1; //本套问卷总分
                Guid personVguid = Guid.Parse(businessPersonnelVguid);
                Guid questionVguid = Guid.Parse(businessQuestionVguid);
                var questionAnswerModel = dbMsSql.Queryable<Business_Questionnaire_Answer>().Where(i => i.Business_PersonnelVguid == personVguid && i.QuestionnaireVguid == questionVguid).SingleOrDefault();
                //是否参与回答问卷
                var questionAnswerDetailList = dbMsSql.Queryable<Business_Questionnaire_AnswerDetail>().Where(i => i.QuestionnaireAnswerVguid == questionAnswerModel.Vguid).ToList();
                foreach (var item in questionAnswerDetailList)
                {
                    if (!string.IsNullOrEmpty(item.Answer))
                    {
                        isParticipateStatus = 2;
                        break;
                    }

                }
                var model = new
                {
                    ParticipateStatus = isParticipateStatus,
                    Status = 2,
                    ChangeDate = DateTime.Now
                };
                exerciseResult.isComplete = dbMsSql.Update<Business_Questionnaire_Answer>(model, i => i.Vguid == questionAnswerModel.Vguid);

                //存入操作日志表
                string logData = JsonHelper.ModelToJson(questionAnswerModel);
                _ll.SaveLog(4, 34, businessPersonnelVguid, "(用户：" + businessPersonnelVguid + "提交了" + businessQuestionVguid + "套题信息)", logData);
                return exerciseResult;
            }
        }

        /// <summary>
        /// 给阅读消息历史并且没有答过题的人重新推送问卷
        /// </summary>
        /// <param name="businessPersonnelVguid"></param>
        /// <param name="wechatMainVguid"></param>
        /// <returns></returns>
        public bool ReWechatPushQuestion(string businessPersonnelVguid, string wechatMainVguid)
        {
            Guid mainVguid = Guid.Parse(wechatMainVguid);
            Guid personnelVguid = Guid.Parse(businessPersonnelVguid);
            bool result = false;
            using (SqlSugarClient db = SugarDao_MsSql.GetInstance())
            {
                try
                {
                    db.BeginTran();
                    var wechatMainModel = db.Queryable<Business_WeChatPush_Information>().Where(i => i.VGUID == mainVguid).SingleOrDefault();
                    var currentUser = db.Queryable<Business_Personnel_Information>().Where(i => i.Vguid == personnelVguid).Select(i => i.UserID).SingleOrDefault();
                    DateTime? dateTime = null;
                    var wechatMain = new Business_WeChatPush_Information()
                    {
                        PushType = 1,
                        Title = wechatMainModel.Title,
                        MessageType = 16,
                        Timed = false,
                        TimedSendTime = null,
                        Important = wechatMainModel.PeriodOfValidity == null,
                        PeriodOfValidity = wechatMainModel.PeriodOfValidity == null ? dateTime : DateTime.Now.AddMonths(1),
                        Message = "从消息历史获取",
                        PushPeople = wechatMainModel.PushPeople,
                        Status = 3,
                        CreatedDate = DateTime.Now,
                        CreatedUser = wechatMainModel.CreatedUser,
                        VGUID = Guid.NewGuid(),
                        CoverImg = wechatMainModel.CoverImg,
                        CoverDescption = wechatMainModel.CoverDescption,
                        QuestionVGUID = wechatMainModel.QuestionVGUID,
                        History = "0",
                        Department_VGUID = Guid.Empty
                    };
                    var wechatDetail = new Business_WeChatPushDetail_Information()
                    {
                        Type = "1",
                        PushObject = currentUser,
                        CreatedUser = wechatMainModel.CreatedUser,
                        CreatedDate = DateTime.Now,
                        Vguid = Guid.NewGuid(),
                        Business_WeChatPushVguid = wechatMain.VGUID,
                        ISRead = "0",
                    };
                    string logdata = Extend.ModelToJson(wechatMain);
                    _ll.SaveLog(10, 34, currentUser, "从消息历史获取问卷", logdata);
                    db.Insert(wechatDetail, false);
                    db.Insert(wechatMain, false);
                    db.CommitTran();
                    result = true;
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                    LogHelper.WriteLog(ex.ToString());
                }
            }
            return result;
        }

        /// <summary>
        /// 当前人是否推送过
        /// </summary>
        /// <param name="businessPersonnelVguid"></param>
        /// <param name="wechatMainVguid"></param>
        /// <returns></returns>
        public bool IsPushed(string businessPersonnelVguid, string wechatMainVguid)
        {
            Guid mainVguid = Guid.Parse(wechatMainVguid);
            Guid personnelVguid = Guid.Parse(businessPersonnelVguid);
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var wechatMainModel = db.Queryable<Business_WeChatPush_Information>().Where(i => i.VGUID == mainVguid).SingleOrDefault(); //推送主信息
                var listVguid = db.Queryable<Business_WeChatPush_Information>().Where(i => i.QuestionVGUID == wechatMainModel.QuestionVGUID).Select(i => i.VGUID).ToList();
                var listWechatPushObj = db.Queryable<Business_WeChatPushDetail_Information>().Where(i => listVguid.Contains(i.Business_WeChatPushVguid)).Select(i => i.PushObject).ToList();  //找到该问卷所有的推送人
                var currentUser = db.Queryable<Business_Personnel_Information>().Where(i => i.Vguid == personnelVguid).Select(i => i.UserID).SingleOrDefault();  //当前人的userid
                if (listWechatPushObj.Contains(currentUser))
                {
                    return true;   //已经推送过
                }
                return false;     //没有推送过
            }
        }

        /// <summary>
        /// 获取问卷的详细信息
        /// </summary>
        /// <param name="exerVguid">问卷的vguid</param>
        /// <returns></returns>
        public Business_Questionnaire GetQuestionInfo(string exerVguid)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                Guid questionVguid = Guid.Parse(exerVguid);
                return db.Queryable<Business_Questionnaire>().Where(i => i.Vguid == questionVguid).SingleOrDefault();
            }
        }
    }
}
