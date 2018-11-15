using System;
using System.Collections.Generic;
using System.Data;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;


namespace DaZhongManagementSystem.Infrastructure.ExerciseLibraryManagement
{

    public class ExerciseLibraryServer
    {
        /// <summary>
        /// 日志
        /// </summary>
        private LogLogic _logLogic;

        public ExerciseLibraryServer()
        {
            _logLogic = new LogLogic();
        }

        /// <summary>
        /// 分页查询草稿习题信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<v_Business_ExercisesLibrary_Infomation> GetExerciseListBySearch(Business_Exercises_Infomation_Search searchParam, GridParams para)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                JsonResultModel<v_Business_ExercisesLibrary_Infomation> jsonResult =
                    new JsonResultModel<v_Business_ExercisesLibrary_Infomation>();
                var query = _dbMsSql.Queryable<v_Business_ExercisesLibrary_Infomation>().Where(i => i.Status == 1);
                if (!string.IsNullOrEmpty(searchParam.ExerciseName))
                {
                    query.Where(i => i.ExerciseName.Contains(searchParam.ExerciseName));
                }
                if (!string.IsNullOrEmpty(searchParam.InputType))
                {
                    int inputType = int.Parse(searchParam.InputType);
                    query.Where(i => i.InputType == inputType);
                }
                if (!string.IsNullOrEmpty(searchParam.ExerciseType))
                {
                    int exerciseType = int.Parse(searchParam.ExerciseType);
                    query.Where(i => i.ExerciseType == exerciseType);
                }
                if (!string.IsNullOrEmpty(searchParam.CreatedDate))
                {
                    DateTime createdDate = DateTime.Parse(searchParam.CreatedDate);
                    query.Where(i => i.CreatedDate < createdDate);
                }
                query.OrderBy(para.sortdatafield + " " + para.sortorder);
                jsonResult.TotalRows = query.Count();
                jsonResult.Rows = query.ToPageList(para.pagenum, para.pagesize);

                //存入操作日志表
                string logData = JsonHelper.ModelToJson<JsonResultModel<v_Business_ExercisesLibrary_Infomation>>(jsonResult);
                _logLogic.SaveLog(3, 38, Common.CurrentUser.GetCurrentUser().LoginName, "草稿习题列表", logData);

