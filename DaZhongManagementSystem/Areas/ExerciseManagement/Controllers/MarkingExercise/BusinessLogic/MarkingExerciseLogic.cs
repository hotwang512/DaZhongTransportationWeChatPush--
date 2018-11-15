using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.ExerciseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.ExerciseManagement.Controllers.MarkingExercise.BusinessLogic
{
    public class MarkingExerciseLogic
    {
        public MarkingExerciseServer _markingExerciseServer;
        public MarkingExerciseLogic()
        {
            _markingExerciseServer = new MarkingExerciseServer();
        }

        /// <summary>
        /// 判断此习题是否阅过
        /// </summary>
        /// <param name="exerciseVguid"></param>
        /// <param name="personVguid"></param>
        /// <returns></returns>
        public bool IsExerciseMarked(string exerciseVguid, string personVguid)
        {
            return _markingExerciseServer.IsExerciseMarked(exerciseVguid, personVguid);
        }

        /// <summary>
        /// 获取习题列表
        /// </summary>
        /// <returns></returns>
        public List<V_ExercisesUserInformation> GetExerciseList()
        {
            return _markingExerciseServer.GetExerciseList();
        }

        /// <summary>
        /// 获取答过本套习题人员列表
        /// </summary>
        /// <param name="vguid">习题主Vguid</param>
        /// <returns></returns>
        public JsonResultModel<U_MarkingPerson> GetAnsweredPersonList(string vguid, string isMarking, GridParams para)
        {
            return _markingExerciseServer.GetAnsweredPersonList(vguid, isMarking,para);
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
            return _markingExerciseServer.SaveShortAnswerMarking(personVguid, vguid, exerciseDetailVguid, score);
        }
    }
}