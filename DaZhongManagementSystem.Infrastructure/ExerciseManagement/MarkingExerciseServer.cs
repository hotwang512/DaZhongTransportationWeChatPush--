using DaZhongManagementSystem.Common.LogHelper;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Aspose.Words.Lists;

namespace DaZhongManagementSystem.Infrastructure.ExerciseManagement
{
    public class MarkingExerciseServer
    {
        public LogLogic _logLogic;
        public MarkingExerciseServer()
        {
            _logLogic = new LogLogic();
        }

        /// <summary>
        /// 判断此习题是否阅过
        /// </summary>
        /// <param name="exerciseVguid"></param>
        /// <param name="personVguid"></param>
        /// <returns></returns>
        public bool IsExerciseMarked(string exerciseVguid, string personVguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                try
                {
                    Guid exerciseVGUID = Guid.Parse(exerciseVguid);
                    Guid personVGUID = Guid.Parse(personVguid);
                    List<Business_ExercisesAnswer_Information> exercisesAnswerList = new List<Business_ExercisesAnswer_Information>();
                    exercisesAnswerList = _dbMsSql.Queryable<Business_ExercisesAnswer_Information>().Where(i => i.BusinessExercisesVguid == exerciseVGUID && i.BusinessPersonnelVguid == personVGUID).ToList();
                    if (exercisesAnswerList[0].Marking == 2)
                    {
                        result = true;
                    }
                }
                catch (Exception exp)
                {
                    LogHelper.WriteLog("MarkingExerciseServer.cs：" + exp.ToString() + "/n" + exp.StackTrace);
                }
                return result;
            }
        }

        /// <summary>
        /// 获取习题列表
        /// </summary>
        /// <returns></returns>
        public List<V_ExercisesUserInformation> GetExerciseList()
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                List<V_ExercisesUserInformation> exerciseList = new List<V_ExercisesUserInformation>();
                exerciseList = _dbMsSql.Queryable<V_ExercisesUserInformation>().ToList();

                return exerciseList;
            }
        }

        /// <summary>
        /// 获取答过本套习题人员列表
        /// </summary>
        /// <param name="vguid">习题主Vguid</param>
        /// <returns></returns>
        public JsonResultModel<U_MarkingPerson> GetAnsweredPersonList(string vguid, string isMarking, GridParams para)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                JsonResultModel<U_MarkingPerson> jsonResult = new JsonResultModel<U_MarkingPerson>();
                //  List<U_MarkingPerson> markingPersonList = new List<U_MarkingPerson>();
                Guid exerciseVguid = Guid.Parse("00000000-0000-0000-0000-000000000000");
                if (!string.IsNullOrEmpty(vguid))
                {
                    exerciseVguid = Guid.Parse(vguid);
                }
                var pars = SqlSugarTool.GetParameters(new
                {
                    ExercisesVguid = exerciseVguid,
                    Start = (para.pagenum - 1) * para.pagesize + 1,
                    End = para.pagenum * para.pagesize,
                    isMark = string.IsNullOrEmpty(isMarking) ? "1,2" : isMarking,
                    Count = 0,
                }); //将匿名对象转成SqlParameter
                _dbMsSql.IsClearParameters = false;//禁止清除参数
                pars[4].Direction = ParameterDirection.Output; //将Count设为 output
                string sql = string.Format(@"EXEC usp_ExercisesUserDetailInformation @ExercisesVguid,@Start,@End,@isMark,@count output");
                var list = _dbMsSql.SqlQuery<U_MarkingPerson>(sql, pars);
                _dbMsSql.IsClearParameters = true;//启动请动清除参数
                var outPutValue = pars[4].Value.ObjToInt();//获取output @Count的值
                jsonResult.Rows = list;
                jsonResult.TotalRows = outPutValue;
                //markingPersonList = _dbMsSql.GetList<U_MarkingPerson>(sql, new { ExercisesVguid = exerciseVguid });
                //return markingPersonList;
                return jsonResult;
            }
        }

        /// <summary>
        /// 保存简答题分数
        /// </summary>
        /// <param name="personVguid">答题人Vguid</param>
        /// <param name="vguid">习题Vguid</param>
        /// <param name="exerciseDetailVguid">具体习题Vguid</param>
        /// <param name="score">得分</param>
        /// <returns></returns>
        public bool SaveShortAnswerMarking(string personVguid, string vguid, string exerciseDetailVguid, string score)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                try
                {
                    int totalScore = 0;
                    Guid personVGUID = Guid.Parse(personVguid);
                    Guid exerciserVGUID = Guid.Parse(vguid);
                    Guid exerciserDetailVGUID = Guid.Parse(exerciseDetailVguid);
                    int personScore = int.Parse(score);

                    Business_ExercisesAnswer_Information exerciserAnswerModel = _dbMsSql.Queryable<Business_ExercisesAnswer_Information>().Where(i => i.BusinessPersonnelVguid == personVGUID && i.BusinessExercisesVguid == exerciserVGUID).SingleOrDefault();
                    //先更新答题的明细表（更新简答题得分）
                    result = _dbMsSql.Update<Business_ExercisesAnswerDetail_Information>(new { Score = personScore, ChangeDate = DateTime.Now, ChangeUser = Common.CurrentUser.GetCurrentUser().LoginName }, i => i.BusinessAnswerExercisesVguid == exerciserAnswerModel.Vguid && i.BusinessExercisesDetailVguid == exerciserDetailVGUID);

                    //获取更新后的总分
                    List<Business_ExercisesAnswerDetail_Information> exerciseAnswerDetailList = new List<Business_ExercisesAnswerDetail_Information>();
                    exerciseAnswerDetailList = _dbMsSql.Queryable<Business_ExercisesAnswerDetail_Information>().Where(i => i.BusinessAnswerExercisesVguid == exerciserAnswerModel.Vguid).ToList();
                    foreach (var item in exerciseAnswerDetailList)
                    {
                        totalScore += item.Score;
                    }
                    var model = new
                    {
                        TotalScore = totalScore,
                        Marking = 2,
                        Status = 2,
                        ChangeDate = DateTime.Now
                    };

                    result = _dbMsSql.Update<Business_ExercisesAnswer_Information>(model, i => i.Vguid == exerciserAnswerModel.Vguid);
                    _logLogic.SaveLog(4, 36, Common.CurrentUser.GetCurrentUser().LoginName, "简答题得分" + score, "vguid总分为" + totalScore);
                }
                catch (Exception exp)
                {
                    LogHelper.WriteLog("保存简答题分数：" + exp.ToString());
                }
                return result;
            }
        }
    }
}
