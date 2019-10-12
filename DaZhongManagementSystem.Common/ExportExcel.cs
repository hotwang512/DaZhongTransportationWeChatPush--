using Aspose.Cells;
using System.Data;
using System.Web;
using Aspose.Pdf.Drawing;

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
        public static string ExportExcelsTo(string templateFileName, string fileName, DataTable dataSource)
        {
            string rootPath = HttpContext.Current.Server.MapPath(string.Format("~/ReportTemplate/{0}", templateFileName));
            string folderPath = HttpContext.Current.Server.MapPath("~/Temp");
            if (System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            string filePath = System.IO.Path.Combine(folderPath, fileName);
            Workbook wk = new Workbook(rootPath);
            WorkbookDesigner designer = new WorkbookDesigner(wk);
            designer.SetDataSource(dataSource);
            designer.Process();
            designer.Workbook.Save(filePath);
            //designer.Workbook.Save(HttpContext.Current.Response, fileName, ContentDisposition.Attachment, designer.Workbook.SaveOptions);
            return fileName;
        }
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
            designer.Workbook.Save(HttpContext.Current.Response, fileName, ContentDisposition.Attachment, designer.Workbook.SaveOptions);
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
            designer.Workbook.Save(HttpContext.Current.Response, fileName, ContentDisposition.Attachment, designer.Workbook.SaveOptions);
        }
    }
}
