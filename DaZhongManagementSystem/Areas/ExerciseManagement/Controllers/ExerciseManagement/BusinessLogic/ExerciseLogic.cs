using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.BasicDataManagement;
using DaZhongManagementSystem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DaZhongManagementSystem.Common.LogHelper;
using DaZhongManagementSystem.Infrastructure.ExerciseManagement;
using Aspose.Cells;
using System.Data;
using DaZhongManagementSystem.Common.Tools;

namespace DaZhongManagementSystem.Areas.ExerciseManagement.Controllers.ExerciseManagement.BusinessLogic
{
    public class ExerciseLogic
    {
        public ExerciseServer _es;
        public ExerciseLogic()
        {
            _es = new ExerciseServer();
        }

        /// <summary>
        /// 分页查询习题信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<V_Business_Exercises_Infomation> GetExerciseListBySearch(Business_Exercises_Infomation_Search searchParam, GridParams para)
        {
            return _es.GetExerciseListBySearch(searchParam, para);
        }

        /// <summary>
        /// 下载习题模板
        /// </summary>
        public void DownLoadTemplate()
        {
            string exerciseFileName = SyntacticSugar.ConfigSugar.GetAppString("ExerciseFileName");
            UploadHelper.ExportExcels("ExerciseTemplate.xlsx", exerciseFileName);
        }

