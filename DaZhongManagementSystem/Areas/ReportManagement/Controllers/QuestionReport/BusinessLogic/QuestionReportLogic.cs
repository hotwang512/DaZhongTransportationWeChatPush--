using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.ReportManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DaZhongManagementSystem.Common;
using System.Data;

namespace DaZhongManagementSystem.Areas.ReportManagement.Controllers.QuestionReport.BusinessLogic
{
    public class QuestionReportLogic
    {
        public QuestionReportServer _ss;
        public QuestionReportLogic()
        {
            _ss = new QuestionReportServer();
        }

        /// <summary>
        /// 获取已审核问卷列表
        /// </summary>
        /// <returns></returns>
        public List<Business_Questionnaire> GetCheckedQuestionList()
        {
            //var list = _ss.GetCheckedQuestionList();
            //foreach (var businessExercisesInfomation in list)
            //{

            //}
            return _ss.GetCheckedQuestionList();
        }

        /// <summary>
        /// 获取导出类型
        /// </summary>
        /// <returns></returns>
        //public List<CS_Master_2> GetExportTypeList()
        //{
        //    return _ss.GetExportTypeList();
        //}

        /// <summary>
        /// 查询问卷报表信息列表
        /// </summary>
        /// <param name="searchScoreReport"></param>
        /// <returns></returns>
        public U_QuestionReport GetQuestionReport(Search_QuestionReport searchScoreReport)
        {
            return _ss.GetQuestionReport(searchScoreReport);
        }

        /// <summary>
        /// 获取每一道习题的答题情况
        /// </summary>
        /// <param name="vguid">习题vguid</param>
        ///  <param name="departmentVguid">部门vguid</param>
        /// <returns></returns>
        public List<U_QuestionDetailCollect> GetQuestionDetail(string vguid, string departmentVguid)
        {
            return _ss.GetQuestionDetail(vguid, departmentVguid);
        }

        /// <summary>
        /// 获取习题答题状况详情（饼图）
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public List<RateModel> GetExerciseRateDetail(string vguid, string departmentVguid)
        {
            List<RateModel> listRateModel = new List<RateModel>();
            List<U_QuestionSsetsRate> exerciseSsetsRate = _ss.GetQuestionRateDetail(vguid, departmentVguid);
            RateModel rateModel1 = new RateModel();
            rateModel1.Browser = "答题人数";

            if (exerciseSsetsRate[0].numberofanswer == 0)
            {
                rateModel1.Share = "0%";
            }
            else
            {
                rateModel1.Share = (exerciseSsetsRate[0].Questionsperson / exerciseSsetsRate[0].numberofanswer * 100).ToString("F2") + "%";
            }

            listRateModel.Add(rateModel1);

            RateModel rateModel2 = new RateModel();
            rateModel2.Browser = "未答题人数";
            if (exerciseSsetsRate[0].numberofanswer == 0)
            {
                rateModel2.Share = "0%";
            }
            else
            {
                rateModel2.Share = ((1 - exerciseSsetsRate[0].Questionsperson / exerciseSsetsRate[0].numberofanswer) * 100).ToString("F2") + "%";
            }

            listRateModel.Add(rateModel2);

            RateModel rateModel3 = new RateModel();
            rateModel3.Browser = "答题总人数";
            rateModel3.Share = exerciseSsetsRate[0].Questionsperson.ToString();
            listRateModel.Add(rateModel3);

            RateModel rateModel4 = new RateModel();
            rateModel4.Browser = "未答题总人数";
            if (exerciseSsetsRate[0].numberofanswer == 0)
            {
                rateModel4.Share = "0";
            }
            else
            {
                rateModel4.Share = (exerciseSsetsRate[0].numberofanswer - exerciseSsetsRate[0].Questionsperson).ToString();
            }

            listRateModel.Add(rateModel4);
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
        public List<PsychologicalEvaluationModel> GetPsychologicalEvaluationSource(string vguid, string start, string end)
        {
            return _ss.GetPsychologicalEvaluationSource(vguid, start, end);
        }

        public string ExportPsychologicalEvaluationSource(string vguid, string start, string end)
        {
            return _ss.ExportPsychologicalEvaluationSource(vguid, start, end);
        }
        public DataTable ExerciseTotalReport(string startDate, string endDate, string dept)
        {
            return _ss.GetExerciseTotalSource(startDate, endDate, dept);
        }
        public void ExportExerciseTotalReport(string startDate, string endDate, string dept)
        {
            var sources = _ss.GetExerciseTotalSource(startDate, endDate, dept);
            ExportExcel.ExportExcels("答题统计.xls", sources);
        }
    }
}