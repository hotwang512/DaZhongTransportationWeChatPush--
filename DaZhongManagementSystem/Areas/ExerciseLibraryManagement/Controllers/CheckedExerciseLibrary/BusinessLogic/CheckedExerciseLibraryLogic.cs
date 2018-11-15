using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.ExerciseLibraryManagement;

namespace DaZhongManagementSystem.Areas.ExerciseLibraryManagement.Controllers.CheckedExerciseLibrary.BusinessLogic
{
    public class CheckedExerciseLibraryLogic
    {
        private CheckedExerciseLibraryServer _checkedExerciseLibraryServer;
        public CheckedExerciseLibraryLogic()
        {
            _checkedExerciseLibraryServer = new CheckedExerciseLibraryServer();
        }
        /// <summary>
        /// 分页查询习题信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<v_Business_ExercisesLibrary_Infomation> GetCheckedExerciseListBySearch(Business_Exercises_Infomation_Search searchParam, GridParams para)
        {
            return _checkedExerciseLibraryServer.GetCheckedExerciseListBySearch(searchParam, para);
        }

        /// <summary>
        /// 随机生成习题
        /// </summary>
        /// <returns></returns>
        public List<v_Business_ExercisesLibrary_Infomation> RandomExercise()
        {
            return _checkedExerciseLibraryServer.RandomExercise();
        }

        /// <summary>
        /// 获取正式习题的分值数量
        /// </summary>
        /// <returns></returns>
        public List<v_ExerciseScore_Information> GetExerciseScoreInfo()
        {
            return _checkedExerciseLibraryServer.GetExerciseScoreInfo();
        }

        /// <summary>
        /// 将习题状态变成草稿
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public bool BackToDraftStatus(List<Guid> vguidList)
        {
            bool result = false;
            foreach (var guid in vguidList)
            {
                result = _checkedExerciseLibraryServer.BackToDraftStatus(guid);
            }
            return result;
        }
    }
}