        /// <summary>
        /// 上传习题Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool InsertExcelToDatabase(DataTable dt)
        {
            return _es.InsertExcelToDatabase(dt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routList"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetRoutName(List<string[]> routList, int index)
        {
            if (routList == null)
                return "";
            if (routList.Count == 0)
                return "";
            return routList[routList.Count - 1][index];
        }

        /// <summary>
        /// 绑定习题状态
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetExerciseStatus()
        {
            return _es.GetExerciseStatus();
        }

        /// <summary>
        /// 绑定习题录入类型
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetInputType()
        {
            return _es.GetInputType();
        }

        /// <summary>
        /// 绑定习题类型
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetExerciseType()
        {
            return _es.GetExerciseType();
        }

        /// <summary>
        /// 通过Vguid获取习题信息（编辑）
        /// </summary>
        /// <param name="Vguid"></param>
        /// <returns></returns>
        public Business_Exercises_Infomation GetExerciseByVguid(string Vguid)
        {
            return _es.GetExerciseByVguid(Vguid);
        }

        /// <summary>
        /// 验证习题总分是否为100分
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public bool CheckScore(string[] vguidList)
        {
            return _es.CheckScore(vguidList);
        }

        /// <summary>
        /// 批量审核习题
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public bool CheckedExercise(string[] vguidList)
        {
            bool result = false;
            foreach (var item in vguidList)
            {
                result = _es.CheckedExercise(item);
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
                result = _es.DeletedExercise(item);
            }
            return result;
        }

        /// <summary>
        /// 通过习题主信息的Vguid获取习题详细列表
        /// </summary>
        /// <param name="Vguid"></param>
        /// <returns></returns>
        public List<Business_ExercisesDetail_Infomation> GetExerciseDetailListByMainVguid(string Vguid)
        {
            return _es.GetExerciseDetailListByMainVguid(Vguid);
        }

        /// <summary>
        /// 检查用户是否做过本套题
        /// </summary>
        /// <param name="Vguid"></param>
        /// <param name="personVguid"></param>
        /// <returns></returns>
        public int CheckUserIsAnswered(string Vguid, string personVguid)
        {
            return _es.CheckUserIsAnswered(Vguid, personVguid);
        }

        /// <summary>
        /// 保存习题信息(主信息、详细信息)
        /// </summary>
        /// <param name="exerciseMainModel"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        public bool SaveExerciseMain(Business_Exercises_Infomation exerciseMainModel, bool isEdit, string exerciseData)
        {
            bool result = false;
            var exerciseModel = new Business_Exercises_Infomation();//习题主信息
            var exerciseList = JsonHelper.JsonToModel<List<Business_ExercisesDetail_Infomation>>(exerciseData);//待保存的习题详细信息
            var exerciseDetailList = new List<Business_ExercisesDetail_Infomation>();
            if (isEdit)
            {
                exerciseModel = GetExerciseByVguid(exerciseMainModel.Vguid.ToString());
                exerciseModel.ExercisesName = exerciseMainModel.ExercisesName;
                exerciseModel.Description = exerciseMainModel.Description;
                exerciseModel.Remarks = exerciseMainModel.Remarks;
                exerciseModel.EffectiveDate = exerciseMainModel.EffectiveDate;
                exerciseModel.Status = 1;
                exerciseModel.ChangeUser = Common.CurrentUser.GetCurrentUser().LoginName;
                exerciseModel.ChangeDate = DateTime.Now;

                //循环遍历习题详情
                foreach (var item in exerciseList)
                {
                    Business_ExercisesDetail_Infomation exerciseDetail = new Business_ExercisesDetail_Infomation();
                    exerciseDetail.ExerciseType = item.ExerciseType;
                    exerciseDetail.ExericseTitleID = item.ExericseTitleID;
                    exerciseDetail.ExerciseName = item.ExerciseName;
                    exerciseDetail.ExerciseOption = item.ExerciseOption;
                    exerciseDetail.Answer = item.Answer;
                    exerciseDetail.Score = item.Score;
                    exerciseDetail.InputType = 1;
                    exerciseDetail.IsEntry = item.IsEntry;        //是否录入习题库
                    exerciseDetail.Vguid = Guid.NewGuid();
                    exerciseDetail.ExercisesInformationVguid = exerciseMainModel.Vguid;//习题主信息Vguid
                    exerciseDetail.CreatedUser = Common.CurrentUser.GetCurrentUser().LoginName;
                    exerciseDetail.CreatedDate = DateTime.Now;
                    exerciseDetail.ChangeDate = DateTime.Now;
                    exerciseDetailList.Add(exerciseDetail);
                }
                result = _es.SaveExerciseMain(exerciseModel, isEdit, exerciseDetailList);
            }
            else
            {
                exerciseModel.ExercisesName = exerciseMainModel.ExercisesName;
                exerciseModel.Description = exerciseMainModel.Description;
                exerciseModel.Remarks = exerciseMainModel.Remarks;
                exerciseModel.EffectiveDate = exerciseMainModel.EffectiveDate;
                exerciseModel.Status = 1;
                exerciseModel.InputType = 1;
                exerciseModel.CreatedDate = DateTime.Now;
                exerciseModel.CreatedUser = Common.CurrentUser.GetCurrentUser().LoginName;
                exerciseModel.ChangeDate = DateTime.Now;
                exerciseModel.Vguid = exerciseMainModel.Vguid;

                foreach (var item in exerciseList)
                {
                    Business_ExercisesDetail_Infomation exerciseDetail = new Business_ExercisesDetail_Infomation();
                    exerciseDetail.ExerciseType = item.ExerciseType;
                    exerciseDetail.ExericseTitleID = item.ExericseTitleID;
                    exerciseDetail.ExerciseName = item.ExerciseName;
                    exerciseDetail.ExerciseOption = item.ExerciseOption;
                    exerciseDetail.Answer = item.Answer;
                    exerciseDetail.Score = item.Score;
                    exerciseDetail.InputType = 1;
                    exerciseDetail.IsEntry = item.IsEntry;        //是否录入习题库
                    exerciseDetail.Vguid = Guid.NewGuid();
                    exerciseDetail.ExercisesInformationVguid = exerciseMainModel.Vguid;//习题主信息Vguid
                    exerciseDetail.CreatedUser = Common.CurrentUser.GetCurrentUser().LoginName;
                    exerciseDetail.CreatedDate = DateTime.Now;
                    exerciseDetail.ChangeDate = DateTime.Now;
                    exerciseDetailList.Add(exerciseDetail);
                }
                result = _es.SaveExerciseMain(exerciseModel, isEdit, exerciseDetailList);
            }
            return result;
        }
    }
}