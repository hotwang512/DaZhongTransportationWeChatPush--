using Aspose.Cells;
using System.Data;
using System.Web;

namespace DaZhongManagementSystem.Common
{
    public class ExportExcel
    {
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="templateFileName">模板名称</param>
        /// <param name="fileName">导出文件名称</param>
        /// <param name="dataSource">数据源</param>
        public static void ExportExcels(string templateFileName, string fileName, DataTable dataSource)
        {
            string rootPath = HttpContext.Current.Server.MapPath(string.Format("~/ReportTemplate/{0}", templateFileName));
            Workbook wk = new Workbook(rootPath);
            WorkbookDesigner designer = new WorkbookDesigner(wk);
            designer.SetDataSource(dataSource);
            designer.Process();
            designer.Workbook.Save(HttpContext.Current.Response, fileName, ContentDisposition.Inline, designer.Workbook.SaveOptions);
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="templateFileName">模板名称</param>
        /// <param name="fileName">导出文件名称</param>
        /// <param name="dataSource">数据源</param>
        public static void ExportExcels(string templateFileName, string fileName, DataSet dataSource)
        {
            string rootPath = HttpContext.Current.Server.MapPath(string.Format("~/ReportTemplate/{0}", templateFileName));
            Workbook wk = new Workbook(rootPath);
            WorkbookDesigner designer = new WorkbookDesigner(wk);
            designer.SetDataSource(dataSource);
            designer.Process();
            designer.Workbook.Save(HttpContext.Current.Response, fileName, ContentDisposition.Inline, designer.Workbook.SaveOptions);
        }
    }
}
