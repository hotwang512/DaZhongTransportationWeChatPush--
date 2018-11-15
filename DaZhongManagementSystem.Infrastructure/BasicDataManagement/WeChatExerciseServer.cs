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
    public class WeChatExerciseServer
    {
        private readonly LogLogic _ll;

        public WeChatExerciseServer()
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
        /// <param name="exerciseVguid"></param>
        /// <param name="personVguid"></param>
        /// <returns></returns>
        public bool UpLoadImg(string url, string exerciseVguid, string personVguid)
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                try
                {
                    Guid mainExerciseVguid = Guid.Parse(exerciseVguid);
                    Guid personalVguid = Guid.Parse(personVguid);
                    Business_ExercisesAnswer_Information exerciseAnswerModel = new Business_ExercisesAnswer_Information();
                    exerciseAnswerModel = dbMsSql.Queryable<Business_ExercisesAnswer_Information>().Where(i => i.BusinessExercisesVguid == mainExerciseVguid && i.BusinessPersonnelVguid == personalVguid).SingleOrDefault();
                    if (exerciseAnswerModel != null)
                    {
                        if (string.IsNullOrEmpty(exerciseAnswerModel.PicturePath))
                        {
                            result = dbMsSql.Update<Business_ExercisesAnswer_Information>(new { PicturePath = url }, i => i.BusinessExercisesVguid == mainExerciseVguid && i.BusinessPersonnelVguid == personalVguid);
                        }
                    }
                    else //如果Business_ExercisesAnswer_Information没有数据要新增一条
                    {
                        Business_ExercisesAnswer_Information mainExercise = new Business_ExercisesAnswer_Information();
                        mainExercise.Vguid = Guid.NewGuid();
                        mainExercise.BusinessExercisesVguid = mainExerciseVguid;
                        mainExercise.BusinessPersonnelVguid = personalVguid;
                        mainExercise.ChangeDate = DateTime.Now;
                        mainExercise.CreatedDate = DateTime.Now;
                        mainExercise.PicturePath = url;
                        mainExercise.SolveNumber = 0;
                        result = dbMsSql.Insert<Business_ExercisesAnswer_Information>(mainExercise, false) != DBNull.Value;
                    }

                    //存入操作日志表
                    string logData = JsonHelper.ModelToJson<Business_ExercisesAnswer_Information>(exerciseAnswerModel);
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
        /// 通过习题主信息Vguid和人员Vguid获取全套习题信息
        /// </summary>
        /// <param name="Vguid"></param>
        /// <param name="personVguid"></param>
        /// <returns></returns>
        public JsonResultEntity<V_Business_ExercisesAndAnswer_Infomation, V_Business_ExercisesDetailAndExercisesAnswerDetail_Information> GetExerciseDoneAllMsg(string vguid, string personVguid)
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid Vguid = Guid.Parse(vguid);
                Guid personalVguid = Guid.Parse(personVguid);
                JsonResultEntity<V_Business_ExercisesAndAnswer_Infomation, V_Business_ExercisesDetailAndExercisesAnswerDetail_Information> exerciseAll = new JsonResultEntity<V_Business_ExercisesAndAnswer_Infomation, V_Business_ExercisesDetailAndExercisesAnswerDetail_Information>();

                string sql = string.Format("exec usp_UserExercisesInformation '{0}','{1}'", personalVguid, Vguid);

                var query1 = dbMsSql.Queryable<V_Business_ExercisesAndAnswer_Infomation>();
                var query2 = dbMsSql.SqlQuery<V_Business_ExercisesDetailAndExercisesAnswerDetail_Information>(sql);

                List<V_Business_ExercisesAndAnswer_Infomation> mainRows = query1.Where(i => i.BusinessExercisesVGUID == Vguid && i.BusinessPersonnelVguid == personalVguid).ToList();
                //如果mainRows为0的话则代表此用户没有答过题
                if (mainRows.Count == 0)
                {
                    mainRows = dbMsSql.Queryable<V_Business_ExercisesAndAnswer_Infomation>().Where(i => i.BusinessExercisesVGUID == Vguid).ToList();
                    mainRows[0].PicturePath = "";
                }
                exerciseAll.MainRow = mainRows;

                exerciseAll.DetailRow = query2.ToList();

                return exerciseAll;
            }
        }

        /// <summary>
        /// 通过习题Vguid获取习题详细信息
        /// </summary>
        /// <param name="exerciseDetailVguid"></param>
        /// <returns></returns>
        public Business_ExercisesDetail_Infomation GetExerciseDetailModel(string exerciseDetailVguid)
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(exerciseDetailVguid);
                Business_ExercisesDetail_Infomation exerciseModel = dbMsSql.Queryable<Business_ExercisesDetail_Infomation>().Where(i => i.Vguid == vguid).SingleOrDefault();
                //存入操作日志表
                string logData = JsonHelper.ModelToJson(exerciseModel);
                _ll.SaveLog(3, 34, "", exerciseModel.ExerciseName, logData);
                return exerciseModel;
            }
        }

        /// <summary>
        /// 获取用户答案
        /// </summary>
        /// <param name="mainExerciseAnswerModel"></param>
        /// <param name="detailAnswerModel"></param>
        /// <returns></returns>
        public bool SaveUserAnswer(Business_ExercisesAnswer_Information mainExerciseAnswerModel, Business_ExercisesAnswerDetail_Information detailAnswerModel)
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                bool mainEdit = false; //习题人员与答案主表中是否存在
                bool detailEdit = false; //习题人员与答案附表中是否存在
                try
                {
                    dbMsSql.BeginTran();
                    mainEdit = dbMsSql.Queryable<Business_ExercisesAnswer_Information>().Any(i => i.BusinessPersonnelVguid == mainExerciseAnswerModel.BusinessPersonnelVguid && i.BusinessExercisesVguid == mainExerciseAnswerModel.BusinessExercisesVguid);

                    if (!mainEdit) //新增
                    {
                        mainExerciseAnswerModel.Vguid = Guid.NewGuid();
                        mainExerciseAnswerModel.CreatedDate = DateTime.Now;
                        mainExerciseAnswerModel.ChangeDate = DateTime.Now;
                        result = dbMsSql.Insert(mainExerciseAnswerModel, false) != DBNull.Value;

                        //存入操作日志表
                        string logData = JsonHelper.ModelToJson(mainExerciseAnswerModel);
                        _ll.SaveLog(1, 34, mainExerciseAnswerModel.BusinessPersonnelVguid.ToString(), "用户" + mainExerciseAnswerModel.BusinessPersonnelVguid + " " + mainExerciseAnswerModel.BusinessExercisesVguid + "套题的答题情况", logData);
                    }
                    else
                    {
                        mainExerciseAnswerModel = dbMsSql.Queryable<Business_ExercisesAnswer_Information>().Where(i => i.BusinessPersonnelVguid == mainExerciseAnswerModel.BusinessPersonnelVguid && i.BusinessExercisesVguid == mainExerciseAnswerModel.BusinessExercisesVguid).SingleOrDefault();
                        result = dbMsSql.Update<Business_ExercisesAnswer_Information>(new { ChangeDate = DateTime.Now }, i => i.BusinessPersonnelVguid == mainExerciseAnswerModel.BusinessPersonnelVguid && i.BusinessExercisesVguid == mainExerciseAnswerModel.BusinessExercisesVguid);
                        //存入操作日志表
                        string logData = JsonHelper.ModelToJson(mainExerciseAnswerModel);
                        _ll.SaveLog(4, 34, mainExerciseAnswerModel.BusinessPersonnelVguid.ToString(), "用户" + mainExerciseAnswerModel.BusinessPersonnelVguid + " " + mainExerciseAnswerModel.BusinessExercisesVguid + "套题的答题情况", logData);
                    }

                    detailEdit = dbMsSql.Queryable<Business_ExercisesAnswerDetail_Information>().Any(i => i.BusinessAnswerExercisesVguid == mainExerciseAnswerModel.Vguid && i.BusinessExercisesDetailVguid == detailAnswerModel.BusinessExercisesDetailVguid);
                    if (result) //主表插入成功
                    {
                        Business_ExercisesAnswerDetail_Information newDeatailAnswerModel = new Business_ExercisesAnswerDetail_Information();
                        if (detailEdit) //编辑
                        {
                            newDeatailAnswerModel = dbMsSql.Queryable<Business_ExercisesAnswerDetail_Information>().Where(i => i.BusinessAnswerExercisesVguid == mainExerciseAnswerModel.Vguid && i.BusinessExercisesDetailVguid == detailAnswerModel.BusinessExercisesDetailVguid).SingleOrDefault();
                            var model = new
                            {
                                Answer = detailAnswerModel.Answer,
                                Score = detailAnswerModel.Score,
                                ChangeDate = DateTime.Now
                            };
                            result = dbMsSql.Update<Business_ExercisesAnswerDetail_Information>(model, i => i.Vguid == newDeatailAnswerModel.Vguid);
                            //存入操作日志表
                            string logData = JsonHelper.ModelToJson(detailAnswerModel);
                            _ll.SaveLog(4, 34, "", detailAnswerModel.BusinessExercisesDetailVguid + "道题的答题情况", logData);
                        }
                        else //新增
                        {
                            detailAnswerModel.BusinessAnswerExercisesVguid = mainExerciseAnswerModel.Vguid;
                            //关联”习题人员与答案信息“主表
                            detailAnswerModel.Vguid = Guid.NewGuid();
                            detailAnswerModel.CreatedDate = DateTime.Now;
                            detailAnswerModel.ChangeDate = DateTime.Now;
                            result = dbMsSql.Insert(detailAnswerModel, false) != DBNull.Value;
                            //存入操作日志表
                            string logData = JsonHelper.ModelToJson(detailAnswerModel);
                            _ll.SaveLog(1, 34, "", detailAnswerModel.BusinessExercisesDetailVguid + "道题的答题情况", logData);
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
        /// <param name="businessExercisesVguid"></param>
        /// <returns></returns>
        public bool IsExistShortMsg(string businessExercisesVguid)
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bool isExistShortMsg = false;
                Guid exerciseVguid = Guid.Parse(businessExercisesVguid);
                var exerciseDetailList = dbMsSql.Queryable<Business_ExercisesDetail_Infomation>().Where(i => i.ExercisesInformationVguid == exerciseVguid).ToList();
                isExistShortMsg = exerciseDetailList.Any(i => i.ExerciseType == 4);
                return isExistShortMsg;
            }
        }

        /// <summary>
        /// 提交全部习题（更新总分数，批改状态和习题完成状态）
        /// </summary>
        /// <param name="businessPersonnelVguid"></param>
        /// <param name="businessExercisesVguid"></param>
        /// <returns></returns>
        public U_ExerciseResult SubmitAllExercise(string businessPersonnelVguid, string businessExercisesVguid)
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bool isExistShortAnswer = IsExistShortMsg(businessExercisesVguid);
                U_ExerciseResult exerciseResult = new U_ExerciseResult();
                int score = 0; //本套习题总分
                Guid personVguid = Guid.Parse(businessPersonnelVguid);
                Guid exerciseVguid = Guid.Parse(businessExercisesVguid);
                var exerciseAnswerModel = dbMsSql.Queryable<Business_ExercisesAnswer_Information>().Where(i => i.BusinessPersonnelVguid == personVguid && i.BusinessExercisesVguid == exerciseVguid).SingleOrDefault();
                exerciseAnswerModel.SolveNumber = exerciseAnswerModel.SolveNumber ?? 0;
                exerciseAnswerModel.SolveNumber++;
                var exerciseAnswerDetailList = dbMsSql.Queryable<Business_ExercisesAnswerDetail_Information>().Where(i => i.BusinessAnswerExercisesVguid == exerciseAnswerModel.Vguid).ToList();
                foreach (var item in exerciseAnswerDetailList)
                {
                    score += item.Score;
                }
                if (!isExistShortAnswer)
                {
                    var model = new
                    {
                        TotalScore = score,
                        Marking = 2,
                        Status = 2,
                        SolveNumber = exerciseAnswerModel.SolveNumber,
                        ChangeDate = DateTime.Now
                    };
                    exerciseResult.isComplete = dbMsSql.Update<Business_ExercisesAnswer_Information>(model, i => i.Vguid == exerciseAnswerModel.Vguid);
                    exerciseResult.score = score;
                }
                else
                {
                    var model = new
                    {
                        TotalScore = score,
                        Marking = 1,
                        Status = 2,
                        SolveNumber = exerciseAnswerModel.SolveNumber,
                        ChangeDate = DateTime.Now
                    };
                    exerciseResult.isComplete = dbMsSql.Update<Business_ExercisesAnswer_Information>(model, i => i.Vguid == exerciseAnswerModel.Vguid);
                    exerciseResult.score = -1;
                }

                //存入操作日志表
                string logData = JsonHelper.ModelToJson(exerciseAnswerModel);
                _ll.SaveLog(4, 34, businessPersonnelVguid, "(用户：" + businessPersonnelVguid + "提交了" + businessExercisesVguid + "套题信息)", logData);
                return exerciseResult;
            }
        }

        /// <summary>
        /// 给阅读消息历史并且没有答过题的人重新推送习题
        /// </summary>
        /// <param name="businessPersonnelVguid"></param>
        /// <param name="wechatMainVguid"></param>
        /// <returns></returns>
        public bool ReWechatPushExercise(string businessPersonnelVguid, string wechatMainVguid)
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
                        MessageType = 4,
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
                        ExercisesVGUID = wechatMainModel.ExercisesVGUID,
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
                    _ll.SaveLog(10, 34, currentUser, "从消息历史获取习题", logdata);
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
                var listVguid = db.Queryable<Business_WeChatPush_Information>().Where(i => i.ExercisesVGUID == wechatMainModel.ExercisesVGUID).Select(i => i.VGUID).ToList();
                var listWechatPushObj = db.Queryable<Business_WeChatPushDetail_Information>().Where(i => listVguid.Contains(i.Business_WeChatPushVguid)).Select(i => i.PushObject).ToList();  //找到该习题所有的推送人
                var currentUser = db.Queryable<Business_Personnel_Information>().Where(i => i.Vguid == personnelVguid).Select(i => i.UserID).SingleOrDefault();  //当前人的userid
                if (listWechatPushObj.Contains(currentUser))
                {
                    return true;   //已经推送过
                }
                return false;     //没有推送过
            }
        }

        /// <summary>
        /// 获取习题的详细信息
        /// </summary>
        /// <param name="exerVguid">习题的vguid</param>
        /// <returns></returns>
        public Business_Exercises_Infomation GetExerciseInfo(string exerVguid)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                Guid exerciseVguid = Guid.Parse(exerVguid);
                return db.Queryable<Business_Exercises_Infomation>().Where(i => i.Vguid == exerciseVguid).SingleOrDefault();
            }
        }
    }
}
