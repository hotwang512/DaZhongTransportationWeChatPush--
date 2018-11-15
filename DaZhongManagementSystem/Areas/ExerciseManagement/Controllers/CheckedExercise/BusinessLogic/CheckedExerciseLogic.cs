using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.ExerciseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.ExerciseManagement.Controllers.CheckedExercise.BusinessLogic
{
    public class CheckedExerciseLogic
    {
        public CheckedExerciseServer _cs;
        public CheckedExerciseLogic()
        {
            _cs = new CheckedExerciseServer();
        }

        /// <summary>
        /// 绑定习题状态
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetExerciseStatus()
        {
            return _cs.GetExerciseStatus();
        }

        /// <summary>
        /// 绑定习题录入类型
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetInputType()
        {
            return _cs.GetInputType();
        }

        /// <summary>
        /// 绑定习题类型
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetExerciseType()
        {
            return _cs.GetExerciseType();
        }

        /// <summary>
        /// 分页查询习题信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<V_Business_Exercises_Infomation> GetExerciseListBySearch(Business_Exercises_Infomation_Search searchParam, GridParams para)
        {
            return _cs.GetExerciseListBySearch(searchParam, para);
        }

        /// <summary>
        /// 批量删除习题
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        //public bool DeletedExercise(string[] vguidList)
        //{
        //    bool result = false;
        //    foreach (var item in vguidList)
        //    {
        //        result = _cs.DeletedExercise(item);
        //    }
        //    return result;
        //}

        /// <summary>
        /// 通过Vguid获取习题信息（编辑）
        /// </summary>
        /// <param name="Vguid"></param>
        /// <returns></returns>
        public Business_Exercises_Infomation GetExerciseByVguid(string Vguid)
        {
            return _cs.GetExerciseByVguid(Vguid);
        }

        /// <summary>
        /// 通过习题主信息的Vguid获取习题详细列表
        /// </summary>
        /// <param name="Vguid"></param>
        /// <returns></returns>
        public List<Business_ExercisesDetail_Infomation> GetExerciseDetailListByMainVguid(string Vguid)
        {
            return _cs.GetExerciseDetailListByMainVguid(Vguid);
        }
    }
}