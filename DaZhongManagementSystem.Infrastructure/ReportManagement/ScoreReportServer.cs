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
    public class ScoreReportServer
    {
        /// <summary>
        /// 日志
        /// </summary>
        public LogLogic _logLogic;
        public ScoreReportServer()
        {
            _logLogic = new LogLogic();
        }

        /// <summary>
        /// 获取已审核习题列表
        /// </summary>
        /// <returns></returns>
        public List<Business_Exercises_Infomation> GetCheckedExerciseList()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                List<Business_Exercises_Infomation> exerciseList = new List<Business_Exercises_Infomation>();
                exerciseList = _dbMsSql.Queryable<Business_Exercises_Infomation>().Where(i => i.Status == 2).OrderBy(i => i.CreatedDate, OrderByType.Desc).ToList();
                //  _dbMsSql.Queryable<string>()
                return exerciseList;
            }
        }

        /// <summary>
        /// 获取导出类型列表
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetExportTypeList()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                List<CS_Master_2> exportTypeList = new List<CS_Master_2>();
                Guid exportTypeVguid = Guid.Parse(Common.Tools.MasterVGUID.ExportType);
                exportTypeList = _dbMsSql.Queryable<CS_Master_2>().Where(i => i.VGUID == exportTypeVguid).ToList().OrderBy("MasterCode", OrderByType.Asc).ToList();

                return exportTypeList;
            }
        }

        /// <summary>
        /// 查询习题报表信息列表
        /// </summary>
        /// <param name="searchScoreReport"></param>
        /// <returns></returns>
        public U_ScoreReport GetScoreReport(Search_ScoreReport searchScoreReport)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                U_ScoreReport reportEntity = new U_ScoreReport();
                string sql = @"EXEC usp_ExercisesAnswerquestionsInfotmation @ExercisesName,@DepartmentVGUID";
                DataTable dt = new DataTable();
                dt = _dbMsSql.GetDataTable(sql, new
                    {
                        ExercisesName = searchScoreReport.ExerciseName,
                        DepartmentVGUID = string.IsNullOrEmpty(CurrentUser.GetCurrentUser().Department) ? "" : CurrentUser.GetCurrentUser().Department
                    });
                reportEntity.exerciseMain = GetExerciseMainList(dt);
                return reportEntity;
            }
        }

        /// <summary>
        /// 明细
        /// </summary>
        /// <param name="vguid">习题vguid</param>
        ///  <param name="departmentVguid">部门vguid</param>
        /// <returns></returns>
        public List<ExerciseDetailReport> GetExerciseDetail(string vguid, string departmentVguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid Vguid = Guid.Parse(vguid);
                List<ExerciseDetailReport> reportEntity = new List<ExerciseDetailReport>();
                string sql = @"EXEC usp_ExercisesAnswerquestionsInfotmationDetail @ExercisesVGUID,@DepartmentVGUID";
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
                    ExercisesVGUID = Vguid,

                    DepartmentVGUID = departVguid
                });
                //reportEntity.exerciseMain = GetExerciseMainList(ds.Tables[0]);
                reportEntity = GetExerciseDetailList(dt);

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
        /// 获取习题答题状况详情（饼图）
        /// </summary>
        /// <param name="vguid"></param>
        /// <param name="departmentVguid">部门vguid</param>
        /// <returns></returns>
        public List<U_ExerciseSsetsRate> GetExerciseRateDetail(string vguid, string departmentVguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                Guid exerciseVguid = Guid.Parse(vguid);
                List<U_ExerciseSsetsRate> exerciseRateEntity = new List<U_ExerciseSsetsRate>();
                string sql = string.Format(@"EXEC usp_ExerciseSsetsRate @ExercisesVGUID,@DepartmentVGUID");
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
                exerciseRateEntity = _dbMsSql.SqlQuery<U_ExerciseSsetsRate>(sql, new { ExercisesVGUID = exerciseVguid, DepartmentVGUID = departVguid });
                //存入操作日志表
                //string logData = JsonHelper.ModelToJson<JsonResultModel<U_ScoreReport>>(jsonResult);
                //_ll.SaveLog(3, 8, "习题列表", logData);

                return exerciseRateEntity;
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


    }
}
