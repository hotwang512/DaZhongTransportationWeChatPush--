using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Pdf.Generator;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;

namespace DaZhongManagementSystem.Infrastructure.ExerciseLibraryManagement
{
    public class CheckedExerciseLibraryServer
    {
        /// <summary>
        /// 日志
        /// </summary>
        private LogLogic _LogLogic;
        public CheckedExerciseLibraryServer()
        {
            _LogLogic = new LogLogic();
        }
        public JsonResultModel<v_Business_ExercisesLibrary_Infomation> GetCheckedExerciseListBySearch(Business_Exercises_Infomation_Search searchParam, GridParams para)
        {
            using (SqlSugarClient db = SugarDao_MsSql.GetInstance())
            {

                JsonResultModel<v_Business_ExercisesLibrary_Infomation> jsonResult = new JsonResultModel<v_Business_ExercisesLibrary_Infomation>();
                var query = db.Queryable<v_Business_ExercisesLibrary_Infomation>().Where(i => i.Status == 2);
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

                if (!string.IsNullOrEmpty(searchParam.CreatedTimeStart) && !string.IsNullOrEmpty(searchParam.CreatedTimeEnd))
                {
                    DateTime createdTimeStart = DateTime.Parse(searchParam.CreatedTimeStart);
                    DateTime createdTimeEnd = DateTime.Parse(searchParam.CreatedTimeEnd);
                    query.Where(i => i.CreatedDate > createdTimeStart && i.CreatedDate < createdTimeEnd);
                }
                else
                {
                    if (!string.IsNullOrEmpty(searchParam.CreatedTimeStart))
                    {
                        DateTime createdTimeStart = DateTime.Parse(searchParam.CreatedTimeStart);
                        query.Where(i => i.CreatedDate > createdTimeStart);
                    }
                    if (!string.IsNullOrEmpty(searchParam.CreatedTimeEnd))
                    {
                        DateTime createdTimeEnd = DateTime.Parse(searchParam.CreatedTimeEnd);
                        query.Where(i => i.CreatedDate < createdTimeEnd);
                    }
                }

                query.OrderBy(para.sortdatafield + " " + para.sortorder);
                var pageCount = 0;
                jsonResult.Rows = query.ToPageList(para.pagenum, para.pagesize, ref pageCount);
                jsonResult.TotalRows = pageCount;
                //存入操作日志表
                string logData = JsonHelper.ModelToJson(jsonResult);
                _LogLogic.SaveLog(3, 39, CurrentUser.GetCurrentUser().LoginName, "正式习题列表", logData);
                return jsonResult;
            }
        }

        /// <summary>
        /// 随机生成习题
        /// </summary>
        /// <returns></returns>
        public List<v_Business_ExercisesLibrary_Infomation> RandomExercise()
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                return db.SqlQuery<v_Business_ExercisesLibrary_Infomation>("exec usp_RandomExerciseLibrary").ToList();
            }
        }

        /// <summary>
        /// 获取正式习题的分值数量
        /// </summary>
        /// <returns></returns>
        public List<v_ExerciseScore_Information> GetExerciseScoreInfo()
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                return db.Queryable<v_ExerciseScore_Information>().ToList();
            }
        }
        /// <summary>
        /// 将习题状态变成草稿
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public bool BackToDraftStatus(Guid vguid)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var list = db.Queryable<v_Business_ExercisesLibrary_Infomation>().Where(i => i.Vguid == vguid).ToList();
                //存入操作日志表
                string logData = JsonHelper.ModelToJson(list);
                _LogLogic.SaveLog(17, 39, CurrentUser.GetCurrentUser().LoginName, "正式习题列表", logData);
                return db.Update<Business_ExerciseLibrary_Information>(new { Status = 1 }, i => i.Vguid == vguid);
            }
        }

    }
}
