using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Infrastructure.ExerciseManagement
{
    public class CheckedExerciseServer
    {
        /// <summary>
        /// 日志
        /// </summary>
        public LogLogic _ll;
        public CheckedExerciseServer()
        {
            _ll = new LogLogic();
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
        /// 分页查询已审核习题信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<V_Business_Exercises_Infomation> GetExerciseListBySearch(Business_Exercises_Infomation_Search searchParam, GridParams para)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                JsonResultModel<V_Business_Exercises_Infomation> jsonResult = new JsonResultModel<V_Business_Exercises_Infomation>();
                //只查询三个月内的数据
                // DateTime endDate = DateTime.Now.AddMonths(-3);

                var query = _dbMsSql.Queryable<V_Business_Exercises_Infomation>().Where(i => i.Status == 2);
                if (!string.IsNullOrEmpty(searchParam.ExercisesName))
                {
                    query.Where(i => i.ExercisesName.Contains(searchParam.ExercisesName));
                }
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

                //if (string.IsNullOrEmpty(searchParam.CreatedTimeStart) && string.IsNullOrEmpty(searchParam.CreatedTimeEnd))
                //{
                //    query.Where(i => i.CreatedDate > endDate);
                //}
                if (!string.IsNullOrEmpty(searchParam.CreatedTimeStart) && !string.IsNullOrEmpty(searchParam.CreatedTimeEnd))
                {
                    DateTime createdTimeStart = DateTime.Parse(searchParam.CreatedTimeStart);
                    DateTime createdTimeEnd = DateTime.Parse(searchParam.CreatedTimeEnd);
                    query.Where(i => i.CreatedDate > createdTimeStart && i.CreatedDate < createdTimeEnd); // && i.CreatedDate > endDate
                }
                else
                {
                    if (!string.IsNullOrEmpty(searchParam.CreatedTimeStart))
                    {
                        DateTime createdTimeStart = DateTime.Parse(searchParam.CreatedTimeStart);
                        query.Where(i => i.CreatedDate > createdTimeStart); //&& i.CreatedDate > endDate
                    }
                    if (!string.IsNullOrEmpty(searchParam.CreatedTimeEnd))
                    {
                        DateTime createdTimeEnd = DateTime.Parse(searchParam.CreatedTimeEnd);
                        query.Where(i => i.CreatedDate < createdTimeEnd); //&& i.CreatedDate > endDate
                    }
                }

                query.OrderBy(para.sortdatafield + " " + para.sortorder);
                jsonResult.TotalRows = query.Count();
                jsonResult.Rows = query.ToPageList(para.pagenum, para.pagesize);

                //存入操作日志表
                string logData = JsonHelper.ModelToJson<JsonResultModel<V_Business_Exercises_Infomation>>(jsonResult);
                _ll.SaveLog(3, 11, Common.CurrentUser.GetCurrentUser().LoginName, "已审核习题列表", logData);

                return jsonResult;
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
                _ll.SaveLog(3, 11, Common.CurrentUser.GetCurrentUser().LoginName, exerciseInfoModel.ExercisesName, logData);

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
    }
}
