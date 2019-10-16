using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;


namespace DaZhongManagementSystem.Infrastructure.ReportManagement
{
    public class QuestionReportServer
    {
        /// <summary>
        /// 日志
        /// </summary>
        public LogLogic _logLogic;
        public QuestionReportServer()
        {
            _logLogic = new LogLogic();
        }

        /// <summary>
        /// 获取已审核问卷列表
        /// </summary>
        /// <returns></returns>
        public List<Business_Questionnaire> GetCheckedQuestionList()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                List<Business_Questionnaire> questionList = new List<Business_Questionnaire>();
                questionList = _dbMsSql.Queryable<Business_Questionnaire>().Where(i => i.Status == "2").OrderBy(i => i.CreatedDate, OrderByType.Desc).ToList();
                //  _dbMsSql.Queryable<string>()
                return questionList;
            }
        }

        /// <summary>
        /// 获取导出类型列表
        /// </summary>
        /// <returns></returns>
        //public List<CS_Master_2> GetExportTypeList()
        //{
        //    using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
        //    {
        //        List<CS_Master_2> exportTypeList = new List<CS_Master_2>();
        //        Guid exportTypeVguid = Guid.Parse(Common.Tools.MasterVGUID.ExportType);
        //        exportTypeList = _dbMsSql.Queryable<CS_Master_2>().Where(i => i.VGUID == exportTypeVguid).ToList().OrderBy("MasterCode", OrderByType.Asc).ToList();

        //        return exportTypeList;
        //    }
        //}

        /// <summary>
        /// 查询问卷报表信息列表
        /// </summary>
        /// <param name="searchQuestionReport"></param>
        /// <returns></returns>
        public U_QuestionReport GetQuestionReport(Search_QuestionReport searchQuestionReport)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                U_QuestionReport reportEntity = new U_QuestionReport();
                string sql = @"EXEC usp_ExercisesAnswerquestionsInfotmation @ExercisesName,@DepartmentVGUID";
                DataTable dt = new DataTable();
                dt = _dbMsSql.GetDataTable(sql, new
                {
                    QuestionName = searchQuestionReport.QuestionName,
                    DepartmentVGUID = string.IsNullOrEmpty(CurrentUser.GetCurrentUser().Department) ? "" : CurrentUser.GetCurrentUser().Department
                });
                //reportEntity.questionMain = GetExerciseMainList(dt);
                return reportEntity;
            }
        }

        /// <summary>
        /// 明细
        /// </summary>
        /// <param name="vguid">习题vguid</param>
        ///  <param name="departmentVguid">部门vguid</param>
        /// <returns></returns>
        public List<U_QuestionDetailCollect> GetQuestionDetail(string vguid, string departmentVguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid Vguid = Guid.Parse(vguid);
                List<U_QuestionDetailCollect> reportEntity = new List<U_QuestionDetailCollect>();
                string sql = @"EXEC usp_QuestionSAnswerRate @ExercisesVGUID,@DepartmentVGUID";
                DataTable dt = new DataTable();
                string departVguid = Guid.Empty.ToString();

                if (CurrentUser.GetCurrentUser().LoginName == "sysAdmin")
                {
                    departVguid = string.IsNullOrEmpty(departmentVguid) ? "00000000-0000-0000-0000-000000000000" : departmentVguid;
                }
                else
                {
                    departVguid = CurrentUser.GetCurrentUser().Department;
                    if (!string.IsNullOrEmpty(departmentVguid))
                    {
                        Guid dep = Guid.Parse(departVguid);
                        var listDep = _dbMsSql.SqlQuery<Guid>("SELECT * FROM dbo.TF_OrganizationFDetail('" + dep + "')");  //找到该部门以及其所有子部门
                        Guid depVguid = Guid.Parse(departmentVguid);
                        departVguid = listDep.Contains(depVguid) ? departmentVguid : departVguid;
                    }
                }
                dt = _dbMsSql.GetDataTable(sql, new
                {
                    ExercisesVGUID = Vguid,

                    DepartmentVGUID = departVguid
                });
                //reportEntity.exerciseMain = GetExerciseMainList(ds.Tables[0]);
                reportEntity = GetQuestionDetailList(dt);

                return reportEntity;
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="exerciseVguid">习题Vguid</param>
        /// <param name="exportType">导出类型</param>
        /// <param name="departmentVguid">部门vguid</param>
        public void Export(string exerciseVguid, string exportType, string departmentVguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                string sql = string.Format(@"EXEC usp_ExercisesAnswerUser @ExercisesVGUID,@Type,@DepartmentVGUID");
                DataTable dt = new DataTable();
                string departVguid = string.Empty;
                if (CurrentUser.GetCurrentUser().LoginName == "sysAdmin")
                {
                    departVguid = string.IsNullOrEmpty(departmentVguid) ? "" : departmentVguid;
                }
                else
                {
                    departVguid = CurrentUser.GetCurrentUser().Department;
                    if (!string.IsNullOrEmpty(departmentVguid))
                    {
                        Guid dep = Guid.Parse(departVguid);
                        var listDep = _dbMsSql.SqlQuery<Guid>("SELECT * FROM dbo.TF_OrganizationFDetail('" + dep + "')");  //找到该部门以及其所有子部门
                        Guid depVguid = Guid.Parse(departmentVguid);
                        departVguid = listDep.Contains(depVguid) ? departmentVguid : departVguid;
                    }
                }
                dt = _dbMsSql.GetDataTable(sql, new
                {
                    ExercisesVGUID = exerciseVguid,
                    Type = exportType,
                    DepartmentVGUID = departVguid
                });
                dt.TableName = "table";
                DataView dv = dt.DefaultView;
                dv.Sort = "Department";
                dt = dv.ToTable();
                //因为导出模板不一样（exportType为3/4/5的是一个模板,其他的是一个模板）
                if (exportType == "3" || exportType == "4" || exportType == "5")
                {
                    string amountFileName = SyntacticSugar.ConfigSugar.GetAppString("ScoreTemplate");
                    Common.ExportExcel.ExportExcels("ScoreTemplate.xlsx", amountFileName, dt);

                    _logLogic.SaveLog(13, 22, Common.CurrentUser.GetCurrentUser().LoginName, "ScoreTemplate", Common.Tools.DataTableHelper.Dtb2Json(dt));
                }
                else
                {
                    string amountFileName = SyntacticSugar.ConfigSugar.GetAppString("PersonReportTemplate");
                    Common.ExportExcel.ExportExcels("ReprotPersonTemplate.xlsx", amountFileName, dt);

                    _logLogic.SaveLog(13, 22, Common.CurrentUser.GetCurrentUser().LoginName, "PersonReportTemplate", Common.Tools.DataTableHelper.Dtb2Json(dt));
                }

            }
        }

        /// <summary>
        /// 获取问卷答题状况详情（饼图）
        /// </summary>
        /// <param name="vguid"></param>
        /// <param name="departmentVguid">部门vguid</param>
        /// <returns></returns>
        public List<U_QuestionSsetsRate> GetQuestionRateDetail(string vguid, string departmentVguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                Guid exerciseVguid = Guid.Parse(vguid);
                List<U_QuestionSsetsRate> questionRateEntity = new List<U_QuestionSsetsRate>();
                string sql = string.Format(@"EXEC usp_QuestionSsetsRate @ExercisesVGUID,@DepartmentVGUID");
                DataTable dt = new DataTable();
                string departVguid = string.Empty;
                if (CurrentUser.GetCurrentUser().LoginName == "sysAdmin")
                {
                    departVguid = string.IsNullOrEmpty(departmentVguid) ? "" : departmentVguid;
                }
                else
                {
                    departVguid = CurrentUser.GetCurrentUser().Department;
                    if (!string.IsNullOrEmpty(departmentVguid))
                    {
                        Guid dep = Guid.Parse(departVguid);
                        var listDep = _dbMsSql.SqlQuery<Guid>("SELECT * FROM dbo.TF_OrganizationFDetail('" + dep + "')");  //找到该部门以及其所有子部门
                        Guid depVguid = Guid.Parse(departmentVguid);
                        departVguid = listDep.Contains(depVguid) ? departmentVguid : departVguid;
                    }
                }
                //dt = _dbMsSql.GetDataTable(sql, new
                //{
                //    ExercisesVGUID = exerciseVguid,
                //    DepartmentVGUID = departVguid
                //});
                //exerciseRateEntity = GetExerciseRateList(dt);
                questionRateEntity = _dbMsSql.SqlQuery<U_QuestionSsetsRate>(sql, new { ExercisesVGUID = exerciseVguid, DepartmentVGUID = departVguid });
                //存入操作日志表
                //string logData = JsonHelper.ModelToJson<JsonResultModel<U_ScoreReport>>(jsonResult);
                //_ll.SaveLog(3, 8, "习题列表", logData);

                return questionRateEntity;
            }
        }

        /// <summary>
        ///将datatable转成list
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<U_ExerciseSsetsRate> GetExerciseRateList(DataTable table)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                List<U_ExerciseSsetsRate> list = new List<U_ExerciseSsetsRate>();
                U_ExerciseSsetsRate t = default(U_ExerciseSsetsRate);
                PropertyInfo[] propertypes = null;
                string tempName = string.Empty;
                foreach (DataRow row in table.Rows)
                {
                    t = Activator.CreateInstance<U_ExerciseSsetsRate>();
                    propertypes = t.GetType().GetProperties();
                    foreach (PropertyInfo pro in propertypes)
                    {
                        tempName = pro.Name;
                        if (table.Columns.Contains(tempName))
                        {
                            object value = row[tempName];
                            if (value != DBNull.Value)
                            {
                                pro.SetValue(t, value, null);
                            }
                        }
                    }
                    list.Add(t);
                }
                return list;
            }
        }

        /// <summary>
        /// 将费用查询费用报表datatable转为list
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<U_ExerciseMainCollect> GetExerciseMainList(DataTable table)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                List<U_ExerciseMainCollect> list = new List<U_ExerciseMainCollect>();
                U_ExerciseMainCollect t = default(U_ExerciseMainCollect);
                PropertyInfo[] propertypes = null;
                string tempName = string.Empty;
                foreach (DataRow row in table.Rows)
                {
                    t = Activator.CreateInstance<U_ExerciseMainCollect>();
                    propertypes = t.GetType().GetProperties();
                    foreach (PropertyInfo pro in propertypes)
                    {
                        tempName = pro.Name;
                        if (table.Columns.Contains(tempName))
                        {
                            object value = row[tempName];
                            if (value != DBNull.Value)
                            {
                                pro.SetValue(t, value, null);
                            }
                        }
                    }
                    list.Add(t);
                }
                return list;
            }
        }

        /// <summary>
        /// 将费用查询费用报表datatable转为list
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<ExerciseDetailReport> GetExerciseDetailList(DataTable table)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                List<ExerciseDetailReport> list = new List<ExerciseDetailReport>();
                ExerciseDetailReport t = default(ExerciseDetailReport);
                PropertyInfo[] propertypes = null;
                string tempName = string.Empty;
                foreach (DataRow row in table.Rows)
                {
                    t = Activator.CreateInstance<ExerciseDetailReport>();
                    propertypes = t.GetType().GetProperties();
                    foreach (PropertyInfo pro in propertypes)
                    {
                        tempName = pro.Name;
                        if (table.Columns.Contains(tempName))
                        {
                            object value = row[tempName];
                            if (value != DBNull.Value)
                            {
                                pro.SetValue(t, value, null);
                            }
                        }
                    }
                    list.Add(t);
                }
                return list;
            }
        }

        public List<U_QuestionDetailCollect> GetQuestionDetailList(DataTable table)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                List<U_QuestionDetailCollect> list = new List<U_QuestionDetailCollect>();
                U_QuestionDetailCollect t = default(U_QuestionDetailCollect);
                PropertyInfo[] propertypes = null;
                string tempName = string.Empty;
                foreach (DataRow row in table.Rows)
                {
                    t = Activator.CreateInstance<U_QuestionDetailCollect>();
                    propertypes = t.GetType().GetProperties();
                    foreach (PropertyInfo pro in propertypes)
                    {
                        tempName = pro.Name;
                        if (table.Columns.Contains(tempName))
                        {
                            object value = row[tempName];
                            if (value != DBNull.Value)
                            {
                                pro.SetValue(t, value, null);
                            }
                        }
                    }
                    list.Add(t);
                }
                return list;
            }
        }

        /// <summary>
        /// 获取心理评测数据源
        /// </summary>
        /// <param name="vguid">习题vguid</param>
        /// <returns></returns>
        public List<PsychologicalEvaluationModel> GetPsychologicalEvaluationSource(string vguid, string start, string end)
        {
            List<PsychologicalEvaluationModel> models = new List<PsychologicalEvaluationModel>();
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                models = _dbMsSql.SqlQuery<PsychologicalEvaluationModel>("exec usp_PsychologicalEvaluation @vguid,@start,@end", new { vguid = vguid, start = start, end = end });
            }

            foreach (PsychologicalEvaluationModel item in models)
            {
                item.Result = "待面试";
                if (item.ptScore.Value >= 11 && item.phqScore.Value <= 2)
                {
                    item.ColorBlock = "blue";
                    item.Result = "可录用";
                }
                else if (
                    (item.ptScore.Value >= 11 && (item.phqScore.Value >= 3 && item.phqScore.Value <= 5))
                    ||
                    (item.ptScore.Value >= 5 && item.ptScore.Value <= 10 && item.phqScore.Value <= 5)
                    )
                {
                    item.ColorBlock = "green";
                    item.Result = "可录用";
                }
                else if (
                  (item.ptScore.Value >= 11 && (item.phqScore.Value >= 6 && item.phqScore.Value <= 8))
                  ||
                  ((item.ptScore.Value >= 5 && item.ptScore.Value <= 10) && (item.phqScore.Value >= 6 && item.phqScore.Value <= 8))
                  )
                {
                    item.ColorBlock = "yellow";

                }
                else
                {
                    item.ColorBlock = "red";
                }
            }
            return models;
        }

        public string ExportPsychologicalEvaluationSource(string vguid, string start, string end)
        {
            List<PsychologicalEvaluationModel> list = GetPsychologicalEvaluationSource(vguid, start, end);
            DataTable dtSource = new DataTable("tb");
            dtSource.Columns.AddRange(new DataColumn[] {
                new DataColumn("Name",typeof(string)),
                new DataColumn("ChangeDate",typeof(string)),
                new DataColumn("ptScore",typeof(decimal)),
                new DataColumn("phqScore",typeof(decimal)),
                new DataColumn("ColorBlock",typeof(string)),
                new DataColumn("Result",typeof(string))
            });
            foreach (var item in list)
            {
                DataRow row = dtSource.NewRow();
                row["Name"] = item.Name;
                row["ChangeDate"] = item.ChangeDate;
                row["ptScore"] = item.ptScore;
                row["phqScore"] = item.phqScore;
                row["ColorBlock"] = item.ColorBlock;
                row["Result"] = item.Result;
                dtSource.Rows.Add(row);
            }
            return ExportExcel.ExportExcelsTo("PsychologicalEvaluation.xlsx", "PsychologicalEvaluation" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", dtSource);
        }


    }
}