                return jsonResult;
            }
        }

        /// <summary>
        /// 绑定习题库录入类型
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetInputType()
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(Common.Tools.MasterVGUID.LibraryInputType);
                return _dbMsSql.Queryable<CS_Master_2>().Where(i => i.VGUID == vguid).ToList();
            }
        }

        /// <summary>
        /// 批量审核习题
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public bool CheckedExercise(string vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                Guid Vguid = Guid.Parse(vguid);
                try
                {
                    _dbMsSql.BeginTran();
                    result = _dbMsSql.Update<Business_ExerciseLibrary_Information>(new { Status = 2 },
                        i => i.Vguid == Vguid);
                    Business_ExerciseLibrary_Information exerciseInfo =
                        _dbMsSql.Queryable<Business_ExerciseLibrary_Information>()
                            .Where(i => i.Vguid == Vguid)
                            .SingleOrDefault();
                    string exerciseJson = JsonHelper.ModelToJson<Business_ExerciseLibrary_Information>(exerciseInfo);
                    //存入操作日志表
                    _logLogic.SaveLog(9, 38, Common.CurrentUser.GetCurrentUser().LoginName, exerciseInfo.ExerciseName,
                        exerciseJson);

                    _dbMsSql.CommitTran();
                }
                catch (Exception ex)
                {
                    _dbMsSql.RollbackTran();
                    Common.LogHelper.LogHelper.WriteLog(ex.ToString());
                    _logLogic.SaveLog(5, 38, Common.CurrentUser.GetCurrentUser().LoginName, "", ex.ToString());
                }
                return result;
            }
        }

        /// <summary>
        /// 删除未提交习题
        /// </summary>
        /// <param name="vguid">习题Vguid</param>
        /// <returns></returns>
        public bool DeletedExercise(string vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                Guid exerciseVguid = Guid.Parse(vguid);
                try
                {
                    _dbMsSql.BeginTran();
                    Business_ExerciseLibrary_Information exerciseModel =
                        _dbMsSql.Queryable<Business_ExerciseLibrary_Information>()
                            .Where(i => i.Vguid == exerciseVguid)
                            .SingleOrDefault();
                    string logData = JsonHelper.ModelToJson<Business_ExerciseLibrary_Information>(exerciseModel);
                    result = _dbMsSql.Delete<Business_ExerciseLibrary_Information>(i => i.Vguid == exerciseVguid);
                    //删除习题
                    _logLogic.SaveLog(2, 38, Common.CurrentUser.GetCurrentUser().LoginName, exerciseModel.ExerciseName,
                        logData);
                    _dbMsSql.CommitTran();
                }
                catch (Exception exp)
                {
                    _dbMsSql.RollbackTran();
                    Common.LogHelper.LogHelper.WriteLog(exp.ToString());
                    _logLogic.SaveLog(5, 38, Common.CurrentUser.GetCurrentUser().LoginName, "", exp.ToString());
                }
                return result;
            }
        }

        /// <summary>
        /// 上传习题Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool InsertExcelToDatabase(DataTable dt)
        {
            bool result = false;
            List<Business_ExerciseLibrary_Information> exerciseDetailList = DataTableToList(dt);
            if (exerciseDetailList.Count == 0)
            {
                result = false;
            }
            else
            {
                result = SaveExercise(exerciseDetailList, false);
            }
            return result;
        }

        /// <summary>
        /// 将DataTable内容绑定到习题详细信息列表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<Business_ExerciseLibrary_Information> DataTableToList(DataTable dt)
        {
            List<Business_ExerciseLibrary_Information> exerciseDetailList =
                new List<Business_ExerciseLibrary_Information>();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 3; i < dt.Rows.Count; i++) //从第3行开始插入,前2行为习题主信息以及注意事项
                {
                    try
                    {
                        DataRow dr = dt.Rows[i];
                        if ((!string.IsNullOrEmpty(dr["column1"].ToString())) && (!string.IsNullOrEmpty(dr["column2"].ToString())) && (!string.IsNullOrEmpty(dr["column4"].ToString())) && (!string.IsNullOrEmpty(dr["column5"].ToString())))
                        {
                            var exerciseDetail = new Business_ExerciseLibrary_Information();
                            exerciseDetail.ExerciseType = int.Parse(dr["column1"].ToString()); //题目类型
                            exerciseDetail.ExerciseName = dr["column2"].ToString(); //题目名称  
                            if (dr["column1"].ToString() == "3") //判断题
                            {
                                switch (dr["column4"].ToString())
                                {
                                    case "A":
                                        exerciseDetail.Answer = dr["column6"].ToString() == "正确" ? "0" : "1";
                                        break;
                                    case "B":
                                        exerciseDetail.Answer = dr["column7"].ToString() == "正确" ? "0" : "1";
                                        break;
                                }
                            }
                            else
                            {
                                exerciseDetail.Answer = dr["column4"].ToString(); //答案
                            }
                            exerciseDetail.Score = int.Parse(dr["column5"].ToString()); //分数
                            string exerciseOption = string.Empty;
                            string[] letter = { "A", "B", "C", "D", "E", "F", "G", "H" }; //选项字典
                            for (int j = 6; j <= dt.Columns.Count; j++)
                            {
                                if (!string.IsNullOrEmpty(dr["column" + j].ToString()))
                                {
                                    exerciseOption += letter[j - 6] + "." + dr["column" + j].ToString() + ",|";
                                }
                            }
                            char[] myChar = { ',', '|' };
                            exerciseDetail.Option = exerciseOption.TrimEnd(myChar); //题目选项
                            exerciseDetail.InputType = 2; //录入类型
                            exerciseDetail.Status = 1;
                            exerciseDetail.CreatedDate = DateTime.Now;
                            exerciseDetail.CreatedUser = CurrentUser.GetCurrentUser().LoginName;
                            exerciseDetail.ChangeDate = DateTime.Now;
                            exerciseDetail.BusinessExercisesVguid = Guid.Empty;
                            exerciseDetail.Vguid = Guid.NewGuid();
                            exerciseDetailList.Add(exerciseDetail);
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.LogHelper.LogHelper.WriteLog(ex.Message + "/n" + ex.ToString() + "/n" + ex.StackTrace);
                    }
                }
            }

            return exerciseDetailList;
        }

        /// <summary>
        /// 保存习题信息
        /// </summary>
        /// <param name="isEdit"></param>
        /// <param name="exerciseDetailList"></param>
        /// <returns></returns>
        public bool SaveExercise(List<Business_ExerciseLibrary_Information> exerciseDetailList, bool isEdit)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                db.BeginTran();
                try
                {
                    if (isEdit)
                    {
                        foreach (var libraryInformation in exerciseDetailList)
                        {
                            db.Update<Business_ExerciseLibrary_Information>(
                                new
                                {
                                    ExerciseName = libraryInformation.ExerciseName,
                                    Option = libraryInformation.Option,
                                    Answer = libraryInformation.Answer,
                                    Score = libraryInformation.Score,
                                    ChangeUser = libraryInformation.ChangeUser,
                                    ChangeDate = libraryInformation.ChangeDate,
                                }, c => c.Vguid == libraryInformation.Vguid);
                            //存入操作日志表
                            string logData = JsonHelper.ModelToJson(libraryInformation);
                            _logLogic.SaveLog(4, 38, Common.CurrentUser.GetCurrentUser().LoginName, libraryInformation.ExerciseName + " " + "习题详细信息", logData);
                        }
                    }
                    else
                    {
                        foreach (var libraryInformation in exerciseDetailList)
                        {
                            db.Insert(libraryInformation, false);
                            //存入操作日志表
                            string logData = JsonHelper.ModelToJson(libraryInformation);

                            _logLogic.SaveLog(1, 38, Common.CurrentUser.GetCurrentUser().LoginName, libraryInformation.ExerciseName + " " + "习题详细信息", logData);
                        }
                    }
                    db.CommitTran();
                    return true;
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                    Common.LogHelper.LogHelper.WriteLog(ex.Message + "/n" + ex.ToString() + "/n" + ex.StackTrace);
                    return false;
                }


            }
        }
        /// <summary>
        /// 获取习题的详细信息
        /// </summary>
        /// <param name="vguid">习题的vguid</param>
        /// <returns></returns>
        public v_Business_ExercisesLibrary_Infomation GetExerciseDetailInfo(Guid vguid)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                return db.Queryable<v_Business_ExercisesLibrary_Infomation>().Where(c => c.Vguid == vguid).SingleOrDefault();
            }
        }

    }
}