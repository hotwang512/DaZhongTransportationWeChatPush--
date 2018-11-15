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

namespace DaZhongManagementSystem.Infrastructure.ExerciseManagement
{
    public class ExerciseServer
    {
        /// <summary>
        /// 日志
        /// </summary>
        public LogLogic _ll;
        public ExerciseServer()
        {
            _ll = new LogLogic();
        }

        /// <summary>
        /// 分页查询习题信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<V_Business_Exercises_Infomation> GetExerciseListBySearch(Business_Exercises_Infomation_Search searchParam, GridParams para)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                JsonResultModel<V_Business_Exercises_Infomation> jsonResult = new JsonResultModel<V_Business_Exercises_Infomation>();
                var query = _dbMsSql.Queryable<V_Business_Exercises_Infomation>().Where(i => i.Status == 1);
                if (!string.IsNullOrEmpty(searchParam.ExercisesName))
                {
                    query.Where(i => i.ExercisesName.Contains(searchParam.ExercisesName));
                }
                //if (!string.IsNullOrEmpty(searchParam.Status))
                //{
                //    int status = int.Parse(searchParam.Status);
                //    query.Where(i => i.Status == status);
                //}
                if (!string.IsNullOrEmpty(searchParam.InputType))
                {
                    int inputType = int.Parse(searchParam.InputType);
                    query.Where(i => i.InputType == inputType);
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
                string logData = JsonHelper.ModelToJson<JsonResultModel<V_Business_Exercises_Infomation>>(jsonResult);
                _ll.SaveLog(3, 7, Common.CurrentUser.GetCurrentUser().LoginName, "习题列表", logData);

                return jsonResult;
            }
        }

        /// <summary>
        /// 上传习题Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool InsertExcelToDatabase(DataTable dt)
        {
            //SaveExerciseMain(Business_Exercises_Infomation exerciseMainModel, bool isEdit, List<Business_ExercisesDetail_Infomation> exerciseDetailList)
            bool result = false;
            Business_Exercises_Infomation exercisesInfomation = DataTableToModel(dt);
            List<Business_ExercisesDetail_Infomation> exerciseDetailList = DataTableToList(dt, exercisesInfomation);
            if (exercisesInfomation == null && exerciseDetailList.Count == 0)
            {
                result = false;
            }
            else
            {
                result = SaveExerciseMain(exercisesInfomation, false, exerciseDetailList);
            }
            return result;
        }

        /// <summary>
        /// 将DataTable内容绑定到习题主信息实体
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public Business_Exercises_Infomation DataTableToModel(DataTable dt)
        {
            Business_Exercises_Infomation exercisesInfomation = new Business_Exercises_Infomation();
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    exercisesInfomation.ExercisesName = dt.Rows[0]["Column2"].ToString();
                    exercisesInfomation.CreatedDate = DateTime.Now;
                    exercisesInfomation.EffectiveDate = DateTime.Parse(dt.Rows[0]["Column4"].ToString());
                    exercisesInfomation.Description = dt.Rows[1]["Column2"].ToString();
                    exercisesInfomation.Remarks = dt.Rows[2]["Column2"].ToString();
                    exercisesInfomation.Status = 1;
                    exercisesInfomation.InputType = 2;
                    exercisesInfomation.Vguid = Guid.NewGuid();
                    exercisesInfomation.ChangeDate = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                Common.LogHelper.LogHelper.WriteLog(ex.Message + "/n" + ex.ToString() + "/n" + ex.StackTrace);
            }

