using DaZhongManagementSystem.Areas.SecondaryCleaningManagement.Models;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.UserManagement.BussinessLogic;
using DaZhongManagementSystem.Controllers;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Aspose.Cells;
using System.IO;
using JQWidgetsSugar;

namespace DaZhongManagementSystem.Areas.ReportManagement.Controllers.CleaningReport
{
    public class CleaningReportController : BaseController
    {
        public UserManageLogic _ul;
        public AuthorityManageLogic _al;
        public CleaningReportController()
        {
            _ul = new UserManageLogic();
            _al = new AuthorityManageLogic();
        }
        public ActionResult Index()
        {
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.UserSystemModule);
            ViewBag.CurrentModulePermission = roleModuleModel;
            var orgName = getCabOrgName();
            ViewBag.OrgName = orgName;
            return View();
        }
        public List<string> getCabOrgName()
        {
            List<string> value = new List<string>();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance2())
            {
                value = _dbMsSql.SqlQuery<string>(@"select distinct OrganizationName from DZ_Organization where OrganizationName is not null ").ToList();
            }
            return value;
        }
        public JsonResult GetCleaningInfo(string cabOrgName, DateTime? operationDate, string couponType)
        {
            List<Business_SecondaryCleaning> cleaningList = new List<Business_SecondaryCleaning>();
            using (SqlSugarClient _db = SugarDao_MsSql.GetInstance())
            {
                var year = DateTime.Now.Year;
                if (operationDate != null)
                {
                    if (operationDate.Value.Year != year)
                    {
                        year = operationDate.Value.Year;
                    }
                }  
                cleaningList = _db.SqlQuery<Business_SecondaryCleaning>(@"select CouponType,CabOrgName,month(OperationDate) as 'Description',OperationDate  from Business_SecondaryCleaning
                                               where year(OperationDate) = @Year and CabOrgName != '' order by OperationDate desc", new { Year = year });
                if (cabOrgName != "" && cabOrgName != null)
                {
                    cleaningList = cleaningList.Where(x => x.CabOrgName.Contains(cabOrgName)).ToList();
                }
                if (operationDate != null)
                {
                    var lastDate = operationDate.Value.AddMonths(1).AddDays(-1);
                    var newLastDate = (lastDate.ToString("yyyy-MM-dd") + " 23:59:59").ObjToDate();
                    cleaningList = cleaningList.Where(x => x.OperationDate >= operationDate && x.OperationDate <= newLastDate).ToList();
                }
                if (couponType != "" && couponType != null)
                {
                    cleaningList = cleaningList.Where(x => x.CouponType == couponType).ToList();
                }
            }
            return Json(cleaningList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ExportCleaningInfo(string cabOrgName, DateTime? operationDate, string couponType)
        {
            List<SecondaryCleaning> cleaningList = new List<SecondaryCleaning>();
            using (SqlSugarClient _db = SugarDao_MsSql.GetInstance())
            {
                var year = DateTime.Now.Year;
                if (operationDate != null)
                {
                    if (operationDate.Value.Year != year)
                    {
                        year = operationDate.Value.Year;
                    }
                }
                cleaningList = _db.SqlQuery<SecondaryCleaning>(@"select sc.CouponType,sc.CabLicense,sc.CabOrgName,sc.CreatedUser,sc.ManOrgName,cc.CompanyName,sc.OperationDate  from Business_SecondaryCleaning as sc
                                left join Business_CleaningCompany as cc on sc.CompanyVguid = CAST(cc.Vguid as varchar(100))
                                where year(OperationDate) = @Year  and CabOrgName != ''  order by OperationDate desc", new { Year = year });
                if (cabOrgName != "" && cabOrgName != null)
                {
                    cleaningList = cleaningList.Where(x => x.CabOrgName.Contains(cabOrgName)).ToList();
                }
                if (operationDate != null)
                {
                    var lastDate = operationDate.Value.AddMonths(1).AddDays(-1);
                    var newLastDate = (lastDate.ToString("yyyy-MM-dd") + " 23:59:59").ObjToDate();
                    cleaningList = cleaningList.Where(x => x.OperationDate >= operationDate && x.OperationDate <= newLastDate).ToList();
                }
                if (couponType != "" && couponType != null)
                {
                    cleaningList = cleaningList.Where(x => x.CouponType == couponType).ToList();
                }
                List<string> header = new List<string>() { "业务类型", "车辆", "车辆所属公司", "当前司机", "司机所属公司", "操作地点", "操作时间" };
                ExportExcel(cleaningList, header);
            }
            return Json(cleaningList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 导出Excel表格
        /// </summary>
        /// <param name="list">数据集合</param>
        /// <param name="header">数据表头</param>
        /// <returns></returns>
        public bool ExportExcel(List<SecondaryCleaning> cleaningList, List<string> header)
        {
            var isSuccess = false;
            Workbook wb = new Workbook(FileFormatType.Xlsx);
            try
            {
                Worksheet sheet = wb.Worksheets[0];
                sheet.Name = "二级清洗明细";
                if (cleaningList.Count <= 0)
                {
                    System.Web.HttpContext.Current.Response.Write("<script>alert('没有检测到需要导出数据!');</script>");
                    return isSuccess = false;
                }
                // 为单元格添加样式
                Style style = wb.CreateStyle();
                style.HorizontalAlignment = TextAlignmentType.Center;  //设置居中
                style.Font.Size = 12;//文字大小
                style.Font.IsBold = true;//粗体
                style.HorizontalAlignment = TextAlignmentType.Center;//文字居中

                int rowIndex = 0;
                for (int i = 0; i < header.Count; i++)
                {
                    sheet.Cells[rowIndex, i].PutValue(header[i]);
                    sheet.Cells[rowIndex, i].SetStyle(style);
                    sheet.Cells.SetColumnWidth(i, 20);//设置宽度
                }
                for (int i = 0; i < cleaningList.Count; i++)//遍历DataTable行
                {
                    sheet.Cells[i + 1, 0].PutValue(cleaningList[i].CouponType.ToString());
                    sheet.Cells[i + 1, 1].PutValue(cleaningList[i].CabLicense.ToString());
                    sheet.Cells[i + 1, 2].PutValue(cleaningList[i].CabOrgName.ToString());
                    sheet.Cells[i + 1, 3].PutValue(cleaningList[i].CreatedUser.ToString());
                    sheet.Cells[i + 1, 4].PutValue(cleaningList[i].ManOrgName.ToString());
                    sheet.Cells[i + 1, 5].PutValue(cleaningList[i].CompanyName.ToString());
                    sheet.Cells[i + 1, 6].PutValue(cleaningList[i].OperationDate.ToString());
                }
            }
            catch (Exception e)
            {
                System.Web.HttpContext.Current.Response.Write("<script>alert('导出异常:" + e.Message + "!');</script>");
                return isSuccess = false;
            }
            #region 输出到Excel
            using (MemoryStream ms = new MemoryStream())
            {

                wb.Save(ms, new OoxmlSaveOptions(SaveFormat.Xlsx));//默认支持xls版，需要修改指定版本
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", "二级清洗报表"+DateTime.Now.ToString("yyyyMMdd")));
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                System.Web.HttpContext.Current.Response.BinaryWrite(ms.ToArray());
                wb = null;
                System.Web.HttpContext.Current.Response.End();
                isSuccess = true;
            }
            #endregion
            return isSuccess;
        }
    }
}
