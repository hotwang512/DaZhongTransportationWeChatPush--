using System;
using System.Collections.Generic;
using System.Data;
using DaZhongManagementSystem.Common.Tools;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.ExerciseLibraryManagement;

namespace DaZhongManagementSystem.Areas.ExerciseLibraryManagement.Controllers.ExerciseLibraryManagement.BusinessLogic
{
    public class ExerciseLibraryLogic
    {
        private ExerciseLibraryServer _exerciseLibraryServer;

        public ExerciseLibraryLogic()
        {
            _exerciseLibraryServer = new ExerciseLibraryServer();
        }
        /// <summary>
        /// 分页查询习题信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<v_Business_ExercisesLibrary_Infomation> GetExerciseListBySearch(Business_Exercises_Infomation_Search searchParam, GridParams para)
        {
            return _exerciseLibraryServer.GetExerciseListBySearch(searchParam, para);
        }

        /// <summary>
        ///  批量审核习题
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public bool CheckedExercise(string[] vguidList)
        {
            bool result = false;
            foreach (var item in vguidList)
            {
                result = _exerciseLibraryServer.CheckedExercise(item);
            }
            return result;
        }

        /// <summary>
        /// 批量删除习题
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public bool DeletedExercise(string[] vguidList)
        {
            bool result = false;
            foreach (var item in vguidList)
            {
                result = _exerciseLibraryServer.DeletedExercise(item);
            }
            return result;
        }

        /// <summary>
        /// 下载习题模板
        /// </summary>
        public void DownLoadTemplate()
        {
            string exerciseFileName = SyntacticSugar.ConfigSugar.GetAppString("ExerciseLibraryFileName");
            UploadHelper.ExportExcels("ExerciseLibraryTemplate.xlsx", exerciseFileName);
        }

        /// <summary>
        /// 上传习题Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool InsertExcelToDatabase(DataTable dt)
        {
            return _exerciseLibraryServer.InsertExcelToDatabase(dt);
        }

        /// <summary>
        /// 绑定习题库录入类型
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetInputType()
        {
            return _exerciseLibraryServer.GetInputType();
        }


        /// <summary>
        /// 获取习题的详细信息
        /// </summary>
        /// <param name="vguid">习题的vguid</param>
        /// <returns></returns>
        public v_Business_ExercisesLibrary_Infomation GetExerciseDetailInfo(Guid vguid)
        {
            return _exerciseLibraryServer.GetExerciseDetailInfo(vguid);
        }

        /// <summary>
        /// 保存习题信息
        /// </summary>
        /// <param name="exerciseData">习题信息</param>
        /// <param name="isEdit">是否编辑</param>
        /// <returns></returns>
        public bool SaveExercise(string exerciseData, bool isEdit)
        {
            bool result = false;
            List<Business_ExerciseLibrary_Information> exerciseList = Common.JsonHelper.JsonToModel<List<Business_ExerciseLibrary_Information>>(exerciseData);//待保存的习题详细信息
            List<Business_ExerciseLibrary_Information> exerciseDetailList = new List<Business_ExerciseLibrary_Information>();
            if (isEdit)
            {
                //循环遍历习题详情
                foreach (var item in exerciseList)
                {
                    Business_ExerciseLibrary_Information exerciseDetail = new Business_ExerciseLibrary_Information();
                    //  exerciseDetail.ExerciseType = item.ExerciseType;
                    //   exerciseDetail.ExericseTitleID = item.ExericseTitleID;
                    exerciseDetail.ExerciseName = item.ExerciseName;
                    exerciseDetail.Option = item.Option;
                    exerciseDetail.Answer = item.Answer;
                    exerciseDetail.Score = item.Score;
                    // exerciseDetail.InputType = 1;
                    exerciseDetail.Vguid = item.Vguid;
                    // exerciseDetail.ExercisesInformationVguid = exerciseMainModel.Vguid;//习题主信息Vguid
                    //  exerciseDetail.CreatedUser = Common.CurrentUser.GetCurrentUser().LoginName;
                    //exerciseDetail.CreatedDate = DateTime.Now;
                    exerciseDetail.ChangeDate = DateTime.Now;
                    exerciseDetail.ChangeUser = Common.CurrentUser.GetCurrentUser().LoginName;
                    exerciseDetailList.Add(exerciseDetail);
                }
                result = _exerciseLibraryServer.SaveExercise(exerciseDetailList, isEdit);
            }
            else
            {
                foreach (var item in exerciseList)
                {
                    Business_ExerciseLibrary_Information exerciseDetail = new Business_ExerciseLibrary_Information();
                    exerciseDetail.ExerciseType = item.ExerciseType;
                    exerciseDetail.ExerciseName = item.ExerciseName;
                    exerciseDetail.Option = item.Option;
                    exerciseDetail.Answer = item.Answer;
                    exerciseDetail.Score = item.Score;
                    exerciseDetail.InputType = 1;
                    exerciseDetail.Status = 1;
                    exerciseDetail.Vguid = Guid.NewGuid();
                    exerciseDetail.BusinessExercisesVguid = Guid.Empty;//习题主信息Vguid
                    exerciseDetail.CreatedUser = Common.CurrentUser.GetCurrentUser().LoginName;
                    exerciseDetail.CreatedDate = DateTime.Now;
                    //  exerciseDetail.ChangeDate = DateTime.Now;
                    exerciseDetailList.Add(exerciseDetail);
                }
                result = _exerciseLibraryServer.SaveExercise(exerciseDetailList, isEdit);
            }
            return result;
        }
    }
}