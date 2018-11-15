using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.ReportManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.ReportManagement.Controllers.ScoreReport.BusinessLogic
{
    public class ScoreReportLogic
    {
        public ScoreReportServer _ss;
        public ScoreReportLogic()
        {
            _ss = new ScoreReportServer();
        }

        /// <summary>
        /// 获取已审核习题列表
        /// </summary>
        /// <returns></returns>
        public List<Business_Exercises_Infomation> GetCheckedExerciseList()
        {
            var list = _ss.GetCheckedExerciseList();
            foreach (var businessExercisesInfomation in list)
            {

            }
            return _ss.GetCheckedExerciseList();
        }

        /// <summary>
        /// 获取导出类型
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetExportTypeList()
        {
            return _ss.GetExportTypeList();
        }

        /// <summary>
        /// 查询习题报表信息列表
        /// </summary>
        /// <param name="searchScoreReport"></param>
        /// <returns></returns>
        public U_ScoreReport GetScoreReport(Search_ScoreReport searchScoreReport)
        {
            return _ss.GetScoreReport(searchScoreReport);
        }

        /// <summary>
        /// 获取每一道习题的答题情况
        /// </summary>
        /// <param name="vguid">习题vguid</param>
        ///  <param name="departmentVguid">部门vguid</param>
        /// <returns></returns>
        public List<ExerciseDetailReport> GetExerciseDetail(string vguid,string departmentVguid)
        {
            return _ss.GetExerciseDetail(vguid, departmentVguid);
        }

        /// <summary>
        /// 获取习题答题状况详情（饼图）
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public List<RateModel> GetExerciseRateDetail(string vguid,string departmentVguid)
        {
            List<RateModel> listRateModel = new List<RateModel>();
            List<U_ExerciseSsetsRate> exerciseSsetsRate = _ss.GetExerciseRateDetail(vguid, departmentVguid);
            RateModel rateModel1 = new RateModel();
            rateModel1.Browser = "答题人数";
            rateModel1.Share = (exerciseSsetsRate[0].Exercisesperson * 100).ToString("F2") + "%";
            listRateModel.Add(rateModel1);

            RateModel rateModel2 = new RateModel();
            rateModel2.Browser = "未答题人数";
            rateModel2.Share = ((1 - exerciseSsetsRate[0].Exercisesperson) * 100).ToString("F2") + "%";
            listRateModel.Add(rateModel2);

            RateModel rateModel3 = new RateModel();
            rateModel3.Browser = "优秀";
            rateModel3.Share = (exerciseSsetsRate[0].Proficiency * 100).ToString("F2") + "%";
            listRateModel.Add(rateModel3);

            RateModel rateModel4 = new RateModel();
            rateModel4.Browser = "及格";
            rateModel4.Share = (exerciseSsetsRate[0].Passrate * 100).ToString("F2") + "%";
            listRateModel.Add(rateModel4);

            RateModel rateModel5 = new RateModel();
            rateModel5.Browser = "未及格";
            rateModel5.Share = (exerciseSsetsRate[0].Dontpass * 100).ToString("F2") + "%";
            listRateModel.Add(rateModel5);

            RateModel rateModel6 = new RateModel();
            rateModel6.Browser = "答题一次";
            rateModel6.Share = (exerciseSsetsRate[0].OneRate * 100).ToString("F2") + "%";
            listRateModel.Add(rateModel6);

            RateModel rateModel7 = new RateModel();
            rateModel7.Browser = "答题两次";
            rateModel7.Share = (exerciseSsetsRate[0].TwoRate * 100).ToString("F2") + "%";
            listRateModel.Add(rateModel7);

            RateModel rateModel8 = new RateModel();
            rateModel8.Browser = "答题三次";
            rateModel8.Share = (exerciseSsetsRate[0].ThreeRate * 100).ToString("F2") + "%";
            listRateModel.Add(rateModel8);

            RateModel rateModel9 = new RateModel();
            rateModel9.Browser = "未答题";
            rateModel9.Share = (exerciseSsetsRate[0].ZoreRate * 100).ToString("F2") + "%";
            listRateModel.Add(rateModel9);

            RateModel rateModel10 = new RateModel();
            rateModel10.Browser = "答题总人数";
            rateModel10.Share = exerciseSsetsRate[0].numberofanswer.ToString();
            listRateModel.Add(rateModel10);

            RateModel rateModel11 = new RateModel();
            rateModel11.Browser = "未答题总人数";
            rateModel11.Share = (exerciseSsetsRate[0].PushUser - exerciseSsetsRate[0].numberofanswer).ToString();
            listRateModel.Add(rateModel11);

            RateModel rateModel12 = new RateModel();
            rateModel12.Browser = "优秀人数";
            rateModel12.Share = exerciseSsetsRate[0].ProficiencyUser.ToString();
            listRateModel.Add(rateModel12);

            RateModel rateModel13 = new RateModel();
            rateModel13.Browser = "及格人数";
            rateModel13.Share = exerciseSsetsRate[0].PassrateUser.ToString();
            listRateModel.Add(rateModel13);

            RateModel rateModel14 = new RateModel();
            rateModel14.Browser = "未及格人数";
            rateModel14.Share = exerciseSsetsRate[0].DontpassUser.ToString();
            listRateModel.Add(rateModel14);

            RateModel rateModel15 = new RateModel();
            rateModel15.Browser = "未答题人数";
            rateModel15.Share = exerciseSsetsRate[0].ZoreUser.ToString();
            listRateModel.Add(rateModel15);

            RateModel rateModel16 = new RateModel();
            rateModel16.Browser = "答题一次人数";
            rateModel16.Share = exerciseSsetsRate[0].OneUser.ToString();
            listRateModel.Add(rateModel16);

            RateModel rateModel17 = new RateModel();
            rateModel17.Browser = "答题两次人数";
            rateModel17.Share = exerciseSsetsRate[0].TwoUser.ToString();
            listRateModel.Add(rateModel17);

            RateModel rateModel18 = new RateModel();
            rateModel18.Browser = "答题三次人数";
            rateModel18.Share = exerciseSsetsRate[0].ThreeUser.ToString();
            listRateModel.Add(rateModel18);
            return listRateModel;
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="exerciseVguid">习题Vguid</param>
        /// <param name="exportType">导出类型</param>
        public void Export(string exerciseVguid, string exportType, string departmentVguid)
        {
            _ss.Export(exerciseVguid, exportType, departmentVguid);
        }
    }
}