            return exercisesInfomation;
        }

        /// <summary>
        /// 将DataTable内容绑定到习题详细信息列表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<Business_ExercisesDetail_Infomation> DataTableToList(DataTable dt, Business_Exercises_Infomation exercisesInfomation)
        {
            List<Business_ExercisesDetail_Infomation> exerciseDetailList = new List<Business_ExercisesDetail_Infomation>();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 7; i < dt.Rows.Count; i++) //从第7行开始插入,前6行为习题主信息以及注意事项
                {
                    try
                    {
                        DataRow dr = dt.Rows[i];
                        if ((!string.IsNullOrEmpty(dr["column1"].ToString())) && (!string.IsNullOrEmpty(dr["column2"].ToString())) && (!string.IsNullOrEmpty(dr["column3"].ToString())) && (!string.IsNullOrEmpty(dr["column6"].ToString())))
                        {
                            Business_ExercisesDetail_Infomation exerciseDetail = new Business_ExercisesDetail_Infomation();
                            exerciseDetail.ExerciseType = int.Parse(dr["column1"].ToString());//题目类型
                            exerciseDetail.ExericseTitleID = int.Parse(dr["column2"].ToString());//题号
                            exerciseDetail.ExerciseName = dr["column3"].ToString();//题目名称   
                            if (dr["column1"].ToString() == "3") //判断题
                            {
                                switch (dr["column5"].ToString())
                                {
                                    case "A":
                                        exerciseDetail.Answer = dr["column7"].ToString() == "正确" ? "0" : "1";
                                        break;
                                    case "B":
                                        exerciseDetail.Answer = dr["column8"].ToString() == "正确" ? "0" : "1";
                                        break;
                                }
                            }
                            else
                            {
                                exerciseDetail.Answer = dr["column5"].ToString();//答案
                            }
                            exerciseDetail.Score = int.Parse(dr["column6"].ToString());//分数
                            string exerciseOption = string.Empty;
                            string[] letter = { "A", "B", "C", "D", "E", "F", "G", "H" };//选项字典
                            for (int j = 7; j <= dt.Columns.Count; j++)
                            {
                                if (!string.IsNullOrEmpty(dr["column" + j].ToString()))
                                {
                                    exerciseOption += letter[j - 7] + "." + dr["column" + j].ToString() + ",|";
                                }
                            }
                            char[] myChar = { ',', '|' };
                            exerciseDetail.ExerciseOption = exerciseOption.TrimEnd(myChar);//题目选项
                            exerciseDetail.InputType = 2;//录入类型
                            exerciseDetail.CreatedDate = DateTime.Now;
                            exerciseDetail.CreatedUser = Common.CurrentUser.GetCurrentUser().LoginName;
                            exerciseDetail.ChangeDate = DateTime.Now;
                            exerciseDetail.ExercisesInformationVguid = exercisesInfomation.Vguid;
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
        /// 绑定习题状态
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetExerciseStatus()
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(DaZhongManagementSystem.Common.Tools.MasterVGUID.ExercisesStatus);
                return _dbMsSql.Queryable<CS_Master_2>().Where(i => i.VGUID == vguid).ToList();
            }
        }

        /// <summary>
        /// 绑定习题录入类型
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetInputType()
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(DaZhongManagementSystem.Common.Tools.MasterVGUID.InputType);
                return _dbMsSql.Queryable<CS_Master_2>().Where(i => i.VGUID == vguid).ToList();
            }
        }

        /// <summary>
        /// 绑定习题类型
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetExerciseType()
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(DaZhongManagementSystem.Common.Tools.MasterVGUID.ExerciseType);
                return _dbMsSql.Queryable<CS_Master_2>().Where(i => i.VGUID == vguid).ToList().OrderBy("MasterCode", OrderByType.Asc).ToList();
            }
        }

        /// <summary>
        /// 通过Vguid获取习题信息（编辑）
        /// </summary>
        /// <param name="Vguid"></param>
        /// <returns></returns>
        public Business_Exercises_Infomation GetExerciseByVguid(string Vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(Vguid);
                Business_Exercises_Infomation exerciseInfoModel = _dbMsSql.Queryable<Business_Exercises_Infomation>().Where(i => i.Vguid == vguid).SingleOrDefault();

                //存入操作日志表
                string logData = JsonHelper.ModelToJson<Business_Exercises_Infomation>(exerciseInfoModel);
                _ll.SaveLog(3, 10, Common.CurrentUser.GetCurrentUser().LoginName, exerciseInfoModel.ExercisesName, logData);

                return exerciseInfoModel;
            }
        }

        /// <summary>
        /// 通过习题主信息的Vguid获取习题详细列表
        /// </summary>
        /// <param name="Vguid"></param>
        /// <returns></returns>
        public List<Business_ExercisesDetail_Infomation> GetExerciseDetailListByMainVguid(string Vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                Guid mainVguid = Guid.Parse(Vguid);
                return _dbMsSql.Queryable<Business_ExercisesDetail_Infomation>().Where(i => i.ExercisesInformationVguid == mainVguid).ToList().OrderBy("ExericseTitleID", OrderByType.Asc).ToList();

            }
        }

        /// <summary>
        /// 验证习题总分是否为100分
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public bool CheckScore(string[] vguidList)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                foreach (var item in vguidList)
                {
                    Guid Vguid = Guid.Parse(item);
                    int score = 0;
                    List<Business_ExercisesDetail_Infomation> exercisesDetailInfomation = new List<Business_ExercisesDetail_Infomation>();
                    exercisesDetailInfomation = _dbMsSql.Queryable<Business_ExercisesDetail_Infomation>().Where(i => i.ExercisesInformationVguid == Vguid).ToList();
                    foreach (var i in exercisesDetailInfomation)
                    {
                        score += i.Score;
                    }
                    if (score == 100)
                    {
                        result = true;
                    }
                    else
                    {
                        return result;
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 审核提交习题
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
                    result = _dbMsSql.Update<Business_Exercises_Infomation>(new { Status = 2 }, i => i.Vguid == Vguid);
                    _dbMsSql.Update<Business_ExerciseLibrary_Information>(new { status = 2 }, i => i.BusinessExercisesVguid == Vguid);
                    Business_Exercises_Infomation exerciseInfo = _dbMsSql.Queryable<Business_Exercises_Infomation>().Where(i => i.Vguid == Vguid).SingleOrDefault();
                    string exerciseJson = JsonHelper.ModelToJson(exerciseInfo);
                    //存入操作日志表
                    _ll.SaveLog(9, 7, CurrentUser.GetCurrentUser().LoginName, exerciseInfo.ExercisesName, exerciseJson);

                    _dbMsSql.CommitTran();
                }
                catch (Exception ex)
                {
                    _dbMsSql.RollbackTran();
                    Common.LogHelper.LogHelper.WriteLog(ex.ToString());
                    _ll.SaveLog(5, 7, CurrentUser.GetCurrentUser().LoginName, "", ex.ToString());
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
                    Business_Exercises_Infomation exerciseModel = _dbMsSql.Queryable<Business_Exercises_Infomation>().Where(i => i.Vguid == exerciseVguid).SingleOrDefault();
                    string logData = JsonHelper.ModelToJson(exerciseModel);
                    result = _dbMsSql.Delete<Business_Exercises_Infomation>(i => i.Vguid == exerciseVguid);     //删除习题主表
                    if (result)
                    {
                        _dbMsSql.Delete<Business_ExerciseLibrary_Information>(i => i.BusinessExercisesVguid == exerciseVguid);
                        List<Business_ExercisesDetail_Infomation> exercisesDetail = _dbMsSql.Queryable<Business_ExercisesDetail_Infomation>().Where(i => i.ExercisesInformationVguid == exerciseVguid).ToList();
                        if (exercisesDetail.Count != 0)
                        {
                            result = _dbMsSql.Delete<Business_ExercisesDetail_Infomation>(i => i.ExercisesInformationVguid == exerciseVguid);       //删除习题附表
                        }
                    }

                    _ll.SaveLog(2, 7, Common.CurrentUser.GetCurrentUser().LoginName, exerciseModel.ExercisesName, logData);
                    _dbMsSql.CommitTran();
                }
                catch (Exception exp)
                {
                    _dbMsSql.RollbackTran();
                    Common.LogHelper.LogHelper.WriteLog(exp.ToString());
                    _ll.SaveLog(5, 7, Common.CurrentUser.GetCurrentUser().LoginName, "", exp.ToString());
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
                List<Business_ExercisesAnswer_Information> exerciseAnswerList = new List<Business_ExercisesAnswer_Information>();
                exerciseAnswerList = _dbMsSql.Queryable<Business_ExercisesAnswer_Information>().Where(i => i.BusinessExercisesVguid == vguid && i.BusinessPersonnelVguid == personVGUID).ToList();
                return exerciseAnswerList.Count();
            }
        }

        /// <summary>
        /// 保存习题信息(主信息、详细信息)
        /// </summary>
        /// <param name="exerciseMainModel"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        public bool SaveExerciseMain(Business_Exercises_Infomation exerciseMainModel, bool isEdit, List<Business_ExercisesDetail_Infomation> exerciseDetailList)
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
                            ExercisesName = exerciseMainModel.ExercisesName,
                            Description = exerciseMainModel.Description,
                            Remarks = exerciseMainModel.Remarks,
                            EffectiveDate = exerciseMainModel.EffectiveDate,
                            Status = exerciseMainModel.Status,
                            ChangeUser = exerciseMainModel.ChangeUser,
                            ChangeDate = exerciseMainModel.ChangeDate
                        };
                        result = _dbMsSql.Update<Business_Exercises_Infomation>(model, i => i.Vguid == exerciseMainModel.Vguid);
                        //存入操作日志表
                        string logData = JsonHelper.ModelToJson(exerciseMainModel);
                        _ll.SaveLog(4, 9, CurrentUser.GetCurrentUser().LoginName, exerciseMainModel.ExercisesName + " " + "主信息", logData);
                    }
                    else
                    {
                        result = _dbMsSql.Insert(exerciseMainModel, false) != DBNull.Value;

                        //存入操作日志表
                        string logData = JsonHelper.ModelToJson(exerciseMainModel);
                        _ll.SaveLog(1, 8, CurrentUser.GetCurrentUser().LoginName, exerciseMainModel.ExercisesName + " " + "主信息", logData);
                    }
                    _dbMsSql.Delete<Business_ExercisesDetail_Infomation>(i => i.ExercisesInformationVguid == exerciseMainModel.Vguid);
                    var listExerciseLibrary = new List<Business_ExerciseLibrary_Information>();
                    foreach (var item in exerciseDetailList)
                    {
                        _dbMsSql.Delete<Business_ExerciseLibrary_Information>(c => c.BusinessExercisesVguid == item.ExercisesInformationVguid);
                        if (item.IsEntry == "1")
                        {
                            bool isExist = _dbMsSql.Queryable<Business_ExerciseLibrary_Information>().Any(c => c.ExerciseType == item.ExerciseType && c.ExerciseName == item.ExerciseName);
                            if (!isExist)
                            {
                                //将习题保存到习题库中
                                var model = new Business_ExerciseLibrary_Information()
                                {
                                    ExerciseName = item.ExerciseName,
                                    ExerciseType = item.ExerciseType,
                                    Option = item.ExerciseOption,
                                    Answer = item.Answer,
                                    Score = item.Score,
                                    BusinessExercisesVguid = item.ExercisesInformationVguid,
                                    Status = 1,
                                    InputType = 3,
                                    TopicType = 0,
                                    CreatedUser = item.CreatedUser,
                                    CreatedDate = item.CreatedDate,
                                    Vguid = Guid.NewGuid()
                                };
                                listExerciseLibrary.Add(model);
                            }
                        }
                        _dbMsSql.Insert(item, false);

                        //存入操作日志表
                        string logData = JsonHelper.ModelToJson(item);
                        _ll.SaveLog(4, 10, CurrentUser.GetCurrentUser().LoginName, exerciseMainModel.ExercisesName + " " + "习题详细信息", logData);
                    }
                    _dbMsSql.InsertRange(listExerciseLibrary);
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
        /// 通过习题Vguid获取习题详细信息
        /// </summary>
        /// <param name="exerciseDetailVguid"></param>
        /// <returns></returns>
        public Business_ExercisesDetail_Infomation GetExerciseDetailModel(string exerciseDetailVguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(exerciseDetailVguid);
                Business_ExercisesDetail_Infomation exerciseModel = _dbMsSql.Queryable<Business_ExercisesDetail_Infomation>().Where(i => i.Vguid == vguid).SingleOrDefault();

                return exerciseModel;
            }
        }

    }
}
